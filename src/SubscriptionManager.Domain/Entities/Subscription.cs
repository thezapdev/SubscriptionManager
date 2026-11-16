using SubscriptionManager.Domain.Enums;
using SubscriptionManager.Domain.Exceptions;

namespace SubscriptionManager.Domain.Entities;

public class Subscription
{
public Guid Id { get; private set; }
public string Name { get; private set; }
public decimal Price { get; private set; }
public BillingCycle BillingCycle { get; private set; }
public DateOnly NextPaymentDate { get; private set; }
public SubscriptionStatus Status { get; private set; }

private Subscription() { } // For EF Core

    public Subscription(string name, decimal price, BillingCycle billingCycle, DateOnly nextPaymentDate)
 {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Subscription name cannot be empty.");

        if (price < 0)
            throw new DomainException("Price cannot be negative.");

        Id = Guid.NewGuid();
        Name = name.Trim();
        Price = price;
        BillingCycle = billingCycle;
        NextPaymentDate = nextPaymentDate;
        Status = SubscriptionStatus.Active;
 }

    public void Cancel()
{
        if (Status != SubscriptionStatus.Active)
            throw new InvalidSubscriptionStateException("Only active subscription can be cancelled.");
        Status = SubscriptionStatus.Cancelled;
}
public void Renew()
{
        if (Status != SubscriptionStatus.Active)
            throw new InvalidSubscriptionStateException("Only active subscription can be renewed.");
        NextPaymentDate = CalculateNext(NextPaymentDate);

}
public void Resume(DateOnly nextPaymentDate)
{
        if (Status != SubscriptionStatus.Cancelled)
            throw new InvalidSubscriptionStateException("Only cancelled subscription can be resumed.");

        NextPaymentDate = nextPaymentDate;
        Status = SubscriptionStatus.Active;
}

 private DateOnly CalculateNext(DateOnly from) => BillingCycle switch
  {
        BillingCycle.Monthly => from.AddMonths(1),
        BillingCycle.Yearly => from.AddYears(1),
        _ => throw new DomainException("Unsupported billing cycle.")
  };

}