using FeiraPreta.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FeiraPreta.Features.Person
{
    public class Update
    {
        public class Command : IRequest<Result>
        {
            public Guid Id { get; set; }

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
                var person = await db.Person.Where(p => !p.DeletedDate.HasValue).SingleOrDefaultAsync(p => p.Id == message.Id);

                if (person == null) return new Result { Message = "Empreendedor não existente", StatusCode = 404 };

                if ((message.Username == null || message.Username.Trim() == "") || (message.PhoneNumber == null || message.PhoneNumber.Trim() == "")) return new Result { Message = "Pelo menos um dos campos deve ser preenchido", StatusCode = 400 };

                if (message.PhoneNumber != null && message.Username == null)
                {
                    person.PhoneNumber = message.PhoneNumber;
                }
                else
                {
                    string url = "https://api.instagram.com/v1/users/search?q=" + message.Username + "&access_token=7207542169.480fb87.1cc924b10c4b43a5915543675bd5f736";

                    WebResponse response = processWebRequest(url);

                    using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        var json = JObject.Parse(await sr.ReadToEndAsync());

                        if (json["data"].Count() == 0) throw new NotFoundException();

                        if (await db.Person.SingleOrDefaultAsync(p => p.UsernameInstagram == json["data"][0]["username"].ToString()) != null) return new Result { Message = "Empreendedor já existente", StatusCode = 409 };

                        person = new Domain.Person
                        {
                            ProfilePictureInstagram = json["data"][0]["profile_picture"].ToString(),
                            FullNameInstagram = json["data"][0]["full_name"].ToString(),
                            UsernameInstagram = json["data"][0]["username"].ToString(),
                            CreatedDate = DateTime.Now,
                            PhoneNumber = message.PhoneNumber
                        };

                        db.Person.Add(person);
                    }
                }
                await db.SaveChangesAsync();

                return new Result { Message = "Empreendedor cadastrado com sucesso!!", StatusCode = 201 };
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
                    throw new HttpException(400, "Erro no servidor do Instagram!!");
                }
            }
        }
    }
}
