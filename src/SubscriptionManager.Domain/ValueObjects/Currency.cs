public sealed record Currency
{
    public static readonly Currency PLN = new("PLN");
    public static readonly Currency EUR = new("EUR");
    public static readonly Currency USD = new("USD");

    private Currency(string code) => Code = code;
    private static readonly HashSet<string> SupportedCodes = ["PLN", "EUR", "USD"];
    public string Code { get; }
    
    public static Currency FromCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Kod waluty nie może być pusty.", nameof(code));

        var normalized = code.Trim().ToUpperInvariant();

        if (!SupportedCodes.Contains(normalized))
            throw new ArgumentException($"Nieobsługiwana waluta ISO: {code}", nameof(code));

        return new Currency(normalized);
    }

    public override string ToString() => Code;
}