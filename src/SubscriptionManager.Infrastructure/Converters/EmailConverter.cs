using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class CurrencyConverter : ValueConverter<Currency, string>
{
    public CurrencyConverter()
        : base(c => c.Code, code => Currency.FromCode(code))
    {
    }
}