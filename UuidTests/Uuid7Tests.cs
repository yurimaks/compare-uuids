namespace UuidTests;

using FoodTech.Uuid.Final;

public class Uuid7Tests
{
    [Fact]
    public void Uuid7Ctor()
    {
        var uuid1 = new Uuid7();
        Assert.NotEqual(Uuid7.Empty, uuid1);

        var guid = Guid.Parse("12345678-9abc-def0-1234-56789abcdef0");
        var uuidFromGuid = new Uuid7(guid);
        Assert.Equal(guid.ToString(), uuidFromGuid.ToString());

        var bytes = Convert.FromHexString("123456789ABCDEF0123456789ABCDEF0");
        var uuidFromBytes = new Uuid7(bytes);
        Assert.Equal(guid.ToString(), uuidFromBytes.ToString());

        var fromStatic = Uuid7.NewUuid7();
        var byteArray = fromStatic.ToByteArray();
        var toUuid = new Uuid7(byteArray);
        Assert.Equal(fromStatic.ToString(), toUuid.ToString());
        Assert.Equivalent(byteArray, toUuid.ToByteArray());

    }

    [Fact]
    void Uuid7Max()
    {
        var uuid = Uuid7.MaxValue;
        var uuidArray = uuid.ToByteArray();
        Assert.All(uuidArray, b => Assert.Equal(255, b));
        Assert.Equal("ffffffff-ffff-ffff-ffff-ffffffffffff", uuid.ToString());
    }

    [Fact]
    void Uuid7Min()
    {
        var uuid = Uuid7.MinValue;
        var uuidArray = uuid.ToByteArray();
        Assert.All(uuidArray, b => Assert.Equal(0, b));
        Assert.Equal("00000000-0000-0000-0000-000000000000", uuid.ToString());
    }

    [Fact]
    void Uuid7Empty()
    {
        var uuid = Uuid7.Empty;
        var uuidArray = uuid.ToByteArray();
        Assert.All(uuidArray, b => Assert.Equal(0, b));
        Assert.Equal("00000000-0000-0000-0000-000000000000", uuid.ToString());
    }

    [Fact]
    public void Uuid7RoundTrip()
    {
        var originalId = new Uuid7();

        var explicitGuid = (Guid)originalId;
        Assert.Equal(originalId.ToString(), explicitGuid.ToString());

        Guid implicitGuid = originalId;
        Assert.Equal(originalId.ToString(), implicitGuid.ToString());

        var roundTripParsedId = ((Uuid7)((Guid)originalId));
        Assert.Equal(originalId, roundTripParsedId);

        Assert.Equal(explicitGuid, originalId.ToGuid());
        Assert.Equal(implicitGuid, originalId.ToGuid());

        var result = originalId == roundTripParsedId;
        Assert.True(result);
    }

    [Fact]
    public void ParseString()
    {
        var originalString = "12345678-9abc-def0-1234-56789abcdef0";
        var result = Uuid7.Parse(originalString);
        Assert.Equal(originalString, result.ToString());
    }

    [Fact]
    public void UuidParseFromCtor()
    {
        var originalString = "12345678-9abc-def0-1234-56789abcdef0";
        var result = new Uuid7(originalString);
        Assert.Equal(originalString, result.ToString());
    }

    [Fact]
    public void UuidParseWithBrakesFromString()
    {
        var result = new Uuid7("{12345678-9abc-def0-1234-56789abcdef0}");
        Assert.Equal("12345678-9abc-def0-1234-56789abcdef0", result.ToString());
    }

    [Fact]
    public void UuidParseWithBrakes()
    {
        var result = Uuid7.Parse("{12345678-9abc-def0-1234-56789abcdef0}");
        Assert.Equal("12345678-9abc-def0-1234-56789abcdef0", result.ToString());
    }
    
    [Fact]
    public void UuidParse()
    {
        var result = Uuid7.Parse("12345678-9abc-def0-1234-56789abcdef0");
        Assert.Equal("12345678-9abc-def0-1234-56789abcdef0", result.ToString());
    }
    
    [Fact]
    public void UuidTryParseWithoutFormat()
    {
        var success = Uuid7.TryParse("12345678-9abc-def0-1234-56789abcdef0", out Uuid7 result);
        Assert.True(success);
        Assert.Equal("12345678-9abc-def0-1234-56789abcdef0", result.ToString());
    }
    
    [Fact]
    public void UuidTryParse()
    {
        var success = Uuid7.TryParse("12345678-9abc-def0-1234-56789abcdef0", null, out Uuid7 result);
        Assert.True(success);
        Assert.Equal("12345678-9abc-def0-1234-56789abcdef0", result.ToString());
    }

    [Theory]
    [InlineData("")] //Unrecognized Guid format
    [InlineData(null)] //Value cannot be null. (Parameter 'input')
    [InlineData("asdasd")] //Unrecognized Guid format.
    [InlineData("asd-asd-asd-asd")] //Unrecognized Guid format
    [InlineData("12345678-9abc-def0-1234-56789abcdef0_F")] //extra char //One of the identified items was in an invalid format.
    [InlineData("12345678-9abc-def0-1234-56789abcdef0_фа")] //extra length with cyrillic char //Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).
    [InlineData("12345678-9abc-def0-1234-56789abcd_фа")] //cyrillic //Guid string should only contain hexadecimal characters.
    [InlineData("1234567-89ab-def0-1234-56789abcd_фа")] //wrong hyphen order ////Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)
    [InlineData("12345678-9ab-cdef0-1234-56789abcd_фа")] //wrong hyphen order //Dashes are in the wrong position for GUID parsing.
    public void UuidInvalidString(string? input)
    {
        if (input == null)
        {
            Assert.Throws<ArgumentNullException>(() => Uuid7.Parse(input));
            Assert.Throws<ArgumentNullException>(() => new Uuid7(input));
        }
        else
        {
            Assert.Throws<FormatException>(() => Uuid7.Parse(input));
            Assert.Throws<FormatException>(() => new Uuid7(input));
        }

        var parsingResult = Uuid7.TryParse(input, out Uuid7 parsedValue);
        Assert.False(parsingResult);
        Assert.True(parsedValue == Uuid7.Empty);
    }

    // BitArray bitArray = new BitArray(new[] { true, false, true, false, });
    // byte[] bytes = new byte[bitArray.Length];
    // bitArray.CopyTo(bytes, 0);
    // char[] result = Encoding.Unicode.GetString(bytes).ToCharArray();

    // var chars = new[]
    // {
    //     'j',
    //     '\u006A',
    //     '\x006A',
    //     (char)106,
    // };
    // Console.WriteLine(string.Join(" ", chars));  // output: j j j j
}