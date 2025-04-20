using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TestValidationHeaders.Api.Extensions;

namespace TestValidationHeaders.Api.Filter.Services;

public sealed class UcidFilterService(ILogger<UcidFilterService> logger, IValidator<UcidHeaderValue> validator)
    : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var ucid = context.HttpContext.GetUcidOrDefault();
        var validationResult = await validator.ValidateAsync(ucid);

        if (!validationResult.IsValid)
        {
            context.Result = new BadRequestObjectResult(new ProblemDetails
            {
                Type = "INVALID_UCID_HEADER",
                Title = "Ucid Header is invalid",
                Status = StatusCodes.Status400BadRequest,
                Detail = $"Ucid Header ({Constants.UcidHeader}) is invalid or missing.",
                Instance = null,
                Extensions =
                    validationResult.Errors.ToDictionary<ValidationFailure, string, object?>(x => x.PropertyName,
                        x => x.ErrorMessage)
            });

            logger.LogWarning("Ucid header is invalid.");
            return;
        }

        await next();
    }
}