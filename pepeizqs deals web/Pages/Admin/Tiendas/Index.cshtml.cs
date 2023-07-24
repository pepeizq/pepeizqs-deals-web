#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tiendas2;

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
						TiendasBaseDatos.ActualizarTiempo(APIs.Steam.Tienda.Generar().Id, DateTime.Now);
						APIs.Steam.Tienda.BuscarOfertas(ViewData);

						//JuegoBaseDatos.LimpiarJuegos();
					}
					else if (id == APIs.GamersGate.Tienda.Generar().Id) 
					{
						TiendasBaseDatos.ActualizarTiempo(APIs.GamersGate.Tienda.Generar().Id, DateTime.Now);
						APIs.GamersGate.Tienda.BuscarOfertas(ViewData);
					}
					else if (id == APIs.Humble.Tienda.Generar().Id)
					{
						TiendasBaseDatos.ActualizarTiempo(APIs.Humble.Tienda.Generar().Id, DateTime.Now);
						APIs.Humble.Tienda.BuscarOfertas(ViewData);
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
