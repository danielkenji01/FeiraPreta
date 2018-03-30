using FeiraPreta.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeiraPreta.Features.EventScore
{
    public class AllData
    {
        public class Query : IRequest<Result>
        {
            public Query()
            {

            }
        }

        public class Result
        {
            public int TotalVotes { get; set; }

            public int TotalVotesOne { get; set; }

            public int TotalVotesTwo { get; set; }

            public int TotalVotesThree { get; set; }

            public int TotalVotesFour { get; set; }

            public int TotalVotesFive { get; set; }

            public int TotalVotesZero { get; set; }

            public double Average { get; set; }
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
                if (db.EventScore.Count() == 0) return new Result { Average = 0, TotalVotes = 0 };

                var votes = db.EventScore.ToList();

                var average = db.EventScore.Average(e => e.Value);

                return new Result
                {
                    Average = Math.Round(average, 2),
                    TotalVotes = votes.Count(),
                    TotalVotesFive = votes.Where(v => v.Value == 5.0).Count(),
                    TotalVotesFour = votes.Where(v => v.Value == 4.0).Count(),
                    TotalVotesOne = votes.Where(v => v.Value == 1.0).Count(),
                    TotalVotesThree = votes.Where(v => v.Value == 3.0).Count(),
                    TotalVotesTwo = votes.Where(v => v.Value == 2.0).Count(),
                    TotalVotesZero = votes.Where(v => v.Value == 0.0).Count()
                };
            }
        }
    }
}
