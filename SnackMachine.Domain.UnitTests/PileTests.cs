using System;
using FluentAssertions;
using SnackMachine.Domain.MachineAggregate;
using SnackMachine.Domain.SnackAggregate;
using SnackMachine.TestUtils;
using Xunit;

namespace SnackMachine.Domain.UnitTests
{
    public class PileTests
    {
        [Theory, BaseDataSource]
        public void CanAddSnack_ToPileWithCapacity_ShouldReturnTrue(Pile sut)
        {
            // Act
            var result = sut.CanAddSnack();

            // Assert
            result.Should().BeTrue();
        }

        [Theory, BaseDataSource]
        public void AddSnack_ToPileWithCapacity_ShouldAddTheSnack(
            Snack snack,
            Pile sut)
        {
            // Act
            sut.AddSnack(snack);

            // Assert
            sut.PiledSnacks.Should().Contain(x => x == snack.Id);
        }

        [Theory, PileBaseDataSource]
        public void CanAddSnack_ToPileWithoutRemainingCapacity_ShouldReturnFalse(Pile sut)
        {
            // Act
            var result = sut.CanAddSnack();

            // Assert
            result.Should().BeFalse();
        }

        [Theory, PileBaseDataSource]
        public void AddSnack_ToPileWithoutRemainingCapacity_ShouldThrowInvalidOperationException(
            Snack snack,
            Pile sut)
        {
            // Act
            Action addSnackActon = () => sut.AddSnack(snack);

            // Assert
            addSnackActon.Should().Throw<InvalidOperationException>();
        }
    }
}
