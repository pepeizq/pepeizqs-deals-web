#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tiendas2;

namespace pepeizqs_deals_web.Pages.Tiendas
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
