#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Pendientes
{

	public static class Insertar
	{
        public static void AñadirId(string nombre, string ids, SqlConnection conexion = null)
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

            bool existe = false;

            string busqueda = "SELECT ids FROM juegosIDs WHERE nombre=@nombre";

            using (SqlCommand comando = new SqlCommand(busqueda, conexion))
            {
                comando.Parameters.AddWithValue("@nombre", nombre);

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        existe = true;
                        break;
                    }
                }
            }

			if (existe == false)
			{
                string sqlInsertar = "INSERT INTO juegosIDs " +
                            "(nombre, ids) VALUES " +
                            "(@nombre, @ids) ";

                using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", nombre);
                    comando.Parameters.AddWithValue("@ids", ids);

                    try
                    {
                        comando.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        BaseDatos.Errores.Insertar.Mensaje("Insertar JuegosIDs", ex);
                    }
                }
            }
        }
    }
}
