using FeiraPreta.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeiraPreta.Features.Publication
{
    public class ListByTag
    {
        public class Query : IRequest<IList<Result>>
        {
            public string Tag { get; set; }

            public Query()
            {

            }

            public Query(string Tag)
            {
                this.Tag = Tag;
            }
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

                public Person PersonResult { get; set; }

                public class Person
                {
                    public Guid Id { get; set; }

                    public string UsernameInstagram { get; set; }

                    public string FullNameInstagram { get; set; }

                    public string ProfilePictureInstagram { get; set; }

                    public DateTime CreatedDate { get; set; }

                    public string PhoneNumber { get; set; }
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
                return await db.Publication
                    .Include(p => p.Person)
                    .Include(p => p.Publication_Tags)
                    .ThenInclude(pt => pt.Tag)
                    .Where(p => !p.DeletedDate.HasValue || !p.Person.DeletedDate.HasValue)
                    .SelectMany(p => p.Publication_Tags.Where(pt => pt.Tag.Nome == message.Tag).Select(pub => new Result
                    {
                        Id = pub.Publication.Id,
                        CreatedDate = pub.Publication.CreatedDate,
                        CreatedDateInstagram = pub.Publication.CreatedDateInstagram,
                        ImageLowResolution = pub.Publication.ImageLowResolution,
                        ImageStandardResolution = pub.Publication.ImageStandardResolution,
                        ImageThumbnail = pub.Publication.ImageThumbnail,
                        IsHighlight = pub.Publication.IsHighlight,
                        Link = pub.Publication.Link,
                        Subtitle = pub.Publication.Subtitle,
                        PersonResult = new Result.Person
                        {
                            Id = pub.Publication.Person.Id,
                            CreatedDate = pub.Publication.Person.CreatedDate,
                            FullNameInstagram = pub.Publication.Person.FullNameInstagram,
                            PhoneNumber = pub.Publication.Person.PhoneNumber,
                            ProfilePictureInstagram = pub.Publication.Person.ProfilePictureInstagram,
                            UsernameInstagram = pub.Publication.Person.UsernameInstagram
                        }
                    })).ToListAsync();
            }
        }
    }
}
