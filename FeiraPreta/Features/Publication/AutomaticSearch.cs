using FeiraPreta.Infraestructure;
using MediatR;
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
                var persons = db.Person.Where(p => !p.DeletedDate.HasValue).ToList();

                foreach (Domain.Person person in persons) {

                    string url = "https://api.instagram.com/v1/users/" + person.IdInstagram + "/media/recent/?access_token=7207542169.480fb87.1cc924b10c4b43a5915543675bd5f736";

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

                            var tags = new List<String>();

                            foreach (var tag in listTags) tags.Add(tag.ToString());

                            if (tags.Contains("feirapreta") && tags.Contains("produto"))
                            {
                                var command = new Create.Command
                                {
                                    Link = media["link"].ToString()
                                };

                                await mediator.Send(command);
                            }
                        }
                    };
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
