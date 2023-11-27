namespace UuidTests;

using FoodTech.Span.SpanStringRefact;

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
    public void Char()
    {
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

}