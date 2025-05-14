#nullable disable

using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Herramientas
{
	public static class RobotsUserAgents
	{
		public static List<string> bots = ["Googlebot",
			"Google-Safety",
			"GoogleAssociationService",
			"fetcher",
			"Feedfetcher-Google",
			"Bingbot",
			"YandexBot", 
			"Applebot", 
			"Twitterbot", 
			"DuckDuckBot",
			"DuckAssistBot",
			"Baiduspider", 
			"Slurp",
			"Yeti",
			"Exabot",
			"archive.org_bot",
			"Valve Client",
			"Valve Steam",
			"MojeekBot",
			"OpenWebSearchBot",
			"Qwantify",
			"Barkrowler",
			"TelegramBot",
			"Discordbot",
			"Lighthouse",
			"Chrome-Lighthouse",
			"SeznamBot",
			"Ecosia"];

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
