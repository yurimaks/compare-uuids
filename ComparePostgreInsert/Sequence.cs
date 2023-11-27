namespace GuidVSUuid7;

public class Sequence
{
    private static readonly object _read = new object();

    private static long _i = 0;

    public static long Next()
    {
        lock (_read)
        {
            _i++;
            return _i;
        }
    }
}