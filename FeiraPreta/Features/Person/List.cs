﻿using FeiraPreta.Infrastructure;
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
        public class Query : IRequest<IList<Result>>
        {
            public Query()
            {

            }
        }

        public class Result
        {
            public Guid Id { get; set; }

            public string UsernameInstagram { get; set; }

            public string FullNameInstagram { get; set; }

            public string ProfilePictureInstagram { get; set; }

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
                return await db.Person.Select(p => new Result
                {
                    Id = p.Id,
                    CreatedDate = p.CreatedDate,
                    FullNameInstagram = p.FullNameInstagram,
                    ProfilePictureInstagram = p.ProfilePictureInstagram,
                    UsernameInstagram = p.UsernameInstagram
                }).ToListAsync();
            }
        }
    }
}