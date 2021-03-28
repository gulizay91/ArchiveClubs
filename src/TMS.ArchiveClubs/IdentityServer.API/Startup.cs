using IdentityServer.Infrastructure.Extensions;
using IdentityServer.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Shared.Configuration;

namespace IdentityServer.API
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
      // Configuration'i kullanmiyorum, appsettings yerine Shared.configuration.json dosyasindan tum projenin configlerini alacagim.
      var configuration = ApplicationConfiguration.Instance.GetSection("IdentityServer");
      services.AddDatabaseConfiguration(configuration.GetValue<string>("DatabaseConnectionString"))
              .AddIdentityServerConfig(configuration)
              .AddServices<AppUser>();

      // ApiVersionConfiguration
      ApiVersionConfiguration.ConfigureService(services, new Shared.Model.ApiVersionConfigModel() { DefaultApiVersionMajor = 1, DefaultApiVersionMinor = 0, GetApiVersionFromHeader = true, ReportApiVersion = false, ApiVersionList = null });

      // Cors Orgin
      CorsOrginConfiguration.ConfigureService(services);

      services.AddControllersWithViews();
      services.AddRazorPages();
      services.AddMvc(options =>
      {
        options.EnableEndpointRouting = false;
      }).SetCompatibilityVersion(CompatibilityVersion.Latest);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        IdentityModelEventSource.ShowPII = true;
      }

      app.UseHsts();
      //app.UseHttpsRedirection();

      CorsOrginConfiguration.Configure(app);

      app.UseStaticFiles();
      app.UseIdentityServer();
      app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });
      app.UseMvc(routes =>
      {
        routes.MapRoute(
             name: "default",
             template: "{controller=Home}/{action=Index}/{id?}");
      });

      // init default admin role
      //InitializeSystemAdministrator(app).GetAwaiter().GetResult();
    }

    //private async Task InitializeSystemAdministrator(IApplicationBuilder app)
    //{
    //    try
    //    {
    //        // TODO: get roles
    //        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
    //        {
    //            var callbackSender = serviceScope.ServiceProvider.GetRequiredService<IEndpointInstance>();

    //            var sendOptions = new SendOptions();
    //            sendOptions.SetDestination(ApplicationConfiguration.Instance.GetValue<string>("CustomerManagementContext:CallbacksReceiverEndpointName"));

    //            CancellationTokenSource source = new CancellationTokenSource();
    //            source.CancelAfter(TimeSpan.FromSeconds(30));
    //            var result = await callbackSender.Request<ServiceResult<bool>>(new EnsureSystemAdministratorAndDefaultRolesRequest(), sendOptions, source.Token);
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Program.Logger.LogError(e,
    //            "Message: {Message}", "ApplicationService->EnsureSystemAdministratorAndDefaultRoles failed");
    //    }
    //}
  }
}