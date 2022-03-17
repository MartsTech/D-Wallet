namespace UnitTests.Accounts.GetAccount;

using Application.UseCases.Accounts.GetAccount;
using Application.UseCases.Transactions.Withdraw;
using Domain.Accounts;
using Domain.Credits;
using Domain.ValueObjects;
using FluentValidation.Results;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public sealed class GetAccountTests : IClassFixture<StandardFixture>
{
    private readonly StandardFixture _fixture;

    public GetAccountTests(StandardFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetAccount_Function_Is_Successful()
    {
        GetAccountInput inputGetAccount = new()
        {
            AccountId = SeedData.DefaultAccountId.Id,
            
        };

        GetAccountUseCase.Query command = new(inputGetAccount);

        GetAccountUseCase.Handler handler = new(
           
            _fixture.AccountRepository
            );


        GetAccountUseCase.CommandValidator validator = new();
        ValidationResult validationResult = validator.Validate(command);

        Assert.True(validationResult.IsValid);

    }


}