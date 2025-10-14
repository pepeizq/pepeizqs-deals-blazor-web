#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using pepeizqs_deals_web.Data;
using System.Text.Json;

namespace Herramientas
{
	public static class Deseados
	{
		public static async Task<List<JuegoDeseadoMostrar>> LeerJuegos(string usuarioId)
		{
			await Task.Yield();

			Usuario deseadosUsuario = global::BaseDatos.Usuarios.Buscar.DeseadosTiene(usuarioId);

			List<JuegoDeseadoMostrar> deseadosGestor = new List<JuegoDeseadoMostrar>();

			#region Deseados Steam

			List<string> deseadosSteam = new List<string>();

			if (string.IsNullOrEmpty(deseadosUsuario.SteamWishlist) == false)
			{
				deseadosSteam = Herramientas.Listados.Generar(deseadosUsuario.SteamWishlist);
			}

			if (deseadosSteam?.Count > 0)
			{
				List<Juego> deseadosSteamJuegos = new List<Juego>();

				deseadosSteamJuegos = global::BaseDatos.Juegos.Buscar.MultiplesJuegosSteam(deseadosSteam);

				if (deseadosSteamJuegos != null)
				{
					int i = 0;

					foreach (var juego in deseadosSteamJuegos)
					{
						i += 1;

						if (juego != null)
						{
							deseadosGestor = AñadirJuegoMostrar(deseadosGestor, juego, JuegoDRM.Steam, true);
						}
					}
				}
			}

			#endregion

			#region Deseados Web

			List<JuegoDeseado> deseadosWeb = new List<JuegoDeseado>();

			if (string.IsNullOrEmpty(deseadosUsuario.Wishlist) == false)
			{
				deseadosWeb = JsonSerializer.Deserialize<List<JuegoDeseado>>(deseadosUsuario.Wishlist);
			}

			if (deseadosWeb?.Count > 0)
			{
				List<Juego> deseadosWebJuegos = new List<Juego>();

				deseadosWebJuegos = global::BaseDatos.Juegos.Buscar.MultiplesJuegos(deseadosWeb);

				int i = 0;

				foreach (var deseadoWeb in deseadosWebJuegos)
				{
					i += 1;

					JuegoDRM drmDeseado = JuegoDRM.NoEspecificado;

					foreach (var deseado in deseadosWeb)
					{
						if (deseado.IdBaseDatos == deseadoWeb.Id.ToString())
						{
							drmDeseado = deseado.DRM;
							break;
						}
					}

					deseadosGestor = AñadirJuegoMostrar(deseadosGestor, deseadoWeb, drmDeseado, false);
				}
			}

			#endregion

			#region Deseados GOG

			List<string> deseadosGog = new List<string>();

			if (string.IsNullOrEmpty(deseadosUsuario.GogWishlist) == false)
			{
				deseadosGog = Herramientas.Listados.Generar(deseadosUsuario.GogWishlist);
			}

			if (deseadosGog?.Count > 0)
			{
				List<Juego> deseadosGogJuegos = new List<Juego>();

				deseadosGogJuegos = global::BaseDatos.Juegos.Buscar.MultiplesJuegosGOG(deseadosGog);

				if (deseadosGogJuegos != null)
				{
					int i = 0;

					foreach (var juego in deseadosGogJuegos)
					{
						i += 1;

						if (juego != null)
						{
							deseadosGestor = AñadirJuegoMostrar(deseadosGestor, juego, JuegoDRM.GOG, true);
						}
					}
				}
			}

			#endregion

			return deseadosGestor;
		}

		private static List<JuegoDeseadoMostrar> AñadirJuegoMostrar(List<JuegoDeseadoMostrar> deseadosGestor, Juego juego, JuegoDRM drm, bool importado)
		{
			bool yaEsta = false;

			if (deseadosGestor.Count > 0)
			{
				foreach (var deseado in deseadosGestor)
				{
					if (deseado.Id == juego.Id && deseado.DRM == drm)
					{
						yaEsta = true;
						break;
					}
				}
			}

			if (yaEsta == false)
			{
				bool añadido = false;

				if (juego?.PrecioMinimosHistoricos?.Count > 0)
				{
					foreach (var historico in juego.PrecioMinimosHistoricos)
					{
						if (historico.DRM == drm)
						{
							if (Herramientas.OfertaActiva.Verificar(historico) == true)
							{
								JuegoDeseadoMostrar nuevoDeseado = new JuegoDeseadoMostrar();
								nuevoDeseado.Id = juego.Id;
								nuevoDeseado.IdSteam = juego.IdSteam;
								nuevoDeseado.IdGog = juego.IdGog;
								nuevoDeseado.SlugEpic = juego.SlugEpic;
								nuevoDeseado.Nombre = juego.Nombre;
								nuevoDeseado.Imagen = juego.Imagenes.Header_460x215;
								nuevoDeseado.DRM = drm;
								nuevoDeseado.Precio = historico;
								nuevoDeseado.Historico = true;
								nuevoDeseado.Importado = importado;

								if (juego.Analisis != null)
								{
									if (string.IsNullOrEmpty(juego.Analisis.Porcentaje) == false)
									{
										nuevoDeseado.ReseñasPorcentaje = juego.Analisis.Porcentaje.Replace("%", null);
									}

									if (string.IsNullOrEmpty(juego.Analisis.Cantidad) == false)
									{
										nuevoDeseado.ReseñasCantidad = juego.Analisis.Cantidad.Replace(",", null);
									}
								}
								else
								{
									nuevoDeseado.ReseñasPorcentaje = "0";
									nuevoDeseado.ReseñasCantidad = "0";
								}

								deseadosGestor.Add(nuevoDeseado);

								añadido = true;
							}

							break;
						}
					}
				}

				if (añadido == false)
				{
					if (juego.PrecioActualesTiendas?.Count > 0)
					{
						JuegoPrecio precioFinal = null;
						decimal precioReferencia = 1000000;

						foreach (var actual in juego.PrecioActualesTiendas)
						{
							if (actual != null)
							{
								if (actual.DRM == drm)
								{
									if (Herramientas.OfertaActiva.Verificar(actual) == true)
									{
										if (actual.Precio > 0)
										{
											if (actual.Moneda != Herramientas.JuegoMoneda.Euro && actual.PrecioCambiado == 0)
											{
												actual.PrecioCambiado = Herramientas.Divisas.Cambio(actual.Precio, actual.Moneda);
											}

											if (precioReferencia > actual.Precio && actual.Precio > 0 && actual.Moneda == Herramientas.JuegoMoneda.Euro)
											{
												precioReferencia = actual.Precio;
												precioFinal = actual;
												precioFinal.Precio = actual.Precio;
											}
											else if (precioReferencia > actual.PrecioCambiado && actual.PrecioCambiado > 0 && actual.Moneda != Herramientas.JuegoMoneda.Euro)
											{
												precioReferencia = actual.PrecioCambiado;
												precioFinal = actual;
												precioFinal.Precio = actual.Precio;
												precioFinal.PrecioCambiado = actual.PrecioCambiado;
											}
										}
									}
								}
							}
						}

						if (precioFinal != null)
						{
							JuegoDeseadoMostrar nuevoDeseado = new JuegoDeseadoMostrar();
							nuevoDeseado.Id = juego.Id;
							nuevoDeseado.IdSteam = juego.IdSteam;
							nuevoDeseado.IdGog = juego.IdGog;
							nuevoDeseado.SlugEpic = juego.SlugEpic;
							nuevoDeseado.Nombre = juego.Nombre;
							nuevoDeseado.Imagen = juego.Imagenes.Header_460x215;
							nuevoDeseado.DRM = drm;
							nuevoDeseado.Precio = precioFinal;
							nuevoDeseado.Historico = false;

							foreach (var minimo in juego.PrecioMinimosHistoricos)
							{
								if (drm == minimo.DRM)
								{
									if (minimo.PrecioCambiado > 0 && minimo.Moneda != Herramientas.JuegoMoneda.Euro)
									{
										nuevoDeseado.HistoricoPrecio = Herramientas.Precios.Euro(minimo.PrecioCambiado);
									}
									else if (minimo.PrecioCambiado == 0 && minimo.Moneda != Herramientas.JuegoMoneda.Euro)
									{
										nuevoDeseado.HistoricoPrecio = Herramientas.Precios.Euro(Herramientas.Divisas.Cambio(minimo.Precio, minimo.Moneda));
									}
									else
									{
										nuevoDeseado.HistoricoPrecio = Herramientas.Precios.Euro(minimo.Precio);
									}
								}
							}

							nuevoDeseado.Importado = importado;

							if (juego.Analisis != null)
							{
								nuevoDeseado.ReseñasPorcentaje = juego.Analisis.Porcentaje.Replace("%", null);
								nuevoDeseado.ReseñasCantidad = juego.Analisis.Cantidad.Replace(",", null);
							}
							else
							{
								nuevoDeseado.ReseñasPorcentaje = "0";
								nuevoDeseado.ReseñasCantidad = "0";
							}

							deseadosGestor.Add(nuevoDeseado);
						}
					}
				}
			}

			return deseadosGestor;
		}

		public static void ActualizarJuegoConUsuarios(int idJuego, List<JuegoUsuariosInteresados> usuariosInteresados, JuegoDRM drm, string usuarioId, bool estado)
		{
			SqlConnection conexion = BaseDatos.Conectar();

			using (conexion)
			{
				if (usuariosInteresados == null)
				{
					usuariosInteresados = new List<JuegoUsuariosInteresados>();
				}

				if (usuariosInteresados.Count > 0)
				{
					if (estado == false)
					{
						int posicion = -1;

						for (int i = 0; i < usuariosInteresados.Count; i += 1)
						{
							if (usuariosInteresados[i].DRM == drm && usuariosInteresados[i].UsuarioId == usuarioId)
							{
								posicion = i;
							}
						}

						if (posicion >= 0)
						{
							usuariosInteresados.RemoveAt(posicion);
						}
					}
					else
					{
						JuegoUsuariosInteresados nuevo = new JuegoUsuariosInteresados();
						nuevo.UsuarioId = usuarioId;
						nuevo.DRM = drm;

						bool añadir = true;

						foreach (var usuario2 in usuariosInteresados)
						{
							if (usuario2.DRM == drm && usuario2.UsuarioId == usuarioId)
							{
								añadir = false;
							}
						}

						if (añadir == true)
						{
							usuariosInteresados.Add(nuevo);
						}
					}
				}
				else
				{
					JuegoUsuariosInteresados nuevo = new JuegoUsuariosInteresados();
					nuevo.UsuarioId = usuarioId;
					nuevo.DRM = drm;

					usuariosInteresados.Add(nuevo);
				}

				global::BaseDatos.Juegos.Actualizar.UsuariosInteresados(idJuego, conexion, usuariosInteresados);
			}
		}

		public static bool ComprobarSiEsta(Usuario deseados, Juego juego, JuegoDRM drm = JuegoDRM.NoEspecificado, bool usarIdMaestra = false)
		{
			if (deseados == null)
			{
				return false;
			}

			string deseadosSteamEnBruto = deseados.SteamWishlist;
			string deseadosWebEnBruto = deseados.Wishlist;
			string deseadosGogEnBruto = deseados.GogWishlist;

			if (usarIdMaestra == true && juego.IdMaestra == 0)
			{
				juego.IdMaestra = juego.Id;
			}

			if (drm == JuegoDRM.Steam || drm == JuegoDRM.NoEspecificado)
			{
				if (juego.IdSteam > 0)
				{
					List<string> deseadosSteam = new List<string>();

					if (string.IsNullOrEmpty(deseadosSteamEnBruto) == false)
					{
						deseadosSteam = Listados.Generar(deseadosSteamEnBruto);
					}

					if (deseadosSteam != null)
					{
						if (deseadosSteam.Count > 0)
						{
							foreach (var deseado in deseadosSteam)
							{
								if (juego.IdSteam > 0)
								{
									if (juego.IdSteam.ToString() == deseado && (drm == JuegoDRM.Steam || drm == JuegoDRM.NoEspecificado))
									{
										return true;
									}
								}
							}
						}
					}
				}
			}

			if (drm == JuegoDRM.GOG || drm == JuegoDRM.NoEspecificado)
			{
				if (juego.IdGog > 0)
				{
					List<string> deseadosGog = new List<string>();

					if (string.IsNullOrEmpty(deseadosGogEnBruto) == false)
					{
						deseadosGog = Listados.Generar(deseadosGogEnBruto);
					}

					if (deseadosGog != null)
					{
						if (deseadosGog.Count > 0)
						{
							foreach (var deseado in deseadosGog)
							{
								if (juego.IdGog.ToString() == deseado && (drm == JuegoDRM.GOG || drm == JuegoDRM.NoEspecificado))
								{
									return true;
								}
							}
						}
					}
				}
			}

			if (juego.Id > 0)
			{
				List<JuegoDeseado> deseadosWeb = new List<JuegoDeseado>();

				if (string.IsNullOrEmpty(deseadosWebEnBruto) == false)
				{
					deseadosWeb = JsonSerializer.Deserialize<List<JuegoDeseado>>(deseadosWebEnBruto);
				}

				if (deseadosWeb != null)
				{
					if (deseadosWeb.Count > 0)
					{
						foreach (var deseado in deseadosWeb)
						{
							if (usarIdMaestra == false)
							{
								if (juego.Id == int.Parse(deseado.IdBaseDatos))
								{
									if (drm == deseado.DRM || drm == JuegoDRM.NoEspecificado)
									{
										return true;
									}
								}
							}
							else
							{
								if (juego.IdMaestra == int.Parse(deseado.IdBaseDatos))
								{
									if (drm == deseado.DRM || drm == JuegoDRM.NoEspecificado)
									{
										return true;
									}
								}
							}
						}
					}
				}
			}

			return false;
		}

		public static bool ComprobarSiEsta(string deseadosSteamEnBruto, string deseadosWebEnBruto, string deseadosGogEnBruto, Juego juego, JuegoDRM drm = JuegoDRM.NoEspecificado, bool usarIdMaestra = false)
		{
			if (usarIdMaestra == true && juego.IdMaestra == 0)
			{
				juego.IdMaestra = juego.Id;
			}

			if (drm == JuegoDRM.Steam || drm == JuegoDRM.NoEspecificado)
			{
				if (juego.IdSteam > 0)
				{
					List<string> deseadosSteam = new List<string>();

					if (string.IsNullOrEmpty(deseadosSteamEnBruto) == false)
					{
						deseadosSteam = Listados.Generar(deseadosSteamEnBruto);
					}

					if (deseadosSteam != null)
					{
						if (deseadosSteam.Count > 0)
						{
							foreach (var deseado in deseadosSteam)
							{
								if (juego.IdSteam > 0)
								{
									if (juego.IdSteam.ToString() == deseado && (drm == JuegoDRM.Steam || drm == JuegoDRM.NoEspecificado))
									{
										return true;
									}
								}
							}
						}
					}
				}
			}

			if (drm == JuegoDRM.GOG || drm == JuegoDRM.NoEspecificado)
			{
				if (juego.IdGog > 0)
				{
					List<string> deseadosGog = new List<string>();

					if (string.IsNullOrEmpty(deseadosGogEnBruto) == false)
					{
						deseadosGog = Listados.Generar(deseadosGogEnBruto);
					}

					if (deseadosGog != null)
					{
						if (deseadosGog.Count > 0)
						{
							foreach (var deseado in deseadosGog)
							{
								if (juego.IdGog.ToString() == deseado && (drm == JuegoDRM.GOG || drm == JuegoDRM.NoEspecificado))
								{
									return true;
								}
							}
						}
					}
				}
			}

			if (juego.Id > 0)
			{
				List<JuegoDeseado> deseadosWeb = new List<JuegoDeseado>();

				if (string.IsNullOrEmpty(deseadosWebEnBruto) == false)
				{
					deseadosWeb = JsonSerializer.Deserialize<List<JuegoDeseado>>(deseadosWebEnBruto);
				}

				if (deseadosWeb != null)
				{
					if (deseadosWeb.Count > 0)
					{
						foreach (var deseado in deseadosWeb)
						{
							if (usarIdMaestra == false)
							{
								if (juego.Id == int.Parse(deseado.IdBaseDatos))
								{
									if (drm == deseado.DRM || drm == JuegoDRM.NoEspecificado)
									{
										return true;
									}
								}
							}
							else
							{
								if (juego.IdMaestra == int.Parse(deseado.IdBaseDatos))
								{
									if (drm == deseado.DRM || drm == JuegoDRM.NoEspecificado)
									{
										return true;
									}
								}
							}
						}
					}
				}
			}
			
			return false;
		}

		public static bool ComprobarSiEsta(string deseadosWebEnBruto, Juego juego, JuegoDRM drm, bool usarIdMaestra = false)
		{
			List<JuegoDeseado> deseadosWeb = new List<JuegoDeseado>();

			if (string.IsNullOrEmpty(deseadosWebEnBruto) == false)
			{
				deseadosWeb = JsonSerializer.Deserialize<List<JuegoDeseado>>(deseadosWebEnBruto);
			}

			if (deseadosWeb != null)
			{
				if (deseadosWeb.Count > 0)
				{
					foreach (var deseado in deseadosWeb)
					{
						if (usarIdMaestra == false)
						{
							if (juego.Id == int.Parse(deseado.IdBaseDatos))
							{
								if (drm == deseado.DRM)
								{
									return true;
								}
							}
						}
						else
						{
							if (juego.IdMaestra == int.Parse(deseado.IdBaseDatos))
							{
								if (drm == deseado.DRM)
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

		public static List<JuegoRazorUsuario> CambiarEstado(List<JuegoRazorUsuario> usuarioTieneDesea, string usuarioId, Juego juego, bool estado, JuegoDRM drm)
		{
			List<JuegoDeseado> deseados = new List<JuegoDeseado>();

			Usuario deseadosCargar = global::BaseDatos.Usuarios.Buscar.DeseadosTiene(usuarioId);

			if (deseadosCargar != null)
			{
				if (string.IsNullOrEmpty(deseadosCargar.Wishlist) == false)
				{
					deseados = JsonSerializer.Deserialize<List<JuegoDeseado>>(deseadosCargar.Wishlist);
				}
			}

			ActualizarJuegoConUsuarios(juego.Id, juego.UsuariosInteresados, drm, usuarioId, estado);

			if (estado == true)
			{
				bool añadir = true;

				if (deseados.Count > 0)
				{
					foreach (var deseado in deseados)
					{
						if (int.Parse(deseado.IdBaseDatos) == juego.Id && deseado.DRM == drm)
						{
							añadir = false;
						}
					}
				}

				if (añadir == true)
				{
					JuegoDeseado deseado = new JuegoDeseado();
					deseado.IdBaseDatos = juego.Id.ToString();
					deseado.DRM = drm;

					deseados.Add(deseado);

					if (usuarioTieneDesea != null)
					{
						JuegoRazorUsuario deseado2 = new JuegoRazorUsuario();
						deseado2.DRM = drm;
						deseado2.Desea = true;
						usuarioTieneDesea.Add(deseado2);
					}
				}

				global::BaseDatos.Usuarios.Actualizar.Opcion("Wishlist", JsonSerializer.Serialize(deseados), usuarioId);
			}
			else
			{
				int posicion = -1;

				if (deseados.Count > 0)
				{
					for (int i = 0; i < deseados.Count; i += 1)
					{
						if (int.Parse(deseados[i].IdBaseDatos) == juego.Id && deseados[i].DRM == drm)
						{
							posicion = i;
						}
					}
				}

				if (posicion >= 0)
				{
					deseados.RemoveAt(posicion);
				}

				global::BaseDatos.Usuarios.Actualizar.Opcion("Wishlist", JsonSerializer.Serialize(deseados), usuarioId);

				if (usuarioTieneDesea != null)
				{
					foreach (var desea in usuarioTieneDesea)
					{
						if (desea.DRM == drm)
						{
							desea.Desea = false;
						}
					}
				}
			}

			return usuarioTieneDesea;
		}
	}
}
