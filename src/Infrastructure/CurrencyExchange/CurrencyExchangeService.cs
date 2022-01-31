namespace Infrastructure.CurrencyExchange;

using Application.Services;
using Domain.ValueObjects;
using Newtonsoft.Json.Linq;

public sealed class CurrencyExchangeService : ICurrencyExchange
{
    public const string HttpClientName = "Fixer";

    private const string _exchangeUrl = "https://api.exchangeratesapi.io/latest?base=USD";

    private readonly IHttpClientFactory _httpClientFactory;

    private readonly Dictionary<Currency, decimal> _usdRates = new();

    public CurrencyExchangeService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Money> Convert(Money originalAmount, Currency destinationCurrency)
    {
        HttpClient httpClient = _httpClientFactory.CreateClient(HttpClientName);

        Uri requestUri = new(_exchangeUrl);

        HttpResponseMessage response = await httpClient.GetAsync(requestUri)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        string responseJson = await response
            .Content
            .ReadAsStringAsync()
            .ConfigureAwait(false);

        ParseCurrencies(responseJson);

        decimal usdAmount = _usdRates[originalAmount.Currency] / originalAmount.Amount;

        decimal destinationAmount = _usdRates[destinationCurrency] / usdAmount;

        return new Money(destinationAmount, destinationCurrency);
    }

    private void ParseCurrencies(string responseJson)
    {
        JObject rates = JObject.Parse(responseJson);

        decimal eur = rates["rates"]![Currency.Euro.Code]!.Value<decimal>();
        decimal cad = rates["rates"]![Currency.Canadian.Code]!.Value<decimal>();
        decimal gbh = rates["rates"]![Currency.BritishPound.Code]!.Value<decimal>();
        decimal sek = rates["rates"]![Currency.Krona.Code]!.Value<decimal>();
        decimal brl = rates["rates"]![Currency.Real.Code]!.Value<decimal>();

        _usdRates.Add(Currency.Dollar, 1);
        _usdRates.Add(Currency.Euro, eur);
        _usdRates.Add(Currency.Canadian, cad);
        _usdRates.Add(Currency.BritishPound, gbh);
        _usdRates.Add(Currency.Krona, sek);
        _usdRates.Add(Currency.Real, brl);
    }
}
