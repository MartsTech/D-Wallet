namespace Infrastructure.CurrencyExchange;

using Application.Services;
using Domain.ValueObjects;

public sealed class CurrencyExchangeFake : ICurrencyExchange
{
    private readonly Dictionary<Currency, decimal> _eurRates = new()
    {
        { Currency.Euro, 1m },
        { Currency.Dollar, 1.091202m },
        { Currency.Lev, 1.95067m },
        { Currency.Pound, 0.836908m },
        { Currency.Canadian, 1.392724m },
        { Currency.Real, 5.537896m },
        { Currency.Krona, 10.636554m },
    };

    public Task<Money> Convert(Money originalAmount, Currency destinationCurrency)
    {
        decimal eurAmount = _eurRates[originalAmount.Currency] / originalAmount.Amount;

        decimal destinationAmount = _eurRates[destinationCurrency] / eurAmount;

        return Task.FromResult(new Money(destinationAmount, destinationCurrency));
    }
}