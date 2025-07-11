﻿#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace BaseDatos.Juegos
{
	public static class Actualizar
	{
		public static void Ejecutar(Juego juego, SqlConnection conexion, bool actualizarAPI = false)
		{
            if (conexion.State != System.Data.ConnectionState.Open)
            {
                conexion = Herramientas.BaseDatos.Conectar();
            }

            string añadirSlugGog = null;

			if (string.IsNullOrEmpty(juego.SlugGOG) == false)
			{
				añadirSlugGog = ", slugGOG=@slugGOG";
			}

            string añadirSlugEpic = null;

            if (string.IsNullOrEmpty(juego.SlugEpic) == false)
            {
                añadirSlugEpic = ", slugEpic=@slugEpic";
            }

            if (actualizarAPI == true)
			{
				if (string.IsNullOrEmpty(juego.Maestro) == true)
				{
					juego.Maestro = "no";
				}

				if (string.IsNullOrEmpty(juego.FreeToPlay) == true)
				{
					juego.FreeToPlay = "false";
				}
			}

			string añadirUltimaModificacion = null;

            if (juego.UltimaModificacion != null)
            {
                añadirUltimaModificacion = ", ultimaModificacion=@ultimaModificacion";
            }

			string añadirEtiquetas = null;

			if (juego.Etiquetas != null)
			{
				if (juego.Etiquetas.Count > 0)
				{
					añadirEtiquetas = ", etiquetas=@etiquetas";
				}
			}

			string añadirDeck = null;

			if (juego.Deck != JuegoDeck.Desconocido)
			{
				añadirDeck = ", deck=@deck";
			}

			string sqlActualizar = "UPDATE juegos " +
					"SET idSteam=@idSteam, idGog=@idGog, " +
						"precioMinimosHistoricos=@precioMinimosHistoricos, precioActualesTiendas=@precioActualesTiendas, historicos=@historicos, " +
                        "nombreCodigo=@nombreCodigo" + añadirUltimaModificacion + añadirEtiquetas + añadirDeck + añadirSlugGog + añadirSlugEpic;

			if (actualizarAPI == true)
			{
				sqlActualizar = sqlActualizar + ", nombre=@nombre, tipo=@tipo, fechaSteamAPIComprobacion=@fechaSteamAPIComprobacion, imagenes=@imagenes, caracteristicas=@caracteristicas, media=@media, analisis=@analisis, maestro=@maestro, freeToPlay=@freeToPlay, categorias=@categorias";
			}

			if (juego.IdSteam > 0)
			{
				sqlActualizar = sqlActualizar + " WHERE idSteam=@idSteam";
			}
			else
			{
				if (juego.IdGog > 0)
				{
					sqlActualizar = sqlActualizar + " WHERE idGog=@idGog";
				}
				else
				{
					sqlActualizar = sqlActualizar + " WHERE id=@id";
				}
			}

			if (sqlActualizar.Contains("WHERE id") == true)
			{
				using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
				{
					comando.Parameters.AddWithValue("@id", juego.Id);
					comando.Parameters.AddWithValue("@idSteam", juego.IdSteam);
					comando.Parameters.AddWithValue("@idGog", juego.IdGog);
					comando.Parameters.AddWithValue("@precioMinimosHistoricos", JsonSerializer.Serialize(juego.PrecioMinimosHistoricos));
					comando.Parameters.AddWithValue("@precioActualesTiendas", JsonSerializer.Serialize(juego.PrecioActualesTiendas));
					comando.Parameters.AddWithValue("@nombreCodigo", Herramientas.Buscador.LimpiarNombre(juego.Nombre));
					comando.Parameters.AddWithValue("@historicos", JsonSerializer.Serialize(juego.Historicos));

					if (juego.UltimaModificacion != null)
                    {
                        comando.Parameters.AddWithValue("@ultimaModificacion", juego.UltimaModificacion);
                    }

					if (juego.Etiquetas != null)
					{
						if (juego.Etiquetas.Count > 0)
						{
							comando.Parameters.AddWithValue("@etiquetas", JsonSerializer.Serialize(juego.Etiquetas));
						}
					}

					if (juego.Deck != JuegoDeck.Desconocido)
					{
						comando.Parameters.AddWithValue("@deck", juego.Deck);
					}

					if (actualizarAPI == true)
					{
						comando.Parameters.AddWithValue("@nombre", juego.Nombre);
						comando.Parameters.AddWithValue("@tipo", juego.Tipo);
						comando.Parameters.AddWithValue("@imagenes", JsonSerializer.Serialize(juego.Imagenes));
						comando.Parameters.AddWithValue("@fechaSteamAPIComprobacion", juego.FechaSteamAPIComprobacion.ToString("yyyy-MM-ddTHH:mm:ss.fffffff"));
						comando.Parameters.AddWithValue("@analisis", JsonSerializer.Serialize(juego.Analisis));
						comando.Parameters.AddWithValue("@caracteristicas", JsonSerializer.Serialize(juego.Caracteristicas));
						comando.Parameters.AddWithValue("@media", JsonSerializer.Serialize(juego.Media));
						comando.Parameters.AddWithValue("@maestro", juego.Maestro);
						comando.Parameters.AddWithValue("@freeToPlay", juego.FreeToPlay);
						comando.Parameters.AddWithValue("@categorias", JsonSerializer.Serialize(juego.Categorias));
				
					}

					if (string.IsNullOrEmpty(juego.SlugGOG) == false)
					{
						comando.Parameters.AddWithValue("@slugGOG", juego.SlugGOG);
					}

                    if (string.IsNullOrEmpty(juego.SlugEpic) == false)
                    {
                        comando.Parameters.AddWithValue("@slugEpic", juego.SlugEpic);
                    }

                    try
					{
						comando.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						Errores.Insertar.Mensaje("Actualizar Datos " + juego.Nombre + " " + juego.Id.ToString(), ex);
					}
				}
			}		
		}

		public static void Comprobacion(int id, List<JuegoPrecio> ofertasActuales, List<JuegoPrecio> ofertasHistoricas, List<JuegoHistorico> historicos, SqlConnection conexion = null, string slugGOG = null, string idGOG = null, string slugEpic = null, DateTime? ultimaModificacion = null, JuegoAnalisis analisis = null)
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

			string añadirSlugGog = null;

			if (string.IsNullOrEmpty(slugGOG) == false)
			{
				añadirSlugGog = ", idGog=@idGog, slugGOG=@slugGOG";
			}

			string añadirSlugEpic = null;

			if (string.IsNullOrEmpty(slugEpic) == false)
			{
				añadirSlugEpic = ", slugEpic=@slugEpic";
			}

			string añadirUltimaModificacion = null;

			if (ultimaModificacion != null)
			{
				añadirUltimaModificacion = ", ultimaModificacion=@ultimaModificacion";
			}

			string añadirAnalisis = null;

			if (analisis != null)
			{
				añadirAnalisis = ", analisis=@analisis";
			}

			string sqlActualizar = "UPDATE juegos " +
					"SET precioMinimosHistoricos=@precioMinimosHistoricos, precioActualesTiendas=@precioActualesTiendas, historicos=@historicos" +
					añadirUltimaModificacion + añadirSlugGog + añadirSlugEpic + añadirAnalisis + " WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);
				comando.Parameters.AddWithValue("@precioMinimosHistoricos", JsonSerializer.Serialize(ofertasHistoricas));
				comando.Parameters.AddWithValue("@precioActualesTiendas", JsonSerializer.Serialize(ofertasActuales));
				comando.Parameters.AddWithValue("@historicos", JsonSerializer.Serialize(historicos));

				if (ultimaModificacion != null)
				{
					comando.Parameters.AddWithValue("@ultimaModificacion", ultimaModificacion);
				}

				if (string.IsNullOrEmpty(slugGOG) == false)
				{
					comando.Parameters.AddWithValue("@idGog", idGOG);
					comando.Parameters.AddWithValue("@slugGOG", slugGOG);
				}

				if (string.IsNullOrEmpty(slugEpic) == false)
				{
					comando.Parameters.AddWithValue("@slugEpic", slugEpic);
				}

				if (analisis != null)
				{
					comando.Parameters.AddWithValue("@analisis", JsonSerializer.Serialize(analisis));
				}

				try
				{
					comando.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					Errores.Insertar.Mensaje("Actualizar Juego " + id, ex);
				}
			}
		}

		public static void UsuariosInteresados(int idJuego, SqlConnection conexion, List<JuegoUsuariosInteresados> usuariosInteresados)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET usuariosInteresados=@usuariosInteresados WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", idJuego);
				comando.Parameters.AddWithValue("@usuariosInteresados", JsonSerializer.Serialize(usuariosInteresados));

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void Imagenes(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET imagenes=@imagenes WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@imagenes", JsonSerializer.Serialize(juego.Imagenes));

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}

			string sqlActualizar2 = "UPDATE seccionMinimos " +
					"SET imagenes=@imagenes WHERE idMaestra=@id";

			using (SqlCommand comando2 = new SqlCommand(sqlActualizar2, conexion))
			{
				comando2.Parameters.AddWithValue("@id", juego.Id);
				comando2.Parameters.AddWithValue("@imagenes", JsonSerializer.Serialize(juego.Imagenes));

				try
				{
					comando2.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void PreciosActuales(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET precioActualesTiendas=@precioActualesTiendas WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@precioActualesTiendas", JsonSerializer.Serialize(juego.PrecioActualesTiendas));

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void PreciosHistoricos(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET precioMinimosHistoricos=@precioMinimosHistoricos WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@precioMinimosHistoricos", JsonSerializer.Serialize(juego.PrecioMinimosHistoricos));

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void Bundles(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET bundles=@bundles WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);

				if (juego.Bundles != null)
				{
					comando.Parameters.AddWithValue("@bundles", JsonSerializer.Serialize(juego.Bundles));
				}
				else
				{
					comando.Parameters.AddWithValue("@bundles", "null");
				}

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void Gratis(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET gratis=@gratis WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);

				if (juego.Gratis != null)
				{
					comando.Parameters.AddWithValue("@gratis", JsonSerializer.Serialize(juego.Gratis));
				}
				else
				{
					comando.Parameters.AddWithValue("@gratis", "null");
				}

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void Suscripciones(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET suscripciones=@suscripciones WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@suscripciones", JsonSerializer.Serialize(juego.Suscripciones));

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}

			string sqlActualizar2 = "UPDATE seccionMinimos " +
					"SET suscripciones=@suscripciones WHERE idMaestra=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar2, conexion))
			{
				comando.Parameters.AddWithValue("@idMaestra", juego.Id);
				comando.Parameters.AddWithValue("@suscripciones", JsonSerializer.Serialize(juego.Suscripciones));

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{
				
				}
			}
		}

		public static void DlcMaestro(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET maestro=@maestro WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@maestro", juego.Maestro);

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void FreeToPlay(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET freeToPlay=@freeToPlay WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@freeToPlay", juego.FreeToPlay);

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void MayorEdad(Juego juego, SqlConnection conexion = null)
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

			string sqlActualizar = "UPDATE juegos " +
					"SET mayorEdad=@mayorEdad WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@mayorEdad", juego.MayorEdad);

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}

			bool actualizar = false;
			string buscarMinimos = "SELECT * FROM seccionMinimos WHERE idMaestra=@idMaestra";

			using (SqlCommand comando2 = new SqlCommand(buscarMinimos, conexion))
			{
				comando2.Parameters.AddWithValue("@idMaestra", juego.Id);

				using (SqlDataReader lector = comando2.ExecuteReader())
				{
					if (lector.Read() == true)
					{
						actualizar = true;
					}
				}
			}

			if (actualizar == true)
			{
				string actualizarMinimos = "UPDATE seccionMinimos " +
					"SET mayorEdad=@mayorEdad WHERE idMaestra=@idMaestra";

				using (SqlCommand comando3 = new SqlCommand(actualizarMinimos, conexion))
				{
					comando3.Parameters.AddWithValue("@idMaestra", juego.Id);
					comando3.Parameters.AddWithValue("@mayorEdad", juego.MayorEdad);

					try
					{
						comando3.ExecuteNonQuery();
					}
					catch
					{

					}
				}
			}
		}

		public static void OcultarPortada(Juego juego, SqlConnection conexion = null)
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

			string sqlActualizar = "UPDATE juegos " +
					"SET ocultarPortada=@ocultarPortada WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@ocultarPortada", juego.OcultarPortada);

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}

			bool actualizar = false;
			string buscarMinimos = "SELECT * FROM seccionMinimos WHERE idMaestra=@idMaestra";

			using (SqlCommand comando2 = new SqlCommand(buscarMinimos, conexion))
			{
				comando2.Parameters.AddWithValue("@idMaestra", juego.Id);

				using (SqlDataReader lector = comando2.ExecuteReader())
				{
					if (lector.Read() == true)
					{
						actualizar = true;
					}
				}
			}

			if (actualizar == true)
			{
				string actualizarMinimos = "UPDATE seccionMinimos " +
					"SET ocultarPortada=@ocultarPortada WHERE idMaestra=@idMaestra";

				using (SqlCommand comando3 = new SqlCommand(actualizarMinimos, conexion))
				{
					comando3.Parameters.AddWithValue("@idMaestra", juego.Id);
					comando3.Parameters.AddWithValue("@ocultarPortada", juego.OcultarPortada);

					try
					{
						comando3.ExecuteNonQuery();
					}
					catch
					{

					}
				}
			}
		}

		public static void Media(Juego nuevoJuego, Juego juego, SqlConnection conexion = null)
		{
			if (nuevoJuego != null && juego != null)
			{
				if (juego.Imagenes == null)
				{
					juego.Imagenes = new JuegoImagenes();
				}

				if (string.IsNullOrEmpty(juego.Imagenes.Library_600x900) == false)
				{
                    if (juego.Imagenes.Library_600x900.Contains("i.imgur.com") == false)
                    {
                        juego.Imagenes.Library_600x900 = nuevoJuego.Imagenes.Library_600x900;
                    }
                }
				else
				{
                    juego.Imagenes.Library_600x900 = nuevoJuego.Imagenes.Library_600x900;
                }

                if (string.IsNullOrEmpty(juego.Imagenes.Library_1920x620) == false)
                {
                    if (juego.Imagenes.Library_1920x620.Contains("i.imgur.com") == false)
                    {
                        juego.Imagenes.Library_1920x620 = nuevoJuego.Imagenes.Library_1920x620;
                    }
                }
                else
                {
                    juego.Imagenes.Library_1920x620 = nuevoJuego.Imagenes.Library_1920x620;
                }

                if (string.IsNullOrEmpty(juego.Imagenes.Logo) == false)
                {
                    if (juego.Imagenes.Logo.Contains("i.imgur.com") == false)
                    {
                        juego.Imagenes.Logo = nuevoJuego.Imagenes.Logo;
                    }
                }
                else
                {
                    juego.Imagenes.Logo = nuevoJuego.Imagenes.Logo;
                }

				juego.Imagenes.Capsule_231x87 = nuevoJuego.Imagenes.Capsule_231x87;
				juego.Imagenes.Header_460x215 = nuevoJuego.Imagenes.Header_460x215;

				juego.Nombre = nuevoJuego.Nombre;
				
				if (juego.Caracteristicas == null)
				{
					juego.Caracteristicas = new JuegoCaracteristicas();
				}

				juego.Caracteristicas = nuevoJuego.Caracteristicas;
				juego.Deck = nuevoJuego.Deck;
				juego.SteamOS = nuevoJuego.SteamOS;

				juego.Media = nuevoJuego.Media;
				juego.Categorias = nuevoJuego.Categorias;
				juego.Etiquetas = nuevoJuego.Etiquetas;

				if (juego.Idiomas == null)
				{
					juego.Idiomas = new List<JuegoIdioma>();
				}

				if (nuevoJuego.Idiomas != null)
				{
					if (nuevoJuego.Idiomas.Count > 0)
					{
						foreach (var nuevoIdioma in nuevoJuego.Idiomas)
						{
							bool existe = false;

							if (juego.Idiomas != null)
							{
								foreach (var viejoIdioma in juego.Idiomas)
								{
									if (viejoIdioma.DRM == nuevoIdioma.DRM && nuevoIdioma.Idioma == viejoIdioma.Idioma)
									{
										existe = true;

										viejoIdioma.Audio = nuevoIdioma.Audio;
										viejoIdioma.Texto = nuevoIdioma.Texto;

										break;
									}
								}
							}

							if (existe == false)
							{
								juego.Idiomas.Add(nuevoIdioma);
							}
						}
					}
				}

				string añadirAnalisis = null;

				if (nuevoJuego.Analisis != null)
				{
					if (string.IsNullOrEmpty(nuevoJuego.Analisis.Porcentaje) == false && string.IsNullOrEmpty(nuevoJuego.Analisis.Cantidad) == false)
					{
						añadirAnalisis = ", analisis=@analisis";
					}
				}

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

				using (conexion)
				{
					string sqlActualizar = "UPDATE juegos " +
										"SET nombre=@nombre, imagenes=@imagenes, caracteristicas=@caracteristicas, media=@media, nombreCodigo=@nombreCodigo, categorias=@categorias, etiquetas=@etiquetas, fechaSteamAPIComprobacion=@fechaSteamAPIComprobacion, idiomas=@idiomas, deck=@deck, steamOS=@steamOS" + añadirAnalisis + " WHERE id=@id";

					using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
					{
						comando.Parameters.AddWithValue("@id", juego.Id);
						comando.Parameters.AddWithValue("@nombre", juego.Nombre);
						comando.Parameters.AddWithValue("@imagenes", JsonSerializer.Serialize(juego.Imagenes));
						comando.Parameters.AddWithValue("@caracteristicas", JsonSerializer.Serialize(juego.Caracteristicas));
						comando.Parameters.AddWithValue("@media", JsonSerializer.Serialize(juego.Media));
						comando.Parameters.AddWithValue("@nombreCodigo", Herramientas.Buscador.LimpiarNombre(juego.Nombre));
						comando.Parameters.AddWithValue("@categorias", JsonSerializer.Serialize(juego.Categorias));
						comando.Parameters.AddWithValue("@etiquetas", JsonSerializer.Serialize(juego.Etiquetas));
						comando.Parameters.AddWithValue("@fechaSteamAPIComprobacion", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffff"));
						comando.Parameters.AddWithValue("@idiomas", JsonSerializer.Serialize(juego.Idiomas));
						comando.Parameters.AddWithValue("@deck", juego.Deck);
						comando.Parameters.AddWithValue("@steamOS", juego.SteamOS);

						if (string.IsNullOrEmpty(añadirAnalisis) == false)
						{
							comando.Parameters.AddWithValue("@analisis", JsonSerializer.Serialize(nuevoJuego.Analisis));
						}

						try
						{
							comando.ExecuteNonQuery();
						}
						catch (Exception ex)
						{
							BaseDatos.Errores.Insertar.Mensaje("Actualizar Steam API", ex);
						}
					}
				}
			}		
		}

		public static void Tipo(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET tipo=@tipo WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@tipo", juego.Tipo);

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void IdSteam(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET idSteam=@idSteam WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@idSteam", juego.IdSteam);

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void Deseados(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET usuariosInteresados=@usuariosInteresados WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@usuariosInteresados", JsonSerializer.Serialize(juego.UsuariosInteresados));

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void IdGOG(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET idGOG=@idGOG WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@idGOG", juego.IdGog);

				comando.ExecuteNonQuery();
				try
				{

				}
				catch
				{

				}
			}
		}

		public static void SlugGOG(Juego juego, SqlConnection conexion = null)
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

			string sqlActualizar = "UPDATE juegos " +
					"SET slugGOG=@slugGOG WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@slugGOG", juego.SlugGOG);

				comando.ExecuteNonQuery();
				try
				{
					
				}
				catch
				{

				}
			}
		}

		public static void SlugEpic(Juego juego, SqlConnection conexion = null)
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

			string sqlActualizar = "UPDATE juegos " +
					"SET slugEpic=@slugEpic WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@slugEpic", juego.SlugEpic);

				comando.ExecuteNonQuery();
				try
				{

				}
				catch
				{

				}
			}
		}

		public static void ExeEpic(Juego juego, SqlConnection conexion = null)
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

			string sqlActualizar = "UPDATE juegos " +
					"SET exeEpic=@exeEpic WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@exeEpic", juego.ExeEpic);

				comando.ExecuteNonQuery();
				try
				{

				}
				catch
				{

				}
			}
		}

		public static void IdXbox(int idJuego, string idXbox, SqlConnection conexion = null)
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

			string sqlActualizar = "UPDATE juegos " +
					"SET idXbox=@idXbox WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", idJuego);
				comando.Parameters.AddWithValue("@idXbox", idXbox);

				comando.ExecuteNonQuery();
				try
				{

				}
				catch
				{

				}
			}
		}

		public static void ExeUbisoft(Juego juego, SqlConnection conexion = null)
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

			string sqlActualizar = "UPDATE juegos " +
					"SET exeUbisoft=@exeUbisoft WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@exeUbisoft", juego.ExeUbisoft);

				comando.ExecuteNonQuery();
				try
				{

				}
				catch
				{

				}
			}
		}

		public static void Deck(Juego juego, SqlConnection conexion = null)
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

			using (conexion)
			{
				string sqlActualizar = "UPDATE juegos " +
					"SET deck=@deck, deckTokens=@deckTokens, deckComprobacion=@deckComprobacion, steamOS=@steamOS, steamOSTokens=@steamOSTokens WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
				{
					comando.Parameters.AddWithValue("@id", juego.Id);
					comando.Parameters.AddWithValue("@deck", juego.Deck);
					comando.Parameters.AddWithValue("@deckTokens", JsonSerializer.Serialize(juego.DeckTokens));
					comando.Parameters.AddWithValue("@deckComprobacion", juego.DeckComprobacion);
					comando.Parameters.AddWithValue("@steamOS", juego.SteamOS);
					comando.Parameters.AddWithValue("@steamOSTokens", JsonSerializer.Serialize(juego.SteamOSTokens));

					comando.ExecuteNonQuery();
					try
					{

					}
					catch
					{

					}
				}
			}
		}

		public static void DeckVacio(Juego juego, SqlConnection conexion = null)
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

			using (conexion)
			{
				string sqlActualizar = "UPDATE juegos " +
					"SET deckComprobacion=@deckComprobacion WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
				{
					comando.Parameters.AddWithValue("@id", juego.Id);
					comando.Parameters.AddWithValue("@deckComprobacion", juego.DeckComprobacion);

					comando.ExecuteNonQuery();
					try
					{

					}
					catch
					{

					}
				}
			}
		}

		public static void Historicos(Juego juego, SqlConnection conexion = null)
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

			using (conexion)
			{
				string sqlActualizar = "UPDATE juegos " +
					"SET historicos=@historicos WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
				{
					comando.Parameters.AddWithValue("@id", juego.Id);
					comando.Parameters.AddWithValue("@historicos", JsonSerializer.Serialize(juego.Historicos));

					comando.ExecuteNonQuery();
					try
					{

					}
					catch
					{

					}
				}
			}
		}

		public static void GalaxyGOG(Juego juego, SqlConnection conexion = null)
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

			using (conexion)
			{
				string sqlActualizar = "UPDATE juegos " +
					"SET galaxyGOG=@galaxyGOG, idiomas=@idiomas WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
				{
					comando.Parameters.AddWithValue("@id", juego.Id);
					comando.Parameters.AddWithValue("@galaxyGOG", JsonSerializer.Serialize(juego.GalaxyGOG));
					comando.Parameters.AddWithValue("@idiomas", JsonSerializer.Serialize(juego.Idiomas));

					comando.ExecuteNonQuery();
					try
					{

					}
					catch
					{

					}
				}
			}
		}

		public static void EpicGames(Juego juego, SqlConnection conexion = null)
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

			using (conexion)
			{
				string sqlActualizar = "UPDATE juegos " +
					"SET epicGames=@epicGames, idiomas=@idiomas WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
				{
					comando.Parameters.AddWithValue("@id", juego.Id);
					comando.Parameters.AddWithValue("@epicGames", JsonSerializer.Serialize(juego.EpicGames));
					comando.Parameters.AddWithValue("@idiomas", JsonSerializer.Serialize(juego.Idiomas));

					comando.ExecuteNonQuery();
					try
					{

					}
					catch
					{

					}
				}
			}
		}

		public static void Xbox(Juego juego, SqlConnection conexion = null)
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

			using (conexion)
			{
				string sqlActualizar = "UPDATE juegos " +
					"SET xbox=@xbox, idiomas=@idiomas WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
				{
					comando.Parameters.AddWithValue("@id", juego.Id);
					comando.Parameters.AddWithValue("@xbox", JsonSerializer.Serialize(juego.Xbox));
					comando.Parameters.AddWithValue("@idiomas", JsonSerializer.Serialize(juego.Idiomas));

					comando.ExecuteNonQuery();
					try
					{

					}
					catch
					{

					}
				}
			}
		}

		public static void CantidadJugadoresSteam(Juego juego, SqlConnection conexion = null)
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

			using (conexion)
			{
				string sqlActualizar = "UPDATE juegos " +
					"SET cantidadJugadoresSteam=@cantidadJugadoresSteam WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
				{
					comando.Parameters.AddWithValue("@id", juego.Id);
					comando.Parameters.AddWithValue("@cantidadJugadoresSteam", JsonSerializer.Serialize(juego.CantidadJugadores));

					comando.ExecuteNonQuery();
					try
					{

					}
					catch
					{

					}
				}
			}
		}

		public static void UltimasActualizacioneseInteligenciaArticial(int idJuego, DateTime? fechaSteam, DateTime? fechaGOG, bool inteligenciaArtificial = false, SqlConnection conexion = null)
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

			string añadirFechaSteam = null;

			if (fechaSteam != null)
			{
				añadirFechaSteam = ", ultimaActualizacionSteam=@ultimaActualizacionSteam";
			}

			string añadirFechaGOG = null;

			if (fechaGOG != null)
			{
				añadirFechaGOG = ", ultimaActualizacionGOG=@ultimaActualizacionGOG";
			}

			using (conexion)
			{
				string sqlActualizar = "UPDATE juegos " +
					"SET inteligenciaArtificial=@inteligenciaArtificial, ultimaActualizacion=@ultimaActualizacion" + añadirFechaSteam + añadirFechaGOG + " WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
				{
					comando.Parameters.AddWithValue("@id", idJuego);
					comando.Parameters.AddWithValue("@inteligenciaArtificial", inteligenciaArtificial);
					comando.Parameters.AddWithValue("@ultimaActualizacion", DateTime.Now);

					if (fechaSteam != null)
					{
						comando.Parameters.AddWithValue("@ultimaActualizacionSteam", fechaSteam);
					}

					if (fechaGOG != null)
					{
						comando.Parameters.AddWithValue("@ultimaActualizacionGOG", fechaGOG);
					}

					try
					{
						comando.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Mensaje("Actualizar SteamCMD " + idJuego.ToString(), ex);
					}
				}
			}
		}
	}
}
