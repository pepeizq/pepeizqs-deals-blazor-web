#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Cupones
{
	public static class Buscar
	{
		public static Cupon Cargar(SqlDataReader lector)
		{
			Cupon cupon = new Cupon();
			cupon.Codigo = lector.GetString(1);
			cupon.FechaEmpieza = lector.GetDateTime(2);
			cupon.FechaTermina = lector.GetDateTime(3);

			if (lector.IsDBNull(4) == false)
			{
				cupon.Porcentaje = lector.GetInt32(4);
			}

			if (lector.IsDBNull(5) == false)
			{
				cupon.PrecioMinimo = lector.GetDecimal(5);
			}

			cupon.TiendaID = lector.GetString(6);

			if (lector.IsDBNull(7) == false)
			{
				cupon.PrecioRebaja = lector.GetDecimal(7);
			}

			return cupon;
		}

		public static Cupon Activos(string tienda, SqlConnection conexion = null)
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

			string busqueda = "SELECT * FROM cupones WHERE fechaEmpieza < GETDATE() AND fechaTermina > GETDATE() AND tienda='@tienda'";

			busqueda = busqueda.Replace("@tienda", tienda);

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read() == true)
					{
						return Cargar(lector);
					}
				}
			}

			return null;
		}
	}

	public class Cupon
	{
		public string Codigo { get; set; }
		public DateTime FechaEmpieza { get; set; }
		public DateTime FechaTermina { get; set; }
		public int? Porcentaje { get; set; }
		public decimal? PrecioMinimo { get; set; }
		public string TiendaID { get; set; }
		public decimal? PrecioRebaja { get; set; }
	}
}
