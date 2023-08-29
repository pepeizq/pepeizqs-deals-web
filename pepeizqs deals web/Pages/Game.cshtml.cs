#nullable disable

using Juegos;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages
{
    public class GameModel : PageModel
    {
		public string idioma = string.Empty;

		public Juego juego = JuegoCrear.Generar();

		public void OnGet()
        {
			try
			{
				idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();
			}
			catch { }

			string id = Request.Query["id"];
			string idSteam = Request.Query["idSteam"];
			string idGog = Request.Query["idGog"];

			juego = BaseDatos.Juegos.Buscar.UnJuego(id, idSteam, idGog);
		}
    }
}
