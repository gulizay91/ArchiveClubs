using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer.API.Model;
using IdentityServer.Infrastructure.Persistence;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AccountController : ControllerBase
  {
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly IClientStore _clientStore;
    private readonly IEventService _events;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IIdentityServerInteractionService interaction, IAuthenticationSchemeProvider schemeProvider, IClientStore clientStore, IEventService events)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _interaction = interaction;
      _schemeProvider = schemeProvider;
      _clientStore = clientStore;
      _events = events;
    }

    [HttpPost("/api/[controller]/register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestViewModel request)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var user = new AppUser { UserName = request.Email, Name = request.Name, Email = request.Email };
      var result = await _userManager.CreateAsync(user, request.Password);
      if (!result.Succeeded)
        return BadRequest(result.Errors);

      await _userManager.AddClaimAsync(user, new Claim("userName", user.UserName));
      await _userManager.AddClaimAsync(user, new Claim("name", user.Name));
      await _userManager.AddClaimAsync(user, new Claim("email", user.Email));
      await _userManager.AddClaimAsync(user, new Claim("role", "archiveuser"));
      return Ok();
    }
  }
}
