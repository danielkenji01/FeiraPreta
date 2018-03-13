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
        public async Task<IActionResult> Create([FromBody] Create.Command command)
        {
            try
            {
                await mediator.Send(command);
                jr = new JsonResult("Empreendedor cadastrado com sucesso");
                jr.ContentType = "application/json";
                jr.StatusCode = 200;
                return Ok(Json(jr));
            }
            catch (System.Exception ex)
            {
                jr = new JsonResult("Ocorreu um erro");
                jr.ContentType = "application/json";
                jr.StatusCode = 400;
                jr.Value = ex.Message;
                return NotFound(Json(jr));
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(Delete.Command command)
        {
            try
            {
                await mediator.Send(command);
                jr = new JsonResult("Empreendedor excluido com sucesso");
                jr.ContentType = "application/json";
                jr.StatusCode = 200;
                return Ok(Json(jr));
            }
            catch (System.Exception ex)
            {
                jr = new JsonResult("Ocorreu um erro");
                jr.ContentType = "application/json";
                jr.StatusCode = 400;
                jr.Value = ex.Message;
                return NotFound(Json(jr));
            }
        }

        [HttpGet]
        public async Task<IList<List.Result>> List()
        {
            return await mediator.Send(new List.Query());
        }
    }
}
