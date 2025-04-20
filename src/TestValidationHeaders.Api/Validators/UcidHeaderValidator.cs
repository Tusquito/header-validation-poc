using FluentValidation;
using TestValidationHeaders.Api.Filter;

namespace TestValidationHeaders.Api.Validators;

public sealed class UcidHeaderValidator : AbstractValidator<UcidHeaderValue>
{
    public UcidHeaderValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty()
            .Length(12);
    }
}