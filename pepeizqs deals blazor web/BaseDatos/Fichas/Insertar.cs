#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Fichas
{
	public static class Insertar
	{
		public static void Ejecutar(int idJuego, int idPlataforma, string metodo, SqlConnection conexion = null)
		{
			if (idJuego > 0 && idPlataforma > 0 && string.IsNullOrEmpty(metodo) == false)
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

				if (Buscar.Existe(idJuego, idPlataforma, metodo, conexion) == false)
				{
					string sqlAñadir = "INSERT INTO fichasActualizar " +
						 "(idJuego, idPlataforma, metodo) VALUES " +
						 "(@idJuego, @idPlataforma, @metodo) ";

					using (SqlCommand comando = new SqlCommand(sqlAñadir, conexion))
					{
						comando.Parameters.AddWithValue("@idJuego", idJuego);
						comando.Parameters.AddWithValue("@idPlataforma", idPlataforma);
						comando.Parameters.AddWithValue("@metodo", metodo);

						try
						{
							comando.ExecuteNonQuery();
						}
						catch (Exception ex)
						{
							BaseDatos.Errores.Insertar.Mensaje("Insertar Fichas Actualizar", ex);
						}
					}
				}
			}
		}
	}
}
