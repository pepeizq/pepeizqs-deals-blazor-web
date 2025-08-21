#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Errores
{
    public static class Insertar
	{
        public static void Mensaje(string seccion, Exception ex, SqlConnection conexion = null, bool reiniciar = true)
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
                string sqlInsertar = "INSERT INTO errores " +
                                "(seccion, mensaje, stacktrace, fecha) VALUES " +
                                "(@seccion, @mensaje, @stacktrace, @fecha) ";

                using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
                {
                    comando.Parameters.AddWithValue("@seccion", seccion);
                    comando.Parameters.AddWithValue("@mensaje", ex.Message);
                    comando.Parameters.AddWithValue("@stacktrace", ex.StackTrace);
                    comando.Parameters.AddWithValue("@fecha", DateTime.Now);

                    try
                    {
                        comando.ExecuteNonQuery();
                    }
                    catch
                    {

                    }
				}
			}
            
            if (reiniciar == true)
            {
				WebApplicationBuilder builder = WebApplication.CreateBuilder();
				string piscinaApp = builder.Configuration.GetValue<string>("PoolWeb:Contenido");
				string piscinaUsada = Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);

				if (piscinaApp != piscinaUsada)
                {
					Environment.Exit(1);
				}
			}  
        }

        public static void Mensaje(string seccion, string mensaje, string enlace = null, SqlConnection conexion = null)
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

            if (enlace == null)
            {
                enlace = "nada";
            }

            string sqlInsertar = "INSERT INTO errores " +
                               "(seccion, mensaje, stacktrace, fecha, enlace) VALUES " +
                               "(@seccion, @mensaje, @stacktrace, @fecha, @enlace) ";

            using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
            {
                comando.Parameters.AddWithValue("@seccion", seccion);
                comando.Parameters.AddWithValue("@mensaje", "nada");
                comando.Parameters.AddWithValue("@stacktrace", mensaje);
                comando.Parameters.AddWithValue("@fecha", DateTime.Now);
                comando.Parameters.AddWithValue("@enlace", enlace);

                try
                {
                    comando.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Mensaje("Errores", ex);
                }
			}
		}
    }
}
