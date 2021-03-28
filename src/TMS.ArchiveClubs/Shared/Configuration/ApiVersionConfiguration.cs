namespace Shared.Configuration
{
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Mvc.Versioning;
  using Microsoft.Extensions.DependencyInjection;
  using Shared.Model;

  /// <summary>
  /// Defines the <see cref="ApiVersionConfiguration" />.
  /// </summary>
  public static class ApiVersionConfiguration
  {
    #region Methods

    /// <summary>
    /// Configures the specified application.
    /// </summary>
    /// <param name="app">The application.</param>
    public static void Configure(IApplicationBuilder app)
    {
    }

    /// <summary>
    /// Adds localization support for the applicatin.
    /// </summary>
    /// <param name="services">.</param>
    /// <param name="model">The model<see cref="ApiVersionConfigModel"/>.</param>
    public static void ConfigureService(IServiceCollection services, ApiVersionConfigModel model)
    {
      services.AddVersionedApiExplorer(setupAction =>
      {
        setupAction.GroupNameFormat = "'v'VV";
      });

      services.AddApiVersioning(options =>
      {
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(model.DefaultApiVersionMajor, model.DefaultApiVersionMinor);//ApiVersion.Default;
        if (model.GetApiVersionFromHeader)
        {
          options.ApiVersionReader = ApiVersionReader.Combine(
            new MediaTypeApiVersionReader("version"),
            new HeaderApiVersionReader("X-Version")
          );
        }
        options.ReportApiVersions = model.ReportApiVersion;
      });
    }

    #endregion
  }
}
