namespace Scratch.Application.Results
{
    public static class ResultExtension
    {
        public static T Match<T>(this Result result, Func<T> onSuccess, Func<Result, T> onFailure)
        {
            return result.IsSuccess ? onSuccess() : onFailure(result);
        }
    }
}
