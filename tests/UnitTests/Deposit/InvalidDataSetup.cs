namespace UnitTests.Deposit;

using Xunit;

internal sealed class InvalidDataSetup : TheoryData<decimal>
{
    public InvalidDataSetup()
    {
        Add(-100);
        Add(0);
    }
}
