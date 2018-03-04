using FeiraPreta.Infrastructure;
using MediatR;
using Newtonsoft.Json.Linq;
using FeiraPreta.Infraestructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FeiraPreta.Features.Person
{
    public class Create
    {
        public class Command : IRequest
        {
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
                var list = db.Person.ToList();

                foreach (var item in list)
                {
                    if (item.UsernameInstagram == message.Username) throw new ConflictException();
                }

                string url = "https://api.instagram.com/v1/users/search?q=" + message.Username + "&access_token=7207542169.480fb87.1cc924b10c4b43a5915543675bd5f736";

                WebResponse response = processWebRequest(url);

                Domain.Person person = new Domain.Person();

                using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    var json = JObject.Parse(await sr.ReadToEndAsync());

                    person = new Domain.Person
                    {
                        ProfilePictureInstagram = json["data"][0]["profile_picture"].ToString(),
                        FullNameInstagram = json["data"][0]["full_name"].ToString(),
                        UsernameInstagram = json["data"][0]["username"].ToString(),
                        CreatedDate = DateTime.Now
                    };

                    db.Person.Add(person);
                }

                await db.SaveChangesAsync();
            }

            private WebResponse processWebRequest(string url)
            {
                WebRequest request;
                WebResponse response;

                request = WebRequest.Create(url);
                response = request.GetResponse();

                return response;
            }
        }
    }
}
