using System.Text;

namespace UuidTests;

public class GuidTests
{
    [Fact]
    public void GuidGenerationTest()
    {
        var originalIdStr = Guid.Parse("12345678-9abc-def0-1234-56789abcdef0");
        var originalHexStringWithBigEndian = "78563412BC9AF0DE123456789ABCDEF0";
        var originalBinaryStringWithBigEndian =
            "01111000010101100011010000010010101111001001101011110000110111100001001000110100010101100111100010011010101111001101111011110000";
        var originalHexStringWithLittleEndian = "123456789ABCDEF0123456789ABCDEF0";
        var originalBinaryStringWithLittleEndian =
            "00010010001101000101011001111000100110101011110011011110111100000001001000110100010101100111100010011010101111001101111011110000";

        var byteArrayBigEndian = originalIdStr.ToByteArray();
        var byteArrayLittleEndian = originalIdStr.ToByteArray(true);

        Assert.Equal(originalHexStringWithBigEndian, Convert.ToHexString(byteArrayBigEndian));
        Assert.Equal(originalHexStringWithLittleEndian, Convert.ToHexString(byteArrayLittleEndian));
        Assert.Equal(originalBinaryStringWithBigEndian, ToBinaryString(byteArrayBigEndian));
        Assert.Equal(originalBinaryStringWithLittleEndian, ToBinaryString(byteArrayLittleEndian));
    }

    [Fact]
    public void CompareGuidRegistry()
    {
        var originalStringLowerCase = "12345678-9abc-def0-1234-56789abcdef0";
        var originalStringUpperCase = "12345678-9ABC-DEF0-1234-56789ABCDEF0";
        var guidLower = new Guid(originalStringLowerCase);
        var guidUpper = new Guid(originalStringUpperCase);

        Assert.Equal(guidLower, guidUpper);
    }

    private static string ToBinaryString(Span<byte> bytes)
    {
        var sb = new StringBuilder();
        foreach (var byteBE in bytes)
        {
            sb.Append(Convert.ToString(byteBE, toBase: 2).PadLeft(8, '0'));
        }
        return sb.ToString();
    }
}