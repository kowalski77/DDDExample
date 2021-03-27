using FluentValidation;

namespace SnackMachine.API.UseCases.GetSnack
{
    public class GetSnackModel
    {
        public record Request(long SnackId);

        public record Response(string Name, decimal Price);

        public class GetSnackRequestValidator : AbstractValidator<Request>
        {
            public GetSnackRequestValidator()
            {
                this.RuleFor(x => x.SnackId).GreaterThan(0);
            }
        }
    }
}