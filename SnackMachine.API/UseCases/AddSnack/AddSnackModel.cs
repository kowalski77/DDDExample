using FluentValidation;

namespace SnackMachine.API.UseCases.AddSnack
{
    public record AddSnackRequest(string Name, decimal Price);

    public class AddSnackRequestValidator : AbstractValidator<AddSnackRequest>
    {
        public AddSnackRequestValidator()
        {
            this.RuleFor(x => x.Name).NotEmpty();
            this.RuleFor(x => x.Price).GreaterThan(0);
        }
    }
}