#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages.Admin.Tiendas
{
    public class IndexModel : PageModel
    {
		public void OnGet()
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
				}
			}
		}

		public IActionResult OnPost(string id) 
        {
			return RedirectToPage("./Index", new { id = id });
		}
	}
}
