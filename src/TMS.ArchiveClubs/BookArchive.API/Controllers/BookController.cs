using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookArchive.API.Controllers
{
  [Route("api/v{version:apiVersion}/[controller]")]
  [ApiController]
  [ApiVersion("1.0")]
  [ApiVersion("2.0")]
  public class BookController : ControllerBase
  {
    [HttpGet]
    [AllowAnonymous]
    [MapToApiVersion("2.0")]
    public string HealthCheck()
    {
      return "book api standing";
    }

    [Authorize]
    [HttpGet("GetBookList")]
    public IActionResult GetBookList()
    {
      var list = new List<string>() {
        "Orman",
        "Acımasız",
        "Yankı"
      };

      return new JsonResult(list);
    }
  }
}