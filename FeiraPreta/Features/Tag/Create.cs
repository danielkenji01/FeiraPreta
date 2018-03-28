using FeiraPreta.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeiraPreta.Features.Tag
{
    public class Create
    {
        public class Command : IRequest
        {
            public string Nome { get; set; }

            public Guid PublicationId { get; set; }
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
                var tag = await db.Tag.SingleOrDefaultAsync(t => t.Nome == message.Nome);

                if (tag == null)
                {
                    tag = new Domain.Tag
                    {
                        Nome = message.Nome
                    };
                }
                
                db.Tag.Add(tag);
                db.SaveChanges();

                var publication_tag = new Domain.Publication_Tag
                {
                    PublicationId = message.PublicationId,
                    TagId = tag.Id
                };

                db.Publication_Tag.Add(publication_tag);
                db.SaveChanges();
            }
        }
    }
}
