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

        [HttpPost]
        public async Task<IActionResult> Vote([FromBody] Vote.Command command)
        {
            await mediator.Send(command);

            return Ok();
        }

        [HttpGet]
        [Route("average")]
        public async Task<Average.Result> Average()
        {
            return await mediator.Send(new Average.Query());
        }

        [HttpGet]
        [Route("allData")]
        public async Task<AllData.Result> AllData()
        {
            return await mediator.Send(new AllData.Query());
        }
    }
}
