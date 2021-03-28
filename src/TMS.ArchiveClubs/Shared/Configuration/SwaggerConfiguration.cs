namespace Shared.Configuration
{
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Mvc.Abstractions;
  using Microsoft.AspNetCore.Mvc.ApiExplorer;
  using Microsoft.AspNetCore.Mvc.Versioning;
  using Microsoft.AspNetCore.Rewrite;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.OpenApi.Models;
  using Shared.Model;
  using System;
  using System.Linq;

  /// <summary>
  /// Defines the <see cref="SwaggerConfiguration" />.
  /// </summary>
  public class SwaggerConfiguration
  {
    #region Methods

    /// <summary>
    /// Configures the specified application.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <param name="model">The model<see cref="SwaggerConfigModel"/>.</param>
    /// <param name="apiVersionDescriptionProvider">The apiVersionDescriptionProvider<see cref="IApiVersionDescriptionProvider"/>.</param>
    public static void Configure(IApplicationBuilder app, SwaggerConfigModel model, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
      // This will redirect default url to Swagger url
      var option = new RewriteOptions();
      option.AddRedirect("^$", "swagger");
      app.UseRewriter(option);

      app.UseStaticFiles();

      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger();

      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(options =>
      {
        //options.SwaggerEndpoint(model.SwaggerUIOptions.Url, model.SwaggerUIOptions.Name);
        // build a swagger endpoint for each discovered API version
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
          options.SwaggerEndpoint($"/swagger/{model.ApiName}{description.GroupName}/swagger.json", $"{description.GroupName.ToUpperInvariant()}");
        }
      });
    }

    /// <summary>
    /// Configures the service.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="model">The model<see cref="SwaggerConfigModel"/>.</param>
    public static void ConfigureService(IServiceCollection services, SwaggerConfigModel model)
    {
      var apiVersionDescriptionProvider = services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();

      // Swagger API documentation
      // Register the Swagger generator, defining one or more Swagger documents
      services.AddSwaggerGen(option =>
      {
        option.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
          option.SwaggerDoc($"{model.ApiName}{description.GroupName}",
              new OpenApiInfo
              {
                Title = model.OpenApiInfo.Title,
                Description = model.OpenApiInfo.Description,
                Version = description.ApiVersion.ToString(),
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
        }

        option.DocInclusionPredicate((documentName, apiDescription) =>
        {
          var actionApiVersionModel = apiDescription.ActionDescriptor.GetApiVersionModel(ApiVersionMapping.Explicit | ApiVersionMapping.Implicit);

          if (actionApiVersionModel == null)
          {
            return true;
          }

          if (actionApiVersionModel.DeclaredApiVersions.Any())
          {
            return actionApiVersionModel.DeclaredApiVersions.Any(v =>
            $"{model.ApiName}v{v}" == documentName);
          }
          return actionApiVersionModel.ImplementedApiVersions.Any(v =>
              $"{model.ApiName}v{v}" == documentName);
        });

        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
          In = ParameterLocation.Header,
          Description = "Please insert JWT with Bearer into field. Example: \"Authorization: Bearer {token}\"",
          Name = "Authorization",
          Type = SecuritySchemeType.ApiKey,
          Scheme = "Bearer"
        });

        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
          {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
          }
        });

        // Set the comments path for the Swagger JSON and UI.
        option.IncludeXmlComments(model.XmlPath);
      });
    }

    #endregion
  }
}
