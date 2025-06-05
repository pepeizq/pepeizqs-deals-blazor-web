#nullable disable

using Bundles2;
using Gratis2;
using Suscripciones2;

namespace Noticias
{
	public class Noticia
	{
		public int Id { get; set; }
		public NoticiaTipo Tipo { get; set; }
		public BundleTipo BundleTipo { get; set; }
		public GratisTipo GratisTipo { get; set; }
		public SuscripcionTipo SuscripcionTipo { get; set; }
		public string TituloEn { get; set; }
		public string TituloEs { get; set; }
		public string ContenidoEn { get; set; }
		public string ContenidoEs { get; set; }
		public string Imagen { get; set; }
		public string Enlace { get; set; }
		public string Juegos { get; set; }
		public DateTime FechaEmpieza { get; set; }
		public DateTime FechaTermina { get; set; }
		public int IdMaestra { get; set; }
		public int BundleId { get; set; }
		public string GratisIds { get; set; }
		public string SuscripcionesIds { get; set; }
	}

	public class NoticiaMostrar
	{
        public NoticiaTipo Tipo;
		public bool Mostrar;
    }
}
