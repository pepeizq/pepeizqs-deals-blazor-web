#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace APIs.AmazonLuna
{
	public static class Suscripcion
	{
		public static Suscripciones2.Suscripcion GenerarClaims()
		{
			Suscripciones2.Suscripcion amazon = new Suscripciones2.Suscripcion
			{
				Id = Suscripciones2.SuscripcionTipo.LunaClaims,
				Nombre = "Luna Claims",
				ImagenLogo = "/imagenes/suscripciones/lunaclaims_logo.webp",
				ImagenIcono = "/imagenes/streaming/amazonluna_icono.webp",
				Enlace = "https://luna.amazon.es/claims/",
				DRMDefecto = JuegoDRM.Amazon,
				AdminInteractuar = true,
				UsuarioEnlacesEspecificos = true,
				ParaSiempre = true,
				Precio = 4.99
			};

			DateTime fechaPrime = DateTime.Now;
			fechaPrime = fechaPrime.AddMonths(1);
			fechaPrime = new DateTime(fechaPrime.Year, fechaPrime.Month, fechaPrime.Day, 19, 0, 0);

			amazon.FechaSugerencia = fechaPrime;

			return amazon;
		}

		public static Suscripciones2.Suscripcion GenerarStandard()
		{
			Suscripciones2.Suscripcion amazon = new Suscripciones2.Suscripcion
			{
				Id = Suscripciones2.SuscripcionTipo.LunaStandard,
				Nombre = "Luna Standard",
				ImagenLogo = "/imagenes/suscripciones/lunastandard_logo.webp",
				ImagenIcono = "/imagenes/streaming/amazonluna_icono.webp",
				Enlace = "https://luna.amazon.es/subscription/luna-standard",
				DRMDefecto = JuegoDRM.AmazonLuna,
				AdminInteractuar = false,
				UsuarioEnlacesEspecificos = false,
				ParaSiempre = false,
				Precio = 4.99,
				AdminPendientes = true,
				TablaPendientes = "suscripcionlunapremium"
			};

			return amazon;
		}

		public static Suscripciones2.Suscripcion GenerarPremium()
		{
			Suscripciones2.Suscripcion amazon = new Suscripciones2.Suscripcion
			{
				Id = Suscripciones2.SuscripcionTipo.LunaPremium,
				Nombre = "Luna Premium",
				ImagenLogo = "/imagenes/suscripciones/lunapremium_logo.webp",
				ImagenIcono = "/imagenes/streaming/amazonluna_icono.webp",
				Enlace = "https://luna.amazon.es/subscription/luna-premium/B085TRCCT6",
				DRMDefecto = JuegoDRM.AmazonLuna,
				AdminInteractuar = true,
				UsuarioEnlacesEspecificos = false,
				ParaSiempre = false,
				Precio = 9.99,
				AdminPendientes = true,
				TablaPendientes = "suscripcionlunapremium"
			};

			return amazon;
		}

		public static string Referido(string enlace)
		{
			if (enlace.Contains("?") == true)
			{
				enlace = enlace + "&tag=ofedeunpan-21";
			}
			else
			{
				enlace = enlace + "?tag=ofedeunpan-21";
			}

			return enlace;
		}

		public static async Task Buscar(SqlConnection conexion = null)
		{
			await Task.Yield();

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

			List<int> idsBorrar = new List<int>();
			int cantidad = 0;
			string busqueda = string.Empty;

			#region Standard

			BaseDatos.Admin.Actualizar.Tiendas(GenerarStandard().Id.ToString().ToLower(), DateTime.Now, 0, conexion);

			idsBorrar = new List<int>();
			cantidad = 0;

			List<AmazonLunaJuego> juegosStandard = null;

			busqueda = "SELECT * FROM temporallunastandardjson";

			try
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

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							if (lector.IsDBNull(1) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(1)) == false)
								{
									juegosStandard = JsonSerializer.Deserialize<List<AmazonLunaJuego>>(lector.GetString(1));
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				BaseDatos.Errores.Insertar.Mensaje("Luna Standard Suscripcion 1", ex);
			}

			if (juegosStandard?.Count > 0)
			{
				foreach (var juego in juegosStandard)
				{
					if (string.IsNullOrEmpty(juego.Id) == false)
					{
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

						string sqlBuscar = "SELECT idJuegos FROM " + GenerarPremium().TablaPendientes + " WHERE enlace=@enlace";

						using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
						{
							comando.Parameters.AddWithValue("@enlace", juego.Id);

							using (SqlDataReader lector = comando.ExecuteReader())
							{
								if (lector.Read() == true)
								{
									cantidad += 1;
									BaseDatos.Admin.Actualizar.Tiendas(GenerarStandard().Id.ToString().ToLower(), DateTime.Now, cantidad, conexion);

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
														Juegos.Juego juegobd = BaseDatos.Juegos.Buscar.UnJuego(int.Parse(id));

														if (juegobd != null)
														{
															bool añadirSuscripcion = true;

															if (juegobd.Suscripciones?.Count > 0)
															{
																bool actualizar = false;

																foreach (var suscripcion in juegobd.Suscripciones)
																{
																	if (suscripcion.Tipo == Suscripciones2.SuscripcionTipo.LunaStandard)
																	{
																		añadirSuscripcion = false;
																		actualizar = true;

																		DateTime nuevaFecha = suscripcion.FechaTermina;
																		nuevaFecha = DateTime.Now;
																		nuevaFecha = nuevaFecha + TimeSpan.FromDays(2);
																		suscripcion.FechaTermina = nuevaFecha;
																	}
																}

																if (actualizar == true)
																{
																	BaseDatos.Juegos.Actualizar.Suscripciones(juegobd, conexion);

																	JuegoSuscripcion suscripcion2 = BaseDatos.Suscripciones.Buscar.UnJuego(juego.Id);

																	if (suscripcion2 != null)
																	{
																		DateTime nuevaFecha = suscripcion2.FechaTermina;
																		nuevaFecha = DateTime.Now;
																		nuevaFecha = nuevaFecha + TimeSpan.FromDays(2);
																		suscripcion2.FechaTermina = nuevaFecha;
																		BaseDatos.Suscripciones.Actualizar.FechaTermina(suscripcion2, conexion);
																	}
																}
															}

															if (añadirSuscripcion == true)
															{
																DateTime nuevaFecha = DateTime.Now;
																nuevaFecha = nuevaFecha + TimeSpan.FromDays(2);

																JuegoSuscripcion nuevaSuscripcion = new JuegoSuscripcion
																{
																	DRM = JuegoDRM.AmazonLuna,
																	Nombre = juegobd.Nombre,
																	FechaEmpieza = DateTime.Now,
																	FechaTermina = nuevaFecha,
																	Imagen = juegobd.Imagenes.Header_460x215,
																	ImagenNoticia = juegobd.Imagenes.Header_460x215,
																	JuegoId = juegobd.Id,
																	Enlace = juego.Id,
																	Tipo = Suscripciones2.SuscripcionTipo.LunaStandard
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
							BaseDatos.Suscripciones.Insertar.Temporal(conexion, GenerarStandard().Id.ToString().ToLower(), juego.Id, juego.Nombre);
						}
					}
				}

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

				string limpieza = "DELETE FROM temporallunastandardjson WHERE enlace='1'";

				using (SqlCommand comandoBorrar = new SqlCommand(limpieza, conexion))
				{
					try
					{
						comandoBorrar.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Mensaje(GenerarStandard().Id.ToString().ToLower(), ex, conexion);
					}
				}
			}

			#endregion

			#region Premium

			BaseDatos.Admin.Actualizar.Tiendas(GenerarPremium().Id.ToString().ToLower(), DateTime.Now, 0, conexion);

			idsBorrar = new List<int>();
			cantidad = 0;

			List<AmazonLunaJuego> juegosPremium = null;

			busqueda = "SELECT * FROM temporallunapremiumjson";

			try
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

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							if (lector.IsDBNull(1) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(1)) == false)
								{
									juegosPremium = JsonSerializer.Deserialize<List<AmazonLunaJuego>>(lector.GetString(1));
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				BaseDatos.Errores.Insertar.Mensaje("Luna Premium Suscripcion 1", ex);
			}

			if (juegosPremium?.Count > 0)
			{
				foreach (var juego in juegosPremium)
				{
					if (string.IsNullOrEmpty(juego.Id) == false)
					{
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

						string sqlBuscar = "SELECT idJuegos FROM " + GenerarPremium().TablaPendientes + " WHERE enlace=@enlace";

						using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
						{
							comando.Parameters.AddWithValue("@enlace", juego.Id);

							using (SqlDataReader lector = comando.ExecuteReader())
							{
								if (lector.Read() == true)
								{
									cantidad += 1;
									BaseDatos.Admin.Actualizar.Tiendas(GenerarPremium().Id.ToString().ToLower(), DateTime.Now, cantidad, conexion);

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
														Juegos.Juego juegobd = BaseDatos.Juegos.Buscar.UnJuego(int.Parse(id));

														if (juegobd != null)
														{
															bool añadirSuscripcion = true;

															if (juegobd.Suscripciones?.Count > 0)
															{
																bool actualizar = false;

																foreach (var suscripcion in juegobd.Suscripciones)
																{
																	if (suscripcion.Tipo == Suscripciones2.SuscripcionTipo.LunaPremium)
																	{
																		añadirSuscripcion = false;
																		actualizar = true;

																		DateTime nuevaFecha = suscripcion.FechaTermina;
																		nuevaFecha = DateTime.Now;
																		nuevaFecha = nuevaFecha + TimeSpan.FromDays(2);
																		suscripcion.FechaTermina = nuevaFecha;
																	}
																}

																if (actualizar == true)
																{
																	BaseDatos.Juegos.Actualizar.Suscripciones(juegobd, conexion);

																	JuegoSuscripcion suscripcion2 = BaseDatos.Suscripciones.Buscar.UnJuego(juego.Id);

																	if (suscripcion2 != null)
																	{
																		DateTime nuevaFecha = suscripcion2.FechaTermina;
																		nuevaFecha = DateTime.Now;
																		nuevaFecha = nuevaFecha + TimeSpan.FromDays(2);
																		suscripcion2.FechaTermina = nuevaFecha;
																		BaseDatos.Suscripciones.Actualizar.FechaTermina(suscripcion2, conexion);
																	}
																}
															}

															if (añadirSuscripcion == true)
															{
																DateTime nuevaFecha = DateTime.Now;
																nuevaFecha = nuevaFecha + TimeSpan.FromDays(2);

																JuegoSuscripcion nuevaSuscripcion = new JuegoSuscripcion
																{
																	DRM = JuegoDRM.AmazonLuna,
																	Nombre = juegobd.Nombre,
																	FechaEmpieza = DateTime.Now,
																	FechaTermina = nuevaFecha,
																	Imagen = juegobd.Imagenes.Header_460x215,
																	ImagenNoticia = juegobd.Imagenes.Header_460x215,
																	JuegoId = juegobd.Id,
																	Enlace = juego.Id,
																	Tipo = Suscripciones2.SuscripcionTipo.LunaPremium
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
							BaseDatos.Suscripciones.Insertar.Temporal(conexion, GenerarPremium().Id.ToString().ToLower(), juego.Id, juego.Nombre);
						}
					}
				}

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

				string limpieza = "DELETE FROM temporallunapremiumjson WHERE enlace='1'";

				using (SqlCommand comandoBorrar = new SqlCommand(limpieza, conexion))
				{
					try
					{
						comandoBorrar.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Mensaje(GenerarPremium().Id.ToString().ToLower(), ex, conexion);
					}
				}
			}

			#endregion
		}
	}

	public class AmazonLunaJuego
	{
		public string Id { get; set; }
		public string Nombre { get; set; }
	}
}
