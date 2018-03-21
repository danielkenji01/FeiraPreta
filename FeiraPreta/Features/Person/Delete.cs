using FeiraPreta.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeiraPreta.Features.Person
{
    public class Delete
    {
        public class Command : IRequest<Result>
        {
            public Guid Id { get; set; }
        }

        public class Result
        {
            public int StatusCode { get; set; }

            public string Message { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Command, Result>
        {
            private readonly Db db;

            public Handler(Db db)
            {
                this.db = db;
            }

            public async Task<Result> Handle(Command message)
            {
                var person = await db.Person.SingleOrDefaultAsync(p => p.Id == message.Id);

                if (person == null || person.DeletedDate.HasValue) return new Result { Message = "Empreendedor não existente", StatusCode = 404};

                person.DeletedDate = DateTime.Now;

                await db.SaveChangesAsync();

                return new Result { StatusCode = 200, Message = "Empreendedor deletado com sucesso" };
            }
        }
    }
}
