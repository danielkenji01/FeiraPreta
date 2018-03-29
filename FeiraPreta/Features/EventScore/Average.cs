using FeiraPreta.Infraestructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeiraPreta.Features.EventScore
{
    public class Average
    {
        public class Query : IRequest<Result>
        {
            public Query()
            {

            }
        }

        public class Result
        {
            public float Value { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Query, Result>
        {
            private readonly Db db;

            public Handler(Db db)
            {
                this.db = db;
            }

            public async Task<Result> Handle(Query message)
            {
                if (db.EventScore.Count() == 0) return new Result { Value = 0 }; 

                var result = db.EventScore.Average(e => e.Value);

                return new Result
                {
                    Value = (float) Math.Round(result, 0)
                };
            }
        }
    }
}
