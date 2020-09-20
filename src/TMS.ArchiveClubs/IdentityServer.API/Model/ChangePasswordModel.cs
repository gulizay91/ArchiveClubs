using System.ComponentModel.DataAnnotations;

namespace IdentityServer.API.Model
{
  public class ChangePasswordModel
    {
        public int Id { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PasswordAgain { get; set; }
    }
}