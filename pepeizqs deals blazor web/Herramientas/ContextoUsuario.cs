#nullable disable

namespace Herramientas
{

	public static class ContextoUsuario
	{
		public static ContextoUsuarioDatos Leer(IHttpContextAccessor contexto)
		{
			ContextoUsuarioDatos datos = new ContextoUsuarioDatos();

            datos.Idioma = contexto?.HttpContext?.Request?.Headers["Accept-Language"].ToString().Split(";")?.FirstOrDefault()?.Split(",").FirstOrDefault() ?? "en";
            datos.UsuarioId = contexto?.HttpContext?.User?.FindFirst(u => u.Type.Contains("nameidentifier"))?.Value;
            datos.UserAgent = contexto?.HttpContext?.Request?.Headers?.UserAgent.ToString();
			datos.Dominio = contexto?.HttpContext?.Request?.Host.Value;

			return datos;
        }
	}

    public class ContextoUsuarioDatos
	{
		public string Idioma {  get; set; }
        public string UsuarioId { get; set; }
        public string UserAgent { get; set; }
		public string Dominio { get; set; }
	}
}
