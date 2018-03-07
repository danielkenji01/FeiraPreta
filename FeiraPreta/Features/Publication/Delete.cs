using FeiraPreta.Infraestructure;
using FeiraPreta.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeiraPreta.Features.Publication
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
                var publication = await db.Publication.SingleOrDefaultAsync(p => p.Id == message.Id);

                if (publication == null || publication.DeletedDate.HasValue) throw new NotFoundException();

                publication.DeletedDate = DateTime.Now;

                await db.SaveChangesAsync();
            }
        }
    }
}
