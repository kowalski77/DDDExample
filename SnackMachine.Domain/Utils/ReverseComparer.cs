using System.Collections.Generic;

namespace SnackMachine.Domain.Utils
{
    public sealed class ReverseComparer<T> : IComparer<T>
    {
        private readonly IComparer<T> original;

        public ReverseComparer(IComparer<T> original)
        {
            this.original = original;
        }

        public int Compare(T? x, T? y)
        {
            return this.original.Compare(y, x);
        }
    }
}