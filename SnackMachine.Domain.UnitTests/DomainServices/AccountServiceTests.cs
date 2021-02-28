using System.Collections.Generic;
using FluentAssertions;
using SnackMachine.Domain.AccountAggregate;
using SnackMachine.Domain.DomainServices;
using SnackMachine.Domain.SnackAggregate;
using SnackMachine.Domain.ValueObjects;
using Xunit;

namespace SnackMachine.Domain.UnitTests.DomainServices
{
    public class AccountServiceTests
    {
        [Theory, AccountServiceDataSource]
        public void BuyWithExchange_WithNotEnoughInsertedMoney_TheSnackIsNotBought(
            IEnumerable<Coin> coinsCollection,
            Name snackName,
            Account account,
            AccountService sut)
        {
            // Arrange
            account.InsertMoney(Money.FiveCents);
            var snack = new Snack(snackName, Money.FiftyCents);

            // Act
            var result = sut.BuyWithExchange(account, snack);

            // Arrange
            result.Code.Should().Be(AccountCode.NotEnoughMoney);
            account.MoneyInTransaction.Should().Be(Money.FiveCents);
            account.Coins.Should().BeEquivalentTo(coinsCollection);
        }

        [Theory, AccountServiceDataSource]
        public void BuyWithExchange_WithAccountWithEnoughMoneyInTransaction_TheSnackIsBought(
            Name snackName,
            Account account,
            AccountService sut)
        {
            // Arrange
            account.InsertMoney(Money.FiftyCents);
            var snack = new Snack(snackName, Money.FiftyCents);

            // Act
            var result = sut.BuyWithExchange(account, snack);

            // Arrange
            result.Code.Should().Be(AccountCode.Ok);
            account.MoneyInTransaction.Should().Be(Money.Zero);
            account.Coins[0].Quantity.Should().Be(1);
            account.Coins[0].Money.Should().Be(Money.FiveCents);
            account.Coins[1].Quantity.Should().Be(0);
            account.Coins[1].Money.Should().Be(Money.TenCents);
            account.Coins[2].Quantity.Should().Be(2);
            account.Coins[2].Money.Should().Be(Money.FiftyCents);
            account.Coins[3].Quantity.Should().Be(1);
            account.Coins[3].Money.Should().Be(Money.One);
        }

        [Theory, AccountServiceDataSource]
        public void BuyWithExchange_CannotGetChange_TheTransactionIsCancelled(
            Name snackName,
            Account account,
            AccountService sut)
        {
            // Arrange
            account.InsertMoney(Money.One);
            var snackPrice = Money.FiveCents + Money.TwentyCents;
            var snack = new Snack(snackName, snackPrice);

            // Act
            var result = sut.BuyWithExchange(account, snack);

            // Assert
            result.Code.Should().Be(AccountCode.NotEnoughChange);
            account.MoneyInTransaction.Should().Be(Money.Zero);
        }

        [Theory, AccountServiceDataSource]
        public void BuyWithExchange_CanGetChange_TheSnackIsBoughtAndTheCoinsAreUpdated(
            Name snackName,
            Account account,
            AccountService sut)
        {
            // Arrange
            account.InsertMoney(Money.FiftyCents);
            account.InsertMoney(Money.TenCents);

            var snackPrice = Money.FiftyCents;
            var snack = new Snack(snackName, snackPrice);

            // Act
            var result = sut.BuyWithExchange(account, snack);

            // Assert
            result.Code.Should().Be(AccountCode.Ok);
            account.MoneyInTransaction.Should().Be(Money.Zero);
            account.Coins[0].Quantity.Should().Be(1);
            account.Coins[0].Money.Should().Be(Money.FiveCents);
            account.Coins[1].Quantity.Should().Be(2);
            account.Coins[1].Money.Should().Be(Money.TenCents);
            account.Coins[2].Quantity.Should().Be(2);
            account.Coins[2].Money.Should().Be(Money.FiftyCents);
            account.Coins[3].Quantity.Should().Be(1);
            account.Coins[3].Money.Should().Be(Money.One);
        }
    }
}