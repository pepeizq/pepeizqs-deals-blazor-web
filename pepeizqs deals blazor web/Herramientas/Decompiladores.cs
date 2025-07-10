﻿#nullable disable

using System.IO.Compression;
using System.Net;
using System.Text;

namespace Herramientas
{

	public interface IDecompiladores
    {
        Task<string> Estandar(string enlace);
    }

	public class Decompiladores2 : IDecompiladores
	{
        private readonly IHttpClientFactory fabrica;

        public Decompiladores2(IHttpClientFactory _fabrica)
		{
			fabrica = _fabrica;
		}

		public async Task<string> Estandar(string enlace)
		{
            HttpClient cliente = fabrica.CreateClient();
		
            string contenido = string.Empty;

            cliente.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/114.0");

            try
            {
                HttpResponseMessage respuesta = await cliente.GetAsync(enlace);
                
				if (respuesta.IsSuccessStatusCode == true)
				{
                    contenido = await respuesta.Content.ReadAsStringAsync();
                }
				
                respuesta.Dispose();
            }
            catch { }

            return contenido;
        }
	}

    public static class Decompiladores
    {
		//private static readonly HttpClient cliente = new HttpClient(new SocketsHttpHandler
		//{
		//	AutomaticDecompression = DecompressionMethods.GZip,
		//	PooledConnectionLifetime = TimeSpan.FromMinutes(15),
		//	PooledConnectionIdleTimeout = TimeSpan.FromMinutes(10),
		//	MaxConnectionsPerServer = 2
		//}, false);

		public static async Task<string> Estandar(string enlace)
        {
			ServiceProvider servicio = new ServiceCollection().AddHttpClient().BuildServiceProvider();
			IHttpClientFactory factoria = servicio.GetService<IHttpClientFactory>() ?? throw new InvalidOperationException();
			HttpClient cliente = factoria.CreateClient("Decompilador");

			using (cliente)
			{
				cliente.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/114.0");

				try
				{
					using (HttpResponseMessage respuesta = await cliente.GetAsync(enlace, HttpCompletionOption.ResponseContentRead))
					{
						return await respuesta.Content.ReadAsStringAsync();
					}
				}
				catch (Exception ex) 
				{
					global::BaseDatos.Errores.Insertar.Mensaje("Decompilador", ex);
				}
			}

			cliente.Dispose();

			return null;
        }

		public static async Task<string> GZipFormato(string enlace) 
        {
            await Task.Yield();

			HttpRequestMessage mensaje = new HttpRequestMessage();
			mensaje.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue() { NoCache = true, NoStore = true };
			mensaje.Headers.Pragma.ParseAdd("no-cache");
			mensaje.RequestUri = new Uri(enlace);
			mensaje.Headers.Accept.ParseAdd("application/json, text/plain, */*");
			mensaje.Headers.AcceptEncoding.ParseAdd("gzip, deflate, br, zstd");
			mensaje.Headers.AcceptLanguage.ParseAdd("es-ES,es;q=0.8,en-US;q=0.5,en;q=0.3");
			mensaje.Headers.Connection.ParseAdd("keep-alive");
			mensaje.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Linux; Android 10; Generic Android-x86_64 Build/QD1A.190821.014.C2; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/79.0.3945.36 Safari/537.36");
			mensaje.Headers.Add("Sec-Fetch-Dest", "empty");
			mensaje.Headers.Add("Sec-Fetch-Mode", "cors");
			mensaje.Headers.Add("Sec-Fetch-Site", "same-origin");
			mensaje.Headers.TE.ParseAdd("trailers");

			CookieContainer cookieContainer = new CookieContainer();

			if (enlace.Contains("https://2game.com") == true)
			{
				cookieContainer.Add(new Uri(enlace), new Cookie("store", "en_es"));
			}
			
			using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer, UseCookies = true, AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate })
			{
				using (HttpClient cliente = new HttpClient(handler) { BaseAddress = new Uri(enlace) })
				{
					cliente.DefaultRequestHeaders.Accept.Clear();

					using (HttpResponseMessage respuesta = await cliente.GetAsync(enlace, HttpCompletionOption.ResponseContentRead))
					{
						Stream stream = await respuesta.Content.ReadAsStreamAsync();

						using (GZipStream descompresion = new GZipStream(stream, CompressionMode.Decompress, false))
						{
							using (StreamReader lector = new StreamReader(stream, Encoding.UTF8))
							{
								return await lector.ReadToEndAsync();
							}
						}
					}				
				}
			}
		}

		public static async Task<string> GZipFormato2(string enlace)
		{
			HttpRequestMessage mensaje = new HttpRequestMessage();
			mensaje.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue() { NoCache = true, NoStore = true };
			mensaje.Headers.Pragma.ParseAdd("no-cache");
			mensaje.RequestUri = new Uri(enlace);
			mensaje.Headers.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
			mensaje.Headers.AcceptEncoding.ParseAdd("gzip, deflate, br");
			mensaje.Headers.AcceptLanguage.ParseAdd("es,en-US;q=0.7,en;q=0.3");
			mensaje.Headers.Connection.ParseAdd("keep-alive");
			mensaje.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Linux; Android 10; Generic Android-x86_64 Build/QD1A.190821.014.C2; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/79.0.3945.36 Safari/537.36");

			CookieContainer cookieContainer = new CookieContainer();
			
			using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer, UseCookies = true, AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate })
			{
				using (HttpClient cliente = new HttpClient(handler) { BaseAddress = new Uri(enlace) })
				{
					HttpResponseMessage respuesta = await cliente.SendAsync(mensaje);

					Stream stream = await respuesta.Content.ReadAsStreamAsync();

					using (GZipStream descompresion = new GZipStream(stream, CompressionMode.Decompress, false))
					{
						using (StreamReader lector = new StreamReader(stream, Encoding.UTF8))
						{
							return await lector.ReadToEndAsync();
						}
					}
				}
			}
		}

		public static async Task<string> NoSeguro(string enlace)
		{
			HttpRequestMessage mensaje = new HttpRequestMessage();
			mensaje.RequestUri = new Uri(enlace);
			mensaje.Headers.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
			mensaje.Headers.AcceptEncoding.ParseAdd("gzip, deflate, br");
			mensaje.Headers.AcceptLanguage.ParseAdd("es,en-US;q=0.7,en;q=0.3");
			mensaje.Headers.Connection.ParseAdd("keep-alive");
			mensaje.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Linux; Android 10; Generic Android-x86_64 Build/QD1A.190821.014.C2; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/79.0.3945.36 Safari/537.36");

			CookieContainer cookieContainer = new CookieContainer();

			var handler = new HttpClientHandler();
			handler.ClientCertificateOptions = ClientCertificateOption.Manual;
			handler.ServerCertificateCustomValidationCallback =
				(mensaje, cert, cetChain, policyErrors) =>
				{
					return true;
				};

			using (handler)
			{
				using (HttpClient cliente = new HttpClient(handler) { BaseAddress = new Uri(enlace) })
				{
					HttpResponseMessage respuesta = await cliente.SendAsync(mensaje);

					Stream stream = await respuesta.Content.ReadAsStreamAsync();

					using (GZipStream descompresion = new GZipStream(stream, CompressionMode.Decompress))
					{
						using (StreamReader lector = new StreamReader(stream))
						{
							return await lector.ReadToEndAsync();
						}
					}
				}
			}
		}
	}
}
