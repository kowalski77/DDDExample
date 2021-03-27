using FluentValidation;

namespace SnackMachine.API.UseCases.InsertMoney
{
    public class InsertMoneyModel
    {
        public record Request(decimal Amount);

        public class InsertMoneyRequestValidator : AbstractValidator<Request>
        {
            public InsertMoneyRequestValidator()
            {
                this.RuleFor(x => x.Amount).GreaterThan(0);
            }
        }
    }
}