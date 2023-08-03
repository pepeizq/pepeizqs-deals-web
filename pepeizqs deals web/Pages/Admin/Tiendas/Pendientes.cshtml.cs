#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages.Admin.Tiendas
{
    public class PendientesModel : PageModel
    {
		private readonly IHttpContextAccessor _contexto;

		public PendientesModel(IHttpContextAccessor contexto)
		{
			_contexto = contexto;
		}

		public void OnGet()
		{
			string nombre = _contexto.HttpContext.User.Identity.Name;

			if (BaseDatos.Usuarios.Buscar.RolDios(nombre) == true)
			{
			}
		}
    }
}
