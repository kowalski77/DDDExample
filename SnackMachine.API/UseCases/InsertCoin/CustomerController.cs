using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SnackMachine.API.Contracts;
using SnackMachine.Domain.MachineAggregate;
using SnackMachine.Domain.ValueObjects;

namespace SnackMachine.API.UseCases.InsertCoin
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class CustomerController : ControllerBase
    {
        private readonly IMachineRepository machineRepository;

        public CustomerController(IMachineRepository machineRepository)
        {
            this.machineRepository = machineRepository ?? throw new ArgumentNullException(nameof(machineRepository));
        }

        [HttpPost(nameof(Insert))]
        public async Task<IActionResult> Insert([FromBody] InsertMoneyRequest request)
        {
            var maybeMachine = await this.machineRepository.GetMainMachineAsync();
            if (!maybeMachine.TryGetValue(out var machine))
            {
                return this.BadRequest("There is no main machine registered");
            }

            var money = Money.CreateInstance(request.Amount);
            var canInsertMoney = machine.Account.CanInsertMoney(money);
            if (!canInsertMoney.Success)
            {
                return this.BadRequest($"{canInsertMoney.Code}: coin not registered");
            }

            machine.Account.InsertMoney(money);
            await this.machineRepository.SaveAsync(machine);

            return this.Ok();
        }
    }
}