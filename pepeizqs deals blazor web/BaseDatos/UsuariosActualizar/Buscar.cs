#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.UsuariosActualizar
{

	public static class Buscar
	{
		public static bool Existe(string idUsuario, string metodo, SqlConnection conexion = null)
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

			string sqlBuscar = "SELECT COUNT(*) FROM usuariosActualizar " +
							   "WHERE idUsuario=@idUsuario AND metodo=@metodo";

			using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
			{
				comando.Parameters.AddWithValue("@idUsuario", idUsuario);
				comando.Parameters.AddWithValue("@metodo", metodo);

				try
				{
					int count = (int)comando.ExecuteScalar();

					if (count > 0)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				catch (Exception ex)
				{
					BaseDatos.Errores.Insertar.Mensaje("Buscar Usuarios Actualizar", ex);
					return false;
				}
			}
		}

		public static List<UsuarioActualizar> Todos(SqlConnection conexion = null)
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

			List<UsuarioActualizar> usuarios = new List<UsuarioActualizar>();

			string busqueda = "SELECT * FROM usuariosActualizar";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						UsuarioActualizar usuario = new UsuarioActualizar
						{
							IdUsuario = lector.GetString(0),
							Metodo = lector.GetString(1)
						};

						usuarios.Add(usuario);
					}
				}
			}

			return usuarios;
		}
	}

	public class UsuarioActualizar
	{
		public string IdUsuario { get; set; }
		public string Metodo { get; set; }
	}
}
