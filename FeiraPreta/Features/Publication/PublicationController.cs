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
        public async Task<Create.Result> Create([FromBody] Create.Command command)
        {
            return await mediator.Send(command);
        }

        [HttpPost]
        [Route("highlight")]
        public async Task<Highlight.Result> Highlight([FromBody] Highlight.Command command)
        {
            return await mediator.Send(command);
        }

        [HttpGet]
        public async Task<IList<List.Result>> List()
        {
            return await mediator.Send(new List.Query());
        }

        [HttpGet]
        [Route("all")]
        public async Task<IList<ListAll.Result>> ListAll()
        {
            return await mediator.Send(new ListAll.Query());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<Delete.Result> Delete(Delete.Command command)
        {
            return await mediator.Send(command);
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
