namespace Infrastructure.CurrencyExchange;

using Application.Services;
using Domain.ValueObjects;
using Newtonsoft.Json.Linq;

public sealed class CurrencyExchangeService : ICurrencyExchange
{
    public const string HttpClientName = "Fixer";

    private const string _exchangeUrl = "http://data.fixer.io/api/latest";

    private readonly IHttpClientFactory _httpClientFactory;

    private readonly Dictionary<Currency, decimal> _eurRates = new();

    public CurrencyExchangeService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Money> Convert(Money originalAmount, Currency destinationCurrency)
    {
        HttpClient httpClient = _httpClientFactory.CreateClient(HttpClientName);

        string? access_key = Environment.GetEnvironmentVariable("FIXER_ACCESS_KEY");

        Uri requestUri = new($"{_exchangeUrl}?access_key={access_key}");

        HttpResponseMessage response = await httpClient.GetAsync(requestUri)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        string responseJson = await response
            .Content
            .ReadAsStringAsync()
            .ConfigureAwait(false);

        ParseCurrencies(responseJson);

        decimal eurAmount = _eurRates[originalAmount.Currency] / originalAmount.Amount;

        decimal destinationAmount = _eurRates[destinationCurrency] / eurAmount;

        return new Money(destinationAmount, destinationCurrency);
    }

    private void ParseCurrencies(string responseJson)
    {
        JObject rates = JObject.Parse(responseJson);

        decimal usd = rates["rates"]![Currency.Dollar.Code]!.Value<decimal>();
        decimal bgn = rates["rates"]![Currency.Lev.Code]!.Value<decimal>();
        decimal cad = rates["rates"]![Currency.Canadian.Code]!.Value<decimal>();
        decimal gbh = rates["rates"]![Currency.Pound.Code]!.Value<decimal>();
        decimal sek = rates["rates"]![Currency.Krona.Code]!.Value<decimal>();
        decimal brl = rates["rates"]![Currency.Real.Code]!.Value<decimal>();

        _eurRates.Add(Currency.Euro, 1);
        _eurRates.Add(Currency.Dollar, usd);
        _eurRates.Add(Currency.Lev, bgn);
        _eurRates.Add(Currency.Canadian, cad);
        _eurRates.Add(Currency.Pound, gbh);
        _eurRates.Add(Currency.Krona, sek);
        _eurRates.Add(Currency.Real, brl);
    }
}