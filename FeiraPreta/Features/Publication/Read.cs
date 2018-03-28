using FeiraPreta.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeiraPreta.Features.Publication
{
    public class Read
    {
        public class Query : IRequest<Result>
        {
            public Guid Id { get; set; }
        }

        public class Result
        {
            public Guid Id { get; set; }

            public string Link { get; set; }

            public string Subtitle { get; set; }

            public DateTime CreatedDateInstagram { get; set; }

            public string ImageLowResolution { get; set; }

            public string ImageThumbnail { get; set; }

            public string ImageStandardResolution { get; set; }

            public DateTime CreatedDate { get; set; }

            public bool IsHighlight { get; set; }

            public PersonResult Person { get; set; }

            public class PersonResult
            {
                public Guid Id { get; set; }

                public string UsernameInstagram { get; set; }

                public string FullNameInstagram { get; set; }

                public string ProfilePictureInstagram { get; set; }

                public DateTime CreatedDate { get; set; }

                public string PhoneNumber { get; set; }
            }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, Result>
        {
            private readonly Db db;

            public QueryHandler(Db db)
            {
                this.db = db;
            }

            public async Task<Result> Handle(Query message)
            {
                if (message.Id == null || message.Id.ToString().Trim() == "") throw new HttpException(400, "Id não pode ser nulo!!");

                var publication = await db.Publication.Include(p => p.Person)
                                                      .Where(p => !p.DeletedDate.HasValue) 
                                                      .Where(p => !p.Person.DeletedDate.HasValue)
                                                      .SingleOrDefaultAsync(p => p.Id == message.Id);

                if (publication == null) throw new HttpException(404, "Publicação não encontrada!!");

                return new Result
                {
                    Id = publication.Id,
                    CreatedDate = publication.CreatedDate,
                    CreatedDateInstagram = publication.CreatedDateInstagram,
                    ImageLowResolution = publication.ImageLowResolution,
                    ImageStandardResolution = publication.ImageStandardResolution,
                    ImageThumbnail = publication.ImageThumbnail,
                    IsHighlight = publication.IsHighlight,
                    Link = publication.Link,
                    Subtitle = publication.Subtitle,
                    Person = new Result.PersonResult
                    {
                        Id = publication.Person.Id,
                        CreatedDate = publication.Person.CreatedDate,
                        FullNameInstagram = publication.Person.FullNameInstagram,
                        PhoneNumber = publication.Person.PhoneNumber,
                        ProfilePictureInstagram = publication.Person.ProfilePictureInstagram,
                        UsernameInstagram = publication.Person.UsernameInstagram
                    }
                };
            }
        }
    }
}
