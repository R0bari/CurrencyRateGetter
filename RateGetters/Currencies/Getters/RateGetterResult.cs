namespace RateGetters.Currencies.Getters
{
    public record RateGetterResult<T>
    {
        public bool IsSuccess { get; }
        public T RateForDate { get; }
        public string ErrorMessage { get; } = "";
        
        private RateGetterResult(bool isSuccess, T rateForDate)
        {
            IsSuccess = isSuccess;
            RateForDate = rateForDate;
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