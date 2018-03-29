using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeiraPreta.Features.Person
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    public class PersonController : Controller
    {
        private IMediator mediator;

        public PersonController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<Create.Result> Create([FromBody] Create.Command command)
        {
            return await mediator.Send(command);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<Delete.Result> Delete(Delete.Command command)
        {
            return await mediator.Send(command);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Update.Command command)
        {
            await mediator.Send(command);

            return Ok();
        }

        [HttpGet]
        public async Task<IList<List.Result>> List(int page)
        {
            return await mediator.Send(new List.Query(page));
        }
    }
}
