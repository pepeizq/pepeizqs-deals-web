﻿#nullable disable

using Microsoft.AspNetCore.Mvc;

namespace Herramientas.Redireccionador
{
	public class Extension : Controller
	{
		[ResponseCache(Duration = 6000)]
		[HttpGet("extension/steam2/{id}/{clave}/")]
		public IActionResult ExtensionSteam2(int id, string clave)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string claveExtension = builder.Configuration.GetValue<string>("Extension:Clave");

			if (clave == claveExtension)
			{
				global::BaseDatos.Extension.Extension juego = global::BaseDatos.Extension.Buscar.Steam2(id.ToString());

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
		[HttpGet("extension/gog2/{slug}/{clave}/")]
		public IActionResult ExtensionGog2(string slug, string clave)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string claveExtension = builder.Configuration.GetValue<string>("Extension:Clave");

			if (clave == claveExtension)
			{
				global::BaseDatos.Extension.Extension juego = global::BaseDatos.Extension.Buscar.Gog2(slug);

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
		[HttpGet("extension/epic2/{slug}/{clave}/")]
		public IActionResult ExtensionEpic2(string slug, string clave)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string claveExtension = builder.Configuration.GetValue<string>("Extension:Clave");

			if (clave == claveExtension)
			{
				global::BaseDatos.Extension.Extension juego = global::BaseDatos.Extension.Buscar.EpicGames2(slug);

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
