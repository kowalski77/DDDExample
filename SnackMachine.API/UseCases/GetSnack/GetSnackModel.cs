using FluentValidation;

namespace SnackMachine.API.UseCases.GetSnack
{
    public class GetSnackModel
    {
        public record SnackRequest(long SnackId);

        public record SnackResponse(string Name, decimal Price);

        public class GetSnackRequestValidator : AbstractValidator<SnackRequest>
        {
            public GetSnackRequestValidator()
            {
                this.RuleFor(x => x.SnackId).GreaterThan(0);
            }
        }
    }
}