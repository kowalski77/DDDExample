using FluentAssertions;
using SnackMachine.Domain.ValueObjects;
using Xunit;

namespace SnackMachine.Domain.UnitTests
{
    public class MoneyTests
    {
        [Fact]
        public void AreEqual_WithTwoEqualInstances_ShouldReturnTrue()
        {
            // Arrange
            var firstFive = Money.Five;
            var secondFive = Money.Five;

            // Act
            var result = firstFive.Equals(secondFive);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void AreEqual_WithTwoDifferentInstances_ShouldReturnFalse()
        {
            // Arrange
            var ten = Money.Five;
            var two = Money.Two;

            // Act
            var result = ten.Equals(two);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Sum_OneFiveAndTwentyCents_ShouldReturnCorrectAmount()
        {
            // Act
            var result = Money.Five + Money.TwentyCents;

            // Assert
            result.Value.Should().Be(5.20m);
        }
    }
}