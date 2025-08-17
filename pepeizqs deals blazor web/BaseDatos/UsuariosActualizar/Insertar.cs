#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.UsuariosActualizar
{
	public static class Insertar
	{
		public static void Ejecutar(string idUsuario, string metodo, SqlConnection conexion = null)
		{
			if (string.IsNullOrEmpty(idUsuario) == false && string.IsNullOrEmpty(metodo) == false)
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

				if (Buscar.Existe(idUsuario, metodo, conexion) == false)
				{
					string sqlAñadir = "INSERT INTO usuariosActualizar " +
						 "(idUsuario, metodo) VALUES " +
						 "(@idUsuario, @metodo) ";

					using (SqlCommand comando = new SqlCommand(sqlAñadir, conexion))
					{
						comando.Parameters.AddWithValue("@idUsuario", idUsuario);
						comando.Parameters.AddWithValue("@metodo", metodo);

						try
						{
							comando.ExecuteNonQuery();
						}
						catch (Exception ex)
						{
							BaseDatos.Errores.Insertar.Mensaje("Insertar Usuario Actualizar", ex);
						}
					}
				}
			}
		}
	}
}
