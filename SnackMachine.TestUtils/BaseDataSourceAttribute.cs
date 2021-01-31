using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using Xunit.Sdk;

namespace SnackMachine.TestUtils
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BaseDataSourceAttribute : DataAttribute
    {
        private readonly Lazy<IFixture> fixtureLazy;

        public BaseDataSourceAttribute()
        {
            this.fixtureLazy = new Lazy<IFixture>(
                () => new Fixture().Customize(new CompositeCustomization(
                    new AutoMoqCustomization(), new SupportMutableValueTypesCustomization())),
                LazyThreadSafetyMode.PublicationOnly);
        }

        private IFixture Fixture => this.fixtureLazy.Value;

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            this.CustomizeFixtureBefore(this.Fixture);

            var specimens = new List<object>();
            foreach (var p in testMethod.GetParameters())
            {
                this.CustomizeFixture(p);

                var specimen = this.Resolve(p);
                specimens.Add(specimen);
            }

            return new[] { specimens.ToArray() };
        }

        protected virtual void CustomizeFixtureBefore(IFixture fixture)
        {
        }

        private void CustomizeFixture(ParameterInfo p)
        {
            var customizeAttributes = p.GetCustomAttributes()
                .OfType<IParameterCustomizationSource>()
                .OrderBy(x => x, new CustomizeAttributeComparer());

            foreach (var ca in customizeAttributes)
            {
                var c = ca.GetCustomization(p);

                this.Fixture.Customize(c);
            }
        }

        private object Resolve(ParameterInfo p)
        {
            var context = new SpecimenContext(this.Fixture);

            return context.Resolve(p);
        }
    }
}