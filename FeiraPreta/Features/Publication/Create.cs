using FeiraPreta.Infrastructure;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                string shortcode = message.Link.Substring(28, 11);

                string url = "https://api.instagram.com/v1/media/shortcode/" + shortcode + "?access_token=7207542169.480fb87.1cc924b10c4b43a5915543675bd5f736";

                WebResponse response = processWebRequest(url);

                Domain.Publication publication = new Domain.Publication();

                using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    // var publication = JsonConvert.ToString();

                    var json = JObject.Parse(await sr.ReadToEndAsync());

                    publication.ImageLowResolution = json["data"]["images"]["low_resolution"]["url"].ToString();
                    publication.ImageStandardResolution = json["data"]["images"]["standard_resolution"]["url"].ToString();
                    publication.ImageThumbnail = json["data"]["images"]["thumbnail"]["url"].ToString();
                    publication.PersonId = new Guid("3783B665-C040-40ED-89FB-FD0B83810201");
                    publication.CreatedDate = DateTime.Now;
                    publication.IsHighlight = true;
                    publication.CreatedDateInstagram = DateTime.Now;
                    publication.Subtitle = json["data"]["caption"]["text"].ToString();
                    publication.Link = message.Link;

                    db.Publication.Add(publication);
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
