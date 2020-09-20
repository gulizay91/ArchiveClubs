using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Infrastructure.Persistence
{
  public class AppUser : IdentityUser<int>
  {
    public string Name { get; set; }
  }
}
