#nullable disable

using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Herramientas
{
	public static class RobotsUserAgents
	{
		public static bool Autorizar(string userAgent)
		{
			if (userAgent.Contains("http://www.google.com/bot.html") == true || 
				userAgent.Contains("http://www.bing.com/bingbot.htm") == true ||
				userAgent.Contains("http://duckduckgo.com/duckduckbot.html") == true)
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
				sb.Append(@"User-agent: AhrefsBot 
							User-agent: BlackWidow
							User-agent: ChinaClaw 
							User-agent: Custo 
							User-agent: DISCo 
							User-agent: Dotbot 
							User-agent: Download\ Demon 
							User-agent: eCatch 
							User-agent: EirGrabber 
							User-agent: EmailSiphon 
							User-agent: EmailWolf 
							User-agent: Exabot 
							User-agent: Express\ WebPictures 
							User-agent: ExtractorPro 
							User-agent: EyeNetIE 
							User-agent: facebook 
							User-agent: facebookexternalhit 
							User-agent: FlashGet 
							User-agent: GetRight 
							User-agent: GetWeb! 
							User-agent: Gigabot
							User-agent: Go!Zilla 
							User-agent: Go-Ahead-Got-It 
							User-agent: Go-http-client 
							User-agent: GrabNet 
							User-agent: Grafula 
							User-agent: HMView 
							User-agent: HTTrack 
							User-agent: Image\ Stripper 
							User-agent: Image\ Sucker 
							User-agent: Indy\ Library
							User-agent: InterGET 
							User-agent: Internet\ Ninja 
							User-agent: JetCar 
							User-agent: JOC\ Web\ Spider 
							User-agent: larbin 
							User-agent: LeechFTP 
							User-agent: Mass\ Downloader 
							User-agent: MIDown\ tool 
							User-agent: Mister\ PiX 
							User-agent: MJ12bot 
							User-agent: Navroad 
							User-agent: NearSite 
							User-agent: NetAnts 
							User-agent: NetSpider 
							User-agent: Net\ Vampire 
							User-agent: NetZIP 
							User-agent: Octopus 
							User-agent: Offline\ Explorer 
							User-agent: Offline\ Navigator 
							User-agent: PageGrabber 
							User-agent: Papa\ Foto 
							User-agent: pavuk 
							User-agent: pcBrowser 
							User-agent: RealDownload 
							User-agent: ReGet 
							User-agent: Rogerbot 
							User-agent: SemrushBot  
							User-agent: SiteSnagger 
							User-agent: SmartDownload 
							User-agent: SuperBot 
							User-agent: SuperHTTP 
							User-agent: Surfbot 
							User-agent: tAkeOut 
							User-agent: Teleport\ Pro 
							User-agent: VoidEYE 
							User-agent: Web\ Image\ Collector 
							User-agent: Web\ Sucker 
							User-agent: WebAuto 
							User-agent: WebCopier 
							User-agent: WebFetch 
							User-agent: WebGo\ IS 
							User-agent: WebLeacher 
							User-agent: WebReaper 
							User-agent: WebSauger 
							User-agent: Website\ eXtractor 
							User-agent: Website\ Quester 
							User-agent: WebStripper 
							User-agent: WebWhacker 
							User-agent: WebZIP 
							User-agent: Wget 
							User-agent: Widow 
							User-agent: WWWOFFLE 
							User-agent: Xaldon\ WebSpider 
							User-agent: Zeus

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
