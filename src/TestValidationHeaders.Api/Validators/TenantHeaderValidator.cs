using FluentValidation;
using TestValidationHeaders.Api.Filter;

namespace TestValidationHeaders.Api.Validators;

public sealed class TenantHeaderValidator : AbstractValidator<TenantHeaderValue>
{
    public TenantHeaderValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty()
            .MinimumLength(2);
    }
}