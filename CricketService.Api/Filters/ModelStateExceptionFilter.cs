using CricketService.Api.Handlers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CricketService.Api.Filters;

public class ModelStateExceptionFilter : IExceptionFilter
{
    private readonly ValidationErrorHandler validationErrorHandler;

    public ModelStateExceptionFilter(ValidationErrorHandler validationErrorHandler)
    {
        this.validationErrorHandler = validationErrorHandler;
    }

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is CricketModelValidationException)
        {
            context.ModelState.AddModelError((context.Exception as CricketModelValidationException)!.ExceptionKey, context.Exception.Message);
            context.Result = validationErrorHandler.HandleError(context);
        }
    }
}
