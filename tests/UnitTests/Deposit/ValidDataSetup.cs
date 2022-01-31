namespace UnitTests.Deposit;

using Xunit;

internal sealed class ValidDataSetup : TheoryData<decimal>
{
    public ValidDataSetup()
    {
        Add(100);
        Add(200);
    }
}
