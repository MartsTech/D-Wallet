using Xunit;

namespace UnitTests.Transfer;

internal sealed class ValidDataSetup : TheoryData<decimal, decimal>
{
    public ValidDataSetup()
    {
        Add(100, 400);
    }
}
