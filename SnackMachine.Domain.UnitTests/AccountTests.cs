using System;
using System.Linq;
using FluentAssertions;
using SnackMachine.Domain.MachineAggregate;
using SnackMachine.Domain.SnackAggregate;
using SnackMachine.Domain.ValueObjects;
using Xunit;

namespace SnackMachine.Domain.UnitTests
{
    public class AccountTests
    {
        [Theory, AccountDataSource]
        public void CanInsertMoney_WithNotAllowedCoin_ShouldReturnCoinNotRegisteredCode(Account sut)
        {
            // Act
            var result = sut.CanInsertMoney(Money.TenCents);

            // Assert
            result.Code.Should().Be(AccountCode.CoinNotRegistered);
        }

        [Theory, AccountDataSource]
        public void CanInsertMoney_WithAllowedCoin_ShouldReturnOkCode(Account sut)
        {
            // Act
            var result = sut.CanInsertMoney(Money.FiftyCents);

            // Assert
            result.Code.Should().Be(AccountCode.Ok);
        }

        [Theory, AccountDataSource]
        public void InsertMoney_WithNotAllowedCoin_ShouldThrowException(Account sut)
        {
            // Act
            Action insertMoneyAction = () => sut.InsertMoney(Money.TenCents);

            // Assert
            insertMoneyAction.Should().Throw<InvalidOperationException>();
        }

        [Theory, AccountDataSource]
        public void InsertMoney_ShouldUpdateMoneyInTransactionAndCoinsCollectionAccordingly(Account sut)
        {
            // Act
            sut.InsertMoney(Money.TwentyCents);

            // Assert
            sut.MoneyInTransaction.Should().Be(Money.TwentyCents);
            sut.Coins.First(x => x.Money == Money.TwentyCents).Quantity.Should().Be(1);
        }

        [Theory, AccountDataSource]
        public void InsertMoneyMoreThanOnce_ShouldUpdateMoneyInTransactionAndCoinsCollectionAccordingly(Account sut)
        {
            // Act
            sut.InsertMoney(Money.TwentyCents);
            sut.InsertMoney(Money.TwentyCents);
            sut.InsertMoney(Money.Two);

            // Assert
            sut.MoneyInTransaction.Should().Be(Money.CreateInstance(2.40m));
            sut.Coins.First(x => x.Money == Money.TwentyCents).Quantity.Should().Be(2);
            sut.Coins.First(x => x.Money == Money.Two).Quantity.Should().Be(1);
        }

        [Theory, AccountDataSource]
        public void CanBuySnack_WithSnackPriceSuperiorThanInsertedMoney_ShouldReturnNotEnoughMoneyCode(
            Snack snack,
            Account sut)
        {
            // Arrange
            sut.InsertMoney(Money.TwentyCents);

            // Act
            var result = sut.CanBuy(snack);

            // Assert
            result.Code.Should().Be(AccountCode.NotEnoughMoney);
        }

        [Theory, AccountDataSource]
        public void CanBuySnack_WithSnackPriceSuperiorThanInsertedMoney_ShouldReturnOkCode(
            Snack snack,
            Account sut)
        {
            // Arrange
            sut.InsertMoney(Money.Two);

            // Act
            var result = sut.CanBuy(snack);

            // Assert
            result.Code.Should().Be(AccountCode.Ok);
        }

        [Theory, AccountDataSource]
        public void Buy_WithSnackPriceSuperiorThanInsertedMoney_ShouldThrowException(
            Snack snack,
            Account sut)
        {
            // Arrange
            sut.InsertMoney(Money.TwentyCents);

            // Act
            Action buyAction = () => sut.Buy(snack);

            // Assert
            buyAction.Should().Throw<InvalidOperationException>();
        }

        [Theory, AccountDataSource]
        public void Buy_SuccessfullyComplete_ShouldUpdateMoneyInTransaction(
            Snack snack,
            Account sut)
        {
            // Arrange
            sut.InsertMoney(Money.Two);

            // Act
            sut.Buy(snack);

            // Assert
            sut.MoneyInTransaction.Should().Be(Money.Zero);
            sut.Coins.First(x => x.Money == Money.Two).Quantity.Should().Be(1);
        }

        [Theory, AccountDataSource]
        public void UpdateMoney_WithNonExistingCoinInCollection_ShouldThrowException(Account sut)
        {
            // Act
            Action updateMoneyAction = () => sut.UpdateMoney(Money.Five, 1);
            
            // Assert
            updateMoneyAction.Should().Throw<InvalidOperationException>();
        }

        [Theory, AccountDataSource]
        public void UpdateMoney_WithExistingCoinInCollection_ShouldUpdateTheCoinCollectionAccordingly(Account sut)
        {
            // Act
            sut.UpdateMoney(Money.One, 3);

            // Assert
            sut.Coins.First(x => x.Money == Money.One).Quantity.Should().Be(4);
        }

        [Theory, AccountDataSource]
        public void CancelTransaction_ShouldSetMoneyTransactionToZero(Account sut)
        {
            // Arrange
            sut.InsertMoney(Money.FiftyCents);

            // Act
            sut.CancelTransaction();

            // Assert
            sut.MoneyInTransaction.Should().Be(Money.Zero);
        }

        [Theory, AccountDataSource]
        public void CancelTransaction_ShouldSetCoinCollectionToOriginalState(Account sut)
        {
            // Arrange
            sut.InsertMoney(Money.FiftyCents);

            // Act
            sut.CancelTransaction();

            // Arrange
            sut.Coins.First(x => x.Money == Money.FiveCents).Quantity.Should().Be(1);
            sut.Coins.First(x => x.Money == Money.FiftyCents).Quantity.Should().Be(1);
            sut.Coins.First(x => x.Money == Money.One).Quantity.Should().Be(1);
        }
    }
}