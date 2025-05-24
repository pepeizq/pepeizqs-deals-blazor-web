﻿#nullable disable

using Microsoft.AspNetCore.Mvc;
using Noticias;

namespace Herramientas.Redireccionador
{
	public class Noticias : Controller
	{
		[ResponseCache(Duration = 6000)]
		[HttpGet("link/news/{id}/")]
		public IActionResult AbrirNoticia(int id)
		{
			Noticia noticia = global::BaseDatos.Noticias.Buscar.UnaNoticia(id);

			if (string.IsNullOrEmpty(noticia.Enlace) == false)
			{
				if (noticia.Tipo == NoticiaTipo.Bundles)
				{
					return Redirect(Herramientas.EnlaceAcortador.Generar(noticia.Enlace, noticia.BundleTipo));
				}
				else if (noticia.Tipo == NoticiaTipo.Gratis)
				{
					return Redirect(Herramientas.EnlaceAcortador.Generar(noticia.Enlace, noticia.GratisTipo));
				}
				else if (noticia.Tipo == NoticiaTipo.Suscripciones)
				{
					return Redirect(Herramientas.EnlaceAcortador.Generar(noticia.Enlace, noticia.SuscripcionTipo));
				}
				else
				{
					return Redirect(noticia.Enlace);
				}
			}

			return Redirect("~/news/" + id.ToString() + "/");
		}
	}
}
