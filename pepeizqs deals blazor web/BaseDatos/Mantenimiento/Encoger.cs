#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Mantenimiento
{

	public static class Encoger
	{
		public static void Ejecutar(SqlConnection conexion = null)
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

			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string baseDatos = builder.Configuration.GetValue<string>("Mantenimiento:BaseDatos");

			string sqlEncoger = "DBCC SHRINKDATABASE (" + baseDatos + ")";

			using (SqlCommand comando = new SqlCommand(sqlEncoger, conexion))
			{
				comando.CommandTimeout = 1000;

				try
				{
					comando.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					BaseDatos.Errores.Insertar.Mensaje("Encoger Base Datos", ex);
				}
			}
		}
	}
}
