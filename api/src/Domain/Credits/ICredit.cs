using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Credits
{
    using Domain.ValueObjects;

    public interface ICredit
    {
        CreditId CreditId { get; }

        Money Amount { get; }
    }
}
