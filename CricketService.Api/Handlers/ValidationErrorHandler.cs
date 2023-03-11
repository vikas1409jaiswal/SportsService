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

    public UnprocessableEntityObjectResult HandleError(ActionContext actionContext)
    {
        var errorModelStates = actionContext.ModelState.Where(kvp => kvp.Value?.Errors.Any() ?? false);
        var parametersString = string.Join(";", errorModelStates.Select(kvp => kvp.Key));
        var errorsString = string.Join(";", errorModelStates
                                                .SelectMany(kvp => kvp.Value!.Errors)
                                                .Where(e => (e.Exception != null || !string.IsNullOrEmpty(e.ErrorMessage)))
                                                .Select(e => e.Exception?.Message ?? e.ErrorMessage));

        var problemDetails = new ValidationProblemDetails(actionContext.ModelState)
        {
            Type = ProblemDetailsType,
            Title = ProblemDetailsTitle,
            Status = StatusCodes.Status422UnprocessableEntity,
        };

        foreach (var kvp in actionContext.ModelState)
        {
            if (kvp.Value.Errors.Any())
            {
                problemDetails.Errors[kvp.Key] = kvp.Value.Errors
                                                    .Select(e => e.Exception?.Message ?? e.ErrorMessage)
                                                    .ToArray();
            }
        }

        problemDetails.Extensions.Add(
            ProblemDetailsExtensionsKeyTraceIdentifier,
            actionContext.HttpContext.TraceIdentifier);

        foreach (var routeValue in actionContext.ActionDescriptor.RouteValues)
        {
            problemDetails.Extensions.Add(routeValue.Key, routeValue.Value);
        }

        return new UnprocessableEntityObjectResult(problemDetails)
        {
            ContentTypes = { MediaTypeNameApplicationProblemJson },
        };
    }
}
