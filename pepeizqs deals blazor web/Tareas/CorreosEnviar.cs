#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace Tareas
{
	public class CorreosEnviar : BackgroundService
	{
		private readonly ILogger<CorreosEnviar> _logger;
		private readonly IServiceScopeFactory _factoria;
		private readonly IDecompiladores _decompilador;

		public CorreosEnviar(ILogger<CorreosEnviar> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
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
				string piscinaApp = builder.Configuration.GetValue<string>("PoolWeb:Contenido");
				string piscinaUsada = Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);

				if (piscinaApp == piscinaUsada)
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

							if (BaseDatos.Admin.Buscar.TareaPosibleUsar("correosEnviar", siguienteComprobacion, conexion) == true)
							{
								BaseDatos.Admin.Actualizar.TareaUso("correosEnviar", DateTime.Now, conexion);

								List<BaseDatos.CorreosEnviar.CorreoPendienteEnviar> pendientes = BaseDatos.CorreosEnviar.Buscar.PendientesEnviar(conexion);

								if (pendientes?.Count > 0)
								{
									foreach (var pendiente in pendientes.ToList())
									{
										if (pendiente.Tipo == BaseDatos.CorreosEnviar.CorreoPendienteTipo.Minimo && pendiente.Fecha.AddMinutes(10) < DateTime.Now)
										{
											List<CorreoMinimoJson> jsons = new List<CorreoMinimoJson>();

											foreach (var pendiente2 in pendientes)
											{
												if (pendiente2.CorreoHacia == pendiente.CorreoHacia && pendiente2.Tipo == BaseDatos.CorreosEnviar.CorreoPendienteTipo.Minimo && string.IsNullOrEmpty(pendiente2.Json) == false)
												{
													jsons.Add(JsonSerializer.Deserialize<CorreoMinimoJson>(pendiente2.Json));
												}
											}

											if (jsons?.Count == 1)
											{
												bool enviado = Herramientas.Correos.EnviarCorreo(pendiente.Html, pendiente.Titulo, pendiente.CorreoDesde, pendiente.CorreoHacia);

												if (enviado == true)
												{
													BaseDatos.CorreosEnviar.Borrar.Ejecutar(pendiente.Id, conexion);
												}
												else
												{
													break;
												}
											}
											else if (jsons?.Count > 1)
											{
												Herramientas.Correos.EnviarNuevosMinimos(jsons, pendiente.CorreoHacia);

												foreach (var pendiente2 in pendientes.ToList())
												{
													if (pendiente2.CorreoHacia == pendiente.CorreoHacia && pendiente2.Tipo == BaseDatos.CorreosEnviar.CorreoPendienteTipo.Minimo)
													{
														BaseDatos.CorreosEnviar.Borrar.Ejecutar(pendiente2.Id, conexion);
													}
												}

												pendientes.RemoveAll(p => p.CorreoHacia == pendiente.CorreoHacia && p.Tipo == BaseDatos.CorreosEnviar.CorreoPendienteTipo.Minimo);
											}
										}
										else
										{
											bool enviado = Herramientas.Correos.EnviarCorreo(pendiente.Html, pendiente.Titulo, pendiente.CorreoDesde, pendiente.CorreoHacia);

											if (enviado == true)
											{
												BaseDatos.CorreosEnviar.Borrar.Ejecutar(pendiente.Id, conexion);
											}
											else
											{
												break;
											}
										}
									}
								}
							}
						}
						catch (Exception ex)
						{
							BaseDatos.Errores.Insertar.Mensaje("Tarea - Correos Enviar", ex, conexion);
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
