#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tiendas2;

namespace pepeizqs_deals_web.Pages.Admin.Tiendas
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
			string id = Request.Query["id"];

			if (id != null)
			{
				if (id.Length > 0)
				{
					if (id == APIs.Steam.Tienda.Generar().Id)
					{
						APIs.Steam.Tienda.BuscarOfertas(ViewData);

						//JuegoBaseDatos.LimpiarJuegos();
					}
					else if (id == APIs.GamersGate.Tienda.Generar().Id) 
					{						
						APIs.GamersGate.Tienda.BuscarOfertas(ViewData);
					}
					else if (id == APIs.Humble.Tienda.Generar().Id)
					{
						//APIs.Humble.Tienda.BuscarOfertas(ViewData);
					}
				}
			}

			return null;
		}

		public IActionResult OnPost(string id) 
        {
			return RedirectToPage("./Index", new { id = id });
		}
	}
}
