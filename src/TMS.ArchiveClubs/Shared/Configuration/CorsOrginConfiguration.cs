using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Configuration
{
  public static class CorsOrginConfiguration
  {
    /// <summary>
    /// Adds localization support for the applicatin
    /// </summary>
    /// <param name="services"></param>
    public static void ConfigureService(IServiceCollection services)
    {
      services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()));
    }

    /// <summary>
    /// Configures the specified application.
    /// </summary>
    /// <param name="app">The application.</param>
    public static void Configure(IApplicationBuilder app)
    {
      app.UseCors("AllowAll");
    }
  }
}