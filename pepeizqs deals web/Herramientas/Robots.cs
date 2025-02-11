#nullable disable

using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Herramientas
{
	public static class RobotsUserAgents
	{
		private static List<string> botsBuenos = ["www.google.com/bot.html", "www.bing.com/bingbot.htm", 
			"duckduckgo.com/duckduckbot.html", "Valve Client", "Steam Client", "Valve/Steam", "archive.org"];

		public static bool EsBotVerificado(string userAgent)
		{
			if (string.IsNullOrEmpty(userAgent) == false)
			{
				foreach (var bot in botsBuenos)
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
Disallow: /

User-agent: Googlebot
Disallow: /account/
Disallow: /link/*
Disallow: /publisher/*
User-agent: Bingbot
Disallow: /account/
Disallow: /link/*
Disallow: /publisher/*
User-agent: DuckDuckBot
Disallow: /account/
Disallow: /link/*
Disallow: /publisher/*
User-agent: Chrome-Lighthouse
Disallow: /account/
Disallow: /link/*
Disallow: /publisher/*
User-agent: archive.org_bot
Disallow: /account/
Disallow: /link/*
Disallow: /publisher/*

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
