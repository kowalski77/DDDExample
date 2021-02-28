using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SnackMachine.Domain.SnackAggregate;
using SnackMachine.Domain.ValueObjects;

namespace SnackMachine.API.UseCases.AddSnack
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class SnackController : ControllerBase
    {
        private readonly ISnackRepository snackRepository;

        public SnackController(ISnackRepository snackRepository)
        {
            this.snackRepository = snackRepository ?? throw new ArgumentNullException(nameof(snackRepository));
        }

        [HttpPost]
        public async Task<IActionResult> AddSnack([FromBody] AddSnackRequest request)
        {
            if (request == null)
            {
                return this.BadRequest($"Request {nameof(AddSnackRequest)} is null");
            }

            var snack = new Snack(Name.CreateInstance(request.Name), Money.CreateInstance(request.Price));
            await this.snackRepository.AddSnack(snack);

            return this.Ok(snack);
        }
    }
}