#nullable disable

using Microsoft.AspNetCore.Mvc;

namespace Herramientas.Redireccionador
{
	public class Enlace : Controller
	{
		[HttpGet("link/{id}/")]
		public IActionResult CogerAcortador(string id)
		{
			return Redirect(EnlaceAcortador.AlargarEnlace(id));
		}
	}
}
