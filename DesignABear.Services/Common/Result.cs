namespace DesignABear.Services.Common
{
    public class Error
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public Error() { }

        public Error(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public static readonly Error None = new Error(string.Empty, string.Empty);
    }

    public class Result<TValue>
    {
        public TValue? Value { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; set; } = Error.None;

        public Result() { }

        protected Result(TValue? value, bool isSuccess, Error error)
        {
            Value = value;
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result<TValue> Success(TValue value) => new Result<TValue>(value, true, Error.None);
        public static Result<TValue> Failure(Error error) => new Result<TValue>(default, false, error);
    }

    public class Result
    {
        public bool IsSuccess { get; set; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; set; } = Error.None;

        public Result() { }

        protected Result(bool isSuccess, Error error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new Result(true, Error.None);
        public static Result Failure(Error error) => new Result(false, error);
    }
}
