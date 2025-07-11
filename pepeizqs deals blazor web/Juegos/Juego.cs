﻿#nullable disable

using APIs.Steam;
using Bundles2;
using Gratis2;
using Herramientas;
using Microsoft.AspNetCore.Components;
using Suscripciones2;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Juegos
{
	public class Juego : ComponentBase, IComponent
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public int IdSteam { get; set; }
		public int IdGog { get; set; }
		public string Nombre { get; set; }
		public JuegoTipo Tipo { get; set; }
		public JuegoImagenes Imagenes { get; set; }
		public List<JuegoPrecio> PrecioMinimosHistoricos { get; set; }
		public List<JuegoPrecio> PrecioActualesTiendas { get; set; }
		public JuegoAnalisis Analisis { get; set; }
		public JuegoCaracteristicas Caracteristicas { get; set; }
		public JuegoMedia Media { get; set; }
		public DateTime FechaSteamAPIComprobacion { get; set; }
		public List<JuegoBundle> Bundles { get; set; }
		public List<JuegoGratis> Gratis { get; set; }
		public List<JuegoSuscripcion> Suscripciones { get; set; }
		public string NombreCodigo { get; set; }
		public int IdMaestra { get; set; }
		public List<JuegoUsuariosInteresados> UsuariosInteresados {  get; set; }
		public string SlugGOG { get; set; }
		public string Maestro { get; set; }
		public string FreeToPlay { get; set; }
		public string MayorEdad { get; set; }
		public string SlugEpic { get; set; }
		public DateTime? UltimaModificacion { get; set; }
		public List<string> Categorias { get; set; }
		public List<string> Etiquetas { get; set; }
		public JuegoDeck Deck { get; set; }
		public List<JuegoDeckToken> DeckTokens { get; set; }
		public DateTime? DeckComprobacion { get; set; }
		public JuegoSteamOS SteamOS { get; set; }
		public List<JuegoDeckToken> SteamOSTokens { get; set; }
		public List<JuegoHistorico> Historicos { get; set; }
		public JuegoGalaxyGOG GalaxyGOG { get; set; }
		public JuegoCantidadJugadoresSteam CantidadJugadores { get; set; }
		public List<JuegoIdioma> Idiomas { get; set; }
		public JuegoEpicGames EpicGames { get; set; }
		public string IdXbox { get; set; }
		public JuegoXbox Xbox { get; set; }
		public string IdAmazon { get; set; }
		public string ExeEpic { get; set; }
		public string ExeUbisoft { get; set; }
		public string ExeEA { get; set; }
		public bool OcultarPortada { get; set; }
		public DateTime? UltimaActualizacionSteam { get; set; }
		public DateTime? UltimaActualizacionGOG { get; set; }
		public DateTime? UltimaActualizacion { get; set; }
		public bool InteligenciaArtificial { get; set; }
	}

	public static class JuegoCrear
	{
		public static Juego Generar()
		{
			JuegoImagenes imagenes = new JuegoImagenes();
			List<JuegoPrecio> precioMinimoHistorico = new List<JuegoPrecio>();
			List<JuegoPrecio> precioActualesTiendas = new List<JuegoPrecio>();
			JuegoAnalisis analisis = new JuegoAnalisis();

			Juego juego = new Juego
			{
				Imagenes = imagenes,
				PrecioMinimosHistoricos = precioMinimoHistorico,
				PrecioActualesTiendas = precioActualesTiendas,
				Analisis = analisis
			};

			return juego;
		}
	}

	//-------------------------------------------------------

	public class JuegoImagenes
	{
		public string Header_460x215 { get; set; }
		public string Capsule_231x87 { get; set; }
		public string Library_600x900 { get; set; }
		public string Library_1920x620 { get; set; }
		public string Logo { get; set; }
	}

	public class JuegoPrecio
	{
		public string Nombre { get; set; }
		public string Imagen { get; set; }
		public JuegoDRM DRM { get; set; }
		public string Enlace { get; set; }
		public int Descuento { get; set; }
		public decimal Precio { get; set; }
		public decimal PrecioCambiado { get; set; }
		public JuegoMoneda Moneda { get; set; }
		public string Tienda { get; set; }
		public DateTime FechaDetectado { get; set; }
		public DateTime FechaActualizacion { get; set; }
		public DateTime FechaTermina { get; set; }
		public int CodigoDescuento { get; set; }
		public string CodigoTexto { get; set; }
		public string SteamID { get; set; }
	}

	public class JuegoAnalisis
	{
		public string Porcentaje { get; set; }
		public string Cantidad { get; set; }
	}

	public class JuegoCaracteristicas
	{
		public bool Windows { get; set; }
		public bool Mac { get; set; }
		public bool Linux { get; set; }
		public string Descripcion { get; set; }
		public List<JuegoCaracteristicasCurator> Desarrolladores2 { get; set; }
		public List<JuegoCaracteristicasCurator> Editores2 { get; set; }
		public List<JuegoCaracteristicasCurator> Franquicias { get; set; }
		public DateTime FechaLanzamientoSteam { get; set; }
		public DateTime FechaLanzamientoOriginal { get; set; }
		public List<string> Enlaces { get; set; }
		public bool AccesoAnticipado { get; set; }
		public JuegoCaracteristicasRealidadVirtual RealidadVirtual { get; set; }
	}

	public class JuegoCaracteristicasCurator
	{
		public int Id { get; set; }
		public string Nombre { get; set; }
	}

	public class JuegoCaracteristicasRealidadVirtual
	{
		public bool Vrhmd { get; set; }
		public bool VrhmdOnly { get; set; }
		public bool HtcVive { get; set; }
		public bool OculusRift { get; set; }
		public bool WindowsMr { get; set; }
		public bool ValveIndex { get; set; }
	}

	public class JuegoMedia
	{ 
		public List<JuegoMediaVideo> Videos { get; set; }
		public List<JuegoMediaCaptura> Capturas2 { get; set; }
	}

	public class JuegoMediaCaptura
	{
		public string Imagen { get; set; }
		public int Posicion { get; set; }
		public bool MayorEdad { get; set; }
	}

	public class JuegoMediaVideo
	{
		public string Nombre { get; set; }
		public string Enlace { get; set; }
		public string Captura { get; set; }
		public string CapturaPequeña { get; set; }
		public bool MayorEdad { get; set; }
		public string Micro { get; set; }
	}

	public class JuegoUsuariosInteresados
	{
		public string UsuarioId { get; set; }
		public JuegoDRM DRM { get; set; }
	}

	public class JuegoBundle
	{
		public int BundleId { get; set; }
		public BundleTipo Tipo { get; set; }
		public int JuegoId { get; set; }
		public string Nombre { get; set; }
		public string Imagen { get; set; }
		public string Enlace { get; set; }
		public DateTime FechaEmpieza { get; set; }
		public DateTime FechaTermina { get; set; }
		public JuegoDRM DRM { get; set; }
		public BundleTier Tier { get; set; }
	}

	public class JuegoGratis
	{
		public GratisTipo Tipo { get; set; }
		public int JuegoId { get; set; }
		public string Nombre { get; set; }
		public string Imagen { get; set; }
		public DateTime FechaEmpieza { get; set; }
		public DateTime FechaTermina { get; set; }
		public JuegoDRM DRM { get; set; }
		public string Enlace { get; set; }
		public string ImagenNoticia { get; set; }
		public Juego Juego { get; set; }
		public int Id {get; set;}
	}

	public class JuegoSuscripcion
	{
		public SuscripcionTipo Tipo { get; set; }
		public int JuegoId { get; set; }
		public string Nombre { get; set; }
		public string Imagen { get; set; }
		public DateTime FechaEmpieza { get; set; }
		public DateTime FechaTermina { get; set; }
		public JuegoDRM DRM { get; set; }
		public string Enlace { get; set; }
		public string ImagenNoticia { get; set; }
        public Juego Juego { get; set; }
        public int Id { get; set; }
    }

	public class JuegoHistorico
	{
		public DateTime Fecha { get; set; }
		public JuegoDRM DRM { get; set; }
		public decimal Precio { get; set; }
		public string Tienda { get; set; }
	}

    public class JuegoGalaxyGOG
    {
        public bool Windows { get; set; }
        public bool Mac { get; set; }
        public bool Linux { get; set; }
        public DateTime Fecha { get; set; }
        public bool Logros { get; set; }
        public bool GuardadoNube { get; set; }
		public bool Preservacion { get; set; }
		public DateTime FechaLanzamiento { get; set; }
	}

	public class JuegoEpicGames
	{
		public bool Windows { get; set; }
		public bool Mac { get; set; }
		public DateTime Fecha { get; set; }
		public bool Logros { get; set; }
		public bool GuardadoNube { get; set; }
	}

	public class JuegoXbox
	{
		public bool Windows { get; set; }
		public DateTime Fecha { get; set; }
		public bool Logros { get; set; }
		public bool GuardadoNube { get; set; }
		public bool Streaming { get; set; }
	}

	public class JuegoCantidadJugadoresSteam
	{
		public int Cantidad { get; set; }
		public DateTime Fecha { get; set; }
	}

	public class JuegoAnalisisAmpliado
	{
		public int CantidadPositivos { get; set; }
		public int CantidadNegativos { get; set; }
		public List<SteamAnalisisAPIAnalisis> Contenido { get; set; }
	}

	public class JuegoAnalisisAmpliadoIdioma
	{
		public int CantidadPositivos { get; set; }
		public int CantidadNegativos { get; set; }
		public string Idioma { get; set; }
	}

	//-------------------------------------------------------

	public enum JuegoTipo
	{
		Game,
		DLC,
		Bundle,
		Music,
		Software
	}

	public static class JuegoTipos
	{
		public static List<JuegoTipo> CargarListado()
		{
			List<JuegoTipo> tipos = Enum.GetValues(typeof(JuegoTipo)).Cast<JuegoTipo>().ToList();

			return tipos;
		}
	}

	//-------------------------------------------------------

	public enum JuegoDeck
	{
		Desconocido,		
		NoSoportado,
		Jugable,
		Verificado
	}

	public class JuegoDeckToken
	{
		public int Tipo { get; set; }
		public string Mensaje { get; set; }
	}

	public enum JuegoSteamOS
	{
		Desconocido,
		NoSoportado,
		Jugable,
		Verificado
	}

	//-------------------------------------------------------

	public class JuegoIdioma
	{
		public JuegoDRM DRM { get; set; }
		public string Idioma { get; set; }
		public bool Audio { get; set; }
		public bool Texto { get; set; }
	}

	//-------------------------------------------------------

	public class JuegoRazorUsuario
	{
		public JuegoDRM DRM { get; set; }
		public bool Tiene { get; set; }
		public bool Desea { get; set; }
		public bool ImportadoDesea { get; set; }
	}
}
