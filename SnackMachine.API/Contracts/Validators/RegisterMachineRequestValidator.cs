using FluentValidation;

namespace SnackMachine.API.Contracts.Validators
{
    public class RegisterMachineRequestValidator : AbstractValidator<RegisterMachineRequest>
    {
        public RegisterMachineRequestValidator()
        {
            this.RuleFor(x => x.Name).NotEmpty();
        }
    }
}