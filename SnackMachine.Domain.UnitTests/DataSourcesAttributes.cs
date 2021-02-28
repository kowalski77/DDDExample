using System.Collections.Generic;
using AutoFixture;
using SnackMachine.Domain.AccountAggregate;
using SnackMachine.Domain.DomainServices;
using SnackMachine.Domain.SnackAggregate;
using SnackMachine.Domain.ValueObjects;
using SnackMachine.TestUtils;

namespace SnackMachine.Domain.UnitTests
{
    public class ExchangeBoxDataSourceAttribute : BaseDataSourceAttribute
    {
        protected override void CustomizeFixtureBefore(IFixture fixture)
        {
            var coins = new List<Coin>(new[]
            {
                new Coin(1, Money.FiveCents), new Coin(1, Money.FiftyCents), new Coin(1, Money.One)
            });
            var exchangeBox = fixture.Create<ExchangeBox>();
            exchangeBox.LoadMoney(coins);

            fixture.Inject(exchangeBox);
        }
    }

    public class AccountServiceDataSourceAttribute : BaseDataSourceAttribute
    {
        protected override void CustomizeFixtureBefore(IFixture fixture)
        {
            var coins = new List<Coin>(new[]
            {
                new Coin(1, Money.FiveCents),
                new Coin(0, Money.TenCents),
                new Coin(1, Money.FiftyCents), 
                new Coin(1, Money.One)
            });

            fixture.Register<IEnumerable<Coin>>(() => coins);

            fixture.Register<IExchangeBox>(() => new ExchangeBox());
        }
    }

    public class AccountDataSourceAttribute : BaseDataSourceAttribute
    {
        protected override void CustomizeFixtureBefore(IFixture fixture)
        {
            var coins = new List<Coin>(new[]
            {
                new Coin(1, Money.FiveCents),
                new Coin(0, Money.TwentyCents), 
                new Coin(1, Money.FiftyCents), 
                new Coin(1, Money.One),
                new Coin(0, Money.Two)
            });

            fixture.Register(() => new Account(coins));

            var snack = new Snack(fixture.Create<Name>(), Money.Two);
            fixture.Inject(snack);
        }
    }
}