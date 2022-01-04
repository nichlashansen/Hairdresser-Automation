using System.Net;
using CloudFlare.Client.Api.Zones;
using HairDresserAPI.Controllers;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Xunit;

namespace HairDresserTests;

public class CloudFlareTest
{
    //use same domain for adding, and updating
    private string DomainName = "mogensjensenertest.dk";
    
    [Fact]
    public void AddCloudFlareDomainTest()
    {
        //Arrange
        var cloudflare = new CloudFlareController();

        //Act
        var json = cloudflare.AddDomain(DomainName);
        var result = json.Result;

        //Assert
        Assert.Equal(DomainName,result.Result.Name);
    }

    [Fact]
    public async void UpdateDomainTest()
    {
        //Arrange
        string serverIp = "104.131.95.246";
        var cloudflare = new CloudFlareController();
        
        //Act
        var json = await cloudflare.UpdateDomainDNS("betterback.dk",serverIp);

        //Assert
        Assert.NotStrictEqual(Results.Ok(),json);


    }
}
