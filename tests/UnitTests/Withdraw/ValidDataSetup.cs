namespace UnitTests.Withdraw;

using Xunit;

internal sealed class ValidDataSetup : TheoryData<decimal, decimal>
{
    public ValidDataSetup()
    {
        Add(100, 400);
    }
}
