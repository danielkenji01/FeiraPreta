using FeiraPreta.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FeiraPreta.Features.Person
{
    public class Update
    {
        public class Command : IRequest<Result>
        {
            public Guid Id { get; set; }
            public string PhoneNumber { get; set; }

            public string UserNameInstagram { get; set; }
        }

        public class Result
        {
            public int StatusCode { get; set; }

            public string Message { get; set; }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, Result>
        {
            private readonly Db db;

            public CommandHandler(Db db)
            {
                this.db = db;
            }

            public async Task<Result> Handle(Command message)
            {
                var person = await db.Person.SingleOrDefaultAsync(p => p.Id == message.Id);

                if (person == null || person.DeletedDate.HasValue) return new Result { Message = "Empreendedor não existente", StatusCode = 404};

                person.UpdatedDate = DateTime.Now;
                person.PhoneNumber = message.PhoneNumber;
                person.UsernameInstagram = message.UserNameInstagram;

                db.Person.Update(person);
                await db.SaveChangesAsync();

                return new Result { StatusCode = 200, Message = "Empreendedor atualizado com sucesso!!" };
            }

        }
    }
}
