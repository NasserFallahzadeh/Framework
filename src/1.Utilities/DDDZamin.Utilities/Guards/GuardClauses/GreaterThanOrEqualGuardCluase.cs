namespace DDDZamin.Utilities.Guards.GuardClauses;

public static class GreaterThanOrEqualGuardCluase
{
    public static void GreaterThanOrEqual<T>(this Guard guard, T value, T minimumValue, IComparer<T> comparer,
        string message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException("Message");

        var comparerResult = comparer.Compare(value, minimumValue);

        if (comparerResult < 0)
            throw new InvalidOperationException(message);
    }

    public static void GreaterThanOrEqual<T>(this Guard guard, T value, T minimumValue, string message)
        where T : IComparer<T>, IComparable =>
        guard.GreaterThanOrEqual(value, minimumValue, Comparer<T>.Default, message);
}