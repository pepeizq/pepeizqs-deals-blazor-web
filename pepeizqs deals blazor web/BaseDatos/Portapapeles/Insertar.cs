#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Portapapeles
{
	public static class Insertar
	{
		public static void Ejecutar(string id, string contenido, SqlConnection conexion = null)
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

			if (Buscar.YaExiste(id, conexion) == false)
			{
				string insercion = "INSERT INTO portapapeles (id, contenido) VALUES (@id, @contenido)";

				using (SqlCommand comando = new SqlCommand(insercion, conexion))
				{
					comando.Parameters.AddWithValue("@id", id);
					comando.Parameters.AddWithValue("@contenido", contenido);

					try
					{
						comando.ExecuteNonQuery();
					}
					catch { }
				}
			}
			else
			{
				string actualizar = "UPDATE portapapeles SET contenido=@contenido WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(actualizar, conexion))
				{
					comando.Parameters.AddWithValue("@id", id);
					comando.Parameters.AddWithValue("@contenido", contenido);

					try
					{
						comando.ExecuteNonQuery();
					}
					catch { }
				}
			}
		}
	}
}
