#nullable disable

using Juegos;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages
{
    public class GameModel : PageModel
    {
		public Juego juego = JuegoCrear.Generar();

		public void OnGet()
        {
			string id = Request.Query["id"];
			string idSteam = Request.Query["idSteam"];
			string idGog = Request.Query["idGog"];

			juego = BaseDatos.Juegos.Buscar.UnJuego(id, idSteam, idGog);
		}
    }
}
