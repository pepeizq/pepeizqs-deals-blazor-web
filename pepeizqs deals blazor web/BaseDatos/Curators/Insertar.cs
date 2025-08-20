#nullable disable

using APIs.Steam;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace BaseDatos.Curators
{
	public static class Insertar
	{
		public static void Ejecutar(SteamCuratorAPI api, SqlConnection conexion = null)
		{
			if (api != null)
			{
				if (string.IsNullOrEmpty(api.Nombre) == false || string.IsNullOrEmpty(api.Slug) == false)
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

					string sqlAñadir = "INSERT INTO curators " +
							 "(idSteam, nombre, imagen, descripcion, slug, steamIds, web, fecha) VALUES " +
							 "(@idSteam, @nombre, @imagen, @descripcion, @slug, @steamIds, @web, @fecha) ";

					string nombre = api.Nombre;

					if (string.IsNullOrEmpty(nombre) == true)
					{
						nombre = api.Slug;
					}

					using (SqlCommand comando = new SqlCommand(sqlAñadir, conexion))
					{
						comando.Parameters.AddWithValue("@idSteam", api.Id);
						comando.Parameters.AddWithValue("@nombre", nombre);
						comando.Parameters.AddWithValue("@imagen", api.Imagen);
						comando.Parameters.AddWithValue("@descripcion", api.Descripcion);
						comando.Parameters.AddWithValue("@steamIds", JsonSerializer.Serialize(api.SteamIds));
						comando.Parameters.AddWithValue("@web", JsonSerializer.Serialize(api.Web));
						comando.Parameters.AddWithValue("@fecha", DateTime.Now);

						if (string.IsNullOrEmpty(api.Slug) == false)
						{
							comando.Parameters.AddWithValue("@slug", api.Slug);
						}
						else
						{
							comando.Parameters.AddWithValue("@slug", Herramientas.EnlaceAdaptador.Nombre(api.Nombre));
						}

						try
						{
							comando.ExecuteNonQuery();
						}
						catch (Exception ex)
						{
							BaseDatos.Errores.Insertar.Mensaje("Insertar Curator", ex);
						}

						comando.Dispose();
					}

					conexion.Dispose();
				}
			}
		}
	}
}
