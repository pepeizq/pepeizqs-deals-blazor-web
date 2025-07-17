#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace Tareas
{
	public class FichasActualizar : BackgroundService
	{
		private readonly ILogger<FichasActualizar> _logger;
		private readonly IServiceScopeFactory _factoria;
		private readonly IDecompiladores _decompilador;

		public FichasActualizar(ILogger<FichasActualizar> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
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

							if (BaseDatos.Admin.Buscar.TareaPosibleUsar("fichasActualizar", siguienteComprobacion, conexion) == true)
							{
								BaseDatos.Admin.Actualizar.TareaUso("fichasActualizar", DateTime.Now, conexion);

								List<BaseDatos.Fichas.FichaActualizar> fichas = BaseDatos.Fichas.Buscar.Todos(conexion);

								if (fichas.Count > 0)
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

												BaseDatos.Fichas.Limpiar.Una(ficha, conexion);
											}
										}

										if (ficha.Metodo == "GOGAPI")
										{
											Juegos.Juego juego = BaseDatos.Juegos.Buscar.UnJuego(ficha.IdJuego);

											if (juego != null)
											{
												juego.GalaxyGOG = await APIs.GOG.Juego.GalaxyDatos(ficha.IdPlataforma.ToString());
												juego.Idiomas = await APIs.GOG.Juego.GalaxyIdiomas(ficha.IdPlataforma.ToString(), juego.Idiomas);

												BaseDatos.Juegos.Actualizar.GalaxyGOG(juego);

												BaseDatos.Fichas.Limpiar.Una(ficha, conexion);
											}
										}
									}
								}
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