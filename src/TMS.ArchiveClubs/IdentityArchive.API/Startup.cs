using System;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Configuration;

namespace IdentityArchive.API
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();

      services.Configure<IISOptions>(iis =>
      {
        iis.AuthenticationDisplayName = "Windows";
        iis.AutomaticAuthentication = false;
      });

      #region IdentityServer4
      var connectionString = ApplicationConfiguration.Instance.GetValue<string>("IdentityServer:DatabaseConnectionString");

      var useRowNumberForPaging = ApplicationConfiguration.Instance.GetValue<int>("MSSQLVersion") >= 2012 ? false : true;

      services.AddIdentityServer(options =>
      {
        options.Events.RaiseSuccessEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseErrorEvents = true;

      })
      .AddSecretValidator<PrivateKeyJwtSecretValidator>()
      .AddAppAuthRedirectUriValidator()
      .AddInMemoryIdentityResources(Helpers.Configuration.GetResources())
      .AddInMemoryApiResources(Helpers.Configuration.GetApis())
      .AddInMemoryClients(Helpers.Configuration.GetClients());

      services.AddSession(options =>
      {
        options.IdleTimeout = TimeSpan.FromSeconds(60);
      });

      //services.AddExternalIdentityProviders(); <-- maybe in the future, should we ever need an external provider like facebook

      #endregion
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseIdentityServer();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapDefaultControllerRoute();
      });
    }
  }
}
