﻿//https://www.indiegala.com/games/ajax/on-sale/percentage-off/1
//https://www.indiegala.com/store_games_rss?&sale=true&page=1

#nullable disable

using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

namespace APIs.IndieGala
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "indiegala",
				Nombre = "IndieGala",
				ImagenLogo = "/imagenes/tiendas/indiegala_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/indiegala_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/indiegala_icono.webp",
				Color = "#ffccd4",
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static string Referido(string enlace)
		{
			return enlace + "?ref=pepeizq";
		}

		public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador)
		{
			BaseDatos.Admin.Actualizar.Tiendas(Generar().Id, DateTime.Now, 0, conexion);

			int juegos2 = 0;

			int i = 1;
			while (i < 10)
			{
				string html = await Decompiladores.Estandar("https://www.indiegala.com/store_games_rss?&sale=true&page=" + i.ToString());

				if (string.IsNullOrEmpty(html) == false)
				{
					if (html == "None")
					{
						break;
					}

					XmlSerializer xml = new XmlSerializer(typeof(IndieGalaRSS));
					IndieGalaRSS listaJuegos = null;

					using (TextReader lector = new StringReader(html))
					{
						listaJuegos = (IndieGalaRSS)xml.Deserialize(lector);
					}

					if (listaJuegos != null)
					{
						if (listaJuegos.Canal.Buscador.Juegos != null)
						{
							if (listaJuegos.Canal.Buscador.Juegos.Count > 0)
							{
								foreach (IndieGalaJuego juego in listaJuegos.Canal.Buscador.Juegos)
								{
									bool buscar = true;

									if (string.IsNullOrEmpty(juego.PaisesRestringidos) == false)
									{
										List<string> listaPaisesRestringidos = new List<string>();

										string[] datosPartidos = juego.PaisesRestringidos.Split(',');
										listaPaisesRestringidos.AddRange(datosPartidos);

										if (listaPaisesRestringidos.Count > 0)
										{
											foreach (var pais in listaPaisesRestringidos)
											{
												if (pais.ToLower() == "es")
												{
													buscar = false;
													break;
												}
											}
										}
									}

									if (string.IsNullOrEmpty(juego.PaisesAprobados) == false)
									{
										List<string> listaPaisesAprobados = new List<string>();

										string[] datosPartidos = juego.PaisesAprobados.Split(',');
										listaPaisesAprobados.AddRange(datosPartidos);

										if (listaPaisesAprobados.Count > 0)
										{
											bool encontrado = false;
											foreach (var pais in listaPaisesAprobados)
											{
												if (pais.ToLower() == "es")
												{
													encontrado = true;
													break;
												}
											}

											if (encontrado == false)
											{
												buscar = false;
											}
										}
									}

									if (buscar == true)
									{
										if (juego.Estado == "available")
										{
											decimal precioBase = decimal.Parse(juego.PrecioBase);
											decimal precioRebajado = decimal.Parse(juego.PrecioRebajado);
											int descuento = Calculadora.SacarDescuento(precioBase, precioRebajado);

											if (descuento > 0)
											{
												string nombre = WebUtility.HtmlDecode(juego.Nombre);

												string enlace = juego.Enlace;

												string imagen = juego.Imagen;

												if (imagen.Contains("https://www.indiegalacdn.com/") == false)
												{
													imagen = "https://www.indiegalacdn.com/" + imagen;
												}

												JuegoDRM drm = JuegoDRM2.Traducir(juego.DRM, Generar().Id);

												if (nombre.Contains("(Epic)") == true)
												{
													drm = JuegoDRM.Epic;
												}
												else if (ComprobarEpicFalsos(enlace) == true)
												{
													drm = JuegoDRM.Epic;
												}
												else if (enlace.Contains("_epic") == true)
												{
													drm = JuegoDRM.Epic;
												}

												if (ComprobarElderFalsos(enlace) == true)
												{
													drm = JuegoDRM.ElderScrolls;
												}

												JuegoPrecio oferta = new JuegoPrecio
												{
													Nombre = nombre,
													Enlace = enlace,
													Imagen = imagen,
													Moneda = JuegoMoneda.Euro,
													Precio = precioRebajado,
													Descuento = descuento,
													Tienda = Generar().Id,
													DRM = drm,
													FechaDetectado = DateTime.Now,
													FechaActualizacion = DateTime.Now
												};

												if (string.IsNullOrEmpty(juego.Fecha) == false)
												{
													if (juego.Fecha != "None")
													{
														DateTime fechaTermina = DateTime.Parse(juego.Fecha, CultureInfo.InvariantCulture);
														oferta.FechaTermina = fechaTermina;
													}
												}

												try
												{
													BaseDatos.Tiendas.Comprobar.Resto(oferta, conexion);
												}
												catch (Exception ex)
												{
													BaseDatos.Errores.Insertar.Mensaje(Generar().Id, ex, conexion);
												}

												juegos2 += 1;

												try
												{
													BaseDatos.Admin.Actualizar.Tiendas(Generar().Id, DateTime.Now, juegos2, conexion);
												}
												catch (Exception ex)
												{
													BaseDatos.Errores.Insertar.Mensaje(Generar().Id, ex, conexion);
												}
											}
										}
									}
								}
							}
						}
					}
				}

				i += 1;
			}
		}

		private static bool ComprobarEpicFalsos(string enlaceJuego)
		{
			List<string> enlaces = [
				"1286680_pass",
				"1286680_pre",
				"1286680_deluxe_pre",
				"1496590",
				"1496590_deluxe",
				"578650",
				"629820",
				"9b06280a",
				"f69bbded"
			];

			foreach (var enlace in enlaces)
			{
				if (enlaceJuego.Contains("/" + enlace) == true)
				{
					return true;
				}
			}

			return false;
		}

		private static bool ComprobarElderFalsos(string enlaceJuego)
		{
			List<string> enlaces = [
				"2662630_elder"
			];

			foreach (var enlace in enlaces)
			{
				if (enlaceJuego.Contains("/" + enlace) == true)
				{
					return true;
				}
			}

			return false;
		}
	}

	#region Clases

	[XmlRoot("rss")]
	public class IndieGalaRSS
	{
		[XmlElement("channel")]
		public IndieGalaCanal Canal { get; set; }
	}

	public class IndieGalaCanal
	{
		[XmlElement("totalPages")]
		public int TotalPaginas { get; set; }

		[XmlElement("totalGames")]
		public int TotalJuegos { get; set; }

		[XmlElement("browse")]
		public IndieGalaJuegos Buscador { get; set; }
	}

	public class IndieGalaJuegos
	{
		[XmlElement("item")]
		public List<IndieGalaJuego> Juegos { get; set; }
	}

	public class IndieGalaJuego
	{
		[XmlElement("title")]
		public string Nombre { get; set; }

		[XmlElement("link")]
		public string Enlace { get; set; }

		[XmlElement("discountPercentEUR")]
		public string Descuento { get; set; }

		[XmlElement("discountPriceEUR")]
		public string PrecioRebajado { get; set; }

		[XmlElement("priceEUR")]
		public string PrecioBase { get; set; }

		[XmlElement("boximg")]
		public string Imagen { get; set; }

		[XmlElement("drminfo")]
		public string DRM { get; set; }

		[XmlElement("discountEnd")]
		public string Fecha { get; set; }

		[XmlElement("state")]
		public string Estado { get; set; }

		[XmlElement("regionStockAvailable")]
		public string PaisesAprobados { get; set; }

		[XmlElement("notAvailableRegions")]
		public string PaisesRestringidos { get; set; }
	}

	#endregion
}
