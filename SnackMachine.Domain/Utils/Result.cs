namespace SnackMachine.Domain.Utils
{
    public sealed class Result<T>
    {
        private Result(
            T code,
            bool success)
        {
            this.Code = code;
            this.Success = success;
        }

        public T Code { get; }

        public bool Success { get; }

        public static Result<T> Ok() => new(default!, true);

        public static Result<T> Ok(T code) => new(code, true);

        public static Result<T> Fail(T code) => new(code, false);
    }
}