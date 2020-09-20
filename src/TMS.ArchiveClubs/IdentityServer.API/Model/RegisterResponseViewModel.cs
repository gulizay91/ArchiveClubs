﻿using IdentityServer.Infrastructure.Persistence;

namespace IdentityServer.API.Model
{
  public class RegisterResponseViewModel
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public RegisterResponseViewModel(AppUser user)
    {
      Id = user.Id.ToString();
      Name = user.Name;
      Email = user.Email;
    }
  }
}
