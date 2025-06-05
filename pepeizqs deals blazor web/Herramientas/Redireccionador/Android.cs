#nullable disable

using Microsoft.AspNetCore.Mvc;

namespace Herramientas.Redireccionador
{
	public class Android : Controller
	{
		[ResponseCache(Duration = 6000)]
		[HttpGet("android/news/{clave}/")]
		public IActionResult Noticias(int id, string clave)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string claveAndroid = builder.Configuration.GetValue<string>("Android:Clave");

			if (clave == claveAndroid)
			{
				List<global::Noticias.Noticia> noticias = global::BaseDatos.Noticias.Buscar.Actuales(null, 3);

				if (noticias != null)
				{
					if (noticias.Count > 0)
					{
						return Ok(noticias);
					}
				}
			}

			return Redirect("~/");
		}

		[ResponseCache(Duration = 6000)]
		[HttpGet("android/last-lows/{clave}/")]
		public IActionResult Minimos(string clave)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string claveAndroid = builder.Configuration.GetValue<string>("Android:Clave");

			if (clave == claveAndroid)
			{
				List<global::Juegos.Juego> juegos = global::BaseDatos.Portada.Buscar.UltimosMinimos(50, null, new List<string>() { "0", "8", "2", "11" });

				if (juegos != null)
				{
					if (juegos.Count > 0)
					{
						return Ok(juegos);
					}
				}
			}

			return Redirect("~/");
		}
	}
}
