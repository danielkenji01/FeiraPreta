using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeiraPreta.Features.EventScore
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    public class EventScoreController : Controller
    {
        private IMediator mediator;

        public EventScoreController(IMediator mediator)
        {
            this.mediator = mediator;
        }

    }
}
