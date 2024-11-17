#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages.Admin.Tiendas
{
    public class HumbleModel : PageModel
    {
		[BindProperty]
		public InputModel Input { get; set; }

		public class InputModel
		{
			public string contenido { get; set; }
		}

		public void OnGet()
        {

        }

		public void OnPost()
		{
			if (Input.contenido != null)
			{
				APIs.Humble.Tienda.RecopilarOfertas(Input.contenido);
			}
		}
	}
}
