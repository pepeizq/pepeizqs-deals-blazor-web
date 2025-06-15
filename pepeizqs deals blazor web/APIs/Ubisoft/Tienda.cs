﻿//https://store.ubisoft.com/on/demandware.store/Sites-es_ubisoft-Site/es_ES/Product-GetAvailabilityAndOwnership?pid=60c30d870d253c1914049e94
//https://xely3u4lod-dsn.algolia.net/1/indexes/es_custom_MFE/query?x-algolia-agent=Algolia%20for%20JavaScript%20(4.8.5)%3B%20Browser&x-algolia-application-id=XELY3U4LOD&x-algolia-api-key=5638539fd9edb8f2c6b024b49ec375bd
//{"query":"5b06a3994e0165fa45ffdcdf"} <-- tienda id


#nullable disable

using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIs.Ubisoft
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "ubisoft",
				Nombre = "Ubisoft Store",
				ImagenLogo = "/imagenes/tiendas/ubisoft2_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/ubisoft2_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/ubisoft_icono.webp",
				Color = "#0a0a0a",
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador)
		{
			BaseDatos.Admin.Actualizar.Tiendas(Generar().Id, DateTime.Now, 0, conexion);

			int paginas = 10;
			int juegos2 = 0;

			int i = 0;
			while (i < paginas - 1)
			{
				HttpClient cliente = new HttpClient();
				cliente.BaseAddress = new Uri("https://store.ubisoft.com/");
				cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpRequestMessage peticion = new HttpRequestMessage(HttpMethod.Post, "https://xely3u4lod-dsn.algolia.net/1/indexes/*/queries?x-algolia-agent=Algolia%20for%20JavaScript%20(3.35.1)%3B%20Browser&x-algolia-application-id=XELY3U4LOD&x-algolia-api-key=5638539fd9edb8f2c6b024b49ec375bd");
				peticion.Content = new StringContent("{\"requests\":[{\"indexName\":\"production__es_ubisoft__products__es_ES__best_sellers\",\"params\":\"hitsPerPage=100&page=" + i.ToString() + "\"}]}");

				HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

				string html = string.Empty;

				try
				{
					html = await respuesta.Content.ReadAsStringAsync();
				}
				catch { }

				if (string.IsNullOrEmpty(html) == false)
				{
					UbisoftDatos datos = JsonSerializer.Deserialize<UbisoftDatos>(html);

					if (datos != null)
					{
						if (datos.Datos != null)
						{
							paginas = datos.Datos[0].Paginas;

							if (datos.Datos[0].Juegos != null)
							{
								if (datos.Datos[0].Juegos.Count > 0)
								{
									foreach (UbisoftJuego juego in datos.Datos[0].Juegos)
									{
										if (juego != null)
										{
											if (juego.PrecioRebajado != null && juego.PrecioBase != null)
											{
												int descuento = Calculadora.SacarDescuento(juego.PrecioBase.Euro, juego.PrecioRebajado.Euro);

												if (descuento > 0)
												{
													string nombre = WebUtility.HtmlDecode(juego.Nombre);
													string enlace = juego.Enlace;
													string imagen = juego.Imagen;

													JuegoPrecio oferta = new JuegoPrecio
													{
														Nombre = nombre,
														Enlace = enlace,
														Imagen = imagen,
														Moneda = JuegoMoneda.Euro,
														Precio = juego.PrecioRebajado.Euro,
														Descuento = descuento,
														Tienda = Generar().Id,
														DRM = JuegoDRM.Ubisoft,
														FechaDetectado = DateTime.Now,
														FechaActualizacion = DateTime.Now
													};

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
				}

				i += 1;
			}
		}
	}

	public class UbisoftDatos
	{
		[JsonPropertyName("results")]
		public List<UbisoftResultados> Datos { get; set; }
	}

	public class UbisoftResultados
	{
		[JsonPropertyName("hits")]
		public List<UbisoftJuego> Juegos { get; set; }

		[JsonPropertyName("nbHits")]
		public int Paginas { get; set; }
	}

	public class UbisoftJuego
	{
		[JsonPropertyName("title")]
		public string Nombre { get; set; }

		[JsonPropertyName("link")]
		public string Enlace { get; set; }

		[JsonPropertyName("image_link")]
		public string Imagen { get; set; }

		[JsonPropertyName("price")]
		public UbisoftJuegoPrecio PrecioRebajado { get; set; }

		[JsonPropertyName("default_price")]
		public UbisoftJuegoPrecio PrecioBase { get; set; }
	}

	public class UbisoftJuegoPrecio
	{
		[JsonPropertyName("EUR")]
		public decimal Euro { get; set; }
	}
}