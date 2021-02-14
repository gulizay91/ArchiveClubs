using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieArchive.API.Configuration;
using Shared.Configuration;
using Shared.Model;

namespace MovieArchive.API
{
  public class Startup
  {
    private SwaggerConfigModel SwaggerConfig { get; set; }

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
      SwaggerConfig = ApiConfiguration.Instance.SwaggerConfigModelInstance;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      // IdentityServerConfiguration
      IdentityServerApiConfiguration.ConfigureService(services, "MovieArchive_API");

      // Swagger API documentation
      SwaggerConfiguration.ConfigureService(services, SwaggerConfig);

      // Cors Orgin
      CorsOrginConfiguration.ConfigureService(services);

      services.AddHealthChecks();
      services.AddControllers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      //app.UseHttpsRedirection();

      app.UseRouting();

      CorsOrginConfiguration.Configure(app);

      app.UseAuthentication();
      app.UseAuthorization();

      SwaggerConfiguration.Configure(app, SwaggerConfig);

      app.UseHealthChecks("/hc");

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}