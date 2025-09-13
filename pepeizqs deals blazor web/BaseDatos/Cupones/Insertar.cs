#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Cupones
{
	public static class Insertar
	{
		public static void Ejecutar(string codigo, int? porcentaje, decimal? precioMinimo, decimal? precioRebaja, string tienda, DateTime fechaEmpieza, DateTime fechaAcaba, SqlConnection conexion = null)
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

			string añadirPorcentaje1 = null;
			string añadirPorcentaje2 = null;

			if (porcentaje != null && porcentaje > 0)
			{
				añadirPorcentaje1 = ", porcentaje";
				añadirPorcentaje2 = ", @porcentaje";
			}

			string añadirMinimo1 = null;
			string añadirMinimo2 = null;

			if (precioMinimo != null && precioMinimo > 0)
			{
				añadirMinimo1 = ", precioMinimo";
				añadirMinimo2 = ", @precioMinimo";
			}

			string añadirRebaja1 = null;
			string añadirRebaja2 = null;

			if (precioRebaja != null && precioRebaja > 0)
			{
				añadirRebaja1 = ", precioRebaja";
				añadirRebaja2 = ", @precioRebaja";
			}

			string sqlInsertar = "INSERT INTO cupones " +
							   "(codigo, tienda, fechaEmpieza, fechaTermina" + añadirPorcentaje1 + añadirMinimo1 + añadirRebaja1 + ") VALUES " +
							   "(@codigo, @tienda, @fechaEmpieza, @fechaTermina" + añadirPorcentaje2 + añadirMinimo2 + añadirRebaja2 + ") ";

			using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
			{
				comando.Parameters.AddWithValue("@codigo", codigo);
				comando.Parameters.AddWithValue("@tienda", tienda);
				comando.Parameters.AddWithValue("@fechaEmpieza", fechaEmpieza);
				comando.Parameters.AddWithValue("@fechaTermina", fechaAcaba);

				if (porcentaje != null && porcentaje > 0)
				{
					comando.Parameters.AddWithValue("@porcentaje", porcentaje);
				}

				if (precioMinimo != null && precioMinimo > 0)
				{
					comando.Parameters.AddWithValue("@precioMinimo", precioMinimo);
				}

				if (precioRebaja != null && precioRebaja > 0)
				{
					comando.Parameters.AddWithValue("@precioRebaja", precioRebaja);
				}

				try
				{
					comando.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					BaseDatos.Errores.Insertar.Mensaje("Insertar Cupones", ex);
				}
			}
		}
	}
}
