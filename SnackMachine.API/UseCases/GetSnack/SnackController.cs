using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SnackMachine.Domain.SnackAggregate;

namespace SnackMachine.API.UseCases.GetSnack
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSnack(Guid id)
        {
            var maybeSnack = await this.snackRepository.GetSnackAsync(id);
            if(!maybeSnack.TryGetValue(out var snack))
            {
                return this.NotFound($"No snack found with id: {id}");
            }

            var response = new GetSnackModel.SnackResponse(snack.Name.Value, snack.Price.Value);

            return this.Ok(response);
        }
    }
}