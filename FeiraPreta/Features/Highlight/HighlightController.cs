using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeiraPreta.Features.Highlight
{
    [Route("api/[controller]")]
    public class HighlightController : Controller
    {
        private IMediator mediator;

        public HighlightController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Create.Command command)
        {
            await mediator.Send(command);

            return Ok();
        }
    } 
}
