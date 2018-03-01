using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeiraPreta.Features.Highlight
{
    public class Create
    {
        public class Command : IRequest
        {
            
        }

        public class Handler : IAsyncRequestHandler<Command>
        {
            public Task Handle(Command message)
            {
                throw new NotImplementedException();
            }
        }
    }
}
