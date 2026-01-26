namespace Common.Tests;

public class Test_kvMessage()
{
    [Fact]
    public void Default_Value()
    {
        var message = new kvMessage();
        Assert.Null(message.accountInfo);
        Assert.Equal(string.Empty, message.clientId);
        Assert.Equal(string.Empty, message.cmd);
        Assert.Null(message.data);
        Assert.Null(message.exData);
    }
    [Fact]
    public void KvMessage_ToString_ReturnsCorrectFormat()
    {
        var message = new kvMessage
        {
            clientId = "12345",
            cmd = "test",
            data = new { Name = "TestData" }
        };

        var result = message.ToString();

        Assert.Contains("cmd=test", result);
        Assert.Contains("clientid=12345", result);
        Assert.Contains("data=", result);
    }
}