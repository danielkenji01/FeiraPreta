using FeiraPreta.Infraestructure;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeiraPreta.Features.EventScore
{
    public class Vote
    {
        public class Command : IRequest<Result>
        {
            public double Value { get; set; }
        }

        public class Result
        {
            public int StatusCode { get; set; }

            public string Message { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(v => v.Value).NotNull().WithMessage("Voto não pode ser nulo")
                                     .NotEmpty().WithMessage("Voto não pode ser nulo");
            }
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
                if (message.Value < 0 || message.Value > 5) return new Result { Message = "Voto inválido", StatusCode = 400 };

                var eventScore = new Domain.EventScore
                {
                    Value = message.Value,
                    CreatedDate = DateTime.Now
                };

                db.EventScore.Add(eventScore);

                await db.SaveChangesAsync();

                return new Result { Message = "Voto concluído com sucesso", StatusCode = 201 };
            }
        }
    }
}
