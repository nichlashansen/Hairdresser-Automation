using System.Net;
using DigitalOcean.API.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HairDresserAPI.Controllers;

[ApiController]
public class DigitalOceanController
{
    public string DigitalOceanToken { get; set; }

    public DigitalOceanController()
    {
        DigitalOceanToken = new ApiCredentials().DigitalOceanAPIToken;
    }
    public async Task<HttpStatusCode> CreateDroplet(string name,string region, string size, string image,string tag)
    {
        
        var digitalOcean = new DigitalOcean.API.DigitalOceanClient(DigitalOceanToken);
        var request = new Droplet {
            Name = name,
            Region = region,
            Size = size,
            Image = image,
            Tags = new List<string> { tag }
        };
        var droplet = await digitalOcean.Droplets.Create(request);

        //check if created correctly
        if (droplet.Name == name)
        {
            return HttpStatusCode.Accepted;
        }

        return HttpStatusCode.BadRequest;
    }
}