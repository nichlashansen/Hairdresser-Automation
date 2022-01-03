using System.Net;
using HairDresserAPI.Controllers;
using Xunit;

namespace HairDresserTests;

public class DigitalOceanTest
{
    [Fact]
    public void CreateDropletSuccessfully()
    {
        //Arrange
        var digitalOceanController = new DigitalOceanController();
        string name = "Test-Droplet";
        string region = "fra1";
        string sizeSlug = "s-1vcpu-1gb";
        string image = "ubuntu-21-04-x64";
        string tag = "Test DROPLET";
        
        //Act
        var responseCode = digitalOceanController.CreateDroplet(name,region,sizeSlug,image,tag);
        
        //Assert
        Assert.Equal(HttpStatusCode.Accepted,responseCode.Result);

    }
}