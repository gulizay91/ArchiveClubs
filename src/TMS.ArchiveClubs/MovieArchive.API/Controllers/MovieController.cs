using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MovieArchive.API.Controllers
{
  [Route("api/v{version:apiVersion}/[controller]")]
  [ApiController]
  [ApiVersion("1.0")]
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