#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.UsuariosActualizar
{
	public static class Limpiar
	{
		public static void Una(UsuarioActualizar usuario, SqlConnection conexion = null)
		{
			if (usuario != null)
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

				string sqlEliminar = "DELETE FROM usuariosActualizar WHERE idUsuario=@idUsuario AND metodo=@metodo";

				using (SqlCommand comando = new SqlCommand(sqlEliminar, conexion))
				{
					comando.Parameters.AddWithValue("@idUsuario", usuario.IdUsuario);
					comando.Parameters.AddWithValue("@metodo", usuario.Metodo);

					try
					{
						comando.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Mensaje("Limpiar Usuarios Actualizar", ex);
					}
				}
			}
		}
	}
}