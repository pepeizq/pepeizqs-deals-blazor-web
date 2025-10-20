using Microsoft.AspNetCore.Mvc;

namespace Herramientas.Redireccionador
{

	public class IndexNow : Controller
	{
        [HttpGet("64d34e14606542e7b66ae9e2bc080d32.txt")]
        public IActionResult Bing()
        {
            return Ok("64d34e14606542e7b66ae9e2bc080d32");
        }

		[HttpGet("d84fd3c5c7b0ea7c0cf394e8d5d6aebf9cd829.txt")]
		public IActionResult Seznam()
		{
			return Ok("d84fd3c5c7b0ea7c0cf394e8d5d6aebf9cd829");
		}
	}
}
