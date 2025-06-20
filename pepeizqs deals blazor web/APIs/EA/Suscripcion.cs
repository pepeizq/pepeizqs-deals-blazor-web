#nullable disable

using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;

namespace APIs.EA
{
	public static class Suscripcion
	{
		public static Suscripciones2.Suscripcion Generar()
		{
			Suscripciones2.Suscripcion eaPlay = new Suscripciones2.Suscripcion
			{
				Id = Suscripciones2.SuscripcionTipo.EAPlay,
				Nombre = "EA Play",
				ImagenLogo = "/imagenes/suscripciones/eaplay.webp",
				ImagenIcono = "/imagenes/tiendas/ea_icono.webp",
				Enlace = "https://www.ea.com/ea-play",
				DRMDefecto = JuegoDRM.EA,
				AdminInteractuar = false,
				UsuarioEnlacesEspecificos = false,
				ParaSiempre = false,
				Precio = 5.99,
				AdminPendientes = true,
				TablaPendientes = "tiendaea"
			};

			return eaPlay;
		}

		public static Suscripciones2.Suscripcion GenerarPro()
		{
			Suscripciones2.Suscripcion eaPlayPro = new Suscripciones2.Suscripcion
			{
				Id = Suscripciones2.SuscripcionTipo.EAPlayPro,
				Nombre = "EA Play Pro",
				ImagenLogo = "/imagenes/suscripciones/eaplaypro.webp",
				ImagenIcono = "/imagenes/tiendas/ea_icono.webp",
				Enlace = "https://www.ea.com/ea-play",
				DRMDefecto = JuegoDRM.EA,
				AdminInteractuar = false,
				UsuarioEnlacesEspecificos = false,
				ParaSiempre = false,
				IncluyeSuscripcion = Suscripciones2.SuscripcionTipo.EAPlay,
                Precio = 16.99,
                AdminPendientes = true,
                TablaPendientes = "tiendaea"
            };

			return eaPlayPro;
		}

		public static async Task Buscar(SqlConnection conexion)
		{
			BaseDatos.Admin.Actualizar.Tiendas(Generar().Id.ToString().ToLower(), DateTime.Now, 0, conexion);

			int cantidad = 0;

			int i = BaseDatos.Admin.Buscar.TiendasValorAdicional(Generar().Id.ToString().ToLower(), "valorAdicional", conexion);

			if (i < 1)
			{
				i = 1;
			}
			else if (i > 19)
			{
				i = 1;
			}

			while (i < 20)
			{
				string html = await Decompiladores.GZipFormato2("https://www.ea.com/pagination/2-eiKprHf3zpqmQ4uWlpOgMQ6ECG%2Br7eNSz%2BiCznR1RZuGI4By6p%2Fn9YG0Ud0PBvnPoiDyJThxmtTrrxvFwVs%2F%2FMaP0KLbegXNWwlBX%2FFqh7%2B6kF9TLUbwZv7dND7mXcxxaskgrsuee8vZ9oY7j9ZC%2BbFNGi%2FHdWoshxUGKvlCSAsQ0m41qig%2Bppq6CYoNAUrevtuZd40pcBT%2BhfxLs9Rr8kKo0nENuHSMrn8TXPgJZmaVfY39ZkSENi%2BauM8%3D/page/" + i.ToString());

				if (string.IsNullOrEmpty(html) == false)
				{
					if (html.Contains("main-link-url=") == true)
					{
						int j = 0;

						while (j < 20)
						{
							if (html.Contains("main-link-url=") == true)
							{
								int int1 = html.IndexOf("main-link-url=");
								string temp1 = html.Remove(0, int1 + 15);

								html = temp1;

								int int2 = temp1.IndexOf(Strings.ChrW(34));
								string temp2 = temp1.Remove(int2, temp1.Length - int2);

								string enlace = temp2.Trim();

								enlace = enlace.Replace("/ar-sa", null);
								enlace = "https://www.ea.com" + enlace;

								int int3 = temp1.IndexOf("main-link-title=");
								string temp3 = temp1.Remove(0, int3 + 17);

								int int4 = temp3.IndexOf(Strings.ChrW(34));
								string temp4 = temp3.Remove(int4, temp3.Length - int4);

								string titulo = temp4.Trim();

								bool encontrado = false;

								string sqlBuscar = "SELECT idJuegos FROM tiendaea WHERE enlace=@enlace";

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
																				if (suscripcion.Tipo == Suscripciones2.SuscripcionTipo.EAPlay)
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
																			DRM = JuegoDRM.EA,
																			Nombre = juegobd.Nombre,
																			FechaEmpieza = DateTime.Now,
																			FechaTermina = nuevaFecha,
																			Imagen = juegobd.Imagenes.Header_460x215,
																			ImagenNoticia = juegobd.Imagenes.Header_460x215,
																			JuegoId = juegobd.Id,
																			Enlace = enlace,
																			Tipo = Suscripciones2.SuscripcionTipo.EAPlay
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

									if (encontrado == false)
									{
										BaseDatos.Suscripciones.Insertar.Temporal(conexion, Generar().Id.ToString().ToLower(), enlace, titulo);
									}
								}
							}

							j += 1;
						}

						BaseDatos.Admin.Actualizar.TiendasValorAdicional(Generar().Id.ToString().ToLower(), "valorAdicional", i + 1, conexion);
					}
					else
					{
						i = 30;
						break;
					}
				}

				i += 1;
			}

			if (cantidad == 0)
			{
				BaseDatos.Admin.Actualizar.TiendasValorAdicional(Generar().Id.ToString().ToLower(), "valorAdicional", 1, conexion);
			}
		}
	}
}
