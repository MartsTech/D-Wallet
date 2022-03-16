namespace UnitTests.Transactions.Transferr;

using Xunit;

internal sealed class InvalidDataSetup : TheoryData<decimal>
{
    public InvalidDataSetup()
    {
        Add(-100);
        Add(-10);
        Add(-300);
    }
}