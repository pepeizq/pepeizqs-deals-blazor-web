using Juegos;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace BaseDatos.Suscripciones
{
	public static class Insertar
	{
		public static void Ejecutar(int juegoId, List<JuegoSuscripcion> suscripciones, JuegoSuscripcion actual, SqlConnection conexion = null)
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

            string sqlInsertar = "INSERT INTO suscripciones " +
				"(suscripcion, juegoId, nombre, imagen, drm, enlace, fechaEmpieza, fechaTermina, imagenNoticia) VALUES " +
				"(@suscripcion, @juegoId, @nombre, @imagen, @drm, @enlace, @fechaEmpieza, @fechaTermina, @imagenNoticia) ";

			using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
			{
				comando.Parameters.AddWithValue("@suscripcion", actual.Tipo);
				comando.Parameters.AddWithValue("@juegoId", actual.JuegoId);
				comando.Parameters.AddWithValue("@nombre", actual.Nombre);
				comando.Parameters.AddWithValue("@imagen", actual.Imagen);
				comando.Parameters.AddWithValue("@drm", actual.DRM);
				comando.Parameters.AddWithValue("@enlace", actual.Enlace);
				comando.Parameters.AddWithValue("@fechaEmpieza", actual.FechaEmpieza);
				comando.Parameters.AddWithValue("@fechaTermina", actual.FechaTermina);
				comando.Parameters.AddWithValue("@imagenNoticia", actual.ImagenNoticia);

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}

            string sqlBuscar = "SELECT TOP(1) id FROM suscripciones ORDER BY id DESC";

            using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
            {
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        if (lector.IsDBNull(0) == false)
                        {
                            actual.Id = lector.GetInt32(0);
                        }
                    }
                }
            }

			if (suscripciones == null)
			{
				suscripciones = new List<JuegoSuscripcion>();
				suscripciones.Add(actual);
			}
			else
			{
				foreach (var suscripcion in suscripciones)
				{
					if (suscripcion.DRM == actual.DRM && suscripcion.Tipo == actual.Tipo && suscripcion.FechaEmpieza == actual.FechaEmpieza && suscripcion.FechaTermina == actual.FechaTermina)
					{
						suscripcion.Id = actual.Id;
					}
				}
			}

			string sqlActualizarJuego = "UPDATE juegos " +
						"SET suscripciones=@suscripciones WHERE id=@id";

            using (SqlCommand comando = new SqlCommand(sqlActualizarJuego, conexion))
            {
                comando.Parameters.AddWithValue("@id", juegoId);
                comando.Parameters.AddWithValue("@suscripciones", JsonSerializer.Serialize(suscripciones));

                try
                {
                    comando.ExecuteNonQuery();
                }
                catch
                {

                }
            }
        }

		public static void Temporal(SqlConnection conexion, string nombreTabla, string enlace, string nombreJuego = "vacio", string imagen = "vacio")
		{
			bool encontrado = false;
			string sqlBuscar = "SELECT enlace FROM temporal" + nombreTabla + " WHERE enlace=@enlace";

			using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
			{
				comando.Parameters.AddWithValue("@enlace", enlace);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					if (lector.Read() == true)
					{
						encontrado = true;
					}
				}
			}

			if (encontrado == false)
			{
				string sqlInsertar = "INSERT INTO temporal" + nombreTabla + " " +
					"(enlace, nombre, imagen) VALUES " +
					"(@enlace, @nombre, @imagen) ";

				using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
				{
					comando.Parameters.AddWithValue("@enlace", enlace);
					comando.Parameters.AddWithValue("@nombre", nombreJuego);
					comando.Parameters.AddWithValue("@imagen", imagen);

					try
					{
						comando.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Mensaje("Insertar Temporal Suscripción", ex);
					}
				}
			}
		}
	}
}
