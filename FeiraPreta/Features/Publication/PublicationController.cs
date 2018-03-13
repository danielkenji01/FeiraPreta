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
        JsonResult jr;

        public PublicationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Create.Command command)
        {
            try
            {
                await mediator.Send(command);
                jr = new JsonResult("Usuário criado com sucesso");
                jr.StatusCode = 200;
                jr.ContentType = "application/json";
                return Ok(Json(jr));
            }
            catch (System.Exception ex)
            {
                jr = new JsonResult("Ocorreu um erro");
                jr.StatusCode = 409;
                jr.ContentType = "application/json";
                jr.Value = ex.Message;
                return BadRequest(Json(jr));
            }
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
            try
            {
                await mediator.Send(command);
                jr = new JsonResult("Publicação excluida com sucesso");
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
