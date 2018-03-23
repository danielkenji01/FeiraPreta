using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeiraPreta.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FeiraPreta.Features.Publication
{
    public class Paginate
    {
        public class Query : IRequest<IList<Result>>
        {
            public int Pages { get; set; }
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
            public class TotalPages
            {
                public int TotalPage { get; set; }
            }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IList<Result>>
        {
            private readonly Db db;

            public QueryHandler(Db db)
            {
                this.db = db;
            }

            public async Task<IList<Result>> Handle(Query message)
            {
                int start, end = 0;
                
                start = ((message.Pages-1)*18);

                var publi = db.Publication.ToList();
                

                return await db.Publication
                            .Where(p => !p.DeletedDate.HasValue)
                            .Skip(start)
                            .Take(18)
                            .Select(p => new Result
                            {
                                Id = p.Id,
                                CreatedDate = p.CreatedDate,
                                CreatedDateInstagram = p.CreatedDateInstagram,
                                ImageLowResolution = p.ImageLowResolution,
                                ImageStandardResolution = p.ImageStandardResolution,
                                ImageThumbnail = p.ImageThumbnail,
                                IsHighlight = p.IsHighlight,
                                Link = p.Link,
                                Subtitle = p.Subtitle,
                                PublicationPerson = new Result.Person
                                {
                                    Id = p.Person.Id,
                                    FullName = p.Person.FullNameInstagram,
                                    ProfilePicture = p.Person.ProfilePictureInstagram,
                                    Telefone = p.Person.PhoneNumber,
                                    Username = p.Person.UsernameInstagram
                                }
                            }).ToListAsync();
                
            }
        }
    }
}