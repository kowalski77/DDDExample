using System;
using System.Collections.Generic;
using AutoFixture;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using SnackMachine.Domain.AccountAggregate;
using SnackMachine.Domain.SnackAggregate;
using SnackMachine.Domain.ValueObjects;
using SnackMachine.TestUtils;

namespace SnackMachine.API.Tests
{
    public class ControllerDataSourceAttribute : CustomDataSourceAttribute
    {
        public ControllerDataSourceAttribute()
            : base(new ControllerCustomization())
        {
        }

        protected ControllerDataSourceAttribute(ICustomization customization)
            : base(new CompositeCustomization(customization, new ControllerCustomization()))
        {
        }

        private class ControllerCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Customize<BindingInfo>(c => c.OmitAutoProperties());
            }
        }
    }

    public class AccountDataSourceAttribute : ControllerDataSourceAttribute
    {
        public AccountDataSourceAttribute() 
            : base(new AccountCustomization())
        {
        }

        protected AccountDataSourceAttribute(ICustomization customization) 
            : base(new CompositeCustomization(new AccountCustomization(), customization))
        {
        }

        private class AccountCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                var coins = new List<Coin>(new[]
                {
                    new Coin(1, Money.FiveCents),
                    new Coin(0, Money.TenCents),
                    new Coin(1, Money.FiftyCents),
                    new Coin(1, Money.One)
                });
                var account = new Account(coins);

                var accountRepositoryMock = fixture.Freeze<Mock<IAccountRepository>>();
                accountRepositoryMock.Setup(x => x.GetAccountAsync()).ReturnsAsync(account);
            }
        }
    }

    public class MachineWithSnacksDataSourceAttribute : AccountDataSourceAttribute
    {
        public MachineWithSnacksDataSourceAttribute() : base(new MachineCustomization())
        {
        }

        private class MachineCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                var snack = fixture.Create<Snack>();
                var snackRepository = fixture.Freeze<Mock<ISnackRepository>>();
                snackRepository.Setup(x => x.GetSnackAsync(It.IsAny<Guid>())).ReturnsAsync(snack);
            }
        }
    }
}