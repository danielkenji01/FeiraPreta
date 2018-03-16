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
    public class Create
    {
        public class Command : IRequest<Result>
        {
            public string Link { get; set; }
        }

        public class Result
        {
            public int StatusCode { get; set; }

            public string Message { get; set; }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, Result>
        {
            private readonly Db db;
            private readonly IMediator mediator;

            public CommandHandler(Db db, IMediator mediator)
            {
                this.db = db;
                this.mediator = mediator;
            }

            public async Task<Result> Handle(Command message)
            {
                if (message.Link == null || message.Link.Trim() == "") return new Result { Message = "Link não pode ser nulo", StatusCode = 400 };

                var exists = await db.Publication.SingleOrDefaultAsync(p => p.Link == message.Link);

                if (exists != null)
                {
                    if (exists.DeletedDate.HasValue)
                    {
                        exists.DeletedDate = null;

                        return new Result { Message = "Link cadastrado com sucesso!!", StatusCode = 201 };
                    }

                    return new Result { Message = "Link já existente", StatusCode = 409 };
                }

                int firstIndex = message.Link.IndexOf("p/");
                int lastIndex = message.Link.LastIndexOf("?");

                string shortcode = message.Link.Substring(firstIndex + 2, lastIndex - firstIndex - 3);

                string url = "https://api.instagram.com/v1/media/shortcode/" + shortcode + "?access_token=7207542169.480fb87.1cc924b10c4b43a5915543675bd5f736";

                WebResponse response = processWebRequest(url);

                Domain.Publication publication = new Domain.Publication();

                using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    var json = JObject.Parse(await sr.ReadToEndAsync());

                    if (json["data"] == null) return new Result { StatusCode = 404, Message = "Link inválido" };

                    var person = await db.Person.SingleOrDefaultAsync(p => p.UsernameInstagram == json["data"]["user"]["username"].ToString());

                    if (person == null) return new Result { StatusCode = 400, Message = "O empreendedor não está cadastrado!!!" };

                    publication = new Domain.Publication
                    {
                        ImageLowResolution = json["data"]["images"]["low_resolution"]["url"].ToString(),
                        ImageStandardResolution = json["data"]["images"]["standard_resolution"]["url"].ToString(),
                        ImageThumbnail = json["data"]["images"]["thumbnail"]["url"].ToString(),
                        PersonId = person.Id,
                        CreatedDate = DateTime.Now,
                        IsHighlight = false,
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

                return new Result { Message = "Link cadastrado com sucesso!!", StatusCode = 201 };
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
