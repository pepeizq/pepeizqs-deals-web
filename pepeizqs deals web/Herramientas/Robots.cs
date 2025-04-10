#nullable disable

using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Herramientas
{
	public static class RobotsUserAgents
	{
		public static List<string> bots = ["Googlebot", 
			"Bingbot",
			"YandexBot", 
			"Applebot", 
			"Twitterbot", 
			"DuckDuckBot", 
			"Baiduspider", 
			"Slurp",
			"Yeti",
			"Exabot",
			"archive.org_bot",
			"Valve Client",
			"Valve Steam"];

		public static bool EsBotVerificado(string userAgent)
		{
			if (string.IsNullOrEmpty(userAgent) == false)
			{
				foreach (var bot in bots)
				{
					if (userAgent.Contains(bot) == true)
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
