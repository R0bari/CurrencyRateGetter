namespace RateGetters.Rates.Interfaces
{
    public interface IOptionalResult<out T>
    {
        public bool IsSuccess { get; }
        public T Result { get; }
        public string ErrorMessage { get; }
    }
}