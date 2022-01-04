using System.Text.Json;
using CloudFlare.Client;
using CloudFlare.Client.Api.Authentication;
using CloudFlare.Client.Api.Result;
using CloudFlare.Client.Api.Zones;
using CloudFlare.Client.Api.Zones.DnsRecord;
using CloudFlare.Client.Client.Zones;
using CloudFlare.Client.Contexts;
using CloudFlare.Client.Enumerators;
using Microsoft.AspNetCore.Mvc;

namespace HairDresserAPI.Controllers;
[Route("/Cloudflare")]
[ApiController]
public class CloudFlareController
{
    private CloudFlareClient CloudFlare { get; set; }
    private string CloudflareEmail { get; set; }
    private string CloudflareApi { get; set; }
    public CloudFlareController()
    {
        CloudflareEmail = new ApiCredentials().CloudflareEmail;
        CloudflareApi = new ApiCredentials().CloudflareToken;
        CloudFlare = new CloudFlareClient(CloudflareEmail, CloudflareApi);

    }

    [HttpPost]
    [Route("AddDomain")]
    public async Task<CloudFlareResult<Zone>> AddDomain(string domainName)
    {
        using (CloudFlare)
        {
            NewZone zone = new NewZone();
            zone.Name = domainName;
            zone.JumpStart = true;
            var result = await CloudFlare.Zones.AddAsync(zone);
            return result;
        }
        
    }

    [HttpPut]
    [Route("UpdateDNS")]
    public async Task<IResult> UpdateDomainDNS(string domainName, string serverIp)
    {
        using (CloudFlare)
        {
            var zoneIdTask = GetZoneId(domainName);
            
            NewDnsRecord wwwDnsRecord = new NewDnsRecord()
            {
                Type = DnsRecordType.A,
                Content = serverIp,
                Name = "www." + domainName,
                Ttl = 1,
                Proxied = true
            };

            NewDnsRecord newDnsRecord = new NewDnsRecord()
            {
                Type = DnsRecordType.A,
                Content = serverIp,
                Name = domainName,
                Ttl = 1,
                Proxied = true
            };

            var _ = await CheckIfARecordsExistsAndDeletes(zoneIdTask.Result);
            var baseDns = await CloudFlare.Zones.DnsRecords.AddAsync(zoneIdTask.Result,newDnsRecord);
            var addAsync = await CloudFlare.Zones.DnsRecords.AddAsync(zoneIdTask.Result,wwwDnsRecord);
            
            if (addAsync.Success && baseDns.Success)
            {
                return Results.Ok();
            }
            
            return Results.Problem(addAsync.Errors.ToString());


        }
    }


  private async Task<string> GetZoneId(string domainName)
{
        ZoneFilter filter = new ZoneFilter();
        filter.Name = domainName;
        var result = await CloudFlare.Zones.GetAsync(filter);
        var correctZone = result.Result[0];
        return correctZone.Id;
}

  private async Task<bool> CheckIfARecordsExistsAndDeletes(string zoneId)
  {
      var filter = new DnsRecordFilter();
      filter.Type = DnsRecordType.A;
      var result = await CloudFlare.Zones.DnsRecords.GetAsync(zoneId, filter);
      if (result.Result.Count !=0)
      {
          DeleteExistingARecords(result);
      }

      return true;
  }

  private async void DeleteExistingARecords(CloudFlareResult<IReadOnlyList<DnsRecord>> dnsRecords)
  {
      foreach (var dns in dnsRecords.Result)
      {
          await CloudFlare.Zones.DnsRecords.DeleteAsync(dns.ZoneId, dns.Id);
      }
  }

}

