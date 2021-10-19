using System;
using FluentValidation;

namespace SnackMachine.API.UseCases.BuySnack
{
    public class BuySnackModel
    {
        public record SnackRequest(Guid SnackId);

        public class BuySnackRequestValidator : AbstractValidator<SnackRequest>
        {
            public BuySnackRequestValidator()
            {
                this.RuleFor(x => x.SnackId).NotEmpty();
            }
        }
    }
}