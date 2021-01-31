using FluentAssertions;
using SnackMachine.Domain.DomainServices;
using SnackMachine.Domain.ValueObjects;
using Xunit;

namespace SnackMachine.Domain.UnitTests.DomainServices
{
    public class ExchangeBoxTests
    {
        [Theory]
        [ExchangeBoxDataSource]
        public void TryGetChange_WithExactImportAndEnoughChange_ShouldReturnCorrectChange(ExchangeBox sut)
        {
            // Arrange
            var amount = Money.FiftyCents;

            // Act
            var canChange = sut.TryGetChange(amount, out var changeDictionary);

            // Assert
            canChange.Should().BeTrue();
            changeDictionary.Count.Should().Be(1);
            changeDictionary.Values[0].Should().Be(1);
            changeDictionary.Keys[0].Value.Should().Be(0.50m);
        }

        [Theory]
        [ExchangeBoxDataSource]
        public void TryGetChange_WithMoreMoneyInsertedAndEnoughChange_ShouldReturnCorrectChange(ExchangeBox sut)
        {
            // Arrange
            var amount = Money.FiftyCents + Money.FiveCents;

            // Act
            var canChange = sut.TryGetChange(amount, out var changeDictionary);

            // Assert
            canChange.Should().BeTrue();
            changeDictionary.Count.Should().Be(2);
            changeDictionary.Values[0].Should().Be(1);
            changeDictionary.Keys[0].Value.Should().Be(0.50m);
            changeDictionary.Values[1].Should().Be(1);
            changeDictionary.Keys[1].Value.Should().Be(0.05m);
        }

        [Theory]
        [ExchangeBoxDataSource]
        public void TryGetChange_WithNotEnoughMoney_ShouldNotChange(ExchangeBox sut)
        {
            // Arrange
            var amount = Money.Two;

            // Act
            var canChange = sut.TryGetChange(amount, out var changeDictionary);

            // Assert
            canChange.Should().BeFalse();
            changeDictionary.Count.Should().Be(0);
        }
    }
}