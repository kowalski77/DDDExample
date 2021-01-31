using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SnackMachine.API.Contracts;
using SnackMachine.API.Repositories;

namespace SnackMachine.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IMachineRepository machineRepository;

        public AdminController(IMachineRepository machineRepository)
        {
            this.machineRepository = machineRepository ?? throw new ArgumentNullException(nameof(machineRepository));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMachineRequest request)
        {
            //var name = Name.CreateInstance(request.Name);

            //var machine = new Machine(name, new Account());
            //await this.machineRepository.CreateAsync(machine);

            return this.Ok();
        }
    }
}