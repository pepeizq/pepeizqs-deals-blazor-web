#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace BaseDatos.Gratis
{
	public static class Insertar
	{
		public static void Ejecutar(int id, Juego juego, JuegoGratis actual, SqlConnection conexion = null)
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
				string sqlInsertar = "INSERT INTO gratis " +
					"(gratis, juegoId, nombre, imagen, drm, enlace, fechaEmpieza, fechaTermina, imagenNoticia) VALUES " +
					"(@gratis, @juegoId, @nombre, @imagen, @drm, @enlace, @fechaEmpieza, @fechaTermina, @imagenNoticia) ";

				using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
				{
					comando.Parameters.AddWithValue("@gratis", actual.Tipo);
					comando.Parameters.AddWithValue("@juegoId", actual.JuegoId);
					comando.Parameters.AddWithValue("@nombre", actual.Nombre);
					comando.Parameters.AddWithValue("@imagen", actual.Imagen);
					comando.Parameters.AddWithValue("@drm", actual.DRM);
					comando.Parameters.AddWithValue("@enlace", actual.Enlace);
					comando.Parameters.AddWithValue("@fechaEmpieza", actual.FechaEmpieza.ToString());
					comando.Parameters.AddWithValue("@fechaTermina", actual.FechaTermina.ToString());
					comando.Parameters.AddWithValue("@imagenNoticia", actual.ImagenNoticia);

					try
					{
						comando.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Mensaje("Insertar juego gratis", ex);
					}
				}

                if (juego.Gratis == null)
                {
                    juego.Gratis = new List<JuegoGratis>();
                }

                string sqlBuscar = "SELECT TOP(1) id FROM gratis ORDER BY id DESC";

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

                juego.Gratis.Add(actual);

                string sqlActualizarJuego = "UPDATE juegos " +
					"SET gratis=@gratis WHERE id=@id";

                using (SqlCommand comando = new SqlCommand(sqlActualizarJuego, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);
                    comando.Parameters.AddWithValue("@gratis", JsonSerializer.Serialize(juego.Gratis));

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
