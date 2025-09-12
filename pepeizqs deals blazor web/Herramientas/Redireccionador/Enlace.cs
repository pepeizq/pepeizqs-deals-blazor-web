#nullable disable

using Microsoft.AspNetCore.Mvc;

namespace Herramientas.Redireccionador
{
	public class Enlace : Controller
	{
		[HttpGet("link/{id}/")]
		public IActionResult CogerAcortador(string id)
		{
			return Redirect(EnlaceAcortador.AlargarEnlace(id));
		}

		[HttpGet("/game/steam/{id}/")]
		public IActionResult Steam(string id)
		{
			Juegos.Juego juego = global::BaseDatos.Juegos.Buscar.UnJuego(null, id);

			if (juego == null)
			{
				return Redirect("~/");
			}

			return Redirect("/game/" + juego.Id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(juego.Nombre) + "/");
		}

		[HttpGet("/game/gog/{id}/")]
		public IActionResult Gog(string id)
		{
			Juegos.Juego juego = global::BaseDatos.Juegos.Buscar.UnJuego(null, null, id);

			if (juego == null)
			{
				return Redirect("~/");
			}

			return Redirect("/game/" + juego.Id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(juego.Nombre) + "/");
		}

		[HttpGet("/game/epic/{id}/")]
		public IActionResult Epic(string id)
		{
			Juegos.Juego juego = global::BaseDatos.Juegos.Buscar.UnJuego(null, null, null, id);

			if (juego == null)
			{
				return Redirect("~/");
			}

			return Redirect("/game/" + juego.Id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(juego.Nombre) + "/");
		}
	}
}
