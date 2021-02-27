using System;

namespace SnackMachine.Domain.Utils
{
    public readonly struct Maybe<T>
        where T : class
    {
        private readonly T value;
        private readonly bool hasValue;

        private Maybe(T value)
        {
            this.value = value;
            this.hasValue = true;
        }

        public static implicit operator Maybe<T>(T value)
        {
            return value == null! ?
                new Maybe<T>() :
                new Maybe<T>(value);
        }

        public bool TryGetValue(out T tryValue)
        {
            if (this.hasValue)
            {
                tryValue = this.value;
                return true;
            }

            tryValue = default!;

            return false;
        }
    }
}