using System.ComponentModel.DataAnnotations;
using SubscriptionManager.Domain.Enums;

namespace SubscriptionManager.Application.DTOs;

public record CreateSubscriptionDto
{
	[Required (ErrorMessage = "Name is required.")]
	[StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
	public required string Name { get; init; }
	[Required(ErrorMessage = "Price is required.")]
	public decimal Price { get; init; }
    [Required(ErrorMessage = "Billing cycle is required.")]
	[EnumDataType(typeof(BillingCycle), ErrorMessage = "Invalid billing cycle.")]
    public required BillingCycle BillingCycle { get; init; }
	[Required(ErrorMessage = "Start date is required.")]
	public required DateOnly StartDate { get; init; }
    [Required(ErrorMessage = "Currency is required.")]
	[RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Currency must be a valid ISO 4217 code.")]
    public string Currency { get; init; } = string.Empty;
}
