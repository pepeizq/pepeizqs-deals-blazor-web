﻿#nullable disable

using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIs.Muvegames
{

	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "muvegames",
				Nombre = "Muve.games",
				ImagenLogo = "/imagenes/tiendas/muvegames_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/muvegames_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/muvegames_icono.ico",
				Color = "#558205",
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static async Task BuscarOfertas(SqlConnection conexion)
		{
			BaseDatos.Admin.Actualizar.Tiendas(Generar().Id, DateTime.Now, 0, conexion);

			int tope = 10;
			int juegos2 = 0;

			int i = 0;
			while (i < tope)
			{
				string html = await Decompiladores.Estandar("https://volkanos.muve.pl/api/matrix/v1/muve/products/?language=EN&currency=EUR&country=ES&category_url_key=pc-games-3&page=" + i.ToString() + "&query={%22ftr_is_promotion%22:[true],%22ftr_countries%22:[%22attr_es%22]}&limit=16&fetch_attributes=true");

				if (string.IsNullOrEmpty(html) == false)
				{
					MuvegamesDatos datos = JsonSerializer.Deserialize<MuvegamesDatos>(html);

					if (datos != null)
					{
						if (datos.Paginas.Cantidad > 0)
						{
							tope = datos.Paginas.Cantidad + 1;
						}

						string ids = string.Empty;
						List<MuvegamesMasOferta> ofertas = new List<MuvegamesMasOferta>();
						
						foreach (var juego in datos.Juegos)
						{
							string nombre = WebUtility.HtmlDecode(juego.Nombre);

							string enlace = "https://muve.games/p/" + juego.Enlace;

							string imagen = null;

							foreach (var imagen2 in juego.Media)
							{
								if (imagen2.Tipo == "picture" && imagen2.Rol == "main")
								{
									imagen = imagen2.Imagenes.FirstOrDefault().Enlace;
								}
							}

							JuegoDRM drm = JuegoDRM.NoEspecificado;

							foreach (var atributo in juego.Atributos)
							{
								if (atributo.Nombre == "DRM")
								{
									drm = JuegoDRM2.Traducir(atributo.Valor, Generar().Id);
								}
							}

							if (juego.EstaEnStock == true && drm != JuegoDRM.NoEspecificado)
							{
								ids = ids + "&sku[]=" + juego.Id;

								MuvegamesMasOferta oferta2 = new MuvegamesMasOferta();
								oferta2.Id = juego.Id;

								JuegoPrecio oferta = new JuegoPrecio
								{
									Nombre = nombre,
									Enlace = enlace,
									Imagen = imagen,
									Moneda = JuegoMoneda.Euro,
									Tienda = Generar().Id,
									DRM = drm,
									FechaDetectado = DateTime.Now,
									FechaActualizacion = DateTime.Now
								};

								oferta2.Oferta = oferta;

								ofertas.Add(oferta2);
							}
						}

						string html2 = await Decompiladores.Estandar("https://volkanos.muve.pl/api/matrix/v1/muve/prices/?currency=EUR&country=ES" + ids);

						if (string.IsNullOrEmpty(html2) == false)
						{
							MuvegamesPrecios precios = JsonSerializer.Deserialize<MuvegamesPrecios>(html2);

							if (precios != null)
							{
								foreach (var precio in precios.Precios)
								{
									foreach (var oferta in ofertas)
									{
										if (precio.Id == oferta.Id && string.IsNullOrEmpty(precio.PrecioBase) == false && string.IsNullOrEmpty(precio.PrecioRebajado) == false)
										{
											decimal precioBase = decimal.Parse(precio.PrecioBase);
											decimal precioRebajado = decimal.Parse(precio.PrecioRebajado);

											int descuento = Calculadora.SacarDescuento(precioBase, precioRebajado);

											if (descuento > 0)
											{
												oferta.Oferta.Descuento = descuento;
												oferta.Oferta.Precio = precioRebajado;

												try
												{
													BaseDatos.Tiendas.Comprobar.Resto(oferta.Oferta, conexion);
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

												break;
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
	}

	public class MuvegamesMasOferta
	{
		public string Id { get; set; }
		public JuegoPrecio Oferta { get; set; }
	}

	public class MuvegamesDatos
	{
		[JsonPropertyName("data")]
		public List<MuvegamesJuego> Juegos { get; set; }

		[JsonPropertyName("pagination")]
		public MuvegamesPaginas Paginas { get; set; }
	}

	public class MuvegamesPaginas
	{
		[JsonPropertyName("pages")]
		public int Cantidad { get; set; }
	}

	public class MuvegamesJuego
	{
		[JsonPropertyName("sku")]
		public string Id { get; set; }

		[JsonPropertyName("name")]
		public string Nombre { get; set; }

		[JsonPropertyName("url_key")]
		public string Enlace { get; set; }

		[JsonPropertyName("attributes")]
		public List<MuvegamesAtributo> Atributos { get; set; }

		[JsonPropertyName("media")]
		public List<MuvegamesMedia> Media { get; set; }

		[JsonPropertyName("on_stock")]
		public bool EstaEnStock { get; set; }
	}

	public class MuvegamesAtributo
	{
		[JsonPropertyName("sku")]
		public string Id { get; set; }

		[JsonPropertyName("feature_name")]
		public string Nombre { get; set; }

		[JsonPropertyName("attribute_value")]
		public string Valor { get; set; }
	}

	public class MuvegamesMedia
	{
		[JsonPropertyName("type")]
		public string Tipo { get; set; }

		[JsonPropertyName("role")]
		public string Rol { get; set; }

		[JsonPropertyName("source_set")]
		public List<MuvegamesMediaImagen> Imagenes { get; set; }
	}

	public class MuvegamesMediaImagen
	{
		[JsonPropertyName("source")]
		public string Enlace { get; set; }
	}

	//------------------------------------------

	public class MuvegamesPrecios
	{
		[JsonPropertyName("data")]
		public List<MuvegamesPrecio> Precios { get; set; }
	}

	public class MuvegamesPrecio
	{
		[JsonPropertyName("sku")]
		public string Id { get; set; }

		[JsonPropertyName("price")]
		public string PrecioBase { get; set; }

		[JsonPropertyName("special_price")]
		public string PrecioRebajado { get; set; }
	}
}
