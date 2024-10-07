#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace pepeizqs_deals_web.Pages.Admin.Tiendas
{
    public class HumbleModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Texto { get; set; }
        }

		private readonly IHttpContextAccessor _contexto;

		public HumbleModel(IHttpContextAccessor contexto)
		{
			_contexto = contexto;
		}

		public void OnGet()
        {

        }

        public void OnPost() 
        {
			string id = _contexto.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (BaseDatos.Usuarios.Buscar.RolDios(id) == true)
            {
				if (Input.Texto != null)
				{
					APIs.Humble.Tienda.RecopilarOfertas(Input.Texto);
				}
			}			
        }
    }
}
