﻿#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using MimeKit.Tnef;
using System.Globalization;
using System.Text.Json;
using static pepeizqs_deals_blazor_web.Componentes.Cuenta.Cuenta.Juegos;
using static pepeizqs_deals_blazor_web.Componentes.Secciones.Minimos;

namespace BaseDatos.Juegos
{
	public static class Buscar
	{
		public static Juego Cargar(Juego juego, SqlDataReader lector)
		{
			try
			{
				if (lector.IsDBNull(0) == false)
				{
					juego.Id = lector.GetInt32(0);
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(1) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(1)) == false)
					{
						juego.Nombre = lector.GetString(1);
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(2) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(2)) == false)
					{
						juego.Tipo = Enum.Parse<JuegoTipo>(lector.GetString(2));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(3) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(3)) == false)
					{
						juego.Imagenes = JsonSerializer.Deserialize<JuegoImagenes>(lector.GetString(3));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(4) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(4)) == false)
					{
						juego.PrecioMinimosHistoricos = JsonSerializer.Deserialize<List<JuegoPrecio>>(lector.GetString(4));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(5) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(5)) == false)
					{
						juego.PrecioActualesTiendas = JsonSerializer.Deserialize<List<JuegoPrecio>>(lector.GetString(5));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(6) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(6)) == false)
					{
						juego.Analisis = JsonSerializer.Deserialize<JuegoAnalisis>(lector.GetString(6));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(7) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(7)) == false)
					{
						juego.Caracteristicas = JsonSerializer.Deserialize<JuegoCaracteristicas>(lector.GetString(7));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(8) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(8)) == false)
					{
						juego.Media = JsonSerializer.Deserialize<JuegoMedia>(lector.GetString(8));
					}
				}
			}
			catch { }

			if (lector.IsDBNull(9) == false)
			{
				juego.IdSteam = lector.GetInt32(9);
			}

			if (lector.IsDBNull(10) == false)
			{
				juego.IdGog = lector.GetInt32(10);
			}

			try
			{
				if (lector.IsDBNull(11) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(11)) == false)
					{
						juego.FechaSteamAPIComprobacion = DateTime.Parse(lector.GetString(11), CultureInfo.InvariantCulture);
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(12) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(12)) == false)
					{
						juego.Suscripciones = JsonSerializer.Deserialize<List<JuegoSuscripcion>>(lector.GetString(12));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(13) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(13)) == false)
					{
						juego.Bundles = JsonSerializer.Deserialize<List<JuegoBundle>>(lector.GetString(13));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(14) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(14)) == false)
					{
						juego.Gratis = JsonSerializer.Deserialize<List<JuegoGratis>>(lector.GetString(14));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(15) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(15)) == false)
					{
						juego.NombreCodigo = lector.GetString(15);
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(16) == false)
				{
					juego.IdMaestra = lector.GetInt32(16);
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(17) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(17)) == false)
					{
						juego.UsuariosInteresados = JsonSerializer.Deserialize<List<JuegoUsuariosInteresados>>(lector.GetString(17));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(18) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(18)) == false)
					{
						juego.SlugGOG = lector.GetString(18);
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(19) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(19)) == false)
					{
						juego.Maestro = lector.GetString(19);
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(20) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(20)) == false)
					{
						juego.FreeToPlay = lector.GetString(20);
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(21) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(21)) == false)
					{
						juego.MayorEdad = lector.GetString(21);
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(22) == false)
				{
					juego.UltimaModificacion = lector.GetDateTime(22);
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(23) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(23)) == false)
					{
						juego.SlugEpic = lector.GetString(23);
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(24) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(24)) == false)
					{
						juego.Categorias = JsonSerializer.Deserialize<List<string>>(lector.GetString(24));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(25) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(25)) == false)
					{
						juego.Etiquetas = JsonSerializer.Deserialize<List<string>>(lector.GetString(25));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(26) == false)
				{
					juego.Deck = Enum.Parse<JuegoDeck>(lector.GetInt32(26).ToString());
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(27) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(27)) == false)
					{
						juego.DeckTokens = JsonSerializer.Deserialize<List<JuegoDeckToken>>(lector.GetString(27));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(28) == false)
				{
					juego.DeckComprobacion = lector.GetDateTime(28);
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(29) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(29)) == false)
					{
						juego.Historicos = JsonSerializer.Deserialize<List<JuegoHistorico>>(lector.GetString(29));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(30) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(30)) == false)
					{
						juego.GalaxyGOG = JsonSerializer.Deserialize<JuegoGalaxyGOG>(lector.GetString(30));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(31) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(31)) == false)
					{
						juego.CantidadJugadores = JsonSerializer.Deserialize<JuegoCantidadJugadoresSteam>(lector.GetString(31));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(32) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(32)) == false)
					{
						juego.Idiomas = JsonSerializer.Deserialize<List<JuegoIdioma>>(lector.GetString(32));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(33) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(33)) == false)
					{
						juego.EpicGames = JsonSerializer.Deserialize<JuegoEpicGames>(lector.GetString(33));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(34) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(34)) == false)
					{
						juego.IdXbox = lector.GetString(34);
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(35) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(35)) == false)
					{
						juego.Xbox = JsonSerializer.Deserialize<JuegoXbox>(lector.GetString(35));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(36) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(36)) == false)
					{
						juego.IdAmazon = lector.GetString(36);
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(37) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(37)) == false)
					{
						juego.ExeEpic = lector.GetString(37);
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(38) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(38)) == false)
					{
						juego.ExeUbisoft = lector.GetString(38);
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(39) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(39)) == false)
					{
						juego.ExeEA = lector.GetString(39);
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(40) == false)
				{
					juego.OcultarPortada = lector.GetBoolean(40);
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(41) == false)
				{
					juego.SteamOS = Enum.Parse<JuegoSteamOS>(lector.GetInt32(41).ToString());
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(42) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(42)) == false)
					{
						juego.SteamOSTokens = JsonSerializer.Deserialize<List<JuegoDeckToken>>(lector.GetString(42));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(43) == false)
				{
					juego.UltimaActualizacionSteam = lector.GetDateTime(43);
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(44) == false)
				{
					juego.UltimaActualizacionGOG = lector.GetDateTime(44);
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(45) == false)
				{
					juego.UltimaActualizacion = lector.GetDateTime(45);
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(46) == false)
				{
					juego.InteligenciaArtificial = lector.GetBoolean(46);
				}
			}
			catch { }

			return juego;
		}

		public static Juego UnJuego(int id)
		{
			return UnJuego(id.ToString());
		}

		public static Juego UnJuego(string id = null, string idSteam = null, string idGog = null, string idEpic = null, SqlConnection conexion = null)
		{
			string sqlBuscar = string.Empty;
			string idParametro = string.Empty;
			string idBuscar = string.Empty;

			if (string.IsNullOrEmpty(id) == false)
			{
				sqlBuscar = "SELECT * FROM juegos WHERE id=@id";
				idParametro = "@id";
				idBuscar = id;
			}
			else
			{
				if (string.IsNullOrEmpty(idSteam) == false)
				{
					sqlBuscar = "SELECT * FROM juegos WHERE idSteam=@idSteam";
					idParametro = "@idSteam";
					idBuscar = idSteam;
				}
				else
				{
					if (string.IsNullOrEmpty(idGog) == false)
					{
						sqlBuscar = "SELECT * FROM juegos WHERE slugGog=@slugGog";
						idParametro = "@slugGog";
						idBuscar = idGog;
					}
					else
					{
						if (string.IsNullOrEmpty(idEpic) == false)
						{
							sqlBuscar = "SELECT * FROM juegos WHERE slugEpic=@slugEpic";
							idParametro = "@slugEpic";
							idBuscar = idEpic;
						}
					}
				}
			}

			Juego juego = null;

			if (sqlBuscar != string.Empty)
			{
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

				using (conexion)
				{
					string buscar = sqlBuscar;

					using (SqlCommand comando = new SqlCommand(buscar, conexion))
					{
						comando.Parameters.AddWithValue(idParametro, idBuscar);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							if (lector.Read())
							{
								juego = Cargar(JuegoCrear.Generar(), lector);
							}
						}
					}
				}
			}

			return juego;
		}

		public static List<Juego> MultiplesJuegosSteam(List<int> ids, SqlConnection conexion = null)
		{
			List<string> idsTexto = new List<string>();

			foreach (var id in ids)
			{
				idsTexto.Add(id.ToString());
			}

			return MultiplesJuegosSteam(idsTexto, conexion);
		}

		public static List<Juego> MultiplesJuegosSteam(List<string> ids, SqlConnection conexion = null)
		{
			List<Juego> juegos = new List<Juego>();
			string sqlBuscar = string.Empty;

			if (ids != null)
			{
				if (ids.Count > 0)
				{
					sqlBuscar = "SELECT * FROM juegos WHERE idSteam IN (";

					int i = 0;
					while (i < ids.Count)
					{
						if (i == 0)
						{
							sqlBuscar = sqlBuscar + "'" + ids[i] + "'";
						}
						else
						{
							sqlBuscar = sqlBuscar + ", '" + ids[i] + "'";
						}

						i += 1;
					}

					sqlBuscar = sqlBuscar + ")";
					sqlBuscar = sqlBuscar + " ORDER BY CASE\r\n WHEN analisis = 'null' OR analisis IS NULL THEN 0 ELSE CONVERT(int, REPLACE(JSON_VALUE(analisis, '$.Cantidad'),',',''))\r\n END DESC";
				}
			}

			if (string.IsNullOrEmpty(sqlBuscar) == false)
			{
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

				using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							Juego juego = new Juego();
							juego = Cargar(juego, lector);
							juegos.Add(juego);
						}
					}
				}
			}

			return juegos;
		}

		public static List<Juego> MultiplesJuegosGOG(List<string> ids, SqlConnection conexion = null)
		{
			List<Juego> juegos = new List<Juego>();
			string sqlBuscar = string.Empty;

			if (ids != null)
			{
				if (ids.Count > 0)
				{
					sqlBuscar = "SELECT * FROM juegos WHERE idGOG IN (";

					int i = 0;
					while (i < ids.Count)
					{
						if (i == 0)
						{
							sqlBuscar = sqlBuscar + "'" + ids[i] + "'";
						}
						else
						{
							sqlBuscar = sqlBuscar + ", '" + ids[i] + "'";
						}

						i += 1;
					}

					sqlBuscar = sqlBuscar + ")";
				}
			}

			if (string.IsNullOrEmpty(sqlBuscar) == false)
			{
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

				using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							Juego juego = new Juego();
							juego = Cargar(juego, lector);
							juegos.Add(juego);
						}
					}
				}
			}

			return juegos;
		}

		public static List<JuegoUsuario> MultiplesJuegosUsuario(List<JuegoUsuario> juegos, JuegoDRM drm, List<string> ids, SqlConnection conexion = null)
		{
			bool cogerNumero = false;
			string campo = string.Empty;

			if (drm == JuegoDRM.Steam)
			{
				cogerNumero = true;
				campo = "idSteam";
			}
			else if (drm == JuegoDRM.GOG)
			{
				cogerNumero = true;
				campo = "idGOG";
			}
			else if (drm == JuegoDRM.Amazon)
			{
				campo = "idAmazon";
			}
			else if (drm == JuegoDRM.Epic)
			{
				campo = "exeEpic";
			}
			else if (drm == JuegoDRM.Ubisoft)
			{
				campo = "exeUbisoft";
			}
			else if (drm == JuegoDRM.EA)
			{
				campo = "exeEA";
			}

			if (string.IsNullOrEmpty(campo) == false)
			{
				if (ids != null)
				{
					string sqlBuscar = string.Empty;

					if (ids.Count > 0)
					{
						sqlBuscar = "SELECT id, nombre, JSON_VALUE(imagenes, '$.Capsule_231x87'), " + campo + " FROM juegos WHERE " + campo + " IN (";

						int i = 0;
						while (i < ids.Count)
						{
							if (i == 0)
							{
								sqlBuscar = sqlBuscar + "'" + ids[i] + "'";
							}
							else
							{
								sqlBuscar = sqlBuscar + ", '" + ids[i] + "'";
							}

							i += 1;
						}

						sqlBuscar = sqlBuscar + ")";
					}

					if (string.IsNullOrEmpty(sqlBuscar) == false)
					{
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

						using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
						{
							using (SqlDataReader lector = comando.ExecuteReader())
							{
								while (lector.Read())
								{
									if (lector.IsDBNull(0) == false)
									{
										bool añadir = true;

										if (juegos != null)
										{
											foreach (var juego in juegos)
											{
												if (juego.Id == lector.GetInt32(0))
												{
													JuegoUsuarioDRM nuevoDRM = new JuegoUsuarioDRM();
													nuevoDRM.DRM = drm;

													if (lector.IsDBNull(3) == false)
													{
														if (cogerNumero == true)
														{
															nuevoDRM.Id = lector.GetInt32(3).ToString();
														}
														else
														{
															nuevoDRM.Id = lector.GetString(3);
														}
													}

													juego.DRMs.Add(nuevoDRM);
													añadir = false;
													break;
												}
											}
										}

										if (añadir == true)
										{
											JuegoUsuario nuevoJuego = new JuegoUsuario();
											nuevoJuego.Id = lector.GetInt32(0);

											if (lector.IsDBNull(1) == false)
											{
												nuevoJuego.Nombre = lector.GetString(1);
											}

											if (lector.IsDBNull(2) == false)
											{
												nuevoJuego.Imagen = lector.GetString(2);
											}

											JuegoUsuarioDRM nuevoDRM = new JuegoUsuarioDRM();
											nuevoDRM.DRM = drm;

											if (lector.IsDBNull(3) == false)
											{
												if (cogerNumero == true)
												{
													nuevoDRM.Id = lector.GetInt32(3).ToString();
												}
												else
												{
													nuevoDRM.Id = lector.GetString(3);
												}
											}

											nuevoJuego.DRMs = [nuevoDRM];

											juegos.Add(nuevoJuego);
										}
									}
								}
							}
						}
					}
				}
			}

			return juegos;
		}

		public static List<Juego> Nombre(string nombre, int cantidad = 30, SqlConnection conexion = null)
		{
			List<Juego> juegos = new List<Juego>();

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

			using (conexion)
			{
				juegos = Nombre(nombre, conexion, cantidad, true, -1, true);
			}

			if (juegos.Count > 0)
			{
				return juegos;
			}

			return null;
		}

		public static List<Juego> Nombre(string nombre, SqlConnection conexion, int cantidad = 30, bool todo = true, int tipo = -1, bool logeado = false)
		{
			List<Juego> juegos = null;

			if (string.IsNullOrEmpty(nombre) == false)
			{
				juegos = new List<Juego>();

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

				string busqueda = string.Empty;
				string busquedaTodo = "*";

				if (todo == false)
				{
					busquedaTodo = "id, nombre, imagenes, precioMinimosHistoricos, precioActualesTiendas, bundles, gratis, suscripciones, tipo, analisis, idSteam, idGog, idAmazon, exeEpic, exeUbisoft, freeToPlay";
				}

				if (nombre.Contains(" ") == true)
				{
					if (nombre.Contains("  ") == true)
					{
						nombre = nombre.Replace("  ", " ");
					}

					string[] palabras = nombre.Split(" ");

					int i = 0;
					foreach (var palabra in palabras)
					{
						if (string.IsNullOrEmpty(palabra) == false)
						{
							string palabraLimpia = Herramientas.Buscador.LimpiarNombre(palabra, true);

							if (palabraLimpia.Length > 0)
							{
								if (i == 0)
								{
									busqueda = "SELECT TOP " + cantidad + " " + busquedaTodo + " FROM juegos WHERE CHARINDEX('" + palabraLimpia + "', nombreCodigo) > 0 ";
								}
								else
								{
									bool buscar = true;

									if (palabra.ToLower() == "and")
									{
										buscar = false;
									}
									else if (palabra.ToLower() == "dlc")
									{
										buscar = false;
									}
									if (palabra.ToLower() == "expansion")
									{
										buscar = false;
									}

									if (buscar == true)
									{
										busqueda = busqueda + " AND CHARINDEX('" + palabraLimpia + "', nombreCodigo) > 0 ";
									}
								}

								i += 1;
							}
						}
					}
				}
				else
				{
					busqueda = "SELECT TOP " + cantidad + " " + busquedaTodo + " FROM juegos WHERE nombreCodigo LIKE '%" + Herramientas.Buscador.LimpiarNombre(nombre) + "%'";
				}

				if (tipo > -1)
				{
					busqueda = busqueda + " AND tipo = " + tipo.ToString();
				}

				if (logeado == false)
				{
					busqueda = busqueda + " AND (mayorEdad='false' OR mayorEdad IS NULL)";
				}

				if (string.IsNullOrEmpty(busqueda) == false)
				{
					busqueda = busqueda + " ORDER BY CASE\r\n WHEN analisis = 'null' OR analisis IS NULL THEN 0 ELSE CONVERT(int, REPLACE(JSON_VALUE(analisis, '$.Cantidad'),',',''))\r\n END DESC";
				}

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							if (todo == true)
							{
								Juego juego = new Juego();
								juego = Cargar(juego, lector);
								juegos.Add(juego);
							}
							else
							{
								Juego juego = new Juego();

								if (lector.IsDBNull(0) == false)
								{
									juego.Id = lector.GetInt32(0);
									juego.IdMaestra = lector.GetInt32(0);
								}

								if (lector.IsDBNull(1) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(1)) == false)
									{
										juego.Nombre = lector.GetString(1);
										juego.NombreCodigo = Herramientas.Buscador.LimpiarNombre(juego.Nombre);
									}
								}

								if (lector.IsDBNull(2) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(2)) == false)
									{
										juego.Imagenes = JsonSerializer.Deserialize<JuegoImagenes>(lector.GetString(2));
									}
								}

								if (lector.IsDBNull(3) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(3)) == false)
									{
										juego.PrecioMinimosHistoricos = JsonSerializer.Deserialize<List<JuegoPrecio>>(lector.GetString(3));
									}
								}

								if (lector.IsDBNull(4) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(4)) == false)
									{
										juego.PrecioActualesTiendas = JsonSerializer.Deserialize<List<JuegoPrecio>>(lector.GetString(4));
									}
								}

								if (lector.IsDBNull(5) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(5)) == false)
									{
										juego.Bundles = JsonSerializer.Deserialize<List<JuegoBundle>>(lector.GetString(5));
									}
								}

								if (lector.IsDBNull(6) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(6)) == false)
									{
										juego.Gratis = JsonSerializer.Deserialize<List<JuegoGratis>>(lector.GetString(6));
									}
								}

								if (lector.IsDBNull(7) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(7)) == false)
									{
										juego.Suscripciones = JsonSerializer.Deserialize<List<JuegoSuscripcion>>(lector.GetString(7));
									}
								}

								if (lector.IsDBNull(8) == false)
								{
									juego.Tipo = Enum.Parse<JuegoTipo>(lector.GetString(8));
								}

								if (lector.IsDBNull(9) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(9)) == false)
									{
										juego.Analisis = JsonSerializer.Deserialize<JuegoAnalisis>(lector.GetString(9));
									}
								}

								if (lector.IsDBNull(10) == false)
								{
									juego.IdSteam = lector.GetInt32(10);
								}

								if (lector.IsDBNull(11) == false)
								{
									juego.IdGog = lector.GetInt32(11);
								}

								if (lector.IsDBNull(12) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(12)) == false)
									{
										juego.IdAmazon = lector.GetString(12);
									}
								}

								if (lector.IsDBNull(13) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(13)) == false)
									{
										juego.ExeEpic = lector.GetString(13);
									}
								}

								if (lector.IsDBNull(14) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(14)) == false)
									{
										juego.ExeUbisoft = lector.GetString(14);
									}
								}

								if (lector.IsDBNull(15) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(15)) == false)
									{
										juego.FreeToPlay = lector.GetString(15);
									}
								}

								juegos.Add(juego);
							}
						}
					}
				}
			}

			return juegos;
		}

		public static List<Juego> Minimos(SqlConnection conexion = null, int ordenar = 0, List<MostrarJuegoTienda> tiendas = null, List<MostrarJuegoDRM> drms = null, List<MostrarJuegoCategoria> categorias = null, int? minimoDescuento = null, decimal? maximoPrecio = null, List<MostrarJuegoSteamDeck> deck = null, int lanzamiento = 0, int inteligenciaArtificial = 0, int? minimoReseñas = 0)
		{
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

			List<Juego> juegos = new List<Juego>();

			using (conexion)
			{
				string busqueda = "SELECT * FROM seccionMinimos";
				string dondeTiendas = string.Empty;

				#region Where

				if (tiendas != null)
				{
					if (tiendas.Count > 0)
					{
						foreach (var tienda in tiendas)
						{
							if (tienda.Estado == true)
							{
								if (string.IsNullOrEmpty(dondeTiendas) == false)
								{
									dondeTiendas = dondeTiendas + " OR ";
								}

								dondeTiendas = dondeTiendas + "JSON_VALUE(precioMinimosHistoricos, '$[0].Tienda') = '" + tienda.TiendaId + "'";
							}
						}
					}
				}

				if (string.IsNullOrEmpty(dondeTiendas) == false)
				{
					dondeTiendas = " (" + dondeTiendas + ")";
				}

				string dondeDRMs = string.Empty;

				if (drms != null)
				{
					if (drms.Count > 0)
					{
						foreach (var drm in drms)
						{
							if (drm.Estado == true)
							{
								if (string.IsNullOrEmpty(dondeDRMs) == false)
								{
									dondeDRMs = dondeDRMs + " OR ";
								}

								dondeDRMs = dondeDRMs + "JSON_VALUE(precioMinimosHistoricos, '$[0].DRM') = '" + ((int)drm.DRMId).ToString() + "'";
							}
						}
					}
				}

				if (string.IsNullOrEmpty(dondeDRMs) == false)
				{
					dondeDRMs = " (" + dondeDRMs + ")";
				}

				string dondeCategorias = string.Empty;

				if (categorias != null)
				{
					if (categorias.Count > 0)
					{
						foreach (var categoria in categorias)
						{
							if (categoria.Estado == true)
							{
								if (string.IsNullOrEmpty(dondeCategorias) == false)
								{
									dondeCategorias = dondeCategorias + " OR ";
								}

								dondeCategorias = dondeCategorias + "tipo = '" + ((int)categoria.Categoria).ToString() + "'";
							}
						}
					}
				}

				if (string.IsNullOrEmpty(dondeCategorias) == false)
				{
					dondeCategorias = " (" + dondeCategorias + ")";
				}

				string dondeMinimoDescuento = string.Empty;

				if (minimoDescuento == null)
				{
					minimoDescuento = 1;
				}

				if (minimoDescuento > 0)
				{
					dondeMinimoDescuento = "JSON_VALUE(precioMinimosHistoricos, '$[0].Descuento') >= " + minimoDescuento.ToString();
				}

				if (string.IsNullOrEmpty(dondeMinimoDescuento) == false)
				{
					dondeMinimoDescuento = " (" + dondeMinimoDescuento + ")";
				}

				string dondeMaximoPrecio = string.Empty;

				if (maximoPrecio == null)
				{
					maximoPrecio = 90;
				}

				if (maximoPrecio > 0)
				{
					dondeMaximoPrecio = "CONVERT(decimal, JSON_VALUE(precioMinimosHistoricos, '$[0].Precio')) <= " + maximoPrecio.ToString();
				}

				if (string.IsNullOrEmpty(dondeMaximoPrecio) == false)
				{
					dondeMaximoPrecio = " (" + dondeMaximoPrecio + ")";
				}

				string dondeDeck = string.Empty;

				if (deck != null)
				{
					if (deck.Count > 0)
					{
						foreach (var d in deck)
						{
							if (d.Estado == true)
							{
								if (string.IsNullOrEmpty(dondeDeck) == false)
								{
									dondeDeck = dondeDeck + " OR ";
								}

								dondeDeck = dondeDeck + "deck = '" + ((int)d.Tipo).ToString() + "'";
							}
						}
					}
				}

				if (string.IsNullOrEmpty(dondeDeck) == false)
				{
					dondeDeck = " (" + dondeDeck + ")";
				}

				if (string.IsNullOrEmpty(dondeTiendas) == false && string.IsNullOrEmpty(dondeDRMs) == false && string.IsNullOrEmpty(dondeCategorias) == false && string.IsNullOrEmpty(dondeMinimoDescuento) == false && string.IsNullOrEmpty(dondeMaximoPrecio) == false && string.IsNullOrEmpty(dondeDeck) == false)
				{
					busqueda = busqueda + " WHERE " + dondeTiendas + " AND " + dondeDRMs + " AND " + dondeCategorias + " AND " + dondeMinimoDescuento + " AND " + dondeMaximoPrecio + " AND " + dondeDeck;
				}

				if (lanzamiento == 1)
				{
					busqueda = busqueda + " AND (JSON_VALUE(caracteristicas, '$.FechaLanzamientoSteam') > DATEADD(MONTH, -6, CAST(GETDATE() as date)) OR JSON_VALUE(caracteristicas, '$.FechaLanzamientoOriginal') > DATEADD(MONTH, -6, CAST(GETDATE() as date))) ";
				}

				if (lanzamiento == 2)
				{
					busqueda = busqueda + " AND (JSON_VALUE(caracteristicas, '$.FechaLanzamientoSteam') > DATEADD(MONTH, -12, CAST(GETDATE() as date)) OR JSON_VALUE(caracteristicas, '$.FechaLanzamientoOriginal') > DATEADD(MONTH, -12, CAST(GETDATE() as date))) ";
				}

				if (lanzamiento == 3)
				{
					busqueda = busqueda + " AND (JSON_VALUE(caracteristicas, '$.FechaLanzamientoSteam') > DATEADD(MONTH, -24, CAST(GETDATE() as date)) OR JSON_VALUE(caracteristicas, '$.FechaLanzamientoOriginal') > DATEADD(MONTH, -24, CAST(GETDATE() as date))) ";
				}

				if (inteligenciaArtificial == 1)
				{
					busqueda = busqueda + " AND (inteligenciaArtificial = 'true')";
				}

				if (inteligenciaArtificial == 2)
				{
					busqueda = busqueda + " AND (inteligenciaArtificial = 'false' OR inteligenciaArtificial IS NULL)";
				}

				if (minimoReseñas != null)
				{
					if (minimoReseñas > 0)
					{
						busqueda = busqueda + " AND analisis IS NOT NULL and CONVERT(int, REPLACE(JSON_VALUE(analisis, '$.Cantidad'),',','')) > " + minimoReseñas.ToString();
					}
				}

				#endregion

				#region Order

				if (ordenar == 0)
				{
					busqueda = busqueda + " ORDER BY CASE\r\n WHEN analisis = 'null' OR analisis IS NULL THEN 0 ELSE CONVERT(int, REPLACE(JSON_VALUE(analisis, '$.Cantidad'),',',''))\r\n END DESC";
				}

				if (ordenar == 1)
				{
					busqueda = busqueda + " ORDER BY CASE\r\n WHEN analisis = 'null' OR analisis IS NULL THEN 0 ELSE CONVERT(int, JSON_VALUE(analisis, '$.Porcentaje'))\r\n END DESC";
				}

				if (ordenar == 2)
				{
					busqueda = busqueda + " ORDER BY nombre";
				}

				if (ordenar == 3)
				{
					busqueda = busqueda + " ORDER BY nombre DESC";
				}

				if (ordenar == 4)
				{
					busqueda = busqueda + " ORDER BY CASE WHEN precioMinimosHistoricos = 'null' OR precioMinimosHistoricos IS NULL THEN 1000000 ELSE CAST(JSON_VALUE(precioMinimosHistoricos, '$[0].Precio') AS decimal(18,2)) END";
				}

				if (ordenar == 5)
				{
					busqueda = busqueda + " ORDER BY CASE WHEN precioMinimosHistoricos = 'null' OR precioMinimosHistoricos IS NULL THEN 0 ELSE CAST(JSON_VALUE(precioMinimosHistoricos, '$[0].Descuento') AS bigint) END DESC";
				}

				if (ordenar == 6)
				{
					busqueda = busqueda + " ORDER BY CASE WHEN precioMinimosHistoricos = 'null' OR precioMinimosHistoricos IS NULL THEN DATEADD(YEAR, -20, CAST(GETDATE() as date)) ELSE CAST(JSON_VALUE(precioMinimosHistoricos, '$[0].FechaDetectado') AS date) END DESC";
				}

				if (ordenar == 7)
				{
					busqueda = busqueda + " ORDER BY CASE WHEN caracteristicas = 'null' OR caracteristicas IS NULL THEN DATEADD(YEAR, -20, CAST(GETDATE() as date)) ELSE CAST(JSON_VALUE(caracteristicas, '$.FechaLanzamientoSteam') AS date) END DESC";
				}

				#endregion

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							Juego juego = new Juego();
							juego = Cargar(juego, lector);

							juegos.Add(juego);
						}
					}
				}
			}

			return juegos;
		}

		public static List<Juego> Ultimos(SqlConnection conexion, string tabla, int cantidad)
		{
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

			List<Juego> juegos = new List<Juego>();

			string busqueda = "SELECT TOP (" + cantidad + ") * FROM " + tabla + " ORDER BY id DESC";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						Juego juego = new Juego();
						juego = Cargar(juego, lector);

						juegos.Add(juego);
					}
				}
			}

			return juegos;
		}

		public static List<Juego> DLCs(string idMaestro = null, SqlConnection conexion = null)
		{
			List<Juego> dlcs = new List<Juego>();

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

			using (conexion)
			{
				string busqueda = null;

				if (string.IsNullOrEmpty(idMaestro) == false)
				{
					busqueda = "SELECT * FROM juegos WHERE maestro='" + idMaestro + "' ORDER BY nombre DESC";
				}
				else
				{
					busqueda = "SELECT * FROM juegos WHERE (maestro IS NULL AND tipo='1') or (maestro='no' AND tipo='1') ORDER BY nombre DESC";
				}

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							Juego dlc = new Juego();
							dlc = Cargar(dlc, lector);

							dlcs.Add(dlc);
						}
					}
				}
			}

			return dlcs;
		}

		public static int DLCsCantidad(SqlConnection conexion = null)
		{
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

			int cantidad = 0;

			string busqueda = "SELECT COUNT(*) FROM juegos WHERE (maestro IS NULL AND tipo='1') or (maestro='no' AND tipo='1')";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				cantidad = (int)comando.ExecuteScalar();
			}

			return cantidad;
		}

		public static List<Juego> Filtro(List<string> ids, int cantidad, SqlConnection conexion = null)
		{
			List<Juego> resultados = new List<Juego>();

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

			using (conexion)
			{
				List<string> etiquetas = new List<string>();
				List<string> categorias = new List<string>();
				List<string> generos = new List<string>();
				List<string> decks = new List<string>();
				List<string> sistemas = new List<string>();
				List<string> tipos = new List<string>();

				if (ids != null)
				{
					if (ids.Count > 0)
					{
						foreach (var id in ids)
						{
							if (id.Contains("t") == true)
							{
								etiquetas.Add(id.Replace("t", null));
							}

							if (id.Contains("c") == true || id.Contains("a") == true)
							{
								categorias.Add(id.Replace("c", null));
								categorias.Add(id.Replace("a", null));
							}

							if (id.Contains("g") == true)
							{
								generos.Add(id.Replace("g", null));
							}

							if (id.Contains("d") == true)
							{
								decks.Add(id.Replace("d", null));
							}

							if (id.Contains("s") == true)
							{
								sistemas.Add(id.Replace("s", null));
							}

							if (id.Contains("i") == true)
							{
								tipos.Add(id.Replace("i", null));
							}
						}
					}
				}

				string etiquetasTexto = string.Empty;

				if (etiquetas.Count > 0)
				{
					int i = 0;

					foreach (var etiqueta in etiquetas)
					{
						if (i == 0)
						{
							etiquetasTexto = "etiquetas LIKE '%" + Strings.ChrW(34) + etiqueta + Strings.ChrW(34) + "%'";
						}
						else
						{
							etiquetasTexto = etiquetasTexto + " OR etiquetas LIKE '%" + Strings.ChrW(34) + etiqueta + Strings.ChrW(34) + "%'";
						}

						i += 1;
					}

					if (string.IsNullOrEmpty(etiquetasTexto) == false)
					{
						etiquetasTexto = " AND ISJSON(etiquetas) > 0 AND (" + etiquetasTexto + ")";
					}
				}

				string categoriasTexto = string.Empty;

				if (categorias.Count > 0)
				{
					int i = 0;

					foreach (var categoria in categorias)
					{
						if (i == 0)
						{
							categoriasTexto = "categorias LIKE '%" + Strings.ChrW(34) + categoria + Strings.ChrW(34) + "%'";
						}
						else
						{
							categoriasTexto = categoriasTexto + " OR categorias LIKE '%" + Strings.ChrW(34) + categoria + Strings.ChrW(34) + "%'";
						}

						i += 1;
					}

					if (string.IsNullOrEmpty(categoriasTexto) == false)
					{
						categoriasTexto = "  AND ISJSON(categorias) > 0 AND (" + categoriasTexto + ")";
					}
				}

				string generosTexto = string.Empty;

				if (generos.Count > 0)
				{
					int i = 0;

					foreach (var genero in generos)
					{
						if (i == 0)
						{
							generosTexto = "generos LIKE '%" + Strings.ChrW(34) + genero + Strings.ChrW(34) + "%'";
						}
						else
						{
							generosTexto = generosTexto + " OR generos LIKE '%" + Strings.ChrW(34) + genero + Strings.ChrW(34) + "%'";
						}

						i += 1;
					}

					if (string.IsNullOrEmpty(generosTexto) == false)
					{
						generosTexto = " AND ISJSON(generos) > 0 AND (" + generosTexto + ")";
					}
				}

				string deckTexto = string.Empty;

				if (decks.Count > 0)
				{
					int i = 0;

					foreach (var deck in decks)
					{
						if (i == 0)
						{
							deckTexto = "deck = " + deck;
						}
						else
						{
							deckTexto = deckTexto + " OR deck = " + deck;
						}

						i += 1;
					}

					if (string.IsNullOrEmpty(deckTexto) == false)
					{
						deckTexto = " AND (" + deckTexto + ")";
					}
				}

				string sistemasTexto = string.Empty;

				if (sistemas.Count > 0)
				{
					foreach (var sistema in sistemas)
					{
						if (string.IsNullOrEmpty(sistemasTexto) == false)
						{
							sistemasTexto = sistemasTexto + " OR ";
						}

						if (sistema == "1")
						{
							sistemasTexto = sistemasTexto + "caracteristicas LIKE '%" + Strings.ChrW(34) + "Windows" + Strings.ChrW(34) + ":true%'";
						}

						if (sistema == "2")
						{
							sistemasTexto = sistemasTexto + "caracteristicas LIKE '%" + Strings.ChrW(34) + "Mac" + Strings.ChrW(34) + ":true%'";
						}

						if (sistema == "3")
						{
							sistemasTexto = sistemasTexto + "caracteristicas LIKE '%" + Strings.ChrW(34) + "Linux" + Strings.ChrW(34) + ":true%'";
						}
					}

					if (string.IsNullOrEmpty(sistemasTexto) == false)
					{
						sistemasTexto = " AND (" + sistemasTexto + ")";
					}
				}

				string tiposTexto = string.Empty;

				if (tipos.Count > 0)
				{
					int i = 0;

					foreach (var tipo in tipos)
					{
						if (i == 0)
						{
							tiposTexto = "tipo = " + tipo;
						}
						else
						{
							tiposTexto = tiposTexto + " OR tipo = " + tipo;
						}

						i += 1;
					}

					if (string.IsNullOrEmpty(tiposTexto) == false)
					{
						tiposTexto = " AND (" + tiposTexto + ")";
					}
				}

				string busqueda = "SELECT TOP " + cantidad.ToString() + " *, CONVERT(bigint, REPLACE(JSON_VALUE(analisis, '$.Cantidad'),',','')) AS Cantidad FROM juegos " + Environment.NewLine +
					"WHERE ISJSON(analisis) > 0 " + etiquetasTexto + " " + categoriasTexto + " " + generosTexto + " " + deckTexto + " " + sistemasTexto + " " + tiposTexto +
					" ORDER BY Cantidad DESC";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							Juego juego = new Juego();
							juego = Cargar(juego, lector);

							resultados.Add(juego);
						}
					}
				}
			}

			return resultados;
		}

		public static List<Juego> Duplicados(SqlConnection conexion = null)
		{
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

			List<Juego> juegos = new List<Juego>();

			using (conexion)
			{
				string busqueda = @"SELECT * FROM juegos
 WHERE idSteam > 0 AND idSteam IN
    (SELECT idSteam FROM juegos GROUP BY idSteam HAVING COUNT(*) > 1)
    ORDER BY idSteam ";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							Juego juego = new Juego();
							juego = Cargar(juego, lector);
							juegos.Add(juego);
						}
					}
				}
			}

			return juegos;
		}
	}
}
