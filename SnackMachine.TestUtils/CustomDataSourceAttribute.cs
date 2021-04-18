using System;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace SnackMachine.TestUtils
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CustomDataSourceAttribute : AutoDataAttribute
    {
        protected CustomDataSourceAttribute(ICustomization customization) : base(() =>
            new Fixture().Customize(new CompositeCustomization(
                new AutoMoqCustomization(),
                customization)))
        {
        }
    }
}