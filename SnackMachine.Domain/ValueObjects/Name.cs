using System;
using System.Collections.Generic;
using SnackMachine.Domain.Utils;

namespace SnackMachine.Domain.ValueObjects
{
    public class Name : ValueObject
    {
        private Name(string value)
        {
            this.Value = value;
        }

        public string Value { get; private set; }

        public static Name CreateInstance(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return new Name(name);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}