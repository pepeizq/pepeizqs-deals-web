#nullable disable

using Herramientas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages.Admin.Tiendas
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            string id = Request.Query["id"];
			string tienda = Request.Query["tienda"];

			if (id != null)
			{
				if (id.Length > 0)
				{
					if (id == APIs.Steam.Tienda.Generar().Id)
					{
						APIs.Steam.Tienda.BuscarOfertas(ViewData);
					}
					else if (id == APIs.GamersGate.Tienda.Generar().Id) 
					{						
						APIs.GamersGate.Tienda.BuscarOfertas(ViewData);
					}
					else if (id == APIs.Gamesplanet.Tienda.GenerarUk().Id)
					{
                        APIs.Gamesplanet.Tienda.BuscarOfertasUk(ViewData);
					}

					if (id == "divisas")
					{
						Divisas.Ejecutar();
					}

					///Admin/Tiendas?id=

					if (id == "limpiar")
					{
						BaseDatos.Juegos.Precios.Limpiar(tienda);
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
