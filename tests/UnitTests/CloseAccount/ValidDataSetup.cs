namespace UnitTests.CloseAccount;

using Xunit;

internal sealed class ValidDataSetup : TheoryData<decimal>
{
    public ValidDataSetup()
    {
        Add(0.5M);
        Add(100M);
        Add(200M);
    }
}
