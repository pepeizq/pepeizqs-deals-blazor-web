#nullable disable

using ApexCharts;
using Juegos;
using Microsoft.Data.SqlClient;
using RedditSharp.Multi;
using System;
using System.Diagnostics.Metrics;
using System.Drawing.Drawing2D;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using X.Bluesky.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APIs.AmazonLuna
{
	public static class Suscripcion
	{
		public static Suscripciones2.Suscripcion Generar()
		{
			Suscripciones2.Suscripcion amazon = new Suscripciones2.Suscripcion
			{
				Id = Suscripciones2.SuscripcionTipo.AmazonLunaPlus,
				Nombre = "Amazon Luna +",
				ImagenLogo = "/imagenes/suscripciones/amazonluna.webp",
				ImagenIcono = "/imagenes/streaming/amazonluna_icono.webp",
				Enlace = "https://luna.amazon.es/subscription/luna-plus/B085TRCCT6",
				DRMDefecto = JuegoDRM.AmazonLuna,
				AdminInteractuar = true,
				UsuarioEnlacesEspecificos = false,
				ParaSiempre = false,
				Precio = 9.99,
				AdminPendientes = true,
				TablaPendientes = "suscripcionamazonlunaplus"
			};

			return amazon;
		}

		public static async Task Buscar(SqlConnection conexion)
		{
			BaseDatos.Admin.Actualizar.Tiendas(Generar().Id.ToString().ToLower(), DateTime.Now, 0, conexion);

			int cantidad = 0;

			HttpClient cliente = new HttpClient();
			cliente.BaseAddress = new Uri("https://luna.amazon.es/");

			string peticionEnBruto = "{\"timeout\":10000,\"featureScheme\":\"WEB_V1\",\"pageContext\":{\"pageUri\":\"subscription/luna-plus/B085TRCCT6\",\"pageId\":\"default\"},\"cacheKey\":\"cfb5550b-0c5f-49cd-852a-1cc4f85206a3\",\"clientContext\":{\"browserMetadata\":{\"browserType\":\"Firefox\",\"browserVersion\":\"142.0\",\"deviceModel\":\"rv:142.0\",\"deviceType\":\"unknown\",\"osName\":\"Windows\",\"osVersion\":\"10\"}},\"inputContext\":{\"gamepadTypes\":[]},\"dynamicFeatures\":[]}";

			HttpRequestMessage peticion = new HttpRequestMessage(HttpMethod.Post, "https://proxy-prod.eu-west-1.tempo.digital.a2z.com/getPage")
			{
				Content = new StringContent(peticionEnBruto, Encoding.UTF8, "*/*"),
				Headers = { {"Host","proxy-prod.eu-west-1.tempo.digital.a2z.com" },
{"User-Agent","Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:142.0) Gecko/20100101 Firefox/142.0" },
{"Accept","*/*" },
{"Accept-Language","en-US" },
{"Accept-Encoding","gzip, deflate, br, zstd" },
{"Referer","https://luna.amazon.es/" },
{"x-amz-timezone","Europe/Madrid" },
{"x-amz-client-version","-" },
{"x-amz-device-serial-number","259-7104538-8853213" },
{"x-amz-device-type","browser" },
{"x-amz-platform","web" },
{"x-amz-react-version","1.0.32195.0" },
{"x-amz-session-id","259-7104538-8853213" },
{"x-amz-tdi","undefined" },
{"x-amz-locale","es_ES" },
{"x-amz-marketplace-id","A1RKKUPIHCS9HS" },
{"x-amzn-RequestId","ddcd36d2-319a-412d-993b-f8c88953f0b1" },
{"x-amz-country-of-residence","ES" },
{"Origin","https://luna.amazon.es" },
{"Sec-GPC","1" },
{"Connection","keep-alive" },
{"Sec-Fetch-Dest","empty" },
{"Sec-Fetch-Mode","cors" },
{"Sec-Fetch-Site","cross-site" },
{"Priority","u=4" },
{"Pragma","no-cache" },
{"Cache-Control","no-cache" }
					}
			};

			HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

			string html = string.Empty;

			try
			{
				Stream stream = await respuesta.Content.ReadAsStreamAsync();

				using (GZipStream descompresion = new GZipStream(stream, CompressionMode.Decompress, false))
				{
					using (StreamReader lector = new StreamReader(stream, Encoding.UTF8))
					{
						html = await lector.ReadToEndAsync();
					}
				}
			}
			catch (Exception ex) 
			{ 
				BaseDatos.Errores.Insertar.Mensaje("Amazon Luna", ex, conexion);
			}

			GestionarHtml(html, cantidad, conexion);

			//HttpClient clienteEa = new HttpClient();
			//clienteEa.BaseAddress = new Uri("https://luna.amazon.es/");
			//clienteEa.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			//string peticionEnBrutoEa = "{\"timeout\":10000,\"featureScheme\":\"WEB_V1\",\"pageContext\":{\"pageUri\":\"ea?ref=tmp_pghq_landingpage\",\"pageId\":\"default\"},\"cacheKey\":\"3411e8dc-75f4-4b58-8860-815159a61183\",\"clientContext\":{\"browserMetadata\":{\"browserType\":\"Firefox\",\"browserVersion\":\"136.0\",\"deviceModel\":\"rv:136.0\",\"deviceType\":\"unknown\",\"osName\":\"Windows\",\"osVersion\":\"10\"}},\"inputContext\":{\"gamepadTypes\":[]},\"dynamicFeatures\":[]}";

			//HttpRequestMessage peticionEa = new HttpRequestMessage(HttpMethod.Post, "https://proxy-prod.eu-west-1.tempo.digital.a2z.com/getPage")
			//{
			//	Content = new StringContent(peticionEnBrutoEa, Encoding.UTF8, "application/json"),
			//	Headers = { { "x-amz-locale", "es_ES" },
			//				{ "x-amz-platform", "web" }
			//	}
			//};

			//HttpResponseMessage respuestaEa = await clienteEa.SendAsync(peticionEa);

			//string htmlEa = string.Empty;

			//try
			//{
			//	htmlEa = await respuestaEa.Content.ReadAsStringAsync();
			//}
			//catch (Exception ex)
			//{
			//	BaseDatos.Errores.Insertar.Mensaje("Amazon Luna EA", ex, conexion);
			//}

			//GestionarHtml(htmlEa, cantidad, conexion);
		}

		private static void GestionarHtml(string html, int cantidad, SqlConnection conexion = null)
		{
			if (string.IsNullOrEmpty(html) == false)
			{
				BaseDatos.Errores.Insertar.Mensaje("Amazon Luna HTML", html);

				AmazonLunaPlusAPI api = JsonSerializer.Deserialize<AmazonLunaPlusAPI>(html);

				if (api != null)
				{
					

					foreach (var juego in api?.Datos?.Contenido?.Widgets[3]?.Widgets)
					{
						if (string.IsNullOrEmpty(juego.Json) == false)
						{
							AmazonLunaPlusAPIJuego apiJuego = JsonSerializer.Deserialize<AmazonLunaPlusAPIJuego>(juego.Json);

							if (apiJuego != null)
							{
								string enlace = apiJuego.Id;

								bool encontrado = false;

								if (conexion == null)
								{
									conexion = Herramientas.BaseDatos.Conectar();
								}
								else
								{
									if (conexion.State != System.Data.ConnectionState.Open)
									{
										conexion = Herramientas.BaseDatos.Conectar();
									}
								}

								string sqlBuscar = "SELECT idJuegos FROM " + Generar().TablaPendientes + " WHERE enlace=@enlace";

								using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
								{
									comando.Parameters.AddWithValue("@enlace", enlace);

									using (SqlDataReader lector = comando.ExecuteReader())
									{
										if (lector.Read() == true)
										{
											cantidad += 1;
											BaseDatos.Admin.Actualizar.Tiendas(Generar().Id.ToString().ToLower(), DateTime.Now, cantidad, conexion);

											if (lector.IsDBNull(0) == false)
											{
												if (string.IsNullOrEmpty(lector.GetString(0)) == false)
												{
													string idJuegosTexto = lector.GetString(0);

													encontrado = true;

													if (idJuegosTexto != "0")
													{
														List<string> idJuegos = Herramientas.Listados.Generar(idJuegosTexto);

														if (idJuegos.Count > 0)
														{
															foreach (var id in idJuegos)
															{
																Juego juegobd = BaseDatos.Juegos.Buscar.UnJuego(int.Parse(id));

																if (juegobd != null)
																{
																	bool añadirSuscripcion = true;

																	if (juegobd.Suscripciones != null)
																	{
																		if (juegobd.Suscripciones.Count > 0)
																		{
																			bool actualizar = false;

																			foreach (var suscripcion in juegobd.Suscripciones)
																			{
																				if (suscripcion.Tipo == Suscripciones2.SuscripcionTipo.AmazonLunaPlus)
																				{
																					añadirSuscripcion = false;
																					actualizar = true;

																					DateTime nuevaFecha = suscripcion.FechaTermina;
																					nuevaFecha = DateTime.Now;
																					nuevaFecha = nuevaFecha + TimeSpan.FromDays(1);
																					suscripcion.FechaTermina = nuevaFecha;
																				}
																			}

																			if (actualizar == true)
																			{
																				BaseDatos.Juegos.Actualizar.Suscripciones(juegobd, conexion);

																				JuegoSuscripcion suscripcion2 = BaseDatos.Suscripciones.Buscar.UnJuego(enlace);

																				if (suscripcion2 != null)
																				{
																					DateTime nuevaFecha = suscripcion2.FechaTermina;
																					nuevaFecha = DateTime.Now;
																					nuevaFecha = nuevaFecha + TimeSpan.FromDays(1);
																					suscripcion2.FechaTermina = nuevaFecha;
																					BaseDatos.Suscripciones.Actualizar.FechaTermina(suscripcion2, conexion);
																				}
																			}
																		}
																	}

																	if (añadirSuscripcion == true)
																	{
																		DateTime nuevaFecha = DateTime.Now;
																		nuevaFecha = nuevaFecha + TimeSpan.FromDays(1);

																		JuegoSuscripcion nuevaSuscripcion = new JuegoSuscripcion
																		{
																			DRM = JuegoDRM.AmazonLuna,
																			Nombre = juegobd.Nombre,
																			FechaEmpieza = DateTime.Now,
																			FechaTermina = nuevaFecha,
																			Imagen = juegobd.Imagenes.Header_460x215,
																			ImagenNoticia = juegobd.Imagenes.Header_460x215,
																			JuegoId = juegobd.Id,
																			Enlace = enlace,
																			Tipo = Suscripciones2.SuscripcionTipo.AmazonLunaPlus
																		};

																		if (juegobd.Suscripciones == null)
																		{
																			juegobd.Suscripciones = new List<JuegoSuscripcion>();
																		}

																		juegobd.Suscripciones.Add(nuevaSuscripcion);

																		BaseDatos.Suscripciones.Insertar.Ejecutar(juegobd.Id, juegobd.Suscripciones, nuevaSuscripcion, conexion);
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

								if (encontrado == false)
								{
									BaseDatos.Suscripciones.Insertar.Temporal(conexion, Generar().Id.ToString().ToLower(), enlace, apiJuego.Nombre, apiJuego.Imagen);
								}
							}
						}
					}
				}
			}
		}
	}

	public class AmazonLunaPlusAPI
	{
		[JsonPropertyName("pageMemberGroups")]
		public AmazonLunaPlusAPIDatos Datos { get; set; }
	}

	public class AmazonLunaPlusAPIDatos
	{
		[JsonPropertyName("mainContent")]
		public AmazonLunaPlusAPIContenido Contenido { get; set; }
	}

	public class AmazonLunaPlusAPIContenido
	{
		[JsonPropertyName("widgets")]
		public List<AmazonLunaPlusAPIWidget> Widgets { get; set; }
	}

	public class AmazonLunaPlusAPIWidget
	{
		[JsonPropertyName("widgets")]
		public List<AmazonLunaPlusAPIWidget> Widgets { get; set; }

		[JsonPropertyName("presentationData")]
		public string Json { get; set; }
	}

	public class AmazonLunaPlusAPIJuego
	{
		[JsonPropertyName("gameId")]
		public string Id { get; set; }

		[JsonPropertyName("title")]
		public string Nombre { get; set; }

		[JsonPropertyName("imagePortrait")]
		public string Imagen { get; set; }
	}
}
