#nullable disable

using Herramientas;
using Juegos;

namespace Bundles2
{
	public class Bundle
	{
		public int Id { get; set; }
		public BundleTipo Tipo { get; set; }
		public string NombreBundle { get; set; }
		public string NombreTienda { get; set; }
		public string ImagenBundle { get; set; }
		public string ImagenTienda { get; set; }
		public string ImagenIcono { get; set; }
		public string ImagenNoticia { get; set; }
		public List<string> ImagenesExtra { get; set; }
		public string Enlace { get; set; }
		public string EnlaceBase { get; set; }
		public DateTime FechaEmpieza { get; set; }
		public DateTime FechaTermina { get; set; }
		public List<BundleJuego> Juegos { get; set; }
		public List<BundleTier> Tiers { get; set; }
		public bool Pick { get; set; }
        public string Twitter { get; set; }
    }

	public class BundleTier
	{
		public int Posicion;
		public string Precio;
		public JuegoMoneda Moneda;
		public int CantidadJuegos;
	}

	public class BundleJuego
	{
		public string JuegoId;
		public string Nombre;
		public string Imagen;
		public BundleTier Tier;
		public JuegoDRM DRM;
		public Juego Juego;
		public List<string> DLCs;
	}
}
