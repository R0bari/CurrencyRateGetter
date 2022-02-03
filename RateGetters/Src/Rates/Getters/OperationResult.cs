using RateGetters.Rates.Interfaces;

namespace RateGetters.Rates.Getters
{
    public record OperationResult<T> : IOptionalResult<T>
    {
        public T Result { get; }
        public bool IsSuccess { get; }
        public string ErrorMessage { get; } = "";
        
        private OperationResult(bool isSuccess, T result)
        {
            IsSuccess = isSuccess;
            Result = result;
        }

        private OperationResult(bool isSuccess, string errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static OperationResult<T> Successful(T rateForDate) => 
            new(true, rateForDate);

        public static OperationResult<T> Failed(string errorMessage) => new(false, errorMessage);

        public override string ToString()
        {
            return IsSuccess ? Result.ToString() : ErrorMessage;
        }
    }
}