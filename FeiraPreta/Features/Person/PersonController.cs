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

        JsonResult jr;

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

        [HttpGet]
        public async Task<IList<List.Result>> List()
        {
            return await mediator.Send(new List.Query());
        }
    }
}
