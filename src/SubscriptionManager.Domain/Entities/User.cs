using SubscriptionManager.Domain.Exceptions;
using SubscriptionManager.Domain.ValueObjects;

namespace SubscriptionManager.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public Email Email { get; private set; }
    public Currency PreferredCurrency  { get; private set; }
    public ICollection<Subscription> Subscriptions { get; private set; }

    private User() { } // For EF Core

    public User(Guid id, Email email, Currency preferredCurrency)
    {
        if (id == Guid.Empty)
        {
            throw new DomainException("User id can't be empty.");
        } 
        Id = id;
        Email = email ?? throw new DomainException("Email is required.");
        PreferredCurrency = preferredCurrency ?? Currency.PLN;
    }

    public void ChangePreferredCurrency(Currency currency)
    {
        ArgumentNullException.ThrowIfNull(currency);

        if (PreferredCurrency == currency)
        {
            return;
        }
        PreferredCurrency = currency;
    }
}