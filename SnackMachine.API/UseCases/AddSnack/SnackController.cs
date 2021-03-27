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
        public async Task<IActionResult> AddSnack([FromBody] AddSnackModel.Request request)
        {
            if (request == null)
            {
                return this.BadRequest($"Request {nameof(AddSnackModel.Request)} is null");
            }

            var snack = new Snack(Name.CreateInstance(request.Name), Money.CreateInstance(request.Price));
            var newlySnack = await this.snackRepository.AddSnack(snack);

            var response = new AddSnackModel.Response(newlySnack.Id, newlySnack.Name.Value, newlySnack.Price.Value);

            return this.Ok(response);
        }
    }
}