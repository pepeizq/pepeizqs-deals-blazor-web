#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Suscripciones
{
	public static class Actualizar
	{
		public static void FechaTermina(JuegoSuscripcion suscripcion, SqlConnection conexion = null)
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

			string sqlActualizar = "UPDATE suscripciones " +
					"SET fechaTermina=@fechaTermina WHERE enlace=@enlace";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@enlace", suscripcion.Enlace);
				comando.Parameters.AddWithValue("@fechaTermina", suscripcion.FechaTermina);

				try
				{
					comando.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					BaseDatos.Errores.Insertar.Mensaje("Actualizar FechaTermina", ex);
				}
			}
		}
	}
}
