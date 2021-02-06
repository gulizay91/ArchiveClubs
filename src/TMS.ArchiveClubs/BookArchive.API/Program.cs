using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Shared.Configuration;

namespace BookArchive.API
{
  public static class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
              webBuilder.UseUrls(ApplicationConfiguration.Instance.GetValue<string>("BookArchiveApi:Url"));
            });
  }
}