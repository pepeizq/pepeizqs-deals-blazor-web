#nullable disable

using ApexCharts;
using Herramientas;
using Herramientas.Redireccionador;
using Juegos;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System.Globalization;
using System.Text.Json;

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
				AdminInteractuar = true,
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
                AdminPendientes = false,
                TablaPendientes = "tiendaea"
            };

			return eaPlayPro;
		}

		public static async Task Buscar(SqlConnection conexion)
		{
			BaseDatos.Admin.Actualizar.Tiendas(Generar().Id.ToString().ToLower(), DateTime.Now, 0, conexion);

			int cantidad = 0;

			int i = 1;
			while (i < 20)
			{
				string html = await Decompiladores.GZipFormato2("https://www.ea.com/pagination/2-eiKprHf3zpqmQ4uWlpOgMQ6ECG%2Br7eNSz%2BiCznR1RZuGI4By6p%2Fn9YG0Ud0PBvnPoiDyJThxmtTrrxvFwVs%2F%2FMaP0KLbegXNWwlBX%2FFqh7%2B6kF9TLUbwZv7dND7mXcxxaskgrsuee8vZ9oY7j9ZC%2BbFNGi%2FHdWoshxUGKvlCSAsQ0m41qig%2Bppq6CYoNAUrevtuZd40pcBT%2BhfxLs9Rr8kKo0nENuHSMrn8TXPgJZmaVfY39ZkSENi%2BauM8%3D/page/" + i.ToString());

				if (string.IsNullOrEmpty(html) == false)
				{
					if (html.Contains("main-link-url=") == true)
					{
						int j = 0;

						while (j < 12)
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

								cantidad += 1;
							}

							j += 1;
						}
					}
					else
					{
						break;
					}
				}

				BaseDatos.Errores.Insertar.Mensaje("EA Suscripciones", i.ToString());

				i += 1;
			}

			BaseDatos.Errores.Insertar.Mensaje("EA Suscripciones", "Cantidad de juegos encontrados: " + cantidad.ToString());
		}
	}
}
