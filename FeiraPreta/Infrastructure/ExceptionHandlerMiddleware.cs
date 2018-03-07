using FeiraPreta.Infraestructure;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FeiraPreta.Infrastructure
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this._next(context);
            }
            catch (HttpException e)
            {
                context.Response.StatusCode = e.StatusCode;
                if (e.Body != null)
                {
                    // Gambiarra pra retornar o body de erro
                    byte[] byteArray = Encoding.UTF8.GetBytes(e.Body);

                    await context.Response.Body.WriteAsync(byteArray, 0, byteArray.Length);
                    await context.Response.Body.FlushAsync();
                }
            }
        }
    }
}