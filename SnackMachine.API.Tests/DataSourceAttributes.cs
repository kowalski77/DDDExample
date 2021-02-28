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
    public class ControllerDataSourceAttribute : BaseDataSourceAttribute
    {
        protected override void CustomizeFixtureBefore(IFixture fixture)
        {
            fixture.Customize<BindingInfo>(c => c.OmitAutoProperties());
        }
    }

    public class AccountDataSourceAttribute : ControllerDataSourceAttribute
    {
        protected override void CustomizeFixtureBefore(IFixture fixture)
        {
            base.CustomizeFixtureBefore(fixture);

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

    public class MachineWithSnacksDataSourceAttribute : AccountDataSourceAttribute
    {
        protected override void CustomizeFixtureBefore(IFixture fixture)
        {
            // snack repository
            var snack = fixture.Create<Snack>();
            var snackRepository = fixture.Freeze<Mock<ISnackRepository>>();
            snackRepository.Setup(x => x.GetSnackAsync(It.IsAny<long>())).ReturnsAsync(snack);

            base.CustomizeFixtureBefore(fixture);
        }
    }
}