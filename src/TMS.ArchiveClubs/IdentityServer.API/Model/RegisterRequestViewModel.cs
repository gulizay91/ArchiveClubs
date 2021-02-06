using System.ComponentModel.DataAnnotations;

namespace IdentityServer.API.Model
{
  public class RegisterRequestViewModel
  {
    [Required]
    public string Name { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
  }
}