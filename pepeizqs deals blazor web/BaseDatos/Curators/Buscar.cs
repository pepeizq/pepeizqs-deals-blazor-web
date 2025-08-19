#nullable disable

using APIs.Steam;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace BaseDatos.Curators
{
	public static class Buscar
	{
		public static Curator Cargar(SqlDataReader lector)
		{
			Curator curator = new Curator
			{
				Id = lector.GetInt32(0),
				IdSteam = lector.GetInt32(1),
				Nombre = lector.GetString(2),
				Imagen = lector.GetString(3),
				Descripcion = lector.GetString(4),
				Slug = lector.GetString(5),
				SteamIds = JsonSerializer.Deserialize<List<int>>(lector.GetString(6)),
				Web = JsonSerializer.Deserialize<SteamCuratorAPIWeb>(lector.GetString(7))
			};

			if (lector.IsDBNull(8) == false)
			{
				curator.ImagenFondo = lector.GetString(8);
			}

			if (lector.IsDBNull(9) == false)
			{
				curator.Fecha = lector.GetDateTime(9);
			}

			return curator;
		}

		public static List<Curator> Todos(SqlConnection conexion = null)
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

			List<Curator> curators = new List<Curator>();

			string busqueda = "SELECT * FROM curators";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read() == true)
					{
						curators.Add(Cargar(lector));
					}

					lector.Dispose();
				}

				comando.Dispose();
			}

			conexion.Dispose();

			return curators;
		}

		public static Curator Uno(int idSteam, SqlConnection conexion = null)
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

			Curator curator = null;

			string busqueda = "SELECT * FROM curators WHERE idSteam=@idSteam";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				comando.Parameters.AddWithValue("@idSteam", idSteam);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					if (lector.Read() == true)
					{
						curator = Cargar(lector);
					}

					lector.Dispose();
				}

				comando.Dispose();
			}

			conexion.Dispose();

			return curator;
		}

		public static Curator Uno(string slug, SqlConnection conexion = null)
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

			Curator curator = null;

			string busqueda = "SELECT * FROM curators WHERE slug=@slug";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				comando.Parameters.AddWithValue("@slug", slug);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					if (lector.Read() == true)
					{
						curator = Cargar(lector);
					}

					lector.Dispose();
				}

				comando.Dispose();
			}

			conexion.Dispose();

			return curator;
		}
	}

	public class Curator : ComponentBase, IComponent
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public int IdSteam { get; set; }
		public string Nombre { get; set; }
		public string Imagen { get; set; }
		public string Descripcion { get; set; }
		public string Slug { get; set; }
		public List<int> SteamIds { get; set; }
		public SteamCuratorAPIWeb Web { get; set; }
		public string ImagenFondo { get; set; }
		public DateTime? Fecha { get; set; }
	}

	public class CuratorFicha
	{
		public Curator Curator;
		public int Posicion;
	}
}
