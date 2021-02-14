using IdentityModel;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace IdentityServer.Infrastructure.Persistence
{
  public static class SeedData
  {
    public static void EnsureSeedData(IServiceProvider provider)
    {
      //var configuration = provider.GetRequiredService<IConfiguration>();
      var configuration = ApplicationConfiguration.Instance.GetSection("IdentityServer");
      provider.GetRequiredService<AppIdentityDbContext>().Database.Migrate();
      provider.GetRequiredService<AppPersistedGrantDbContext>().Database.Migrate();
      provider.GetRequiredService<AppConfigurationDbContext>().Database.Migrate();

      Console.WriteLine("Start Seed Data");

      var context = provider.GetRequiredService<AppConfigurationDbContext>();
      if (!context.Clients.Any())
      {
        var clients = new List<Client>();
        configuration.GetSection("Clients").Bind(clients);
        foreach (var client in clients)
          context.Clients.Add(client.ToEntity());
        context.SaveChanges();
      }
      if (!context.ApiResources.Any())
      {
        var apiResources = new List<ApiResource>();
        configuration.GetSection("ApiResources").Bind(apiResources);
        foreach (var apiResource in apiResources)
          context.ApiResources.Add(apiResource.ToEntity());
        context.SaveChanges();
      }
      if (!context.ApiScopes.Any())
      {
        var apiScopes = new List<ApiScope>();
        configuration.GetSection("ApiScopes").Bind(apiScopes);
        foreach (var apiScope in apiScopes)
          context.ApiScopes.Add(apiScope.ToEntity());
        context.SaveChanges();
      }
      if (!context.IdentityResources.Any())
      {
        var identityResources = new List<IdentityResource>();
        configuration.GetSection("IdentityResources").Bind(identityResources);
        foreach (var identityResource in identityResources)
          context.IdentityResources.Add(identityResource.ToEntity());
        context.SaveChanges();
      }

      Console.WriteLine("Completed Seed Data");

      string secretEncrypt = ApplicationConfiguration.secretKey.ToSha256();
      Console.WriteLine($"Your client_secret for '{ApplicationConfiguration.secretKey}'.ToSha256 : " + secretEncrypt);

      var _sysUserInfo = configuration.GetSection("DefaultSystemAdministrator");
      var userMgr = provider.GetRequiredService<UserManager<AppUser>>();
      var sysUser = userMgr.FindByNameAsync(_sysUserInfo.GetValue<string>("UserName")).Result;
      if (sysUser == null)
      {
        sysUser = new AppUser
        {
          Name = _sysUserInfo.GetValue<string>("Name"),
          UserName = _sysUserInfo.GetValue<string>("UserName"),
          Email = _sysUserInfo.GetValue<string>("UserName"),
          EmailConfirmed = true
        };
        var result = userMgr.CreateAsync(sysUser, _sysUserInfo.GetValue<string>("Password")).Result;
        if (!result.Succeeded)
        {
          throw new Exception(result.Errors.First().Description);
        }

        result = userMgr.AddClaimsAsync(sysUser, new Claim[]{
                            new Claim(JwtClaimTypes.Name, sysUser.Name),
                            new Claim(JwtClaimTypes.PreferredUserName, sysUser.UserName),
                            new Claim(JwtClaimTypes.Email, sysUser.Email),
                            new Claim(JwtClaimTypes.Role, "archiveuser"),
                        }).Result;
        if (!result.Succeeded)
        {
          throw new Exception(result.Errors.First().Description);
        }
        Console.WriteLine("DefaultSystemAdministrator created");
      }
      else
      {
        Console.WriteLine("DefaultSystemAdministrator already exists");
      }
    }
  }
}