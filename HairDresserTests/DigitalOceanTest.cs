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
        string tag = "Test DROPLET";
        
        //Act
        var responseCode = digitalOceanController.CreateDroplet(name,tag);
        
        //Assert
        Assert.Equal(HttpStatusCode.Accepted,responseCode.Result);

    }
}