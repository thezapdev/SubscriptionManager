using FluentValidation;
using SubscriptionManager.Application.DTOs;

namespace SubscriptionManager.Application.Validators;

public class CreateSubscriptionDtoValidator : AbstractValidator<CreateSubscriptionDto>
{
    public CreateSubscriptionDtoValidator()
    {
        RuleFor(s => s.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(30)
            .WithMessage("Name cannot exceed 30 characters");

        RuleFor(s => s.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0");

        RuleFor(s => s.Currency)
            .Matches("^(PLN|EUR|USD)$")
            .WithMessage("Allowed currencies are: PLN, EUR, USD");

        RuleFor(s => s.BillingCycle)
            .IsInEnum()
            .WithMessage("The billing cycle is not valid");

        RuleFor(s => s.StartDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30)))
            .WithMessage("Start date cannot be older than 30 days");
    }
}
