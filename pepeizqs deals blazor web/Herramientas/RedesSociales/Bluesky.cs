#nullable disable

using X.Bluesky;
using X.Bluesky.Models;

namespace Herramientas.RedesSociales
{
	public static class Bluesky
	{
		public static async Task<bool> Postear(Noticias.Noticia noticia)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();

			string correo = builder.Configuration.GetValue<string>("Bluesky:Correo");
			string contraseña = builder.Configuration.GetValue<string>("Bluesky:Contraseña");

			if (string.IsNullOrEmpty(correo) == false && string.IsNullOrEmpty(contraseña) == false)
			{
				IBlueskyClient cliente = new BlueskyClient(correo, contraseña);
				
				string enlace = string.Empty;

				if (noticia.Id == 0)
				{
					enlace = "/link/news/" + noticia.IdMaestra.ToString() + "/";
				}
				else
				{
					enlace = "/link/news/" + noticia.Id.ToString() + "/";
				}

				if (string.IsNullOrEmpty(enlace) == false)
				{
					if (enlace.Contains("https://pepeizqdeals.com") == false)
					{
						enlace = "https://pepeizqdeals.com" + enlace;
					}
				}

				Uri enlaceFinal = new Uri(enlace);

				bool error = false;

				try
				{
					string tituloEn = noticia.TituloEn;
					tituloEn = tituloEn.Replace("'", null);

					string imagenEnlace = noticia.Imagen;
					HttpClient clienteWeb = new HttpClient();

					using (HttpResponseMessage respuesta = await clienteWeb.GetAsync(imagenEnlace, HttpCompletionOption.ResponseContentRead))
					{
						byte[] imageBytes = await respuesta.Content.ReadAsByteArrayAsync();

						if (imageBytes != null)
						{
							if (imageBytes.Length > 0)
							{
								Image imagen = new Image
								{
									Content = imageBytes,
									Alt = tituloEn,
									MimeType = "image/jpeg"
								};

								var post = new Post
								{
									Text = tituloEn + Environment.NewLine + Environment.NewLine + enlaceFinal,
									Images = new[]
									{
										imagen
									},
									Url = enlaceFinal,
									Languages = new[] { "en" },
									GenerateCardForUrl = true  
								};

								await cliente.Post(post);
						
								return true;
							}
						}

						error = true;
					}
				}
				catch (Exception ex)
				{
					global::BaseDatos.Errores.Insertar.Mensaje("Bluesky enviar imagen", ex);
					error = true;
				}

				if (error == true)
				{
					try
					{
						await cliente.Post(noticia.TituloEn + Environment.NewLine + Environment.NewLine + enlaceFinal);
						return true;
					}
					catch (Exception ex)
					{
						global::BaseDatos.Errores.Insertar.Mensaje("Bluesky enviar texto", ex);
						return false;
					}
				}
			}

			return false;
		}
	}
}
