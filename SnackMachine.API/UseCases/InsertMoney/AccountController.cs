﻿using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SnackMachine.Domain.AccountAggregate;
using SnackMachine.Domain.ValueObjects;

namespace SnackMachine.API.UseCases.InsertMoney
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        [HttpPut(nameof(InsertMoney))]
        public async Task<IActionResult> InsertMoney([FromBody] InsertMoneyModel.MoneyRequest request)
        {
            if (request == null)
            {
                return this.BadRequest($"Request {nameof(InsertMoneyModel.MoneyRequest)} is null");
            }

            var maybeAccount = await this.accountRepository.GetAccountAsync();
            if (!maybeAccount.TryGetValue(out var account))
            {
                return this.NotFound("No account available");
            }

            var money = Money.CreateInstance(request.Amount);
            var canInsertMoney = account.CanInsertMoney(money);
            if (!canInsertMoney.Success)
            {
                return this.BadRequest($"{canInsertMoney.Code}: coin not registered");
            }

            account.InsertMoney(money);

            await this.accountRepository.UpdateAccountAsync(account);

            return this.NoContent();
        }
    }
}