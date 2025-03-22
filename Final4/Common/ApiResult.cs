namespace Repositories.Commons
{
    public record ApiResult<T>
    {
        public bool ApiIsSuccess { get; set; }
        public T? Data { get; set; }
        public string? ApiMessage { get; set; }
        public static ApiResult<T> Succeed(T? data, string message)
        {
            return new ApiResult<T> { ApiIsSuccess = true, Data = data, ApiMessage = message };
        }

        public static ApiResult<T> Error(T? data, string Message)
        {
            return new ApiResult<T> { ApiIsSuccess = false, Data = data, ApiMessage = Message };
        }

        public static ApiResult<T> Fail(Exception ex)
        {
            return new ApiResult<T>
            {
                ApiIsSuccess = false,
                Data = default,
                ApiMessage = ex.Message
            };
        }
    }
}
