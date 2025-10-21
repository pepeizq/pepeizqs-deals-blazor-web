﻿#nullable disable

using APIs.Steam;
using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;

namespace Tareas
{
	public class JuegosActualizar : BackgroundService
	{
		private readonly ILogger<JuegosActualizar> _logger;
		private readonly IServiceScopeFactory _factoria;
		private readonly IDecompiladores _decompilador;

		public JuegosActualizar(ILogger<JuegosActualizar> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
		{
			_logger = logger;
			_factoria = factory;
			_decompilador = decompilador;
		}

		protected override async Task ExecuteAsync(CancellationToken tokenParar)
		{
			using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(20));

			while (await timer.WaitForNextTickAsync(tokenParar))
			{
				WebApplicationBuilder builder = WebApplication.CreateBuilder();
				string piscinaTiendas = builder.Configuration.GetValue<string>("PoolTiendas:Contenido");
				string piscinaUsada = Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);

				if (piscinaTiendas == piscinaUsada)
				{
					SqlConnection conexion = new SqlConnection();

					try
					{
						conexion = Herramientas.BaseDatos.Conectar();
					}
					catch { }

					if (conexion.State == System.Data.ConnectionState.Open)
					{
						try
						{
							TimeSpan siguienteComprobacion = TimeSpan.FromMinutes(1);

							if (BaseDatos.Admin.Buscar.TareaPosibleUsar("fichasActualizar", siguienteComprobacion, conexion) == true && BaseDatos.Admin.Buscar.TiendasLibre(conexion) == true)
							{
								BaseDatos.Admin.Actualizar.TareaUso("fichasActualizar", DateTime.Now, conexion);

								#region Buscar 

								List<Juegos.Juego> juegosAleatorios = BaseDatos.Juegos.Buscar.Aleatorios(true);

								if (juegosAleatorios?.Count > 0)
								{
									foreach (var juego in juegosAleatorios)
									{
										bool actualizarDatosAPI = false;
										TimeSpan tiempoComprobar = TimeSpan.FromDays(60);

										if (juego.Caracteristicas?.FechaLanzamientoSteam != null)
										{
											if (DateTime.Now.Subtract(juego.Caracteristicas.FechaLanzamientoSteam).TotalDays < 1)
											{
												tiempoComprobar = TimeSpan.FromHours(6);
											}
											else if (DateTime.Now.Subtract(juego.Caracteristicas.FechaLanzamientoSteam).TotalDays < 3)
											{
												tiempoComprobar = TimeSpan.FromHours(12);
											}
											else if (DateTime.Now.Subtract(juego.Caracteristicas.FechaLanzamientoSteam).TotalDays < 7)
											{
												tiempoComprobar = TimeSpan.FromDays(1);
											}
											else if (DateTime.Now.Subtract(juego.Caracteristicas.FechaLanzamientoSteam).TotalDays < 14)
											{
												tiempoComprobar = TimeSpan.FromDays(3);
											}
											else if (DateTime.Now.Subtract(juego.Caracteristicas.FechaLanzamientoSteam).TotalDays < 30)
											{
												tiempoComprobar = TimeSpan.FromDays(5);
											}
											else if (DateTime.Now.Subtract(juego.Caracteristicas.FechaLanzamientoSteam).TotalDays < 120)
											{
												tiempoComprobar = TimeSpan.FromDays(30);
											}
										}

										if (juego.FechaSteamAPIComprobacion.Add(tiempoComprobar) < DateTime.Now)
										{
											actualizarDatosAPI = true;
										}

										if (actualizarDatosAPI == true)
										{
											BaseDatos.JuegosActualizar.Insertar.Ejecutar(juego.Id, juego.IdSteam, "SteamAPI");
										}
									}
								}

								#endregion

								#region Actualizar

								List<BaseDatos.JuegosActualizar.JuegoActualizar> fichas = BaseDatos.JuegosActualizar.Buscar.Todos(conexion);

								if (fichas?.Count > 0)
								{
									foreach (var ficha in fichas)
									{
										if (ficha.Metodo == "SteamAPI")
										{
											Juegos.Juego nuevoJuego = await APIs.Steam.Juego.CargarDatosJuego(ficha.IdPlataforma.ToString());
											Juegos.Juego juego = BaseDatos.Juegos.Buscar.UnJuego(ficha.IdJuego);

											if (nuevoJuego != null && juego != null)
											{
												BaseDatos.Juegos.Actualizar.Media(nuevoJuego, juego);

												BaseDatos.JuegosActualizar.Limpiar.Una(ficha, conexion);
											}
										}

										if (ficha.Metodo == "GOGAPI")
										{
											Juegos.Juego juego = BaseDatos.Juegos.Buscar.UnJuego(ficha.IdJuego);

											if (juego != null)
											{
												Juegos.JuegoGalaxyGOG datosGOG = await APIs.GOG.Juego.GalaxyDatos(ficha.IdPlataforma.ToString());

												if (datosGOG != null)
												{
													juego.GalaxyGOG = datosGOG;
													juego.Idiomas = await APIs.GOG.Juego.GalaxyIdiomas(ficha.IdPlataforma.ToString(), juego.Idiomas);

													BaseDatos.Juegos.Actualizar.GalaxyGOG(juego);
												}

												BaseDatos.JuegosActualizar.Limpiar.Una(ficha, conexion);
											}
										}

										if (ficha.Metodo == "EpicAPI")
										{
											Juegos.Juego juego = BaseDatos.Juegos.Buscar.UnJuego(ficha.IdJuego);

											if (juego != null)
											{
												if (string.IsNullOrEmpty(juego.SlugEpic) == false)
												{
													juego.EpicGames = await APIs.EpicGames.Juego.EpicGamesDatos(juego.SlugEpic);
													juego.Idiomas = await APIs.EpicGames.Juego.EpicGamesIdiomas(juego.SlugEpic, juego.Idiomas);

													BaseDatos.Juegos.Actualizar.EpicGames(juego);
												}

												BaseDatos.JuegosActualizar.Limpiar.Una(ficha, conexion);
											}
										}

										if (ficha.Metodo == "XboxAPI")
										{
											Juegos.Juego juego = BaseDatos.Juegos.Buscar.UnJuego(ficha.IdJuego);

											if (juego != null)
											{
												if (string.IsNullOrEmpty(juego.IdXbox) == false)
												{
													juego.Xbox = await APIs.Xbox.Juego.XboxDatos(juego.IdXbox);
													juego.Idiomas = await APIs.Xbox.Juego.XboxIdiomas(juego.IdXbox, juego.Idiomas);

													BaseDatos.Juegos.Actualizar.Xbox(juego);
												}

												BaseDatos.JuegosActualizar.Limpiar.Una(ficha, conexion);
											}
										}

										if (ficha.Metodo == "SteamCMD")
										{
											Juegos.Juego juego = BaseDatos.Juegos.Buscar.UnJuego(ficha.IdJuego);

											bool inteligenciaArtificial = false;
											DateTime? fechaSteam = null;

											if (juego.IdSteam > 0)
											{
												SteamCMDDatos datosCmd = await APIs.Steam.Juego.UltimaActualizacioneInteligenciaArtificial(juego.IdSteam);

												if (datosCmd != null)
												{
													if (datosCmd.UltimaActualizacion != null)
													{
														fechaSteam = datosCmd.UltimaActualizacion;
													}

													if (datosCmd.InteligenciaArtificial == true)
													{
														inteligenciaArtificial = true;
													}
												}
											}

											DateTime? fechaGOG = null;

											if (juego.IdGog > 0)
											{
												fechaGOG = await APIs.GOG.Juego.UltimaActualizacion(juego.IdGog.ToString());
											}

											global::BaseDatos.Juegos.Actualizar.UltimasActualizacioneseInteligenciaArticial(juego.Id, fechaSteam, fechaGOG, inteligenciaArtificial);
										}
									}
								}

								#endregion
							}
						}
						catch (Exception ex)
						{
							BaseDatos.Errores.Insertar.Mensaje("Tarea - Fichas Actualizar", ex, conexion);
						}
					}
				}
			}
		}

		public override async Task StopAsync(CancellationToken stoppingToken)
		{
			await base.StopAsync(stoppingToken);
		}
	}
}