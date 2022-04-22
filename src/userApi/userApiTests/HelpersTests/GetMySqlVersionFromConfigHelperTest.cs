using System;
using userApi.Helpers;
using Xunit;

namespace userApiTests.HelpersTests;

public class GetMySqlVersionFromConfigHelperTest
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Error_InputNullOrEmpty(string testValue)
    {
        var exception = Assert.Throws<ArgumentNullException>(()=> GetMySqlVersionFromConfigHelper.GetVersion(testValue));
        Assert.Contains("null", exception.Message);
        Assert.Contains(GetMySqlVersionFromConfigHelper.ErrorMessage, exception.Message);
    }

    [Theory]
    [InlineData("2.222")]
    [InlineData("2.222.3.1")]
    [InlineData("Test")]
    public void Error_InputInvalidFormatNo3Dots(string testValue)
    {
        var exception = Assert.Throws<ArgumentException>(()=> GetMySqlVersionFromConfigHelper.GetVersion(testValue));
        Assert.Contains(GetMySqlVersionFromConfigHelper.ErrorMessage, exception.Message);
    }
    
    [Theory]
    [InlineData("8.test.5")]
    [InlineData("test.8.5")]
    [InlineData("8.5.test")]
    public void Error_InputInvalidFormatNotIntValues(string testValue)
    {
        var exception = Assert.Throws<ArgumentException>(()=> GetMySqlVersionFromConfigHelper.GetVersion(testValue));
        Assert.Contains("Value on position", exception.Message);
        Assert.Contains(GetMySqlVersionFromConfigHelper.ErrorMessage, exception.Message);
    }
    
    [Fact]
    public void Valid_VersionReturned()
    {
        Version version = GetMySqlVersionFromConfigHelper.GetVersion("8.31.1");
        Assert.Equal(8, version.Major);
        Assert.Equal(31, version.Minor);
        Assert.Equal(1, version.Build);
    }
}