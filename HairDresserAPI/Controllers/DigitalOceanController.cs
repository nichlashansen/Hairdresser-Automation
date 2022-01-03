using System.Net;
using DigitalOcean.API.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HairDresserAPI.Controllers;

[ApiController]
[Route("/DigitalOcean/")]
public class DigitalOceanController
{
    private string DigitalOceanToken { get; set; }
    private string UserData { get; set; }
    private string PathToYamlFile { get; set; }

    
    public DigitalOceanController()
    {
        DigitalOceanToken = new ApiCredentials().DigitalOceanAPIToken;
        PathToYamlFile = $"{AppDomain.CurrentDomain.BaseDirectory}cloud-config.yaml";
        UserData = new StreamReader(PathToYamlFile).ReadToEnd();
    }
    [HttpPost]
    [Route("CreateDroplet")]
    public async Task<HttpStatusCode> CreateDroplet(string name,string tag)
    {
        string region = "fra1";
        string size = "s-1vcpu-1gb";
        string image = "ubuntu-21-04-x64";
        
        var digitalOcean = new DigitalOcean.API.DigitalOceanClient(DigitalOceanToken);
        var request = new Droplet {
            Name = name,
            Region = region,
            Size = size,
            Image = image,
            Tags = new List<string> { tag },
            UserData = UserData
        };
        var droplet = await digitalOcean.Droplets.Create(request);

        if (droplet.Name == name)
        {
            return HttpStatusCode.Accepted;
        }

        return HttpStatusCode.BadRequest;
    }
}