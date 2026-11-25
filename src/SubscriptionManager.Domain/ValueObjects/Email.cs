namespace SubscriptionManager.Domain.ValueObjects;

public sealed record Email
{
    public string Value { get; }

    private Email(string value) => Value = value;

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("The email field must not be empty", nameof(value));

        var normalized = value.Trim().ToLowerInvariant();

        if (!normalized.Contains('@') || !normalized.Contains('.'))
            throw new ArgumentException("Incorrect email format", nameof(value));

        return new Email(normalized);
    }

    public override string ToString() => Value;
}