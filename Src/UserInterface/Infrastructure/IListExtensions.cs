namespace UserInterface.Infrastructure
{
    public static class IListExtensions
    {
        public static (T first, T second, IList<T> rest) Deconstruct<T>(this IList<T> list) where T : notnull => (
                list.Count > 0 ? list[0] : default,
                list.Count > 1 ? list[1] : default,
                list.Skip(2).ToList());
    }
}
