#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.CorreosEnviar
{
	public static class Insertar
	{
		public static void Ejecutar(string html, string titulo, string correoDesde, string correoHacia, DateTime fecha, CorreoPendienteTipo tipo, string json = null, SqlConnection conexion = null)
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

			string añadirJson1 = string.Empty;
			string añadirJson2 = string.Empty;

			if (string.IsNullOrEmpty(json) == false)
			{
				añadirJson1 = ", json";
				añadirJson2 = ", @json";
			}

			string sqlAñadir = "INSERT INTO correosEnviar " +
					 "(html, titulo, correoDesde, correoHacia, tipo, fecha" + añadirJson1 + ") VALUES " +
					 "(@html, @titulo, @correoDesde, @correoHacia, @tipo, @fecha" + añadirJson2 + ") ";

			using (SqlCommand comando = new SqlCommand(sqlAñadir, conexion))
			{
				comando.Parameters.AddWithValue("@html", html);
				comando.Parameters.AddWithValue("@titulo", titulo);
				comando.Parameters.AddWithValue("@correoDesde", correoDesde);
				comando.Parameters.AddWithValue("@correoHacia", correoHacia);
				comando.Parameters.AddWithValue("@tipo", tipo);
				comando.Parameters.AddWithValue("@fecha", fecha);

				if (string.IsNullOrEmpty(json) == false)
				{
					comando.Parameters.AddWithValue("@json", json);
				}

				try
				{
					comando.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					BaseDatos.Errores.Insertar.Mensaje("Insertar Correo Enviar", ex, conexion, false, comando);
				}
			}
		}
	}
}
