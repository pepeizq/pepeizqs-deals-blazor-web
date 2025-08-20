#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.JuegosActualizar
{

	public static class Buscar
	{
		public static bool Existe(int idJuego, int idPlataforma, string metodo, SqlConnection conexion = null)
		{
			bool existe = false;

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
						existe = true;
					}
				}
				catch (Exception ex)
				{
					BaseDatos.Errores.Insertar.Mensaje("Buscar Fichas Actualizar", ex);
				}

				comando.Dispose();
			}

			return existe;
		}

		public static List<JuegoActualizar> Todos(SqlConnection conexion = null)
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

			List<JuegoActualizar> fichas = new List<JuegoActualizar>();

			string busqueda = "SELECT * FROM fichasActualizar";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						JuegoActualizar ficha = new JuegoActualizar
						{
							IdJuego = lector.GetInt32(0),
							IdPlataforma = lector.GetInt32(1),
							Metodo = lector.GetString(2)
						};

						fichas.Add(ficha);
					}

					lector.Dispose();
				}

				comando.Dispose();
			}

			conexion.Dispose();

			return fichas;
		}
	}

	public class JuegoActualizar
	{
		public int IdJuego { get; set; }
		public int IdPlataforma { get; set; }
		public string Metodo { get; set; }
	}
}
