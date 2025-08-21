//https://embed.gog.com/public_wishlist/464176759200/search?hiddenFlag=0&mediaType=0&page=1&sortBy=date_added&totalPages=4
//https://www.gog.com/u/pepeizq/games/stats?sort=recent_playtime&order=desc&page=3

#nullable disable

using Herramientas;
using Microsoft.VisualBasic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIs.GOG
{
	public static class Cuenta
	{
		public static async Task<string> UsuarioId(string usuario)
		{
			if (string.IsNullOrEmpty(usuario) == false)
			{
				usuario = usuario.Replace("https://www.gog.com/u/", null);
				usuario = usuario.Replace("http://www.gog.com/u/", null);

				if (usuario.Contains("?") == true)
				{
					int int1 = usuario.IndexOf("?");
					usuario = usuario.Remove(int1, usuario.Length - int1);
				}

				usuario = usuario.Replace("/", null);

				string html = await Decompiladores.Estandar("https://www.gog.com/u/" + usuario + "/wishlist");

				if (string.IsNullOrEmpty(html) == false)
				{
					if (html.Contains("gog-user=") == true)
					{
						int int1 = html.IndexOf("gog-user=");
						
						if (int1 > -1)
						{
							string temp1 = html.Remove(0, int1 + 10);

							int int2 = temp1.IndexOf(Strings.ChrW(34));
							string temp2 = temp1.Remove(int2, temp1.Length - int2);

							return temp2;
						}
					}
				}
			}

			return null;
		}

		public static async Task<List<GOGUsuarioJuego>> BuscarJuegos(string usuario)
		{
			if (string.IsNullOrEmpty(usuario) == false)
			{
				usuario = usuario.Replace("https://www.gog.com/u/", null);
				usuario = usuario.Replace("http://www.gog.com/u/", null);

				if (usuario.Contains("?") == true)
				{
					int int1 = usuario.IndexOf("?");
					usuario = usuario.Remove(int1, usuario.Length - int1);
				}

				usuario = usuario.Replace("/", null);

				string html = await Decompiladores.Estandar("https://www.gog.com/u/" + usuario + "/games/stats?page=1");

				if (string.IsNullOrEmpty(html) == false)
				{
					GOGCuenta cuenta = JsonSerializer.Deserialize<GOGCuenta>(html);

					if (cuenta != null)
					{
						int limite = cuenta.Paginas;

						if (limite > 0)
						{
							List<GOGUsuarioJuego> juegos = null;

							int i = 1;
							while (i < limite + 1)
							{
								string html2 = await Decompiladores.Estandar("https://www.gog.com/u/" + usuario + "/games/stats?page=" + i.ToString());

								if (string.IsNullOrEmpty(html2) == false)
								{
									GOGCuenta cuenta2 = JsonSerializer.Deserialize<GOGCuenta>(html2);

									if (cuenta2 != null)
									{
										foreach (var juego in cuenta2.Datos.Juegos)
										{
											string idJuego = juego.Datos.Id;

											idJuego = await BaseDatos.Juegos.Insertar.GogReferencia(idJuego);

											GOGUsuarioJuego nuevoJuego = new GOGUsuarioJuego
											{
												Id = int.Parse(idJuego)
											};

											if (juego.Estadisticas != null)
											{
												if (string.IsNullOrEmpty(juego.Estadisticas?.ToString()) == false)
												{
													if (juego.Estadisticas.ToString() != "[]")
													{
														string jsonJuego = juego.Estadisticas.ToString();

														jsonJuego = jsonJuego.Remove(jsonJuego.Length - 1, 1);
														jsonJuego = jsonJuego.Remove(0, 1);
														jsonJuego = jsonJuego.Remove(0, jsonJuego.IndexOf("{"));

														JsonSerializerOptions opciones = new JsonSerializerOptions
														{
															WriteIndented = true,
															Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
														};

														GOGCuentaJuegoEstadisticas2 estadisticas2 = JsonSerializer.Deserialize<GOGCuentaJuegoEstadisticas2>(jsonJuego, opciones);

														nuevoJuego.TiempoJugadoEnMinutos = estadisticas2.JugadoEnMinutos;
														nuevoJuego.TiempoJugadoUltimaVez = (int)((DateTimeOffset)DateTime.Parse(estadisticas2.JugadoUltimaVez)).ToUnixTimeSeconds();
													}
												}
											}
											
											if (juegos == null)
											{
												juegos = new List<GOGUsuarioJuego>();
											}

											juegos.Add(nuevoJuego);
										}
									}
								}

								i += 1;
							}

							if (juegos != null)
							{
								return juegos;
							}
						}
					}
				}
			}

			return null;
		}

		public static async Task<string> BuscarDeseados(string usuarioId)
		{
			if (string.IsNullOrEmpty(usuarioId) == false)
			{
				string html = await Decompiladores.Estandar("https://embed.gog.com/public_wishlist/" + usuarioId + "/search?hiddenFlag=0&mediaType=0&page=1&sortBy=date_added");
			
				if (string.IsNullOrEmpty(html) == false)
				{
					GOGDeseados deseados = null;
					
					try
					{
						deseados = JsonSerializer.Deserialize<GOGDeseados>(html);
					}
					catch (JsonException ex)
					{
						BaseDatos.Errores.Insertar.Mensaje("GOG Deseados", ex);
					}

					if (deseados != null)
					{
						int limite = deseados.Paginas;

						if (limite > 0)
						{
							string juegos = string.Empty;

							int i = 1;
							while (i < limite + 1)
							{
								string html2 = await Decompiladores.Estandar("https://embed.gog.com/public_wishlist/" + usuarioId + "/search?hiddenFlag=0&mediaType=0&page=" + i.ToString() + "&sortBy=date_added");

								if (string.IsNullOrEmpty(html2) == false)
								{
									GOGDeseados deseados2 = JsonSerializer.Deserialize<GOGDeseados>(html2);

									if (deseados2 != null)
									{
										foreach (var juego in deseados2.Juegos)
										{
											if (string.IsNullOrEmpty(juegos) == false)
											{
												juegos = juegos + "," + juego.Id.ToString();
											}
											else
											{
												juegos = juego.Id.ToString();
											}
										}
									}
								}

								i += 1;
							}

							if (string.IsNullOrEmpty(juegos) == false)
							{
								return juegos;
							}
						}
					}
				}
			}

			return null;
		}
	}

	public class GOGUsuarioJuego
	{
		public int Id { get; set; }
		public int TiempoJugadoEnMinutos { get; set; }
		public int TiempoJugadoUltimaVez { get; set; }
	}

	//-----------------------------------------------

	public class GOGCuenta
	{
		[JsonPropertyName("page")]
		public int Pagina { get; set; }

		[JsonPropertyName("pages")]
		public int Paginas { get; set; }

		[JsonPropertyName("_embedded")]
		public GOGCuentaDatos Datos { get; set; }
	}

	public class GOGCuentaDatos
	{
		[JsonPropertyName("items")]
		public List<GOGCuentaJuego> Juegos { get; set; }
	}

	public class GOGCuentaJuego
	{
		[JsonPropertyName("game")]
		public GOGCuentaJuegoDatos Datos { get; set; }

		[JsonPropertyName("stats")]
		public object Estadisticas { get; set; }
	}

	public class GOGCuentaJuegoDatos
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }
	}

	public class GOGCuentaJuegoEstadisticas2
	{
		[JsonPropertyName("playtime")]
		public int JugadoEnMinutos { get; set; }

		[JsonPropertyName("lastSession")]
		public string JugadoUltimaVez { get; set; }
	}

	//-----------------------------------------------

	public class GOGDeseados
	{
		[JsonPropertyName("page")]
		public int Pagina { get; set; }

		[JsonPropertyName("totalPages")]
		public int Paginas { get; set; }

		[JsonPropertyName("products")]
		public List<GOGDeseadosJuego> Juegos { get; set; }
	}

	public class GOGDeseadosJuego
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }
	}
}
