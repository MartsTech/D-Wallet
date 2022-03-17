namespace UnitTests.Accounts.CloseAccount;

using System;
using Xunit;

internal sealed class InvalidDataSetup : TheoryData<Guid>
{
    public InvalidDataSetup()
    {
        Add(Guid.NewGuid());
        Add(Guid.NewGuid());
        Add(Guid.NewGuid());

    }
}
