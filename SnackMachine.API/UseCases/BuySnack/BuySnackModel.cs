using FluentValidation;

namespace SnackMachine.API.UseCases.BuySnack
{
    public record BuySnackRequest(long SnackId);

    public class BuySnackRequestValidator : AbstractValidator<BuySnackRequest>
    {
        public BuySnackRequestValidator()
        {
            this.RuleFor(x => x.SnackId).GreaterThan(0);
        }
    }
}