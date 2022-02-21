using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Debits
{
    using Domain.ValueObjects;

    public interface IDebit
    {
        DebitId DebitId { get; }

        Money Amount { get; }
    }
}
