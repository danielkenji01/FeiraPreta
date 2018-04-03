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
using FluentValidation;

namespace FeiraPreta.Features.Publication
{
    public class Highlight
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
                if (message.Link == null || message.Link.Trim() == "") throw new HttpException(400, "Link não pode ser nulo");

                var exists = await db.Publication.SingleOrDefaultAsync(p => p.Link == message.Link);

                if (exists != null)
                {
                    if (exists.DeletedDate.HasValue)
                    {
                        exists.DeletedDate = null;
                        exists.IsHighlight = true;
                    }
                    else throw new HttpException(409, "Publicação já existente");
                }
                else
                {

                    int firstIndex = message.Link.IndexOf("p/");
                    int lastIndex = message.Link.LastIndexOf("?");

                    string shortcode = message.Link.Substring(firstIndex + 2, lastIndex - firstIndex - 3);

                    string url = "https://api.instagram.com/v1/media/shortcode/" + shortcode + "?access_token=7207542169.480fb87.1cc924b10c4b43a5915543675bd5f736";

                    WebResponse response = processWebRequest(url);

                    Domain.Publication publication = new Domain.Publication();

                    using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        var json = JObject.Parse(await sr.ReadToEndAsync());

                        if (json["data"] == null) throw new HttpException(404, "Link inválido");

                        var person = await db.Person.SingleOrDefaultAsync(p => p.UsernameInstagram == json["data"]["user"]["username"].ToString());

                        if (person == null) throw new HttpException(400, "O empreendedor não está cadastrado");

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
                }

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
