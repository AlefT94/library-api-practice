using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LibraryApi.Api.Controllers;

public class ApiExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is InvalidOperationException || context.Exception is ArgumentException)
        {
            context.Result = new BadRequestObjectResult(new
            {
                mensagem = context.Exception.Message
            });
            context.ExceptionHandled = true;
        }
    }
}
