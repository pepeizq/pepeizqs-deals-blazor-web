#nullable disable

using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Herramientas
{
	public static class Bots
	{
		public static List<string> botsUserAgents = [
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
			"PerplexityBot",
			"Perplexity-User",
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

		public static List<string> botsIps = [
			"5.35.",
			"8.210.",
			"20.163.",
			"43.130.",
			"43.131.",
			"43.157.",
			"47.76.", 
			"47.79.", 
			"47.82.", 
			"47.239.",
			"47.242.", 
			"47.243.",
			"49.36.",
			"58.69.",
			"81.177.",
			"89.163.",
			"91.186.",
			"103.120.",
			"103.207.",
			"113.172.",
			"119.13.",
			"126.209.",
			"146.174.",
			"178.156.",
			"184.72.",
			"185.201.",
			"188.239.",
			"190.80.",
			"202.76."
		];

		public static bool UserAgentEsBot(string userAgent)
		{
			if (string.IsNullOrEmpty(userAgent) == false)
			{
				foreach (var bot in botsUserAgents)
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

	public class BotsControlador : Controller
	{
		[HttpGet("robots.txt")]
		public IActionResult Robots()
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string piscinaApp = builder.Configuration.GetValue<string>("PoolWeb:Contenido");
			string piscinaUsada = Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);

			StringBuilder sb = new StringBuilder();

			if (piscinaApp == piscinaUsada)
			{
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
