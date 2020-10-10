using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Configuration
{
    public static class IdentityServerApiConfiguration
    {
        /// <summary>
        /// Configures the service.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void ConfigureService(IServiceCollection services, string apiResourceName)
        {
            string identityServerUrl = ApplicationConfiguration.Instance.GetValue<string>("IdentityServer:Url");
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = identityServerUrl; // identityserver.api
                options.Audience = apiResourceName; // APIResources.Name
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters.ValidateAudience = true;
                options.TokenValidationParameters.ValidIssuer = identityServerUrl; //jwtToken.Payload.iss
                options.TokenValidationParameters.ValidateIssuer = true;
                options.TokenValidationParameters.ValidateLifetime = true; //jwtToken.Payload.exp
            });
            //.AddIdentityServerAuthentication("Bearer", options =>
            //{
            //  options.LegacyAudienceValidation = true;
            //  options.Authority = identityServerUrl;
            //  options.ApiSecret = "book-archive_api_secret";
            //  options.ApiName = apiResourceName;
            //  options.SupportedTokens = SupportedTokens.Both;
            //  // required if you want to return a 403 and not a 401 for forbidden responses
            //  options.RequireHttpsMetadata = false;
            //});
        }
    }
}
