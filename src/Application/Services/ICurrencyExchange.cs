namespace Application.Services;

using Domain.ValueObjects;

public interface ICurrencyExchange
{
    Task<Money> Convert(Money originalAmount, Currency destinationCurrency);
}
