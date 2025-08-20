#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace BaseDatos.Divisas
{
	public static class Buscar
	{
		public static Divisa Ejecutar(SqlConnection conexion, string id)
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

			if (string.IsNullOrEmpty(id) == false) 
			{
                string sqlBuscar = "SELECT * FROM divisas WHERE id=@id";

                using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            Divisa divisa = new Divisa
                            {
                                Id = lector.GetString(0),
                                Cantidad = Convert.ToDecimal(lector.GetString(1)),
                                FechaActualizacion = DateTime.Parse(lector.GetString(2), CultureInfo.InvariantCulture)
                            };

                            return divisa;
                        }

                        lector.Dispose();
					}

                    comando.Dispose();
				}

                conexion.Dispose();
            }

			return null;
		}
	}
}
