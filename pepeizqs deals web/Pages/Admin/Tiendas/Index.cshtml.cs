#nullable disable

using Herramientas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages.Admin.Tiendas
{
    public class IndexModel : PageModel
    {
		private readonly IHttpContextAccessor _contexto;

		public IndexModel(IHttpContextAccessor contexto)
		{
			_contexto = contexto;
		}

		public void OnGet()
		{
			string nombre = _contexto.HttpContext.User.Identity.Name;

			if (BaseDatos.Usuarios.Buscar.RolDios(nombre) == true)
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
						else if (id == APIs.Gamesplanet.Tienda.GenerarFr().Id)
						{
							APIs.Gamesplanet.Tienda.BuscarOfertasFr(ViewData);
						}
						else if (id == APIs.Gamesplanet.Tienda.GenerarDe().Id)
						{
							APIs.Gamesplanet.Tienda.BuscarOfertasDe(ViewData);
						}
						else if (id == APIs.Gamesplanet.Tienda.GenerarUs().Id)
						{
							APIs.Gamesplanet.Tienda.BuscarOfertasUs(ViewData);
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
			}
		}

		public IActionResult OnPost(string id) 
        {
			return RedirectToPage("./Index", new { id = id });
		}
	}
}
