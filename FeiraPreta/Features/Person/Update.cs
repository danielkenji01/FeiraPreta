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
        public class Command : IRequest
        {
            public Guid Id { get; set; }

            public string PhoneNumber { get; set; }

            public string Username { get; set; }
        }

        public class CommandHandler : IAsyncRequestHandler<Command>
        {
            private readonly Db db;

            public CommandHandler(Db db)
            {
                this.db = db;
            }

            public async Task Handle(Command message)
            {
                if (message.PhoneNumber == null || message.Username == null) throw new HttpException(400, "Preencha pelo menos um dos campos");

                var person = await db.Person.SingleOrDefaultAsync(p => p.Id == message.Id);

                if (person == null) throw new HttpException(404, "Usuário não existe");

                if (message.PhoneNumber != null) person.PhoneNumber = message.PhoneNumber;

                if (message.Username != null)
                {
                    if (await db.Person.SingleOrDefaultAsync(p => p.UsernameInstagram == message.Username) != null) throw new HttpException(409, "Usuário já existe");

                    string url = "https://api.instagram.com/v1/users/search?q=" + message.Username + "&access_token=7207542169.480fb87.1cc924b10c4b43a5915543675bd5f736";

                    WebResponse response = processWebRequest(url);

                    using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        var json = JObject.Parse(await sr.ReadToEndAsync());

                        if (json["data"].Count() == 0) throw new HttpException(404, "Usuário não encontrado");

                        person = new Domain.Person
                        {
                            ProfilePictureInstagram = json["data"][0]["profile_picture"].ToString(),
                            FullNameInstagram = json["data"][0]["full_name"].ToString(),
                            UsernameInstagram = json["data"][0]["username"].ToString(),
                            CreatedDate = DateTime.Now,
                            IdInstagram = json["data"][0]["id"].ToString()
                        };
                    }
                }

                db.Person.Update(person);
                await db.SaveChangesAsync();
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
