#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Portapapeles
{
	public static class Buscar
	{
		public static bool YaExiste(string id, SqlConnection conexion = null)
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

			string busqueda = "SELECT COUNT(*) FROM portapapeles WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);

				try
				{
					int resultado = (int)comando.ExecuteScalar();
					return resultado > 0;
				}
				catch
				{
					return false;
				}
			}
		}

		public static string Contenido(string id, SqlConnection conexion = null)
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

			string busqueda = "SELECT contenido FROM portapapeles WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);

				try
				{
					return comando.ExecuteScalar()?.ToString() ?? string.Empty;
				}
				catch
				{
					return string.Empty;
				}
			}
		}
	}
}
