using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Debits
{
    using Domain.ValueObjects;

    public sealed class DebitNull : IDebit
    {
        public DebitId DebitId { get; } = new(Guid.Empty);

        public Money Amount { get; } = new(0, new Currency(string.Empty));

        public static DebitNull Instance { get; } = new();
    }
}
