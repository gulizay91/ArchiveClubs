namespace Shared.Configuration
{
  using Microsoft.AspNetCore.Authentication.JwtBearer;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using System.Linq;

  /// <summary>
  /// Defines the <see cref="IdentityServerApiConfiguration" />.
  /// </summary>
  public static class IdentityServerApiConfiguration
  {
    #region Methods

    /// <summary>
    /// Configures the service.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="apiResourceName">The apiResourceName<see cref="string"/>.</param>
    public static void ConfigureService(IServiceCollection services, string apiResourceName)
    {
      string identityServerUrl = ApplicationConfiguration.Instance.GetValue<string>("IdentityServer:Url");
      string apiSecret = ApplicationConfiguration.Instance.GetSection("IdentityServer:APIResources").GetChildren()
                              .Select(r => new
                              {
                                Name = r.GetValue<string>("Name"),
                                ApiSecret = r.GetSection("ApiSecrets") != null ? r.GetSection("ApiSecrets:0").GetValue<string>("Value") : ""
                              })
                              .FirstOrDefault(r => r.Name == apiResourceName)?.ApiSecret;
      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer("Bearer", options =>
      {
        options.Authority = identityServerUrl;
        options.Audience = apiResourceName;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters.ValidateAudience = true;
        options.TokenValidationParameters.ValidIssuer = identityServerUrl;
        options.TokenValidationParameters.ValidateIssuer = true;
        options.TokenValidationParameters.ValidateLifetime = true;
      });
    }

    #endregion
  }
}
