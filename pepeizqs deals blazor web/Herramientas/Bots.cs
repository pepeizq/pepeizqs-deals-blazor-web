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
            "CloudFlare",
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
			string dominio = HttpContext.Request.Host.Value;

			StringBuilder sb = new StringBuilder();
			sb.Append(@"Sitemap: https://" + dominio + @"/sitemap.xml

User-agent: *
Disallow: /account/*
Disallow: /link/*
Disallow: /publisher/*
Disallow: /es/*
Disallow: /category/*
Disallow: /1*
Disallow: /2*
Disallow: /3*
Disallow: /4*
Disallow: /5*
Disallow: /6*
Disallow: /7*
Disallow: /8*
Disallow: /9*
Disallow: /*.apng$
Disallow: /*.avif$
Disallow: /*.gif$
Disallow: /*.jpg$
Disallow: /*.jpeg$
Disallow: /*.png$
Disallow: /*.svg$
Disallow: /*.webp$
Disallow: /*.bmp$
Disallow: /*.ico$
Disallow: /*.tiff$
Disallow: /_framework/*
Disallow: /_blazor/*
");

			//sb.Append("User-agent: *\r\nDisallow: /");

			return new ContentResult
			{
				Content = sb.ToString(),
				StatusCode = 200
			};
		}
	}
}
