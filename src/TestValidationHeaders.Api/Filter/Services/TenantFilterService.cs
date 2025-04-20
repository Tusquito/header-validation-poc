using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TestValidationHeaders.Api.Extensions;

namespace TestValidationHeaders.Api.Filter.Services;

public sealed class TenantFilterService(ILogger<TenantFilterService> logger, IValidator<TenantHeaderValue> validator)
    : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var tenant = context.HttpContext.GetTenantOrDefault();
        var validationResult = await validator.ValidateAsync(tenant);
        
        if (!validationResult.IsValid)
        {
            context.Result = new BadRequestObjectResult(new ProblemDetails
            {
                Type = "INVALID_TENANT_HEADER",
                Title = "Tenant Header is invalid",
                Status = StatusCodes.Status400BadRequest,
                Detail = $"Tenant header value ({Constants.TenantHeader}) is invalid or missing.",
                Instance = null,
                Extensions = validationResult.Errors.ToDictionary<ValidationFailure, string, object?>(x => x.PropertyName, x => x.ErrorMessage)
            });

            logger.LogWarning("Tenant header is invalid.");
            return;
        }

        await next();
    }
}