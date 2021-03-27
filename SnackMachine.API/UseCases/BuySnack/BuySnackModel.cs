using System;
using FluentValidation;

namespace SnackMachine.API.UseCases.BuySnack
{
    public class BuySnackModel
    {
        public record Request(Guid SnackId);

        public class BuySnackRequestValidator : AbstractValidator<Request>
        {
            public BuySnackRequestValidator()
            {
                this.RuleFor(x => x.SnackId).NotEmpty();
            }
        }
    }
}