using CloudFlare.Client;
using CloudFlare.Client.Api.Authentication;
using CloudFlare.Client.Api.Result;
using CloudFlare.Client.Api.Zones;
using Microsoft.AspNetCore.Mvc;

namespace HairDresserAPI.Controllers;
[ApiController]
[Route("/Cloudflare")]
public class CloudFlareController
{
    private CloudFlareClient CloudFlare { get; set; }
    private ApiKeyAuthentication CloudflareAuth{ get; set; }
    private string cloudflareEmail { get; set; }
    private string cloudflareApi { get; set; }
    public CloudFlareController()
    {
        cloudflareEmail = new ApiCredentials().CloudflareEmail;
        cloudflareApi = new ApiCredentials().CloudflareToken;
        CloudflareAuth = new ApiKeyAuthentication(cloudflareEmail, cloudflareApi);
        CloudFlare = new CloudFlareClient(CloudflareAuth);

    }

    [HttpPost]
    [Route("AddDomain")]
    public async Task<bool> AddDomain(string domainName)
    {
        NewZone zone = new NewZone();
        zone.Name = domainName;
        zone.JumpStart = true;
        var result = await CloudFlare.Zones.AddAsync(zone);
        return result.Success;
    }
}