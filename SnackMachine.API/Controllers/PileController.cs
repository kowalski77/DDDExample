using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SnackMachine.API.Contracts;
using SnackMachine.API.Repositories;

namespace SnackMachine.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PileController : ControllerBase
    {
        private readonly ISnackRepository snackRepository;
        private readonly IMachineRepository machineRepository;

        public PileController(
            ISnackRepository snackRepository, 
            IMachineRepository machineRepository)
        {
            this.snackRepository = snackRepository ?? throw new ArgumentNullException(nameof(snackRepository));
            this.machineRepository = machineRepository ?? throw new ArgumentNullException(nameof(machineRepository));
        }

        [HttpPost]
        public async Task<IActionResult> AddSnack([FromBody] AddSnackRequest request)
        {
            var maybeSnack = await this.snackRepository.GetSnackAsync(request.SnackId);
            if(!maybeSnack.TryGetValue(out var snack))
            {
                return this.BadRequest($"Snack with id: {request.SnackId} does not exists");
            }

            var machine = await this.machineRepository.GetMainMachineAsync();

            var pile = machine.Piles[request.Pile];
            if (!pile.CanAddSnack())
            {
                return this.BadRequest($"Cannot add more snacks in pile: {request.Pile}");
            }
            pile.AddSnack(snack);

            await this.machineRepository.SaveAsync();

            return this.Ok();
        }
    }
}