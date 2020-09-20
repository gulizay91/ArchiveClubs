using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookArchive.API.Configuration
{
  public static class IdentityServerConfiguration
  {
    /// <summary>
    /// Configures the service.
    /// </summary>
    /// <param name="services">The services.</param>
    public static void ConfigureService(IServiceCollection services)
    {
      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer("Bearer", options =>
      {
        options.Authority = "http://localhost:5000"; // identityserver.api
        options.Audience = "BookArchive_API"; // APIResources.Name
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters.ValidateAudience = true;
        options.TokenValidationParameters.ValidIssuer = "http://localhost:5000"; //jwtToken.Payload.iss
        options.TokenValidationParameters.ValidateIssuer = true;
        options.TokenValidationParameters.ValidateLifetime = true; //jwtToken.Payload.exp
      });
      //.AddIdentityServerAuthentication("Bearer", options =>
      //{
      //  options.LegacyAudienceValidation = true;
      //  options.Authority = "http://localhost:5000";
      //  options.ApiSecret = "book-archive_api_secret";
      //  options.ApiName = "BookArchive_API";
      //  options.SupportedTokens = SupportedTokens.Both;
      //  // required if you want to return a 403 and not a 401 for forbidden responses
      //  options.RequireHttpsMetadata = false;
      //});
    }
  }
}
