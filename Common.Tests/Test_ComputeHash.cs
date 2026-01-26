namespace Common.Tests;

public class Test_ComputeHash
{
    [Theory]
    [InlineData(null, "salt", "")]
    [InlineData("pin", null, "")]
    public void ComputeHash_ReturnsEmptyString_WhenInputIsNullOrEmpty(string? pin, string? salt, string expected)
    {
        var result = HashHelper.ComputeHash(pin, salt);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ComputeHash_ReturnsSameHash_ForSameInput()
    {
        var hash1 = HashHelper.ComputeHash("1234", "salt");
        var hash2 = HashHelper.ComputeHash("1234", "salt");

        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void ComputeHash_ReturnsDifferentHash_ForDifferentPin()
    {
        var hash1 = HashHelper.ComputeHash("1234", "salt");
        var hash2 = HashHelper.ComputeHash("4321", "salt");

        Assert.NotEqual(hash1, hash2);
    }


}
