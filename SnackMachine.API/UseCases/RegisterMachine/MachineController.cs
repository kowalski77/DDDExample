using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SnackMachine.API.Contracts;
using SnackMachine.Domain.MachineAggregate;
using SnackMachine.Domain.ValueObjects;

namespace SnackMachine.API.UseCases.RegisterMachine
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class MachineController : ControllerBase
    {
        private readonly IMachineRepository machineRepository;

        public MachineController(IMachineRepository machineRepository)
        {
            this.machineRepository = machineRepository ?? throw new ArgumentNullException(nameof(machineRepository));
        }

        [HttpPost]
        public async Task<IActionResult> RegisterMachine([FromBody] RegisterMachineRequest request)
        {
            if(request == null)
            {
                return this.BadRequest("Request cannot be null.");
            }

            var name = Name.CreateInstance(request.Name);
            var machine = new Machine(name, new Account(new List<Coin>()));

            await this.machineRepository.CreateAsync(machine);

            return this.Ok();
        }
    }
}