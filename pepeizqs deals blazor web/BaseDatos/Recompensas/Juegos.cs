#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Recompensas
{
	public class RecompensaJuego
	{
		public int Id;
		public int JuegoId;
		public string Clave;
		public int Coins;
		public DateTime FechaEmpieza;
		public string UsuarioId;
		public JuegoDRM DRM;
		public string JuegoNombre;
		public DateTime? FechaCaduca;
	}

	public static class Juegos
	{
		public static RecompensaJuego Cargar(SqlDataReader lector)
		{
			RecompensaJuego juego = new RecompensaJuego
			{
				Id = lector.GetInt32(0),
				JuegoId = lector.GetInt32(1),
				Clave = lector.GetString(2),
				Coins = lector.GetInt32(3),
				FechaEmpieza = lector.GetDateTime(5)
			};

			if (lector.IsDBNull(4) == false)
			{
				juego.UsuarioId = lector.GetString(4);
			}

			if (lector.IsDBNull(6) == false)
			{
				juego.DRM = Enum.Parse<JuegoDRM>(lector.GetInt32(6).ToString());
			}
			else
			{
				juego.DRM = JuegoDRM.Steam;
			}

			if (lector.IsDBNull(7) == false)
			{
				juego.JuegoNombre = lector.GetString(7);
			}

			if (lector.IsDBNull(8) == false)
			{
				juego.FechaCaduca = lector.GetDateTime(8);
			}

			return juego;
		}

		public static void Insertar(RecompensaJuego recompensa, SqlConnection conexion = null)
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
				string añadirFecha1 = null;
				string añadirFecha2 = null;

				if (recompensa.FechaCaduca != null)
				{
					añadirFecha1 = ", fechaCaduca";
					añadirFecha2 = ", @fechaCaduca";
				}

				string sqlInsertar = "INSERT INTO recompensasJuegos " +
					"(juegoId, clave, coins, fecha, juegoNombre, drm" + añadirFecha1 + ") VALUES " +
					"(@juegoId, @clave, @coins, @fecha, @juegoNombre, @drm" + añadirFecha2 + ") ";

				using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
				{
					comando.Parameters.AddWithValue("@juegoId", recompensa.JuegoId.ToString());
					comando.Parameters.AddWithValue("@clave", recompensa.Clave);
					comando.Parameters.AddWithValue("@coins", recompensa.Coins);
					comando.Parameters.AddWithValue("@fecha", recompensa.FechaEmpieza);
					comando.Parameters.AddWithValue("@juegoNombre", recompensa.JuegoNombre);
					comando.Parameters.AddWithValue("@drm", recompensa.DRM);

					if (recompensa.FechaCaduca != null)
					{
						comando.Parameters.AddWithValue("@fechaCaduca", recompensa.FechaCaduca);
					}

					comando.ExecuteNonQuery();
					try
					{

					}
					catch
					{

					}
				}
			}
		}

		public static List<RecompensaJuego> Disponibles(SqlConnection conexion = null)
		{
			List<RecompensaJuego> juegos = new List<RecompensaJuego>();

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
				string busqueda = "SELECT * FROM recompensasJuegos WHERE usuarioId IS NULL AND (fechaCaduca IS NULL OR fechaCaduca > GETDATE()) ORDER BY juegoNombre";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							juegos.Add(Cargar(lector));
						}
					}
				}
			}

			return juegos;
		}

		public static void Actualizar(int id, string usuarioId, SqlConnection conexion = null)
		{
			string sqlActualizar = "UPDATE recompensasJuegos " +
					"SET usuarioId=@usuarioId WHERE id=@id";

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
				using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
				{
					comando.Parameters.AddWithValue("@usuarioId", usuarioId);
					comando.Parameters.AddWithValue("@id", id);

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

        public static List<RecompensaJuego> LeerJuegosUsuario(string usuarioId, SqlConnection conexion = null)
		{
            List<RecompensaJuego> juegos = new List<RecompensaJuego>();

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
                string busqueda = "SELECT * FROM recompensasJuegos WHERE usuarioId=@usuarioId ORDER BY juegoNombre";

                using (SqlCommand comando = new SqlCommand(busqueda, conexion))
                {
                    comando.Parameters.AddWithValue("@usuarioId", usuarioId);

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            juegos.Add(Cargar(lector));
                        }
                    }
                }
            }

            return juegos;
        }
    }
}
