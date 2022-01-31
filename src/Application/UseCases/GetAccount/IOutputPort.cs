﻿namespace Application.UseCases.GetAccount;

using Domain.Accounts;

public interface IOutputPort
{
    void Invalid();

    void NotFound();

    void Ok(Account account);
}
