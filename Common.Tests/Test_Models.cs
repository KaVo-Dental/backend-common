namespace Common.Tests;

public class Test_Models()
{
    [Theory]
    [InlineData("criticalerrors", "ErrorType", "404", true, true)]
    [InlineData("inHygiene", "Status", "Active", false, true)]
    public void Default_Value(string name, string attributeKey, string attributeValue, bool deleteProperty, bool expected)
    {
        var existingProperties = new List<Property>
        {
            new Property {
                Name = "criticalerrors",
                Attributes = new Dictionary<string, string> { { "ErrorType", "404" } }
            },
            new Property {
                Name = "inHygiene",
                Attributes = new Dictionary<string, string> { { "Status", "Inactive" } }
            }
        };

        var prop = new Property
        {
            Name = name,
            Attributes = new Dictionary<string, string> { { attributeKey, attributeValue } }
        };

        bool retVal = Property.MergePropertyIntoList(existingProperties, prop, deleteProperty);

        Assert.Equal(expected, retVal);
    }

}