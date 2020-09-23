using IdentityServer.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Infrastructure.Extensions
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddIdentityServerConfig(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddIdentity<AppUser, AppRole>(options =>
      {
        options.User.RequireUniqueEmail = configuration.GetValue<bool>("UserOptions:RequireUniqueEmail");
        options.User.AllowedUserNameCharacters = configuration.GetValue<string>("UserOptions:AllowedUserNameCharacters");
        options.Password.RequiredLength = configuration.GetValue<int>("PasswordOptions:RequiredLength");
        options.Password.RequiredUniqueChars = configuration.GetValue<int>("PasswordOptions:RequiredUniqueChars");
        options.Password.RequireLowercase = configuration.GetValue<bool>("PasswordOptions:RequireLowercase");
        options.Password.RequireUppercase = configuration.GetValue<bool>("PasswordOptions:RequireUppercase");
        options.Password.RequireDigit = configuration.GetValue<bool>("PasswordOptions:RequireDigit");
        options.Password.RequireNonAlphanumeric = configuration.GetValue<bool>("PasswordOptions:RequireNonAlphanumeric");
      }).AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddDefaultTokenProviders();

      services.AddIdentityServer()
              .AddDeveloperSigningCredential()
              .AddOperationalStore(options =>
              {
                options.ConfigureDbContext = builder => builder.UseSqlServer(configuration.GetValue<string>("DatabaseConnectionString"));
                options.EnableTokenCleanup = true;
              })
               .AddConfigurationStore(options =>
               {
                 options.ConfigureDbContext = builder => builder.UseSqlServer(configuration.GetValue<string>("DatabaseConnectionString"));
               })
              .AddAspNetIdentity<AppUser>();

      return services;
    }

    public static IServiceCollection AddServices<TUser>(this IServiceCollection services) where TUser : IdentityUser<int>, new()
    {
      //services.AddTransient<IProfileService, IdentityClaimsProfileService>();
      return services;
    }

    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, string connectionString)
    {
      services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(connectionString));
      services.AddDbContext<AppPersistedGrantDbContext>(options => options.UseSqlServer(connectionString));
      services.AddDbContext<AppConfigurationDbContext>(options => options.UseSqlServer(connectionString));
      return services;
    }
  }
}
