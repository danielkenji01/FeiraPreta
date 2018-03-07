using FeiraPreta.Infraestructure;
using FeiraPreta.Infrastructure;
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
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Command>
        {
            private readonly Db db;

            public Handler(Db db)
            {
                this.db = db;
            }

            public async Task Handle(Command message)
            {
                var person = await db.Person.SingleOrDefaultAsync(p => p.Id == message.Id);

                if (person == null || person.DeletedDate.HasValue) throw new NotFoundException();

                person.DeletedDate = DateTime.Now;

                await db.SaveChangesAsync();
            }
        }
    }
}
