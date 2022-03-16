namespace UnitTests.Accounts.OpenAccount;

using Xunit;

internal sealed class ValidDataSetup : TheoryData<decimal, string>
{
    public ValidDataSetup()
    {
        Add(100, "BGN");
        Add(25, "EUR");
        Add(10, "USD");
    }
}