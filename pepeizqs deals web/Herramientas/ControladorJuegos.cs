#nullable disable

using Juegos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Herramientas
{
	public class ControladorJuegos : Controller
	{
		[ResponseCache(Duration = 2000)]
		[HttpGet("game/{id}")]
		public IActionResult CogerJuegoId(int Id)
		{
			return Redirect("~/game?id=" + Id.ToString());
		}

		[ResponseCache(Duration = 2000)]
		[HttpGet("steam/{id}")]
		public IActionResult CogerJuegoIdSteam(int Id)
		{
			return Redirect("~/game?idSteam=" + Id.ToString());
		}

		[ResponseCache(Duration = 2000)]
		[HttpGet("api/{id}")]
		public IActionResult CogerApiId(int Id)
		{
			Juego juego = global::BaseDatos.Juegos.Buscar.UnJuego(Id.ToString());

			if (juego != null)
			{
				return Content(JsonConvert.SerializeObject(juego));
			}
			else
			{
				return null;
			}
		}

		[ResponseCache(Duration = 2000)]
		[HttpGet("link/{id}")]
		public IActionResult CogerAcortador(int Id)
		{
			Enlace enlace = global::BaseDatos.Enlaces.Buscar.Id(Id.ToString());

			if (enlace != null) 
			{
				return Redirect(enlace.Base);
			}
			else
			{
				return Redirect("~/");
			}			
		}

		[ResponseCache(Duration = 2000)]
		[HttpGet("news/{id}")]
		public IActionResult CogerNoticiaId(int Id)
		{
			return Redirect("~/news?id=" + Id.ToString());
		}

		[ResponseCache(Duration = 2000)]
		[HttpGet("bundle/{id}")]
		public IActionResult CogerBundleId(int Id)
		{
			return Redirect("~/bundle?id=" + Id.ToString());
		}
	}
}
