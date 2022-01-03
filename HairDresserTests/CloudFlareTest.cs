using System.Net;
using HairDresserAPI.Controllers;
using Xunit;

namespace HairDresserTests;

public class CloudFlareTest
{
    [Fact]
    public void AddCloudFlareDomainTest()
    {
        //Arrange
        string domainName = "mogensjensenertest.dk";
        var Cloudflare = new CloudFlareController();

        //Act
        var json = Cloudflare.AddDomain(domainName);
        

        //Assert
        bool result = json.Result;
        Assert.True(result);
    }
}
