using System;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using SnackMachine.Domain.MachineAggregate;

namespace SnackMachine.API.UseCases
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
    }
}