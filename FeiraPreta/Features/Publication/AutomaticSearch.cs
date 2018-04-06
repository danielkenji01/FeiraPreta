using FeiraPreta.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FeiraPreta.Features.Publication
{
    public class AutomaticSearch
    {
        public class Command : IRequest
        {
            public Command()
            {

            }
        }

        public class Handler : IAsyncRequestHandler<Command>
        {
            private readonly Db db;
            private readonly IMediator mediator;

            public Handler(Db db, IMediator mediator)
            {
                this.db = db;
                this.mediator = mediator;
            }

            public async Task Handle(Command message)
            {
                string url = "https://api.instagram.com/v1/tags/feirapreta/media/recent?access_token=7207542169.480fb87.1cc924b10c4b43a5915543675bd5f736";

                WebResponse response = processWebRequest(url);

                using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    var json = JObject.Parse(await sr.ReadToEndAsync());

                    if (json["data"] == null) throw new HttpException(404, "Sem publicações existentes");

                    var medias = json["data"].ToList();

                    foreach (var media in medias)
                    {
                        var listTags = media["tags"].ToList();

                        if (listTags.Count == 0) continue;

                        var person = await db.Person.SingleOrDefaultAsync(p => p.UsernameInstagram == media["user"]["username"].ToString());

                        if (person == null) continue;

                        var tags = new List<String>();

                        foreach (var tag in listTags) tags.Add(tag.ToString());

                        if (await db.Publication.SingleOrDefaultAsync(p => p.Link == media["link"].ToString()) != null) continue;

                        if (tags.Contains("feirapreta") && tags.Contains("produto"))
                        {
                            Domain.Publication publication = new Domain.Publication
                            {
                                ImageLowResolution = media["images"]["low_resolution"]["url"].ToString(),
                                ImageStandardResolution = media["images"]["standard_resolution"]["url"].ToString(),
                                ImageThumbnail = media["images"]["thumbnail"]["url"].ToString(),
                                PersonId = person.Id,
                                CreatedDate = DateTime.Now,
                                IsHighlight = false,
                                CreatedDateInstagram = DateTime.Now,
                                Subtitle = media["caption"]["text"].ToString(),
                                Link = media["link"].ToString()
                            };

                            db.Publication.Add(publication);
                            
                            foreach (var t in tags)
                            {
                                var command = new Tag.Create.Command
                                {
                                    Nome = t,
                                    PublicationId = publication.Id
                                };

                                await mediator.Send(command);
                            }

                            await db.SaveChangesAsync();
                        }
                    }
                };
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

