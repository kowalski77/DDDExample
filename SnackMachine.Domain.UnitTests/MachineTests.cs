using System;
using System.Collections.Generic;
using FluentAssertions;
using SnackMachine.Domain.MachineAggregate;
using Xunit;

namespace SnackMachine.Domain.UnitTests
{
    public class MachineTests
    {
        [Theory, MachineDataSource]
        public void AddPile_WithRemainingPlace_ShallAddANewPile(
            Pile pile,
            Machine sut)
        {
            // Act
            sut.AddPile(pile);

            // Assert
            sut.Piles.Count.Should().Be(4);
        }

        [Theory, MachineDataSource]
        public void AddPile_WithNoRemainingPlace_ShallThrowInvalidOperationException(
            List<Pile> piles,
            Machine sut)
        {
            // Act
            Action addPileListAction = () =>
            {
                foreach (var pile in piles)
                {
                    sut.AddPile(pile);
                }
            };

            addPileListAction.Should().Throw<InvalidOperationException>();
        }
    }
}