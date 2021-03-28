namespace Shared.Configuration
{
  using Microsoft.AspNetCore.Builder;
  using Microsoft.Extensions.DependencyInjection;

  /// <summary>
  /// Defines the <see cref="CorsOrginConfiguration" />.
  /// </summary>
  public static class CorsOrginConfiguration
  {
    #region Methods

    /// <summary>
    /// Configures the specified application.
    /// </summary>
    /// <param name="app">The application.</param>
    public static void Configure(IApplicationBuilder app)
    {
      app.UseCors("AllowAll");
    }

    /// <summary>
    /// Adds localization support for the applicatin.
    /// </summary>
    /// <param name="services">.</param>
    public static void ConfigureService(IServiceCollection services)
    {
      services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()));
    }

    #endregion
  }
}
