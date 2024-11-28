namespace weather.Common
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; }
        public T Value { get; }
        public string Error { get; }

        private ServiceResult(bool isSuccess, T value, string error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public static ServiceResult<T> Success(T value)
        {
            return new ServiceResult<T>(true, value, null);
        }

        public static ServiceResult<T> Failure(string error)
        {
            return new ServiceResult<T>(false, default, error);
        }

        public bool TryGetValue(out T value)
        {
            if (IsSuccess)
            {
                value = Value;
                return true;
            }

            value = default;
            return false;
        }

        public bool TryGetError(out string error)
        {
            if (!IsSuccess)
            {
                error = Error;
                return true;
            }

            error = null;
            return false;
        }
    }

}
