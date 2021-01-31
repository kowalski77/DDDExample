using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SnackMachine.API.Contracts;
using SnackMachine.API.Repositories;
using SnackMachine.Domain.DomainServices;
using SnackMachine.Domain.ValueObjects;

namespace SnackMachine.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BuyerController : ControllerBase
    {
        private readonly IMachineRepository machineRepository;
        private readonly ISnackRepository snackRepository;
        private readonly AccountService accountService;

        public BuyerController(
            ISnackRepository snackRepository,
            IMachineRepository machineRepository, 
            AccountService accountService)
        {
            this.snackRepository = snackRepository ?? throw new ArgumentNullException(nameof(snackRepository));
            this.machineRepository = machineRepository ?? throw new ArgumentNullException(nameof(machineRepository));
            this.accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        [HttpPost(nameof(Insert))]
        public async Task<IActionResult> Insert([FromBody] InsertMoneyRequest request)
        {
            var machine = await this.machineRepository.GetMainMachineAsync();

            var money = Money.CreateInstance(request.Amount);
            var canInsertMoney = machine.Account.CanInsertMoney(money);
            if (!canInsertMoney.Success)
            {
                return this.BadRequest($"{canInsertMoney.Code}: coin not registered");
            }

            machine.Account.InsertMoney(money);

            await this.machineRepository.SaveAsync();

            return this.Ok();
        }

        [HttpPost(nameof(Buy))]
        public async Task<IActionResult> Buy([FromBody] BuySnackRequest request)
        {
            var machine = await this.machineRepository.GetMainMachineAsync();
            var snack = await this.snackRepository.GetSnackAsync(request.Id);

            if(snack == null)
            {
                return this.BadRequest($"Snack with id: {request.Id} does not exists.");
            }

            var result = this.accountService.BuyWithExchange(machine.Account, snack);
            if (!result.Success)
            {
                return this.BadRequest(result.Code);
            }

            return this.Ok();
        }
    }
}