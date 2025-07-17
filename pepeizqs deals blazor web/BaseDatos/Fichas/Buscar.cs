#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Fichas
{

	public static class Buscar
	{
		public static bool Existe(int idJuego, int idPlataforma, string metodo, SqlConnection conexion = null)
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

			string sqlBuscar = "SELECT COUNT(*) FROM fichasActualizar " +
							   "WHERE idJuego=@idJuego AND idPlataforma=@idPlataforma AND metodo=@metodo";

			using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
			{
				comando.Parameters.AddWithValue("@idJuego", idJuego);
				comando.Parameters.AddWithValue("@idPlataforma", idPlataforma);
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
					BaseDatos.Errores.Insertar.Mensaje("Buscar Fichas Actualizar", ex);
					return false;
				}
			}
		}

		public static List<FichaActualizar> Todos(SqlConnection conexion = null)
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

			List<FichaActualizar> fichas = new List<FichaActualizar>();

			string busqueda = "SELECT * FROM fichasActualizar";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						FichaActualizar ficha = new FichaActualizar
						{
							IdJuego = lector.GetInt32(0),
							IdPlataforma = lector.GetInt32(1),
							Metodo = lector.GetString(2)
						};

						fichas.Add(ficha);
					}
				}
			}

			return fichas;
		}
	}

	public class FichaActualizar
	{
		public int IdJuego { get; set; }
		public int IdPlataforma { get; set; }
		public string Metodo { get; set; }
	}
}
