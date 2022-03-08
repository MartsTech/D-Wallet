using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

using Domain.ValueObjects;

public interface ICurrencyExchange
{
    Task<Money> Convert(Money originalAmount, Currency destinationCurrency);
}
