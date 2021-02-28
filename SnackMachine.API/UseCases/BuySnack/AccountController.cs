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

        [HttpPost(nameof(BuySnack))]
        public async Task<IActionResult> BuySnack([FromBody] BuySnackRequest request)
        {
            if (request == null)
            {
                return this.BadRequest($"Request {nameof(BuySnackRequest)} is null");
            }

            var maybeAccount = await this.accountRepository.GetAccountAsync();
            if (!maybeAccount.TryGetValue(out var account))
            {
                return this.BadRequest("No account available");
            }

            var maybeSnack = await this.snackRepository.GetSnackAsync(request.SnackId);
            if (!maybeSnack.TryGetValue(out var snack))
            {
                return this.BadRequest($"Snack with id: {request.SnackId} does not exists");
            }

            var result = this.accountService.BuyWithExchange(account, snack);
            if (!result.Success)
            {
                return this.BadRequest(result.Code);
            }

            await this.accountRepository.UpdateAccountAsync(account);

            return this.Ok();
        }
    }
}