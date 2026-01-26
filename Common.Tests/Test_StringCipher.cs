namespace Common.Tests;

public class Test_StringCipher
{
    [Theory]
    [InlineData("TestDecryption")]
    [InlineData("TestDecryption-1")]
    [InlineData("!TestDecryption#!§%$&/")]
    public void Decrypt_Encrypt(string input)
    {
        var encrypt = StringCipher.Encrypt(input);

        var result = StringCipher.Decrypt(encrypt);

        Assert.Equal(input, result);
    }

}
