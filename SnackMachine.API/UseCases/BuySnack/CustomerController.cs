﻿using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SnackMachine.API.Contracts;
using SnackMachine.Domain.DomainServices;
using SnackMachine.Domain.MachineAggregate;
using SnackMachine.Domain.SnackAggregate;

namespace SnackMachine.API.UseCases.BuySnack
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class CustomerController : ControllerBase
    {
        private readonly IMachineRepository machineRepository;
        private readonly ISnackRepository snackRepository;
        private readonly AccountService accountService;

        public CustomerController(
            IMachineRepository machineRepository, 
            ISnackRepository snackRepository, 
            AccountService accountService)
        {
            this.machineRepository = machineRepository ?? throw new ArgumentNullException(nameof(machineRepository));
            this.snackRepository = snackRepository ?? throw new ArgumentNullException(nameof(snackRepository));
            this.accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }


        [HttpPost(nameof(Buy))]
        public async Task<IActionResult> Buy([FromBody] BuySnackRequest request)
        {
            var maybeSnack = await this.snackRepository.GetSnackAsync(request.Id);
            if (!maybeSnack.TryGetValue(out var snack))
            {
                return this.BadRequest($"Snack with id: {request.Id} does not exists.");
            }

            var maybeMachine = await this.machineRepository.GetMainMachineAsync();
            if (!maybeMachine.TryGetValue(out var machine))
            {
                return this.BadRequest("There is no main machine registered");
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