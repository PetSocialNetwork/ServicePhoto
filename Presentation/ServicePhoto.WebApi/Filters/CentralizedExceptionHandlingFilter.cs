using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ServicePhoto.WebApi.Models.Responses;
using ServicePhoto.Domain.Exceptions;
namespace ServicePhoto.WebApi.Filters
{
    public class CentralizedExceptionHandlingFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var (message, statusCode) = TryGetUserMessageFromException(context);

            if (message != null && statusCode != 0)
            {
                context.Result = new ObjectResult(new ErrorResponse(message, statusCode))
                {
                    StatusCode = statusCode
                };
                context.ExceptionHandled = true;
            }
        }

        private (string?, int) TryGetUserMessageFromException(ExceptionContext context)
        {
            return context.Exception switch
            {
                PhotoNotFoundException => ("Фотография не найдена.", StatusCodes.Status400BadRequest),
                Exception => ("Неизвестная ошибка.", StatusCodes.Status500InternalServerError),
                _ => (null, 0)
            };
        }
    }
}
