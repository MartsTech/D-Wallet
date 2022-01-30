using Domain.ValueObjects;

namespace Application.Services;

public interface ICurrencyExchange
{
    Task<Money> Convert(Money originalAmount, Currency destinationCurrency);
}
