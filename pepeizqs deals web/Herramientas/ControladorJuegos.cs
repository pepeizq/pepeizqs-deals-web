using Microsoft.AspNetCore.Mvc;

namespace Herramientas
{
	public class ControladorJuegos : Controller
	{
		
		[HttpGet("game/{id}")]
		public IActionResult CogerJuegoId(int Id)
		{
			return Redirect("~/Game?id=" + Id.ToString());
		}
	}
}
