using MassTransit;

namespace UuidTests;

public class NewIdTests
{
    [Fact]
    public void NewIdTest()
    {
        var newId = NewId.Next();
        var str = newId.ToString();

        var orStr = "be550000-066b-b445-f66c-08dbef666f37";
        var parsed = new NewId(orStr);

    }
}