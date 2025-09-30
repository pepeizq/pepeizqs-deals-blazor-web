#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;

namespace BaseDatos.RedesSociales
{
    public static class Insertar
    {
        public static void Ejecutar(int id, JuegoPrecio juego, SqlConnection conexion = null)
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

            if (Buscar.ExisteEnlace(juego.Enlace, conexion) == true)
            {
                Actualizar.Tienda(juego.Enlace, juego.Tienda);
            }
            else
            {
                string sqlAñadir = "INSERT INTO redesSocialesPosteador " +
                        "(enlace, idJuego, tienda) VALUES " +
                        "(@enlace, @idJuego, @tienda) ";

                using (SqlCommand comando = new SqlCommand(sqlAñadir, conexion))
                {
                    comando.Parameters.AddWithValue("@enlace", juego.Enlace);
                    comando.Parameters.AddWithValue("@idJuego", id);
                    comando.Parameters.AddWithValue("@tienda", juego.Tienda);

                    try
                    {
                        comando.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Errores.Insertar.Mensaje("Añadir Redes Sociales Posteador", ex);
                    }
                }
            }   
        }
    }
}