using FluentValidation;

namespace SnackMachine.API.UseCases.InsertMoney
{
    public record InsertMoneyRequest(decimal Amount);

    public class InsertMoneyRequestValidator : AbstractValidator<InsertMoneyRequest>
    {
        public InsertMoneyRequestValidator()
        {
            this.RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}