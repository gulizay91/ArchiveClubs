using BookArchive.API.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
      IdentityServerApiConfiguration.ConfigureService(services, "BookArchive_API");

      // Swagger API documentation
      SwaggerConfiguration.ConfigureService(services, SwaggerConfig);

      // Cors Orgin
      CorsOrginConfiguration.ConfigureService(services);

      services.AddControllers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      CorsOrginConfiguration.Configure(app);

      app.UseAuthentication();
      app.UseAuthorization();

      SwaggerConfiguration.Configure(app, SwaggerConfig);

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}