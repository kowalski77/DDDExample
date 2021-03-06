﻿using System;
using System.Collections.Generic;
using SnackMachine.Domain.Utils;

namespace SnackMachine.Domain.ValueObjects
{
    public sealed class Money : ValueObject, IComparable<Money>
    {
        private Money(decimal value)
        {
            this.Value = value;
        }

        public static Money Zero => new(0);

        public static Money One => new(1m);

        public static Money Two => new(2m);

        public static Money Five => new(5m);

        public static Money FiveCents => new(0.05m);

        public static Money TenCents => new(0.10m);

        public static Money TwentyCents => new(0.20m);

        public static Money FiftyCents => new(0.50m);

        public decimal Value { get; private set; }

        public int CompareTo(Money? other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            return other is null ? 1 : this.Value.CompareTo(other.Value);
        }

        public static Money CreateInstance(decimal value)
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));

            return new Money(value);
        }

        public static Money operator +(Money money1, Money money2)
        {
            return new(money1.Value + money2.Value);
        }

        public static Money operator -(Money money1, Money money2)
        {
            return new(money1.Value - money2.Value);
        }

        public static Money operator *(Money money1, int quantity)
        {
            return new(money1.Value * quantity);
        }

        public static bool operator <=(Money a, Money b)
        {
            return a.Value <= b.Value;
        }

        public static bool operator >=(Money a, Money b)
        {
            return a.Value >= b.Value;
        }

        public static bool operator >(Money a, Money b)
        {
            return a.Value > b.Value;
        }

        public static bool operator <(Money a, Money b)
        {
            return a.Value < b.Value;
        }

        public static bool operator ==(Money a, Money b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(Money money1, Money money2)
        {
            return !(money1 == money2);
        }

        public override bool Equals(object? obj)
        {
            return obj != null && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}