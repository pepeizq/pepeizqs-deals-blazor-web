#nullable disable

using System.Text.Json;

namespace Herramientas.Correos
{

	public static class Enviar
	{
		public static bool Ejecutar(string html, string titulo, string correoDesde, string correoHacia)
		{
			if (string.IsNullOrEmpty(html) == false &&
				string.IsNullOrEmpty(titulo) == false &&
				string.IsNullOrEmpty(correoDesde) == false &&
				string.IsNullOrEmpty(correoHacia) == false)
			{
				WebApplicationBuilder builder = WebApplication.CreateBuilder();

				string host = builder.Configuration.GetValue<string>("Correo:Host");
				string contraseña = builder.Configuration.GetValue<string>("Correo:Contraseña");

				System.Net.Mail.MailMessage mensaje = new System.Net.Mail.MailMessage();
				mensaje.From = new System.Net.Mail.MailAddress(correoDesde, "pepeizq's deals");
				mensaje.To.Add(correoHacia);
				mensaje.Subject = titulo;
				mensaje.Body = html;
				mensaje.IsBodyHtml = true;

				System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();
				cliente.Host = host;

				string texto1 = "gmail.com";
				string texto2 = correoDesde.ToLower();

				if (texto2.Contains(texto1) == true)
				{
					cliente.Port = 587;
					cliente.Credentials = new System.Net.NetworkCredential(correoDesde, contraseña);
					cliente.EnableSsl = true;

					try
					{
						cliente.Send(mensaje);
						return true;
					}
					catch (Exception ex)
					{
						DateTime nuevaFecha = DateTime.Now;
						nuevaFecha = nuevaFecha.AddMinutes(10);

						global::BaseDatos.Admin.Actualizar.TareaUso("correosEnviar", nuevaFecha);

						global::BaseDatos.Errores.Insertar.Mensaje("Correo Enviar", ex);
						return false;
					}
				}
				else
				{
					cliente.Port = 8889;
					cliente.Credentials = new System.Net.NetworkCredential(correoDesde, contraseña);
					cliente.EnableSsl = false;

					try
					{
						cliente.Send(mensaje);
						return true;
					}
					catch (Exception ex)
					{
						DateTime nuevaFecha = DateTime.Now;
						nuevaFecha = nuevaFecha.AddMinutes(10);

						global::BaseDatos.Admin.Actualizar.TareaUso("correosEnviar", nuevaFecha);

						List<global::BaseDatos.CorreosEnviar.CorreoPendienteEnviar> pendientes = global::BaseDatos.CorreosEnviar.Buscar.PendientesEnviar();

						if (pendientes?.Count > 0)
						{
							foreach (var pendiente in pendientes.ToList())
							{
								if (pendiente.Tipo == global::BaseDatos.CorreosEnviar.CorreoPendienteTipo.Minimo && pendiente.Fecha.AddMinutes(10) < DateTime.Now)
								{
									List<CorreoMinimoJson> jsons = new List<CorreoMinimoJson>();

									foreach (var pendiente2 in pendientes)
									{
										if (pendiente2.CorreoHacia == pendiente.CorreoHacia && pendiente2.Tipo == global::BaseDatos.CorreosEnviar.CorreoPendienteTipo.Minimo && string.IsNullOrEmpty(pendiente2.Json) == false)
										{
											jsons.Add(JsonSerializer.Deserialize<CorreoMinimoJson>(pendiente2.Json));
										}
									}

									if (jsons?.Count == 1)
									{
										bool enviado = Herramientas.Correos.Enviar.Ejecutar(pendiente.Html, pendiente.Titulo, pendiente.CorreoDesde, pendiente.CorreoHacia);

										if (enviado == true)
										{
											global::BaseDatos.CorreosEnviar.Borrar.Ejecutar(pendiente.Id);
										}
										else
										{
											break;
										}
									}
									else if (jsons?.Count > 1)
									{
										Herramientas.Correos.DeseadoMinimo.Nuevos(jsons, pendiente.CorreoHacia);

										foreach (var pendiente2 in pendientes.ToList())
										{
											if (pendiente2.CorreoHacia == pendiente.CorreoHacia && pendiente2.Tipo == global::BaseDatos.CorreosEnviar.CorreoPendienteTipo.Minimo)
											{
												global::BaseDatos.CorreosEnviar.Borrar.Ejecutar(pendiente2.Id);
											}
										}

										pendientes.RemoveAll(p => p.CorreoHacia == pendiente.CorreoHacia && p.Tipo == global::BaseDatos.CorreosEnviar.CorreoPendienteTipo.Minimo);
									}
								}
							}
						}

						global::BaseDatos.Errores.Insertar.Mensaje("Correo Enviar", ex);

						return false;
					}
				}
			}

			return false;
		}
	}
}
