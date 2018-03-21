using MediatR;
using Newtonsoft.Json.Linq;
using FeiraPreta.Infraestructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FeiraPreta.Features.Person
{
    public class Create
    {
        public class Command : IRequest<Result>
        {
            public string Username { get; set; }

            public string PhoneNumber { get; set; }
        }

        public class Result
        {
            public int StatusCode { get; set; }

            public string Message { get; set; }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, Result>
        {
            private readonly Db db;

            public CommandHandler(Db db)
            {
                this.db = db;
            }

            public async Task<Result> Handle(Command message)
            {
                if (message.Username == null || message.Username.Trim() == "") return new Result { Message = "Username não pode ser nulo", StatusCode = 400 };

                if (message.PhoneNumber == null || message.PhoneNumber.Trim() == "") return new Result { Message = "Telefone não pode ser nulo", StatusCode = 400 };

                var exists = await db.Person.SingleOrDefaultAsync(p => p.UsernameInstagram == message.Username);

                if (exists != null)
                {
                    if (exists.DeletedDate.HasValue)
                    {
                        exists.DeletedDate = null;
                        db.Person.Update(exists);
                        db.SaveChanges();
                        return new Result { Message = "Empreendedor cadastrado com sucesso", StatusCode = 201 };
                    }

                    return new Result { Message = "Empreendedor já existente", StatusCode = 409 };
                }
                else
                {
                    string url = "https://api.instagram.com/v1/users/search?q=" + message.Username + "&access_token=7207542169.480fb87.1cc924b10c4b43a5915543675bd5f736";

                    WebResponse response = processWebRequest(url);

                    Domain.Person person = new Domain.Person();

                    using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        var json = JObject.Parse(await sr.ReadToEndAsync());

                        if (json["data"].Count() == 0) return new Result { StatusCode = 404, Message = "Perfil não encontrado" };

                        person = new Domain.Person
                        {
                            ProfilePictureInstagram = json["data"][0]["profile_picture"].ToString(),
                            FullNameInstagram = json["data"][0]["full_name"].ToString(),
                            UsernameInstagram = json["data"][0]["username"].ToString(),
                            CreatedDate = DateTime.Now,
                            PhoneNumber = message.PhoneNumber,
                            IdInstagram = json["data"][0]["id"].ToString()
                        };

                        db.Person.Add(person);
                    }

                    await db.SaveChangesAsync();

                    return new Result { Message = "Empreendedor cadastrado com sucesso", StatusCode = 201 };
                }


            }

            private WebResponse processWebRequest(string url)
            {
                WebRequest request;
                WebResponse response;

                try
                {
                    request = WebRequest.Create(url);

                    response = request.GetResponse();

                    return response;
                }
                catch (Exception e)
                {
                    throw new HttpException(400, "Erro no servidor do Instagram");
                }
            }
        }
    }
}
