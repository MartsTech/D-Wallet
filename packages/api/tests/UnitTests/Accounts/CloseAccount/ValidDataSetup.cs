namespace UnitTests.Accounts.CloseAccount;

using Xunit;

internal sealed class ValidDataSetup : TheoryData<decimal>
{
    public ValidDataSetup()
    {
        Add(50M);
        Add(100M);
        Add(200M);
    }
}