using FeiraPreta.Infraestructure;
using FeiraPreta.Infraestructure.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeiraPreta.Features.Person
{
    public class List
    {
        public class Query : PageQuery<IList<Result>>
        {
            public int Page { get; set; }

            public Query()
            {

            }

            public Query(int Page)
            {
                this.Page = Page;
            }
        }

        public class Result
        {
            public Guid Id { get; set; }

            public string UsernameInstagram { get; set; }

            public string FullNameInstagram { get; set; }

            public string ProfilePictureInstagram { get; set; }

            public string PhoneNumber { get; set; }

            public DateTime CreatedDate { get; set; }
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
                return await db.Person
                               .Where(p => !p.DeletedDate.HasValue)
                               .Paginate(message)
                               .Select(p => new Result
                               {
                                   Id = p.Id,
                                   CreatedDate = p.CreatedDate,
                                   FullNameInstagram = p.FullNameInstagram,
                                   ProfilePictureInstagram = p.ProfilePictureInstagram,
                                   UsernameInstagram = p.UsernameInstagram,
                                   PhoneNumber = p.PhoneNumber
                               }).ToListAsync();
            }
        }
    }
}
