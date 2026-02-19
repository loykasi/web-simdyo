namespace Application.Results
{
    public class Result
    {
        public bool IsSuccess { get; }

        public ErrorType? TypeOfError { get; }
        public List<Error> Errors { get; } = [];

        public Result()
        {
            IsSuccess = true;
        }

        public Result(ErrorType errorType, params Error[] errors)
        {
            IsSuccess = false;
            TypeOfError = errorType;
            Errors = [.. errors];
        }

        public static Result Success() => new();
        public static Result Failure(params Error[] errors) => new(ErrorType.Failure, errors);
        public static Result NotFound(params Error[] errors) => new(ErrorType.NotFound, errors);
        public static Result BadRequest(params Error[] errors) => new(ErrorType.AccessUnAuthorized, errors);
        public static Result Conflict(params Error[] errors) => new(ErrorType.Conflict, errors);
        public static Result UnAuthorized(params Error[] errors) => new(ErrorType.AccessUnAuthorized, errors);

        public static Result<T> Success<T>(T value) => new(value);
        public static Result<T> Failure<T>(params Error[] errors) => new(ErrorType.Failure, errors);
        public static Result<T> NotFound<T>(params Error[] errors) => new(ErrorType.NotFound, errors);
        public static Result<T> BadRequest<T>(params Error[] errors) => new(ErrorType.AccessUnAuthorized, errors);
        public static Result<T> Conflict<T>(params Error[] errors) => new(ErrorType.Conflict, errors);
        public static Result<T> UnAuthorized<T>(params Error[] errors) => new(ErrorType.AccessUnAuthorized, errors);
    }

    public class Result<T> : Result
    {
        public Result(T value) : base()
        {
            Value = value;
        }

        public Result(ErrorType errorType, params Error[] errors) : base(errorType, errors)
        {
        }

        public T? Value;
    }
}
