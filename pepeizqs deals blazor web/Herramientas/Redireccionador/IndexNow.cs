using Microsoft.AspNetCore.Mvc;

namespace Herramientas.Redireccionador
{

	public class IndexNow : Controller
	{
        [HttpGet("732aae09f61d44d697d4bcd931ff2051 .txt")]
        public IActionResult Bing()
        {
            return Ok("732aae09f61d44d697d4bcd931ff2051 ");
        }
	}
}
