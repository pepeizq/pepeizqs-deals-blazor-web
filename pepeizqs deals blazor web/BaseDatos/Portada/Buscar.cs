#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace BaseDatos.Portada
{
	public static class Buscar
	{
		public static List<Juego> BuscarMinimos(SqlConnection conexion = null)
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
				string busqueda = @"SELECT * FROM juegos
									WHERE ultimaModificacion >= DATEADD(day, -3, GETDATE()) AND JSON_PATH_EXISTS(analisis, '$.Cantidad') > 0 AND 
									CONVERT(bigint, REPLACE(JSON_VALUE(analisis, '$.Cantidad'),',','')) > 99 AND 
									(gratis IS NULL) AND (nombre IS NOT NULL AND LEN(nombre) > 0) AND (imagenes IS NOT NULL) AND
									((mayorEdad IS NOT NULL AND mayorEdad = 'false') OR (mayorEdad IS NULL)) AND 
									(freeToPlay = 'false' OR freeToPlay IS NULL)
									 ORDER BY CASE
									 WHEN analisis = 'null' OR analisis IS NULL THEN 0 ELSE CONVERT(int, REPLACE(JSON_VALUE(analisis, '$.Cantidad'),',',''))
									 END DESC";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							Juego juego = new Juego();
							juego = BaseDatos.Juegos.Buscar.Cargar(juego, lector);

							resultados.Add(juego);
						}
					}
				}
			}
			
			return resultados;
		}

		public static List<Juego> Destacados(SqlConnection conexion = null)
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
				string busqueda = @"SELECT TOP 6 idMaestra, nombre, JSON_VALUE(imagenes, '$.Logo') as logo, JSON_VALUE(imagenes, '$.Library_1920x620') as fondo, JSON_VALUE(imagenes, '$.Header_460x215') as header, precioMinimosHistoricos, JSON_VALUE(media, '$.Videos[0].Micro') as video, idSteam FROM seccionMinimos
WHERE tipo = 0 AND 
year(getdate()) < year(JSON_VALUE(caracteristicas, '$.FechaLanzamientoSteam')) + 5 AND
CONVERT(float, JSON_VALUE(precioMinimosHistoricos, '$[0].Precio')) > 1.99 AND 
JSON_VALUE(precioMinimosHistoricos, '$[0].DRM') = 0 AND 
CONVERT(datetime2, JSON_VALUE(precioMinimosHistoricos, '$[0].FechaActualizacion')) > DATEADD(HOUR,-24,GetDate()) AND 
(CONVERT(bigint, REPLACE(JSON_VALUE(analisis, '$.Cantidad'),',','')) > 1999 AND 
bundles IS NULL AND 
gratis IS NULL AND 
(suscripciones IS NULL OR (suscripciones IS NOT NULL AND NOT suscripciones LIKE '%,""DRM"":0,%')) OR CONVERT(bigint, REPLACE(JSON_VALUE(analisis, '$.Cantidad'),',','')) > 29999) AND 
(ocultarPortada IS NULL OR ocultarPortada = 'false') 
ORDER BY NEWID()";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
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
								}
							}

							if (lector.IsDBNull(2) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(2)) == false)
								{
									if (juego.Imagenes == null)
									{
										juego.Imagenes = new JuegoImagenes();
									}

									juego.Imagenes.Logo = lector.GetString(2);
								}
							}

							if (lector.IsDBNull(3) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(3)) == false)
								{
									if (juego.Imagenes == null)
									{
										juego.Imagenes = new JuegoImagenes();
									}

									juego.Imagenes.Library_1920x620 = lector.GetString(3);
								}
							}

							if (lector.IsDBNull(4) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(4)) == false)
								{
									if (juego.Imagenes == null)
									{
										juego.Imagenes = new JuegoImagenes();
									}

									juego.Imagenes.Header_460x215 = lector.GetString(4);
								}
							}

							if (lector.IsDBNull(5) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(5)) == false)
								{
									juego.PrecioMinimosHistoricos = JsonSerializer.Deserialize<List<JuegoPrecio>>(lector.GetString(5));
								}
							}

							if (lector.IsDBNull(6) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(6)) == false)
								{
									JuegoMedia media = new JuegoMedia();
									
									JuegoMediaVideo video = new JuegoMediaVideo();
									video.Micro = lector.GetString(6);

									media.Videos = [video];

									juego.Media = media;
								}
							}

							if (lector.IsDBNull(7) == false)
							{
								juego.IdSteam = lector.GetInt32(7);
							}

							resultados.Add(juego);
						}
					}
				}
			}

			return resultados;
		}

		public static List<Juego> Minimos(int tipo, int cantidadJuegos, List<string> categorias = null, List<string> drms = null, SqlConnection conexion = null, int cantidadAnalisis = 199)
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
				string categoria = null;

				if (categorias != null)
				{
					if (categorias.Count > 0)
					{
						int i = 0;
						foreach (var valor in categorias)
						{
							if (i == 0)
							{
								categoria = categoria + " AND (tipo = " + valor;
							}
							else if (i > 0)
							{
								categoria = categoria + " OR tipo = " + valor;
							}

							i += 1;
						}

						if (string.IsNullOrEmpty(categoria) == false)
						{
							categoria = categoria + ")";
						}
					}
				}

				string drm = null;

				if (drms != null)
				{
					if (drms.Count > 0)
					{
						int i = 0;
						foreach (var valor in drms)
						{
							if (i == 0)
							{
								drm = drm + " AND (JSON_VALUE(precioMinimosHistoricos, '$[0].DRM') = " + valor;
							}
							else if (i > 0)
							{
								drm = drm + " OR JSON_VALUE(precioMinimosHistoricos, '$[0].DRM') = " + valor;
							}

							i += 1;
						}

						if (string.IsNullOrEmpty(drm) == false)
						{
							drm = drm + ")";
						}
					}
				}

				string busqueda = @"SELECT TOP @cantidadJuegos idMaestra, nombre, imagenes, precioMinimosHistoricos, JSON_VALUE(media, '$.Videos[0].Micro'), bundles, gratis, suscripciones, idSteam, CONVERT(datetime2, JSON_VALUE(precioMinimosHistoricos, '$[0].FechaDetectado')) AS Fecha, idGog, analisis FROM seccionMinimos 
                                    WHERE CONVERT(bigint, REPLACE(JSON_VALUE(analisis, '$.Cantidad'),',','')) > @cantidadAnalisis @categoria @drm";

				if (tipo == 0)
				{
					busqueda = busqueda + " ORDER BY Fecha DESC";
				}
				else if (tipo == 1)
				{
					busqueda = busqueda + " ORDER BY CASE\r\n WHEN analisis = 'null' OR analisis IS NULL THEN 0 ELSE CONVERT(int, REPLACE(JSON_VALUE(analisis, '$.Cantidad'),',',''))\r\n END DESC";
				}
				else if (tipo == 2)
				{
					busqueda = busqueda + " AND CONVERT(datetime2, JSON_VALUE(caracteristicas, '$.FechaLanzamientoSteam')) > DATEADD(DAY,-15,GetDate()) ORDER BY Fecha DESC";
				}

				busqueda = busqueda.Replace("@cantidadJuegos", cantidadJuegos.ToString());
				busqueda = busqueda.Replace("@categoria", categoria);
				busqueda = busqueda.Replace("@drm", drm);
				busqueda = busqueda.Replace("@cantidadAnalisis", cantidadAnalisis.ToString());

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
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
									JuegoMedia media = new JuegoMedia();

									JuegoMediaVideo video = new JuegoMediaVideo();
									video.Micro = lector.GetString(4);

									media.Videos = [video];

									juego.Media = media;
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
								juego.IdSteam = lector.GetInt32(8);
							}

							if (lector.IsDBNull(10) == false)
							{
								juego.IdGog = lector.GetInt32(10);
							}

							if (lector.IsDBNull(11) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(11)) == false)
								{
									juego.Analisis = JsonSerializer.Deserialize<JuegoAnalisis>(lector.GetString(11));
								}
							}

							resultados.Add(juego);
						}
					}
				}
			}

			return resultados;
		}
	}
}
