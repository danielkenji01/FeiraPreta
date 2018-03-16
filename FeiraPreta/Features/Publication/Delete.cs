using FeiraPreta.Infraestructure;
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
                var publication = await db.Publication.SingleOrDefaultAsync(p => p.Id == message.Id);

                if (publication == null || publication.DeletedDate.HasValue) return new Result { Message = "Publicação não existe", StatusCode = 404 };

                publication.DeletedDate = DateTime.Now;

                await db.SaveChangesAsync();

                return new Result { StatusCode = 200, Message = "Publicação deletada com sucesso!!!!" };
            }
        }
    }
}
