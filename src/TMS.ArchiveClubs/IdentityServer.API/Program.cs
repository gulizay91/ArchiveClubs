using IdentityServer.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Configuration;

namespace IdentityServer.API
{
  public static class Program
  {
    public static void Main(string[] args)
    {
      var host = CreateHostBuilder(args).Build();
      using (var scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
      {
        SeedData.EnsureSeedData(scope.ServiceProvider);
      }
      host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
              webBuilder.UseUrls(ApplicationConfiguration.Instance.GetValue<string>("IdentityServer:Url"));
            });
  }
}
