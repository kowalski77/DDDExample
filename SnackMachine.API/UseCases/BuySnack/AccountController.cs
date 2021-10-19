using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SnackMachine.Domain.AccountAggregate;
using SnackMachine.Domain.DomainServices;
using SnackMachine.Domain.SnackAggregate;

namespace SnackMachine.API.UseCases.BuySnack
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class AccountController : ControllerBase
    {
        private readonly ISnackRepository snackRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IAccountService accountService;

        public AccountController(
            ISnackRepository snackRepository,
            IAccountRepository accountRepository,
            IAccountService accountService)
        {
            this.snackRepository = snackRepository ?? throw new ArgumentNullException(nameof(snackRepository));
            this.accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            this.accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        [HttpPut(nameof(BuySnack))]
        public async Task<IActionResult> BuySnack([FromBody] BuySnackModel.SnackRequest snackRequest)
        {
            if (snackRequest == null)
            {
                return this.BadRequest($"Request {nameof(BuySnackModel.SnackRequest)} is null");
            }

            var maybeAccount = await this.accountRepository.GetAccountAsync();
            if (!maybeAccount.TryGetValue(out var account))
            {
                return this.NotFound("No account available");
            }

            var maybeSnack = await this.snackRepository.GetSnackAsync(snackRequest.SnackId);
            if (!maybeSnack.TryGetValue(out var snack))
            {
                return this.NotFound($"Snack with id: {snackRequest.SnackId} does not exists");
            }

            var result = this.accountService.BuyWithExchange(account, snack);
            if (!result.Success)
            {
                return this.BadRequest(result.Code);
            }

            await this.accountRepository.UpdateAccountAsync(account);

            return this.NoContent();
        }
    }
}