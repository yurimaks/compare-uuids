namespace Uuid;

internal static class ThrowHelper
{
    public static string ThrowInvalidUuidFormatSpecification()
    {
        throw new FormatException("InvalidUuidFormatSpecification");
    }

    public static void ThrowCannotBeNull()
    {
        throw new ArgumentNullException("Cannot be null.");
    }

    public static void ThrowOutOfRange16Bytes()
    {
        throw new ArgumentOutOfRangeException("Must be exactly 16 bytes in length.");
    }
}