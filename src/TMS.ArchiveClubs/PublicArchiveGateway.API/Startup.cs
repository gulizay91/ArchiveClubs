
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace PublicArchiveGateway.API
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
            //const string authenticationProviderKey = "IdentityApiKey";
            //string identityServerUrl = ApplicationConfiguration.Instance.GetValue<string>("IdentityServer:Url");

            //// NUGET - Microsoft.AspNetCore.Authentication.JwtBearer
            //services.AddAuthentication()
            // .AddJwtBearer(authenticationProviderKey, x =>
            // {
            //     x.Authority = identityServerUrl;
            //     x.RequireHttpsMetadata = false;
            //     x.TokenValidationParameters = new TokenValidationParameters
            //     {
            //         ValidateAudience = false
            //     };
            // });

            services.AddOcelot();
            services.AddSwaggerForOcelot(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseSwaggerForOcelotUI(opt => opt.PathToSwaggerGenerator = "/swagger/docs");

            app.UseEndpoints(endpoints => endpoints.MapGet("/apigateway/", async context => await context.Response.WriteAsync("ArchiveClubs Api Gateway!")));

            app.UseOcelot().Wait();
        }
    }
}
