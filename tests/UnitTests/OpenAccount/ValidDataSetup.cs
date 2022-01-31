namespace UnitTests.OpenAccount;

using Xunit;

internal sealed class ValidDataSetup : TheoryData<decimal, string>
{
    public ValidDataSetup()
    {
        Add(100, "SEK");
        Add(25, "BRL");
        Add(10, "USD");
    }
}
