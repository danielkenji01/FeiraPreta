using FeiraPreta.Infraestructure;
using FeiraPreta.Infrastructure;
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

            public Person PublicationPerson { get; set; }

            public class Person
            {
                public Guid Id { get; set; }

                public string Username { get; set; }

                public string FullName { get; set; }

                public string Telefone { get; set; }

                public string ProfilePicture { get; set; }
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

                var publication = await db.Publication.Include(p => p.Person).Where(p => !p.DeletedDate.HasValue || !p.Person.DeletedDate.HasValue).SingleOrDefaultAsync(p => p.Id == message.Id);

                if (publication == null) throw new NotFoundException();

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
                    PublicationPerson = new Result.Person
                    {
                        Id = publication.Person.Id,
                        FullName = publication.Person.FullNameInstagram,
                        ProfilePicture = publication.Person.ProfilePictureInstagram,
                        Telefone = publication.Person.PhoneNumber,
                        Username = publication.Person.UsernameInstagram
                    }
                };
            }
        }
    }
}
