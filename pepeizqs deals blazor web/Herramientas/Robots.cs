#nullable disable

using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Herramientas
{
	public static class RobotsUserAgents
	{
		public static List<string> bots = [
			"Applebot",
			"Applebot-Extended",
			"archive.org_bot",
			"ArchiveBot",
			"Baiduspider",
			"Barkrowler",
			"Bingbot",
			"BingPreview",
			"Chrome-Lighthouse",
			"Discordbot",
			"DuckAssistBot",
			"DuckDuckBot",
			"Ecosia",
			"Exabot",
			"feedburner",
			"fetcher",
			"Feedfetcher-Google",
			"Feedly",
			"FlipboardBot",
			"Googlebot",
			"Googlebot-Mobile",
			"Google-Extended",
			"Google-Safety",
			"GoogleAssociationService",
			"Google StoreBot",
			"Google Web Preview",
			"ia_archiver",
			"Lighthouse",
			"MojeekBot",
			"OpenWebSearchBot",
			"Owler",
			"Qwantify",
			"SeznamBot",
			"Slurp",
			"TelegramBot",
			"Twitterbot",
			"Valve Client",
			"Valve Steam",
			"YandexBot",
			"Yeti"
		];

		public static bool EsBotVerificado(string userAgent)
		{
			if (string.IsNullOrEmpty(userAgent) == false)
			{
				foreach (var bot in bots)
				{
					if (userAgent.ToLower().Contains(bot.ToLower()) == true)
					{
						return true;
					}
				}
			}			

			return false;
		}
	}

	public class Robots : Controller
	{
		[HttpGet("robots.txt")]
		public IActionResult Ejecutar()
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string piscinaApp = builder.Configuration.GetValue<string>("PoolWeb:Contenido");
			string piscinaUsada = Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);

			StringBuilder sb = new StringBuilder();

			if (piscinaApp == piscinaUsada)
			{
				sb.Append(@"
User-agent: *
Disallow: /");

				foreach (var bot in RobotsUserAgents.bots)
				{
					sb.Append($"\r\nUser-agent: {bot}\r\nDisallow: /account/\r\nDisallow: /link/*\r\nDisallow: /publisher/*");
				}

				sb.Append(@"
Sitemap: https://pepeizqdeals.com/sitemap.xml");
			}
            else
            {
				sb.Append("User-agent: *\r\nDisallow: /");
			}

            return new ContentResult
			{
				Content = sb.ToString(),
				StatusCode = 200
			};
		}
	}
}
