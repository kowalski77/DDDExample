using FluentValidation;

namespace SnackMachine.API.UseCases.AddSnack
{
    public record AddSnackRequest(long SnackId, int Pile);

    public class AddSnackRequestValidator : AbstractValidator<AddSnackRequest>
    {
        public AddSnackRequestValidator()
        {
            this.RuleFor(x => x.Pile > 0);
            this.RuleFor(x => x.SnackId > 0);
        }
    }
}