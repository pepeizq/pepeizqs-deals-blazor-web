#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Portapapeles
{
	public static class Borrar
	{
		public static void Ejecutar(string id, SqlConnection conexion = null)
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
			string borrar = "DELETE FROM portapapeles WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(borrar, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);

				try
				{
					comando.ExecuteNonQuery();
				}
				catch { }
			}
		}

		public static void Limpieza(SqlConnection conexion = null)
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

			using (conexion)
			{
				string sqlActualizar = "TRUNCATE TABLE portapapeles";

				using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
				{
					try
					{
						comando.ExecuteNonQuery();
					}
					catch
					{

					}
				}
			}
		}
	}
}
