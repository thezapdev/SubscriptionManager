using SubscriptionManager.Domain.Enums;

namespace SubscriptionManager.Application.DTOs;

public record SubscriptionDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
    public Currency Currency { get; init; }
    public BillingCycle BillingCycle { get; init; }
    public DateOnly? NextPaymentDate { get; init; }
    public SubscriptionStatus Status { get; init; }
}
