using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeiraPreta.Features.Publication
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    public class PublicationController : Controller
    {
        private IMediator mediator;

        public PublicationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Create.Command command)
        {
            await mediator.Send(command);

            return Ok();
        }

        [HttpGet]
        public async Task<IList<List.Result>> List()
        {
            return await mediator.Send(new List.Query());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(Delete.Command command)
        {
            await mediator.Send(command);

            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Read.Result> Read(Read.Query query)
        {
            return await mediator.Send(query);
        }

        [HttpGet]
        [Route("search/{search}")]
        public async Task<IList<ListByTag.Result>> ListByTag(string search)
        {
            return await mediator.Send(new ListByTag.Query(search));
        }
    }
}
