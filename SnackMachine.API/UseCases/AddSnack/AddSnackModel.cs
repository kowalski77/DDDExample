using System;
using FluentValidation;

namespace SnackMachine.API.UseCases.AddSnack
{
    public class AddSnackModel
    {
        public record Request(string Name, decimal Price);

        public record Response(Guid Id, string Name, decimal Price);

        public class AddSnackRequestValidator : AbstractValidator<Request>
        {
            public AddSnackRequestValidator()
            {
                this.RuleFor(x => x.Name).NotEmpty();
                this.RuleFor(x => x.Price).GreaterThan(0);
            }
        }
    }
}