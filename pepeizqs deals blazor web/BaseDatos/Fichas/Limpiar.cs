#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Fichas
{
	public static class Limpiar
	{
		public static void Una(FichaActualizar ficha, SqlConnection conexion = null)
		{
			if (ficha != null)
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
				string sqlEliminar = "DELETE FROM fichasActualizar WHERE idJuego=@idJuego AND idPlataforma=@idPlataforma AND metodo=@metodo";

				using (SqlCommand comando = new SqlCommand(sqlEliminar, conexion))
				{
					comando.Parameters.AddWithValue("@idJuego", ficha.IdJuego);
					comando.Parameters.AddWithValue("@idPlataforma", ficha.IdPlataforma);
					comando.Parameters.AddWithValue("@metodo", ficha.Metodo);

					try
					{
						comando.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Mensaje("Limpiar Fichas Actualizar", ex);
					}
				}
			}
		}
	}
}
