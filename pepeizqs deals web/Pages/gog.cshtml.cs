#nullable disable

using Juegos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages
{
	public class gogModel : PageModel
	{
		public string idioma = string.Empty;

		public Juego juego = JuegoCrear.Generar();

		[BindProperty(SupportsGet = true)]
		public string id { get; set; }

		[BindProperty(SupportsGet = true)]
		public string nombre { get; set; }

		public void OnGet()
		{
			idioma = Request.Query["language"];

			if (string.IsNullOrEmpty(idioma) == true)
			{
				try
				{
					idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();
				}
				catch { }
			}

			if (string.IsNullOrEmpty(id) == false)
			{
				juego = BaseDatos.Juegos.Buscar.UnJuego(null, null, id);
			}
		}
	}
}