using System.Reflection;

namespace Common.Tests;

public class Test_kvMessage()
{
    [Fact]
    public void KvMessage_Has_Expected_Properties_And_Types()
    {
        var expected = new Dictionary<string, Type>
        {
            { "accountInfo", typeof(AccountInfo) },
            { "clientId", typeof(string) },
            { "cmd", typeof(string) },
            { "data", typeof(object) },
            { "exData", typeof(object) }
        };

        var props = typeof(kvMessage).GetProperties();

        foreach (var kv in expected)
        {
            var prop = Array.Find(props, p => p.Name == kv.Key);
            Assert.NotNull(prop);

            Assert.Equal(kv.Value, prop!.PropertyType);
        }
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

    [Fact]
    public void testMsgConnectionInfoData()
    {
        var expected = new Dictionary<string, (Type type, NullabilityState nullability)>
        {
            { "clientId", (typeof(string), NullabilityState.Nullable) },
            { "accountId", (typeof(string), NullabilityState.Nullable) },
            { "contactId", (typeof(string), NullabilityState.Nullable) },
            { "accountType", (typeof(string), NullabilityState.Nullable) },
            { "jwtToken", (typeof(string), NullabilityState.Nullable) },
            { "hash", (typeof(string), NullabilityState.Nullable) }
        };

        var ctx = new NullabilityInfoContext();
        var props = typeof(MsgConnectionInfoData).GetProperties();

        foreach (var (name, contract) in expected)
        {
            var prop = props.SingleOrDefault(p => p.Name == name);
            Assert.NotNull(prop);

            Assert.Equal(contract.type, prop!.PropertyType);

            var info = ctx.Create(prop);
            Assert.Equal(contract.nullability, info.ReadState);
        }
    }
}