#nullable disable

using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Herramientas
{
	public static class RobotsUserAgents
	{
		public static List<string> bots = [
			"AhrefsBot",
			"Applebot",
			"Applebot-Extended",
			"archive.org_bot",
			"ArchiveBot",
			"Baiduspider",
			"Barkrowler",
			"Bingbot",
			"BingPreview",
			"bot",
			"CCBot",
			"Chrome-Lighthouse",
			"Discordbot",
			"DotBot",
			"DuckAssistBot",
			"DuckDuckBot",
			"Ecosia",
			"Exabot",
			"facebook",
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
			"link",
			"MJ12bot",
			"MojeekBot",
			"nbot",
			"OpenWebSearchBot",
			"Owler",
			"Qwantify",
			"SeznamBot",
			"sift",
			"Slurp",
			"spider",
			"TelegramBot",
			"Twitterbot",
			"Valve Client",
			"Valve Steam",
			"YandexBot",
			"Yeti",
			"zoominfobot"
		];

		public static bool EsBot(string userAgent)
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
			else
			{
				return true;
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
				//				sb.Append(@"
				//User-agent: *
				//Disallow: /");

				//				foreach (var bot in RobotsUserAgents.bots)
				//				{
				//					sb.Append($"\r\nUser-agent: {bot}\r\nDisallow: /account/\r\nDisallow: /link/*\r\nDisallow: /publisher/*");
				//				}

				sb.Append(@"Sitemap: https://pepeizqdeals.com/sitemap.xml");
				sb.Append($"\r\n\r\nUser-agent: *\r\nDisallow: /account/\r\nDisallow: /link/*\r\nDisallow: /publisher/*\\r\nDisallow: /es/*");
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
