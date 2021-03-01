using System;
using FluentValidation;

namespace SnackMachine.API.UseCases.BuySnack
{
    public record BuySnackRequest(Guid SnackId);

    public class BuySnackRequestValidator : AbstractValidator<BuySnackRequest>
    {
        public BuySnackRequestValidator()
        {
            this.RuleFor(x => x.SnackId).NotEmpty();
        }
    }
}