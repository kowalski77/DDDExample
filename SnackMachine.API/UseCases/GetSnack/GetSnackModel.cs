using FluentValidation;

namespace SnackMachine.API.UseCases.GetSnack
{
    public class GetSnackModel
    {
        public record GetSnackRequest(long SnackId);

        public record GetSnackResponse(string Name, decimal Price);

        public class GetSnackRequestValidator : AbstractValidator<GetSnackRequest>
        {
            public GetSnackRequestValidator()
            {
                this.RuleFor(x => x.SnackId).GreaterThan(0);
            }
        }
    }
}