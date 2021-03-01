using System;

namespace SnackMachine.Domain.Utils
{
    public abstract class Entity
    {
        public Guid Id { get; init; }

        public override bool Equals(object? obj)
        {
            if (obj is not Entity other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (this.GetType() != other.GetType())
                return false;

            if (this.Id == Guid.Empty || other.Id == Guid.Empty)
                return false;

            return this.Id == other.Id;
        }

        public static bool operator ==(Entity a, Entity b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (this.GetType().ToString() + this.Id).GetHashCode();
        }
    }
}