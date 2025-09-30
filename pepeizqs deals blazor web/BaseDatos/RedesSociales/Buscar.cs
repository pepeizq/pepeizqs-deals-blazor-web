#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.RedesSociales
{
    public static class Buscar
    {
        public static bool ExisteEnlace(string enlace, SqlConnection conexion = null)
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

            string busqueda = "SELECT COUNT(*) FROM redesSocialesPosteador WHERE enlace=@enlace";

            using (SqlCommand comando = new SqlCommand(busqueda, conexion))
            {
                comando.Parameters.AddWithValue("@enlace", enlace);

                int count = (int)comando.ExecuteScalar();
                return count > 0;
            }
        }

        public static async Task PendientesPosteo(SqlConnection conexion = null)
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

            List<string> enlacesBorrar = new List<string>();

            string busqueda = "SELECT * FROM redesSocialesPosteador";

            using (SqlCommand comando = new SqlCommand(busqueda, conexion))
            {
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read() == true)
                    {
                        if (enlacesBorrar.Count >= 4)
                        {
                            break;
                        }

                        if (lector.IsDBNull(0) == false && lector.IsDBNull(1) == false && lector.IsDBNull(2) == false)
                        {
                            string enlace = lector.GetString(0);
                            enlacesBorrar.Add(enlace);

                            int idJuego = lector.GetInt32(1);

                            string tienda = lector.GetString(2);

                            await Herramientas.RedesSociales.Reddit.Postear(enlace, idJuego, tienda);
                        }
                    }
                }
            }

            if (enlacesBorrar.Count > 0)
            {
                foreach (var enlace in enlacesBorrar)
                {
                    string borrar = "DELETE FROM redesSocialesPosteador WHERE enlace=@enlace";

                    using (SqlCommand comando = new SqlCommand(borrar, conexion))
                    {
                        comando.Parameters.AddWithValue("@enlace", enlace);

                        comando.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}