
namespace AudioBlend.Core.Shared.Results
{
    public class Result<T> where T : class
    {
        protected Result(bool isSuccess, T value, string errorMsg)
        {
            IsSuccess = isSuccess;
            Value = value;
            ErrorMsg = errorMsg;
        }

        public bool IsSuccess { get; }
        public T Value { get; }
        public string ErrorMsg { get; }
        public static Result<T> Success(T value) => new(true, value, null);
        public static Result<T> Failure(string errorMsg) => new(false, null, errorMsg);

    }
}