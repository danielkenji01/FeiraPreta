using System;

namespace FeiraPreta.Infraestructure
{
    public class HttpException : Exception
    {
        public int StatusCode { get; set; }

        public dynamic Body { get; set; }

        public HttpException(int statusCode)
        {
            this.StatusCode = statusCode;
        }

        public HttpException(int statusCode, dynamic body)
        {
            this.StatusCode = statusCode;
            this.Body = body;
        }
    }

    /// <summary>
    /// 404 status code, resource not found
    /// </summary>
    public class NotFoundException : HttpException
    {
        public NotFoundException() : base(404)
        {
        }
    }

    /// <summary>
    /// 409 status code, resource already exists
    /// </summary>
    public class ConflictException : HttpException
    {
        public ConflictException() : base(409)
        {
        }
    }
}