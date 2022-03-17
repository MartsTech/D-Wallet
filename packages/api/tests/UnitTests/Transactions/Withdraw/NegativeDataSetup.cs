namespace UnitTests.Transactions.Withdraw;

using Xunit;

internal sealed class NegativeDataSetup : TheoryData<decimal>
{
    public NegativeDataSetup()
    {
        Add(-900m);
        Add(-530m);
        Add(-746m);
    }
}
