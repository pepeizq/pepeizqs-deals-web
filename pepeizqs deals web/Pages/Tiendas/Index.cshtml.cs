#nullable disable

using Juegos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages.Tiendas
{
    public class IndexModel : PageModel
    {
		public string mensaje = string.Empty;

        public void OnGet()
        {
			string id = Request.Query["id"];

			if (id != null)
			{
				if (id.Length > 0)
				{
					if (id == APIs.Steam.Tienda.Generar().Id)
					{
						List<JuegoPrecio> ofertas = APIs.Steam.Tienda.BuscarOfertas().Result;

						mensaje = ofertas.Count.ToString();

						JuegoBaseDatos.ComprobarSteam(ofertas, mensaje);
					}
				}
			}
		}

		public IActionResult OnPost(string id) 
        {
			return RedirectToPage("./Index", new { id = id });
		}
	}
}
