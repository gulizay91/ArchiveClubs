using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace BookArchive.API.Configuration
{
  public static class SwaggerConfiguration
  {
    /// <summary>
    /// Configures the service.
    /// </summary>
    /// <param name="services">The services.</param>
    public static void ConfigureService(IServiceCollection services)
    {
      // Swagger API documentation
      // Register the Swagger generator, defining one or more Swagger documents
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1",
        new OpenApiInfo
        {
          Title = "BookArchive API",
          Description = "Book Archive API for ArchiveClubs",
          TermsOfService = new Uri("https://example.com/terms"),
          Contact = new OpenApiContact
          {
            Name = "Güliz AY",
            Email = "gulizay91@gmail.com",
            Url = new Uri("https://twitter.com/GlizAY"),
          },
          License = new OpenApiLicense
          {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
          }
        });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
          In = ParameterLocation.Header,
          Description = "Please insert JWT with Bearer into field. Example: \"Authorization: Bearer {token}\"",
          Name = "Authorization",
          Type = SecuritySchemeType.ApiKey
        });

        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
      });
    }

    /// <summary>
    /// Configures the specified application.
    /// </summary>
    /// <param name="app">The application.</param>
    public static void Configure(IApplicationBuilder app)
    {
      // This will redirect default url to Swagger url
      var option = new RewriteOptions();
      option.AddRedirect("^$", "swagger");
      app.UseRewriter(option);

      app.UseStaticFiles();

      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger();

      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ArchiveClubs BookArchive API V1.0");
      });
    }
  
  }
}
