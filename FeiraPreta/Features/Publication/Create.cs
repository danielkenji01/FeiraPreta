using FeiraPreta.Infrastructure;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using FeiraPreta.Infraestructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
            private readonly IMediator mediator;

            public CommandHandler(Db db, IMediator mediator)
            {
                this.db = db;
                this.mediator = mediator;
            }

            public async Task Handle(Command message)
            {
                if (await db.Publication.SingleOrDefaultAsync(p => p.Link == message.Link) != null) throw new ConflictException();

                //string shortcode = message.Link.Substring(28, 11);

                int firstIndex = message.Link.IndexOf("p/");
                int lastIndex = message.Link.LastIndexOf("?");

                string shortcode = message.Link.Substring(firstIndex + 2, lastIndex - firstIndex - 3);

                string url = "https://api.instagram.com/v1/media/shortcode/" + shortcode + "?access_token=7207542169.480fb87.1cc924b10c4b43a5915543675bd5f736";

                WebResponse response = processWebRequest(url);

                Domain.Publication publication = new Domain.Publication();

                using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    var json = JObject.Parse(await sr.ReadToEndAsync());

                    var person = await db.Person.SingleOrDefaultAsync(p => p.UsernameInstagram == json["data"]["user"]["username"].ToString());

                    if (person == null) throw new BadRequestException();

                    publication = new Domain.Publication
                    {
                        ImageLowResolution = json["data"]["images"]["low_resolution"]["url"].ToString(),
                        ImageStandardResolution = json["data"]["images"]["standard_resolution"]["url"].ToString(),
                        ImageThumbnail = json["data"]["images"]["thumbnail"]["url"].ToString(),
                        PersonId = person.Id,
                        CreatedDate = DateTime.Now,
                        IsHighlight = true,
                        CreatedDateInstagram = DateTime.Now,
                        Subtitle = json["data"]["caption"]["text"].ToString(),
                        Link = message.Link
                    };

                    db.Publication.Add(publication);

                    var tags = json["data"]["tags"];

                    foreach (var t in tags)
                    {
                        var command = new Tag.Create.Command
                        {
                            Nome = t.ToString(),
                            PublicationId = publication.Id
                        };

                        await mediator.Send(command);
                    }

                };

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
