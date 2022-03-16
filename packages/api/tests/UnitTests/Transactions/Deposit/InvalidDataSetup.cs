namespace UnitTests.Transactions.Deposit;

using Xunit;

internal sealed class InvalidDataSetup : TheoryData<decimal>
{
    public InvalidDataSetup()
    {
        Add(-900m);
        Add(-530m);
        Add(-746m);
    }
}