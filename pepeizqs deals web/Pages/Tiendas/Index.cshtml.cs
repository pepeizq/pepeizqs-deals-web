#nullable disable

using Juegos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages.Tiendas
{
    public class IndexModel : PageModel
    {
        public async Task OnGetAsync()
        {
			string id = Request.Query["id"];

			if (id != null)
			{
				if (id.Length > 0)
				{
					if (id == APIs.Steam.Tienda.Generar().Id)
					{
						await APIs.Steam.Tienda.BuscarOfertas();

						//JuegoBaseDatos.LimpiarJuegos();

						//mensaje = ofertas.Count.ToString();

						//JuegoBaseDatos.ComprobarSteam(ofertas);
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
