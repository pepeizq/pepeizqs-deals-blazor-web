#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Portada
{
	public static class Limpiar
	{
		public static void Total(SqlConnection conexion = null)
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

            string limpiar = @"DELETE FROM seccionMinimos WHERE ultimaModificacion > DATEADD(hour, -24, GETDATE()) 
                                OR (mayorEdad = 'true' AND mayorEdad IS NOT NULL) 
                                OR (freeToPlay = 'true' AND freeToPlay IS NOT NULL)";

			using (SqlCommand comando = new SqlCommand(limpiar, conexion))
			{
				comando.ExecuteNonQuery();
			}
		}

		public static void JuegoNoActivo(int id, JuegoDRM drm, string tienda, SqlConnection conexion = null)
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

			string limpiar = @"DELETE FROM seccionMinimos WHERE idMaestra=@id AND JSON_VALUE(precioMinimosHistoricos, '$[0].DRM')=@drm AND JSON_VALUE(precioMinimosHistoricos, '$[0].Tienda')=@tienda";

			using (SqlCommand comando = new SqlCommand(limpiar, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);
				comando.Parameters.AddWithValue("@drm", drm);
				comando.Parameters.AddWithValue("@tienda", tienda);

				comando.ExecuteNonQuery();
			}
		}
	}
}
