using System;
using System.Collections.Generic;
using SnackMachine.Domain.Utils;

namespace SnackMachine.Domain.ValueObjects
{
    public class Capacity : ValueObject
    {
        private Capacity(int value)
        {
            this.Value = value;
        }

        public int Value { get; private set; }

        public static Capacity CreateInstance(int value)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));

            return new Capacity(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}