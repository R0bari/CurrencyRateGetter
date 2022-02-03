namespace RateGetters.Rates.Getters
{
    public record RateGetterResult<T>
    {
        public bool IsSuccess { get; }
        public T Result { get; }
        public string ErrorMessage { get; } = "";
        
        private RateGetterResult(bool isSuccess, T result)
        {
            IsSuccess = isSuccess;
            Result = result;
        }

        private RateGetterResult(bool isSuccess, string errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static RateGetterResult<T> Successful(T rateForDate) => 
            new(true, rateForDate);

        public static RateGetterResult<T> Failed(string errorMessage) => new(false, errorMessage);
    }
}