namespace UnitTests.Accounts.OpenAccount;

using Xunit;

internal sealed class InvalidDataSetup : TheoryData<decimal, string>
{
    public InvalidDataSetup()
    {
        Add(0, "BGN");
        Add(-100, "EUR");
        Add(10, "abc");
    }
}