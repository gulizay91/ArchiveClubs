using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookArchive.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BookController : ControllerBase
  {
    [HttpGet]
    [AllowAnonymous]
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