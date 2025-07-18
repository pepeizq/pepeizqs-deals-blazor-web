﻿#nullable disable

using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;
using pepeizqs_deals_web.Data;

namespace BaseDatos.Usuarios
{
	public static class Buscar
	{
		public static bool RolDios(string id)
		{
			if (string.IsNullOrEmpty(id) == false) 
			{
				SqlConnection conexion = Herramientas.BaseDatos.Conectar();

				using (conexion)
				{
					string busqueda = "SELECT * FROM AspNetUsers WHERE Id=@Id";

					using (SqlCommand comando = new SqlCommand(busqueda, conexion))
					{
						comando.Parameters.AddWithValue("@Id", id);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							while (lector.Read())
							{
								if (lector.GetString(2) == "God")
								{
									return true;
								}
							}
						}
					}
				}
			}

			return false;
		}

		public static string IdiomaSobreescribir(string usuarioId, SqlConnection conexion = null)
		{
			if (string.IsNullOrEmpty(usuarioId) == false)
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

				string busqueda = "SELECT LanguageOverride FROM AspNetUsers WHERE Id=@Id";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@Id", usuarioId);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							if (lector.IsDBNull(0) == false)
							{
								return lector.GetString(0);
							}
						}
					}
				}
			}

			return "en";
		}

		public static string IdiomaJuegos(string usuarioId, SqlConnection conexion = null)
		{
			if (string.IsNullOrEmpty(usuarioId) == false)
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

				string busqueda = "SELECT LanguageGames FROM AspNetUsers WHERE Id=@Id";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@Id", usuarioId);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							if (lector.IsDBNull(0) == false)
							{
								return lector.GetString(0);
							}
						}
					}
				}
			}

			return "en";
		}

		public static Usuario JuegosTiene(string usuarioId, SqlConnection conexion = null)
		{
			if (string.IsNullOrEmpty(usuarioId) == false)
			{
				Usuario juegos = new Usuario();

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

				string busqueda = "SELECT SteamGames, GogGames, AmazonGames, EpicGames, UbisoftGames, EaGames FROM AspNetUsers WHERE Id=@Id";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@Id", usuarioId);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							if (lector.IsDBNull(0) == false)
							{
								juegos.SteamGames = lector.GetString(0);
							}

							if (lector.IsDBNull(1) == false)
							{
								juegos.GogGames = lector.GetString(1);
							}

							if (lector.IsDBNull(2) == false)
							{
								juegos.AmazonGames = lector.GetString(2);
							}

							if (lector.IsDBNull(3) == false)
							{
								juegos.EpicGames = lector.GetString(3);
							}

							if (lector.IsDBNull(4) == false)
							{
								juegos.UbisoftGames = lector.GetString(4);
							}

							if (lector.IsDBNull(5) == false)
							{
								juegos.EaGames = lector.GetString(5);
							}
						}
					}
				}

				return juegos;
			}

			return null;
		}

		public static DateTime? FechaPatreon(string usuarioId, SqlConnection conexion = null)
		{
			if (string.IsNullOrEmpty(usuarioId) == false)
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

				string busqueda = "SELECT PatreonLastCheck FROM AspNetUsers WHERE Id=@Id";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@Id", usuarioId);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							if (lector.IsDBNull(0) == false)
							{
								return lector.GetDateTime(0);
							}
						}
					}
				}
			}

			return null;
		}

		public static Usuario DeseadosTiene(string usuarioId, SqlConnection conexion = null)
		{
			if (string.IsNullOrEmpty(usuarioId) == false)
			{
				Usuario deseados = new Usuario();

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

				string busqueda = "SELECT SteamWishlist, Wishlist, GogWishlist FROM AspNetUsers WHERE Id=@Id";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@Id", usuarioId);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							if (lector.IsDBNull(0) == false)
							{
								deseados.SteamWishlist = lector.GetString(0);
							}

							if (lector.IsDBNull(1) == false)
							{
								deseados.Wishlist = lector.GetString(1);
							}

							if (lector.IsDBNull(2) == false)
							{
								deseados.GogWishlist = lector.GetString(2);
							}
						}
					}
				}

				return deseados;
			}

			return null;
		}

		public static Usuario OpcionesCabecera(string usuarioId, SqlConnection conexion = null)
		{
			if (string.IsNullOrEmpty(usuarioId) == false)
			{
				Usuario opciones = new Usuario();

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

				string busqueda = "SELECT Avatar, Email, Nickname, PatreonCoins FROM AspNetUsers WHERE Id=@Id";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@Id", usuarioId);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							if (lector.IsDBNull(0) == false)
							{
								opciones.Avatar = lector.GetString(0);
							}

							if (lector.IsDBNull(1) == false)
							{
								opciones.Email = lector.GetString(1);
							}

							if (lector.IsDBNull(2) == false)
							{
								opciones.Nickname = lector.GetString(2);
							}

							if (lector.IsDBNull(3) == false)
							{
								opciones.PatreonCoins = lector.GetInt32(3);
							}
						}
					}
				}

				return opciones;
			}

			return null;
		}

		public static Usuario OpcionesPortada(string usuarioId, SqlConnection conexion = null)
		{
			if (string.IsNullOrEmpty(usuarioId) == false)
			{
				Usuario opciones = new Usuario();

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

				string busqueda = "SELECT IndexOption1, IndexOption2, IndexDRMs, IndexCategories, ForumIndex FROM AspNetUsers WHERE Id=@Id";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@Id", usuarioId);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							if (lector.IsDBNull(0) == false)
							{
								opciones.IndexOption1 = lector.GetBoolean(0);
							}

							if (lector.IsDBNull(1) == false)
							{
								opciones.IndexOption2 = lector.GetBoolean(1);
							}

							if (lector.IsDBNull(2) == false)
							{
								opciones.IndexDRMs = lector.GetString(2);
							}

							if (lector.IsDBNull(3) == false)
							{
								opciones.IndexCategories = lector.GetString(3);
							}

							if (lector.IsDBNull(4) == false)
							{
								opciones.ForumIndex = lector.GetBoolean(4);
							}
						}
					}
				}

				return opciones;
			}

			return null;
		}

		public static Usuario OpcionesDeseados(string usuarioId, SqlConnection conexion = null)
		{
			if (string.IsNullOrEmpty(usuarioId) == false)
			{
				Usuario opciones = new Usuario();

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

				string busqueda = "SELECT WishlistPublic, WishlistNickname, WishlistSort, WishlistOption3, WishlistOption4 FROM AspNetUsers WHERE Id=@Id";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@Id", usuarioId);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							if (lector.IsDBNull(0) == false)
							{
								opciones.WishlistPublic = lector.GetBoolean(0);
							}

							if (lector.IsDBNull(1) == false)
							{
								opciones.WishlistNickname = lector.GetString(1);
							}

							if (lector.IsDBNull(2) == false)
							{
								opciones.WishlistSort = lector.GetInt32(2);
							}

							if (lector.IsDBNull(3) == false)
							{
								opciones.WishlistOption3 = lector.GetInt32(3);
							}

							if (lector.IsDBNull(4) == false)
							{
								opciones.WishlistOption4 = lector.GetDecimal(4);
							}
						}
					}
				}

				return opciones;
			}

			return null;
		}

		public static Usuario OpcionesMinimos(string usuarioId, SqlConnection conexion = null)
		{
			if (string.IsNullOrEmpty(usuarioId) == false)
			{
				Usuario opciones = new Usuario();

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

				string busqueda = "SELECT HistoricalLowsOption1, HistoricalLowsOption4, HistoricalLowsOption2, HistoricalLowsOption3, HistoricalLowsDRMs, HistoricalLowsStores, HistoricalLowsCategories, HistoricalLowsSteamDeck, HistoricalLowsSort, HistoricalLowsRelease, HistoricalLowsAI FROM AspNetUsers WHERE Id=@Id";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@Id", usuarioId);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							if (lector.IsDBNull(0) == false)
							{
								opciones.HistoricalLowsOption1 = lector.GetBoolean(0);
							}

							if (lector.IsDBNull(1) == false)
							{
								opciones.HistoricalLowsOption4 = lector.GetBoolean(1);
							}

							if (lector.IsDBNull(2) == false)
							{
								opciones.HistoricalLowsOption2 = lector.GetInt32(2);
							}

							if (lector.IsDBNull(3) == false)
							{
								opciones.HistoricalLowsOption3 = lector.GetDecimal(3);
							}

							if (lector.IsDBNull(4) == false)
							{
								opciones.HistoricalLowsDRMs = lector.GetString(4);
							}

							if (lector.IsDBNull(5) == false)
							{
								opciones.HistoricalLowsStores = lector.GetString(5);
							}

							if (lector.IsDBNull(6) == false)
							{
								opciones.HistoricalLowsCategories = lector.GetString(6);
							}

							if (lector.IsDBNull(7) == false)
							{
								opciones.HistoricalLowsSteamDeck = lector.GetString(7);
							}

							if (lector.IsDBNull(8) == false)
							{
								opciones.HistoricalLowsSort = lector.GetInt32(8);
							}

							if (lector.IsDBNull(9) == false)
							{
								opciones.HistoricalLowsRelease = lector.GetInt32(9);
							}

							if (lector.IsDBNull(10) == false)
							{
								opciones.HistoricalLowsAI = lector.GetInt32(10);
							}
						}
					}
				}

				return opciones;
			}

			return null;
		}

		public static Usuario OpcionesCuenta(string usuarioId, SqlConnection conexion = null)
		{
			if (string.IsNullOrEmpty(usuarioId) == false)
			{
				Usuario opciones = new Usuario();

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

				string busqueda = "SELECT EmailConfirmed, PatreonCoins, SteamAccountLastCheck, GogAccountLastCheck, AmazonLastImport, EpicGamesLastImport, UbisoftLastImport, EaLastImport FROM AspNetUsers WHERE Id=@Id";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@Id", usuarioId);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							if (lector.IsDBNull(0) == false)
							{
								opciones.EmailConfirmed = lector.GetBoolean(0);
							}

							if (lector.IsDBNull(1) == false)
							{
								opciones.PatreonCoins = lector.GetInt32(1);
							}

							if (lector.IsDBNull(2) == false)
							{
								opciones.SteamAccountLastCheck = lector.GetString(2);
							}

							if (lector.IsDBNull(3) == false)
							{
								opciones.GogAccountLastCheck = lector.GetDateTime(3);
							}

							if (lector.IsDBNull(4) == false)
							{
								opciones.AmazonLastImport = lector.GetDateTime(4);
							}

							if (lector.IsDBNull(5) == false)
							{
								opciones.EpicGamesLastImport = lector.GetDateTime(5);
							}

							if (lector.IsDBNull(6) == false)
							{
								opciones.UbisoftLastImport = lector.GetDateTime(6);
							}

							if (lector.IsDBNull(7) == false)
							{
								opciones.EaLastImport = lector.GetDateTime(7);
							}
						}
					}
				}

				return opciones;
			}

			return null;
		}

		public static string UsuarioDeseadosNickname(string otroUsuario, SqlConnection conexion = null)
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

			string busqueda = "SELECT Id FROM AspNetUsers WHERE WishlistPublic='true' AND WishlistNickname=@WishlistNickname";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				comando.Parameters.AddWithValue("@WishlistNickname", otroUsuario);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						if (lector.IsDBNull(0) == false)
						{
							return lector.GetString(0);
						}
					}
				}
			}

			return null;
		}

		public static List<Usuario> UsuariosIdioma(SqlConnection conexion = null)
		{
			List<Usuario> usuarios = new List<Usuario>();

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

			string busqueda = "SELECT Id, Language FROM AspNetUsers";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						if (lector.IsDBNull(0) == false && lector.IsDBNull(1) == false)
						{
							Usuario usuario = new Usuario
							{
								Id = lector.GetString(0),
								Language = lector.GetString(1)
							};

							usuarios.Add(usuario);
						}
					}
				}
			}

			return usuarios;
		}

		public static List<Usuario> UsuariosNotificacionesCorreo(SqlConnection conexion = null)
		{
			List<Usuario> usuarios = new List<Usuario>();

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

			string busqueda = "SELECT Id, NotificationBundles, NotificationFree, NotificationSubscriptions, NotificationOthers, Email, Language FROM AspNetUsers";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						if (lector.IsDBNull(0) == false)
						{
							bool añadir = false;

							Usuario usuario = new Usuario
							{
								Id = lector.GetString(0)
							};

							if (lector.IsDBNull(1) == false)
							{
								añadir = true;
								usuario.NotificationBundles = lector.GetBoolean(1);
							}

							if (lector.IsDBNull(2) == false)
							{
								añadir = true;
								usuario.NotificationFree = lector.GetBoolean(2);
							}

							if (lector.IsDBNull(3) == false)
							{
								añadir = true;
								usuario.NotificationSubscriptions = lector.GetBoolean(3);
							}

							if (lector.IsDBNull(4) == false)
							{
								añadir = true;
								usuario.NotificationOthers = lector.GetBoolean(4);
							}

							if (lector.IsDBNull(5) == false)
							{
								añadir = true;
								usuario.Email = lector.GetString(5);
							}

							if (lector.IsDBNull(6) == false)
							{
								añadir = true;
								usuario.Language = lector.GetString(6);
							}

							if (añadir == true)
							{
								usuarios.Add(usuario);
							}
						}
					}
				}
			}

			return usuarios;
		}

		public static List<Usuario> UsuariosNotificacionesPush(SqlConnection conexion = null)
		{
			List<Usuario> usuarios = new List<Usuario>();

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

			string busqueda = "SELECT Id, NotificationPushBundles, NotificationPushFree, NotificationPushSubscriptions, NotificationPushOthers, Language FROM AspNetUsers";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						if (lector.IsDBNull(0) == false)
						{
							bool añadir = false;

							Usuario usuario = new Usuario
							{
								Id = lector.GetString(0)
							};

							if (lector.IsDBNull(1) == false)
							{
								añadir = true;
								usuario.NotificationPushBundles = lector.GetBoolean(1);
							}

							if (lector.IsDBNull(2) == false)
							{
								añadir = true;
								usuario.NotificationPushFree = lector.GetBoolean(2);
							}

							if (lector.IsDBNull(3) == false)
							{
								añadir = true;
								usuario.NotificationPushSubscriptions = lector.GetBoolean(3);
							}

							if (lector.IsDBNull(4) == false)
							{
								añadir = true;
								usuario.NotificationPushOthers = lector.GetBoolean(4);
							}

							if (lector.IsDBNull(5) == false)
							{
								añadir = true;
								usuario.Language = lector.GetString(5);
							}

							if (añadir == true)
							{
								usuarios.Add(usuario);
							}
						}
					}
				}
			}

			return usuarios;
		}

		public static bool CorreoYaUsado(string correo, SqlConnection conexion = null)
		{
			if (string.IsNullOrEmpty(correo) == false)
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

				string busqueda = "SELECT Id FROM AspNetUsers WHERE Email=@Email";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@Email", correo);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							if (lector.IsDBNull(0) == false)
							{
								return true;
							}
						}
					}
				}
			}

			return false;
		}



		public static bool UsuarioTieneJuego(string usuarioId, int juegoId, JuegoDRM drm, SqlConnection conexion = null)
		{
			string busqueda = string.Empty;

			if (drm == JuegoDRM.Steam)
			{
				busqueda = "SELECT id FROM AspNetUsers WHERE EXISTS(SELECT * FROM STRING_SPLIT(SteamGames, ',') WHERE VALUE IN ((SELECT idSteam FROM juegos WHERE id=@juegoId))) AND id=@id";
			}

			if (drm == JuegoDRM.GOG)
			{
				busqueda = "SELECT id FROM AspNetUsers WHERE EXISTS(SELECT * FROM STRING_SPLIT(GogGames, ',') WHERE VALUE IN ((SELECT idGog FROM juegos WHERE id=@juegoId))) AND id=@id";
			}

			if (drm == JuegoDRM.Amazon)
			{
				busqueda = "SELECT id FROM AspNetUsers WHERE EXISTS(SELECT * FROM STRING_SPLIT(AmazonGames, ',') WHERE VALUE IN ((SELECT idAmazon FROM juegos WHERE id=@juegoId))) AND id=@id";
			}

			if (drm == JuegoDRM.Epic)
			{
				busqueda = "SELECT id FROM AspNetUsers WHERE EXISTS(SELECT * FROM STRING_SPLIT(EpicGames, ',') WHERE VALUE IN ((SELECT exeEpic FROM juegos WHERE id=@juegoId))) AND id=@id";
			}

			if (drm == JuegoDRM.Ubisoft)
			{
				busqueda = "SELECT id FROM AspNetUsers WHERE EXISTS(SELECT * FROM STRING_SPLIT(UbisoftGames, ',') WHERE VALUE IN ((SELECT exeUbisoft FROM juegos WHERE id=@juegoId))) AND id=@id";
			}

			if (drm == JuegoDRM.EA)
			{
				busqueda = "SELECT id FROM AspNetUsers WHERE EXISTS(SELECT * FROM STRING_SPLIT(EaGames, ',') WHERE VALUE IN ((SELECT exeEa FROM juegos WHERE id=@juegoId))) AND id=@id";
			}

			if (string.IsNullOrEmpty(busqueda) == false)
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

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@id", usuarioId);
					comando.Parameters.AddWithValue("@juegoId", juegoId);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							if (lector.IsDBNull(0) == false)
							{
								return true;
							}
						}
					}
				}
			}

			return false;
		}

		public static bool UsuarioTieneDeseado(string usuarioId, string juegoId, JuegoDRM drm, SqlConnection conexion = null)
		{
			if (string.IsNullOrEmpty(usuarioId) == false)
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

				if (drm == JuegoDRM.Steam)
				{
					string busquedaSteam = "SELECT id FROM AspNetUsers WHERE EXISTS(SELECT * FROM STRING_SPLIT(SteamWishlist, ',') WHERE VALUE IN ((SELECT idSteam FROM juegos WHERE id=@juegoId))) AND id=@id";

					using (SqlCommand comando = new SqlCommand(busquedaSteam, conexion))
					{
						comando.Parameters.AddWithValue("@id", usuarioId);
						comando.Parameters.AddWithValue("@juegoId", juegoId);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							if (lector.Read() == true)
							{
								if (lector.IsDBNull(0) == false)
								{
									return true;
								}
							}
						}
					}
				}

				if (drm == JuegoDRM.GOG)
				{
					string busquedaGog = "SELECT id FROM AspNetUsers WHERE EXISTS(SELECT * FROM STRING_SPLIT(GogWishlist, ',') WHERE VALUE IN ((SELECT idGog FROM juegos WHERE id=@juegoId))) AND id=@id";

					using (SqlCommand comando = new SqlCommand(busquedaGog, conexion))
					{
						comando.Parameters.AddWithValue("@id", usuarioId);
						comando.Parameters.AddWithValue("@juegoId", juegoId);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							if (lector.Read() == true)
							{
								if (lector.IsDBNull(0) == false)
								{
									return true;
								}
							}
						}
					}
				}

				string busqueda = @"DECLARE @array NVARCHAR(MAX);
									SET @array = (SELECT Wishlist FROM AspNetUsers WHERE id=@id);

									SELECT * FROM OPENJSON(@array)
									WITH (IdBaseDatos int,
									DRM int) WHERE IdBaseDatos=@juegoId AND DRM=@drm";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@id", usuarioId);
					comando.Parameters.AddWithValue("@juegoId", juegoId);
					comando.Parameters.AddWithValue("@drm", (int) drm);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							if (lector.IsDBNull(0) == false)
							{
								return true;
							}
						}
					}
				}
			}

			return false;
		}

		public static string UsuarioQuiereCorreos(string usuarioId, SqlConnection conexion = null)
		{
			if (string.IsNullOrEmpty(usuarioId) == false)
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

				string busqueda = "SELECT NotificationLows, EmailConfirmed, Email FROM AspNetUsers WHERE Id=@Id";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@Id", usuarioId);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							bool notificaciones = false;
							bool correo = false;

							//Enseñar Notificaciones Minimos
							if (lector.IsDBNull(0) == false)
							{
								if (lector.GetBoolean(0) == true)
								{
									notificaciones = true;
								}
							}

							//Correo Confirmado
							if (lector.IsDBNull(1) == false)
							{
								if (lector.GetBoolean(1) == true)
								{
									correo = true;
								}
							}

							if (correo == true && notificaciones == true)
							{
								//Correo
								if (lector.IsDBNull(2) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(2)) == false)
									{
										return lector.GetString(2);
									}
								}
							}
						}
					}
				}
			}

			return null;
		}

        public static bool CuentaSteamUsada(string id64Steam, string idUsuario)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string busqueda = "SELECT Id FROM AspNetUsers WHERE SteamId=@SteamId";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@SteamId", id64Steam);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							if (lector.IsDBNull(0) == false)
							{
								if (lector.GetString(0) != idUsuario)
								{
									return true;
								}							
							}
						}
					}
				}
			}

			return false;
        }

		public static bool CuentaGogUsada(string idGog, string idUsuario, SqlConnection conexion = null)
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

			string busqueda = "SELECT Id FROM AspNetUsers WHERE GogId=@GogId";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				comando.Parameters.AddWithValue("@GogId", idGog);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						if (lector.IsDBNull(0) == false)
						{
							if (lector.GetString(0) != idUsuario)
							{
								return true;
							}
						}
					}
				}
			}

			return false;
		}

		public static NotificacionSuscripcion UnUsuarioNotificacionesPush(string usuarioId, SqlConnection conexion = null)
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

			string busqueda = "SELECT * FROM usuariosNotificaciones WHERE usuarioId=@usuarioId";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				comando.Parameters.AddWithValue("@usuarioId", usuarioId);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					if (lector.Read() == true)
					{
						NotificacionSuscripcion usuario = new NotificacionSuscripcion();
						usuario.UserId = lector.GetString(0);
						usuario.NotificationSubscriptionId = lector.GetInt32(1);
						usuario.Url = lector.GetString(2);
						usuario.P256dh = lector.GetString(3);
						usuario.Auth = lector.GetString(4);
						usuario.UserAgent = lector.GetString(5);

						return usuario;
					}
				}
			}

			return null;
		}

		public static bool UsuarioQuiereNotificacionesPushMinimos(string usuarioId, SqlConnection conexion = null)
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

			string busqueda = "SELECT NotificationPushLows FROM AspNetUsers WHERE Id=@Id";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				comando.Parameters.AddWithValue("@Id", usuarioId);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					if (lector.Read() == true)
					{
						if (lector.IsDBNull(0) == false)
						{
							return lector.GetBoolean(0);
						}
					}
				}
			}

			return false;
		}

		public static bool UsuarioNombreRepetido(string nombre, SqlConnection sqlConnection = null)
		{
			if (string.IsNullOrEmpty(nombre) == false)
			{
				if (sqlConnection == null)
				{
					sqlConnection = Herramientas.BaseDatos.Conectar();
				}
				else
				{
					if (sqlConnection.State != System.Data.ConnectionState.Open)
					{
						sqlConnection = Herramientas.BaseDatos.Conectar();
					}
				}

				string busqueda = "SELECT Id FROM AspNetUsers WHERE Nickname=@Nickname";

				using (SqlCommand comando = new SqlCommand(busqueda, sqlConnection))
				{
					comando.Parameters.AddWithValue("@Nickname", nombre);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							if (lector.IsDBNull(0) == false)
							{
								return true;
							}
						}
					}
				}
			}

			return false;
		}
	}
}
