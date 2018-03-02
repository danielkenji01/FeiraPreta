using FeiraPreta.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeiraPreta.Features.Publication
{
    public class List
    {
        public class Query : IRequest<IList<Result>>
        {
            public Query()
            {

            }
        }

        public class Result
        {
            public Guid Id { get; set; }

            public string ImageLowResolution { get; set; }

            public string ImageThumbnail { get; set; }

            public string ImageStandardResolution { get; set; }

            public bool IsHighlight { get; set; }

            public string Subtitle { get; set; }
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
                               .Where(p => p.IsHighlight)
                               .Select(p => new Result
                               {
                                   Id = p.Id,
                                   ImageLowResolution = p.ImageLowResolution,
                                   ImageStandardResolution = p.ImageStandardResolution,
                                   ImageThumbnail = p.ImageThumbnail,
                                   IsHighlight = p.IsHighlight,
                                   Subtitle = p.Subtitle
                               }).ToListAsync();
            }
        }
    }
}
