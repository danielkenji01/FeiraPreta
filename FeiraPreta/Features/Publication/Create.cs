using FeiraPreta.Infrastructure;
using MediatR;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeiraPreta.Features.Publication
{
    public class Create
    {
        public class Command : IRequest
        {
            public string Link { get; set; }
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
                var shortcode = message.Link.Substring(28);

                var media = "https://api.instagram.com/v1/media/" + shortcode + "D?access_token=4262635600.1677ed0.c1c01fc89e8242fe803084b010db36dc";

                var client = new RestClient(media);

                var request = new RestRequest(Method.GET);

                var response = client.Execute(request);

                Console.WriteLine(response);

                var publication = new Domain.Publication
                {
                    Link = message.Link,
                };

                db.Publication.Add(publication);

                await db.SaveChangesAsync();
            }
        }
    }
}
