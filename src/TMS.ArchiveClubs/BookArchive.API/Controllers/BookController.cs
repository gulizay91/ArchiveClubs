using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookArchive.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BookController : ControllerBase
  {
    [HttpGet]
    [AllowAnonymous]
    public string Control()
    {
      return "book api standing";
    }

    [Authorize]
    [HttpGet("/api/GetBookList")]
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
