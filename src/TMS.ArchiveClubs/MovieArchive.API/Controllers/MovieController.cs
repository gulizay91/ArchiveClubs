using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MovieArchive.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class MovieController : ControllerBase
  {
    [HttpGet]
    [AllowAnonymous]
    public string HealthCheck()
    {
      return "movie api standing";
    }
  }
}