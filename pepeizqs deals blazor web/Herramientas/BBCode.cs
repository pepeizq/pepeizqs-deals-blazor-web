#nullable disable

namespace Herramientas
{

	public static class BBCode
	{
		public static string Bundle(string idioma, Bundles2.Bundle bundle)
		{
			string texto = null;

			if (bundle != null)
			{
				if (string.IsNullOrEmpty(bundle.ImagenNoticia) == false)
				{
                    texto = "[url=" + bundle.Enlace + "][img]" + bundle.ImagenNoticia + "[/img][/url]" + Environment.NewLine + Environment.NewLine;
                }
                
				texto = texto + "[url=" + bundle.Enlace + "]" + bundle.Enlace + "[/url]" + Environment.NewLine + Environment.NewLine;

                if (bundle.Tiers != null)
                {
                    bundle.Tiers.Sort(delegate (Bundles2.BundleTier t1, Bundles2.BundleTier t2)
                    {
                        return t1.Posicion.CompareTo(t2.Posicion);
                    });

                    decimal totalMinimos = 0;

                    foreach (var tier in bundle.Tiers)
                    {
                        List<Bundles2.BundleJuego> juegosTier = new List<Bundles2.BundleJuego>();

                        if (bundle.Juegos != null)
                        {
                            if (bundle.Juegos.Count > 0)
                            {
                                foreach (var juego in bundle.Juegos)
                                {
                                    if (juego.Tier != null)
                                    {
                                        if (juego.Tier.Posicion == tier.Posicion)
                                        {
                                            juegosTier.Add(juego);
                                        }
                                    }
                                }
                            }
                        }

                        if (juegosTier.Count > 0)
                        {
                            juegosTier = juegosTier.OrderBy(x => x.Nombre).ToList();
                        }

                        foreach (var juego in juegosTier)
                        {
                            if (juego.Juego?.Tipo == Juegos.JuegoTipo.DLC)
                            {
                                if (string.IsNullOrEmpty(juego.Juego?.Maestro) == false)
                                {
                                    if (juego.Juego?.Maestro != "no")
                                    {
                                        foreach (var juego2 in juegosTier)
                                        {
                                            if (juego2.JuegoId == juego.Juego.Maestro)
                                            {
                                                if (juego2.DLCs == null)
                                                {
                                                    juego2.DLCs = new List<string>();
                                                }

                                                bool añadir = true;

                                                if (juego2.DLCs.Count > 0)
                                                {
                                                    foreach (var dlc in juego2.DLCs)
                                                    {
                                                        if (dlc == juego.JuegoId)
                                                        {
                                                            añadir = false;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (añadir == true)
                                                {
                                                    juego2.DLCs.Add(juego.JuegoId);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (bundle.Pick == true)
                        {
                            if (tier.Posicion == 1)
                            {
                                foreach (var tier2 in bundle.Tiers)
                                {
                                    if (tier2.CantidadJuegos == 1)
                                    {
                                        texto = texto + "[b]" + tier2.CantidadJuegos.ToString() + " " + Herramientas.Idiomas.BuscarTexto(idioma, "String21", "Bundle") + " • " + Herramientas.Precios.Euro(decimal.Parse(tier2.Precio)) + "[/b]" + Environment.NewLine;
                                    }
                                    else if (tier2.CantidadJuegos > 1)
                                    {
                                        texto = texto + "[b]" + tier2.CantidadJuegos.ToString() + " " + Herramientas.Idiomas.BuscarTexto(idioma, "String8", "Bundle") + " • " + Herramientas.Precios.Euro(decimal.Parse(tier2.Precio)) + "[/b] / " + Herramientas.Precios.Euro(decimal.Parse(tier2.Precio) / tier2.CantidadJuegos) + " (" + Herramientas.Idiomas.BuscarTexto(idioma, "String20", "Bundle") + ")" + Environment.NewLine;
                                    }
                                }

                                texto = texto + Environment.NewLine;
                            }
                        }
                        else
                        {
                            texto = texto + "[b]Tier " + tier.Posicion.ToString() + ": " + Herramientas.Precios.Euro(decimal.Parse(tier.Precio)) + "[/b]" + Environment.NewLine;

                            foreach (var juego in juegosTier)
                            {
                                if (juego.Juego == null)
                                {
                                    juego.Juego = global::BaseDatos.Juegos.Buscar.UnJuego(juego.JuegoId);
                                }

                                if (juego.Juego?.PrecioMinimosHistoricos != null)
                                {
                                    foreach (var historico in juego.Juego.PrecioMinimosHistoricos)
                                    {
                                        if (historico.DRM == juego.DRM)
                                        {
                                            if (historico.PrecioCambiado > 0)
                                            {
                                                totalMinimos = totalMinimos + historico.PrecioCambiado;
                                            }
                                            else
                                            {
                                                totalMinimos = totalMinimos + historico.Precio;
                                            }

                                            break;
                                        }
                                    }
                                }
                            }

                            texto = texto + Herramientas.Idiomas.BuscarTexto(idioma, "String14", "Bundle") + ": " + Herramientas.Precios.Euro(totalMinimos) + Environment.NewLine + Environment.NewLine;
                        }

                        if (juegosTier.Count > 0)
                        {
                            texto = texto + "[list]";

                            foreach (var juego in juegosTier)
                            {
                                if (juego.Juego == null)
                                {
                                    juego.Juego = global::BaseDatos.Juegos.Buscar.UnJuego(juego.JuegoId);
                                }

                                bool mostrar = true;

                                if (juego.Juego?.Tipo == Juegos.JuegoTipo.DLC)
                                {
                                    if (string.IsNullOrEmpty(juego.Juego.Maestro) == false)
                                    {
                                        if (juego.Juego.Maestro != "no")
                                        {
                                            foreach (var juego2 in juegosTier)
                                            {
                                                if (juego2.JuegoId == juego.Juego.Maestro)
                                                {
                                                    mostrar = false;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (mostrar == true)
                                {
                                    string nombre = juego.Nombre;

                                    if (juego.Juego != null)
                                    {
                                        if (juego.DRM == Juegos.JuegoDRM.Steam && juego.Juego.IdSteam > 0)
                                        {
                                            nombre = "[url=https://store.steampowered.com/app/" + juego.Juego.IdSteam + "]" + nombre + "[/url]";
                                        }
                                        else if (juego.DRM == Juegos.JuegoDRM.GOG && string.IsNullOrEmpty(juego.Juego.SlugGOG) == false)
                                        {
                                            nombre = "[url=https://www.gog.com/game/" + juego.Juego.SlugGOG + "]" + nombre + "[/url]";
                                        }
                                        else if (juego.DRM == Juegos.JuegoDRM.Epic && string.IsNullOrEmpty(juego.Juego.SlugEpic) == false)
                                        {
                                            nombre = "[url=https://www.epicgames.com/store/p/" + juego.Juego.SlugEpic + "]" + nombre + "[/url]";
                                        }
                                    }

                                    texto = texto + "[*]" + nombre + " (" + Juegos.JuegoDRM2.DevolverDRM(juego.DRM) + ")";

                                    if (juego.DLCs?.Count > 0)
                                    {
                                        texto = texto + " +" + juego.DLCs?.Count.ToString() + " DLCs";
                                    }

                                    if (juego.Juego.Bundles.Count == 1)
                                    {
                                        texto = texto + " • [b]" + Herramientas.Idiomas.BuscarTexto(idioma, "String24", "Bundle") + "[/b]";
                                    }

                                    texto = texto + Environment.NewLine;
                                }
                            }

                            texto = texto + "[/list]" + Environment.NewLine + Environment.NewLine;
                        }
                    }
                }
            }

			return texto;
		}
	}
}
