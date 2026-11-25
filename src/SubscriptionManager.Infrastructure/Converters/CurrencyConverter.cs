using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SubscriptionManager.Domain.ValueObjects;

public class EmailConverter : ValueConverter<Email, string>
{
    public EmailConverter()
        : base(e => e.Value, value => Email.Create(value))
    {
    }
}