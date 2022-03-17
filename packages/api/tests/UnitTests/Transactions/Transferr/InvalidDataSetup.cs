namespace UnitTests.Transactions.Transferr;

using Xunit;

internal sealed class InvalidDataSetup : TheoryData<decimal>
{
    public InvalidDataSetup()
    {
        Add(-100m);
        Add(-10m);
        Add(-300m);
    }
}