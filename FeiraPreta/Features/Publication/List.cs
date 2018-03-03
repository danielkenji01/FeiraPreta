﻿using FeiraPreta.Infrastructure;
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

            public string Link { get; set; }

            public PersonResult Person { get; set; }

            public class PersonResult
            {
                public Guid Id { get; set; }

                public string UsernameInstagram { get; set; }

                public string FullNameInstagram { get; set; }
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
                               .Where(p => p.IsHighlight)
                               .Select(p => new Result
                               {
                                   Id = p.Id,
                                   ImageLowResolution = p.ImageLowResolution,
                                   ImageStandardResolution = p.ImageStandardResolution,
                                   ImageThumbnail = p.ImageThumbnail,
                                   IsHighlight = p.IsHighlight,
                                   Subtitle = p.Subtitle,
                                   Link = p.Link,
                                   Person = new Result.PersonResult
                                   {
                                       Id = p.Person.Id,
                                       FullNameInstagram = p.Person.FullNameInstagram,
                                       UsernameInstagram = p.Person.UsernameInstagram
                                   }
                               }).ToListAsync();
            }
        }
    }
}