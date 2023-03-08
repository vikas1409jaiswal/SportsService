using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CricketService.Api.Handlers;

public class ValidationErrorHandler
{
    private const string MediaTypeNameApplicationProblemJson = "application/problem+json";
    private const string ProblemDetailsExtensionsKeyTraceIdentifier = "trace_id";
    private const string ProblemDetailsTitle = "One or more validation errors occurred.";
    private const string ProblemDetailsType = "errors:cricket_dekho:validation_failed";
    private const string DefaultErrorValue = "validation error";

    public UnprocessableEntityObjectResult HandleError(ExceptionContext exceptionContext)
    {
        Dictionary<string, string[]> errorDictionary = new();

        errorDictionary.Add((exceptionContext.Exception as CricketModelValidationException)!.ExceptionKey, new string[] { exceptionContext.Exception.Message });

        var problemDetails = new ValidationProblemDetails(errorDictionary)
        {
            Type = ProblemDetailsType,
            Title = ProblemDetailsTitle,
            Status = StatusCodes.Status422UnprocessableEntity,
        };

        return new UnprocessableEntityObjectResult(problemDetails)
        {
            ContentTypes = { MediaTypeNameApplicationProblemJson },
        };
    }
}
