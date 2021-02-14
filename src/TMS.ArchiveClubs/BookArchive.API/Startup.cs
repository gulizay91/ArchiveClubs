using BookArchive.API.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Configuration;
using Shared.Model;

namespace BookArchive.API
{
  public class Startup
  {
    private SwaggerConfigModel SwaggerConfig { get; set; }
    private ApiVersionConfigModel ApiVersionConfig { get; set; }

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
      SwaggerConfig = ApiConfiguration.Instance.SwaggerConfigModelInstance;
      ApiVersionConfig = ApiConfiguration.Instance.ApiVersionConfigModelInstance;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      // IdentityServerConfiguration
      IdentityServerApiConfiguration.ConfigureService(services, "BookArchive_API");

      // ApiVersionConfiguration
      ApiVersionConfiguration.ConfigureService(services, ApiVersionConfig);

      // Swagger API documentation
      SwaggerConfiguration.ConfigureService(services, SwaggerConfig);

      // Cors Orgin
      CorsOrginConfiguration.ConfigureService(services);

      services.AddHealthChecks();
      services.AddControllers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      CorsOrginConfiguration.Configure(app);

      app.UseAuthentication();
      app.UseAuthorization();

      SwaggerConfiguration.Configure(app, SwaggerConfig, apiVersionDescriptionProvider);

      app.UseHealthChecks("/hc");

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}