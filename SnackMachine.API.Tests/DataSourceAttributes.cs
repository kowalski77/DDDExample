using System.Collections.Generic;
using AutoFixture;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using SnackMachine.API.UseCases.AddSnack;
using SnackMachine.Domain.MachineAggregate;
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

    public class BuyerDataSourceAttribute : ControllerDataSourceAttribute
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
            var machine = new Machine(fixture.Create<Name>(), new Account(coins));

            var machineRepositoryMock = fixture.Freeze<Mock<IMachineRepository>>();
            machineRepositoryMock.Setup(x => x.GetMainMachineAsync()).ReturnsAsync(machine);
        }
    }

    public class MachineWithNoSnacksDataSourceAttribute : ControllerDataSourceAttribute
    {
        protected override void CustomizeFixtureBefore(IFixture fixture)
        {
            var machineRepositoryMock = fixture.Freeze<Mock<IMachineRepository>>();
            machineRepositoryMock.Setup(x => x.GetMainMachineAsync()).ReturnsAsync(fixture.Create<Machine>());

            base.CustomizeFixtureBefore(fixture);
        }
    }

    public class MachineWithSnacksDataSourceAttribute : ControllerDataSourceAttribute
    {
        protected override void CustomizeFixtureBefore(IFixture fixture)
        {
            // machine repository
            var machineRepositoryMock = fixture.Freeze<Mock<IMachineRepository>>();
            machineRepositoryMock.Setup(x => x.GetMainMachineAsync()).ReturnsAsync(fixture.Create<Machine>());

            // snack repository
            var snack = fixture.Create<Snack>();
            var snackRepository = fixture.Freeze<Mock<ISnackRepository>>();
            snackRepository.Setup(x => x.GetSnackAsync(It.IsAny<long>())).ReturnsAsync(snack);

            base.CustomizeFixtureBefore(fixture);
        }
    }

    public class MachineWithPilesDataSourceAttribute : ControllerDataSourceAttribute
    {
        protected override void CustomizeFixtureBefore(IFixture fixture)
        {
            base.CustomizeFixtureBefore(fixture);

            // snack repository returns a snack
            var snack = fixture.Create<Snack>();

            var snackRepository = fixture.Freeze<Mock<ISnackRepository>>();
            snackRepository.Setup(x => x.GetSnackAsync(It.IsAny<long>())).ReturnsAsync(snack);

            // machine repository returns a machine with a pile with max capacity of 1
            var machine = fixture.Create<Machine>();
            machine.AddPile(new Pile(Capacity.CreateInstance(0)));
            machine.AddPile(new Pile(Capacity.CreateInstance(1)));

            var machineRepositoryMock = fixture.Freeze<Mock<IMachineRepository>>();
            machineRepositoryMock.Setup(x => x.GetMainMachineAsync()).ReturnsAsync(machine);
        }
    }

    public class PileZeroRequestDataSourceAttribute : MachineWithPilesDataSourceAttribute
    {
        protected override void CustomizeFixtureBefore(IFixture fixture)
        {
            fixture.Customize<AddSnackRequest>(x => x.With(y => y.Pile, 0));
            base.CustomizeFixtureBefore(fixture);
        }
    }

    public class PileOneRequestDataSourceAttribute : MachineWithPilesDataSourceAttribute
    {
        protected override void CustomizeFixtureBefore(IFixture fixture)
        {
            fixture.Customize<AddSnackRequest>(x => x.With(y => y.Pile, 1));
            base.CustomizeFixtureBefore(fixture);
        }
    }
}