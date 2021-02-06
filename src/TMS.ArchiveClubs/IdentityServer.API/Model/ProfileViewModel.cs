using IdentityModel.Client;
using IdentityServer.Infrastructure.Persistence;
using System.Collections.Generic;

namespace IdentityServer.API.Model
{
  public class ProfileViewModel
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public string Token { get; set; }

    public int Expiry { get; set; }

    public ProfileViewModel()
    {
    }

    public ProfileViewModel(AppUser user, TokenResponse UToken = null)
    {
      Id = user.Id;
      FirstName = user.Name;
      //LastName = user.LastName;
      Email = user.Email;
      Token = UToken.AccessToken;
      Expiry = UToken.ExpiresIn;
    }

    public static IEnumerable<ProfileViewModel> GetUserProfiles(IEnumerable<AppUser> users)
    {
      var profiles = new List<ProfileViewModel>();
      foreach (AppUser user in users)
      {
        profiles.Add(new ProfileViewModel(user));
      }

      return profiles;
    }
  }
}