using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using Shared.Configuration;
using System.Collections.Generic;

namespace IdentityArchive.API.Helpers
{
  public static class Configuration
  {
    public static IEnumerable<IdentityResource> GetResources()
    {
      // => new List<IdentityResource> {
      //  new IdentityResources.OpenId()
      //};
      var identityResources = new List<IdentityResource>();
      ApplicationConfiguration.Instance.GetSection("IdentityServer:IdentityResources").Bind(identityResources);
      return identityResources;
    }
      

    public static IEnumerable<ApiResource> GetApis()
    {
      // => new List<ApiResource> {
      //  new ApiResource("BookArchiveApi"),
      //  new ApiResource("MovieArchiveApi"),
      //  new ApiResource("GameArchiveApi"),
      //};
      var apiResources = new List<ApiResource>();
      ApplicationConfiguration.Instance.GetSection("IdentityServer:APIResources").Bind(apiResources);
      return apiResources;
    }

    public static IEnumerable<Client> GetClients()
    {
      //=> new List<Client> {
      //  new Client {
      //    ClientId = "client_id",
      //    ClientSecrets = { new Secret("client_secret".ToSha256())},
      //    AllowedGrantTypes = GrantTypes.ClientCredentials,
      //    AllowedScopes = { "BookArchiveApi", "MovieArchiveApi", "GameArchiveApi" }
      //  },
      //};
      var clients = new List<Client>();
      ApplicationConfiguration.Instance.GetSection("IdentityServer:Clients").Bind(clients);
      return clients;
    }
      
  }
}
