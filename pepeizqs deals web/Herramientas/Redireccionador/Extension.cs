#nullable disable

using Microsoft.AspNetCore.Mvc;

namespace Herramientas.Redireccionador
{
	public class Extension : Controller
	{
		[ResponseCache(Duration = 6000)]
		[HttpGet("extension/steam/{id}/{clave}/")]
		public IActionResult ExtensionSteam(int id, string clave)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string claveExtension = builder.Configuration.GetValue<string>("Extension:Clave");

			if (clave == claveExtension)
			{
				global::BaseDatos.Extension.Extension juego = global::BaseDatos.Extension.Buscar.Steam(id.ToString());

				if (juego != null)
				{
					if (juego.Id > 0)
					{
						return Ok(juego);
					}
				}
			}

			return Redirect("~/");
		}

		[ResponseCache(Duration = 6000)]
		[HttpGet("extension/gog/{slug}/{clave}/")]
		public IActionResult ExtensionGog(string slug, string clave)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string claveExtension = builder.Configuration.GetValue<string>("Extension:Clave");

			if (clave == claveExtension)
			{
				global::BaseDatos.Extension.Extension juego = global::BaseDatos.Extension.Buscar.Gog(slug);

				if (juego != null)
				{
					if (juego.Id > 0)
					{
						return Ok(juego);
					}
				}
			}

			return Redirect("~/");
		}

		[ResponseCache(Duration = 6000)]
		[HttpGet("extension/epic/{slug}/{clave}/")]
		public IActionResult ExtensionEpic(string slug, string clave)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string claveExtension = builder.Configuration.GetValue<string>("Extension:Clave");

			if (clave == claveExtension)
			{
				global::BaseDatos.Extension.Extension juego = global::BaseDatos.Extension.Buscar.EpicGames(slug);

				if (juego != null)
				{
					if (juego.Id > 0)
					{
						return Ok(juego);
					}
				}
			}

			return Redirect("~/");
		}
	}
}
