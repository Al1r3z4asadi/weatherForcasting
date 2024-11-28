using System.Net;
using System.Text.Json;
using weather.Api.Models;

namespace weather.Api.Middleware
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
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

                switch (error)
                {
                    case NullReferenceException e:
                        statusCode = HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        statusCode = HttpStatusCode.NotFound;
                        break;
                }

                var responseModel = new ErrorResponse(error?.Message, context.Request.Path , statusCode);
                var result = JsonSerializer.Serialize(responseModel);

                response.StatusCode = (int)statusCode;
                await response.WriteAsync(result);
            }
        }
    }
}
