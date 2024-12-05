#nullable disable

using Juegos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages
{
    public class GameModel : PageModel
    {
		public string idioma = string.Empty;

		public Juego juego = JuegoCrear.Generar();

		[BindProperty(SupportsGet = true)]
		public int id { get; set; }

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

			if (id > 0)
			{
				juego = BaseDatos.Juegos.Buscar.UnJuego(id);
			}			
		}
	}
}
