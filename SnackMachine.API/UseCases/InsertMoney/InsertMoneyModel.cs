using FluentValidation;

namespace SnackMachine.API.UseCases.InsertMoney
{
    public class InsertMoneyModel
    {
        public record MoneyRequest(decimal Amount);

        public class InsertMoneyRequestValidator : AbstractValidator<MoneyRequest>
        {
            public InsertMoneyRequestValidator()
            {
                this.RuleFor(x => x.Amount).GreaterThan(0);
            }
        }
    }
}