#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages
{
    public class curatorModel : PageModel
    {
		public BaseDatos.Curators.Curator curator = new BaseDatos.Curators.Curator();

		public string idioma = string.Empty;

		[BindProperty(SupportsGet = true)]
		public string id { get; set; }

		public void OnGet()
        {
			idioma = Request.Query["language"];

			if (string.IsNullOrEmpty(idioma) == true)
			{
				try
				{
					idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();
				}
				catch { }
			}

			if (string.IsNullOrEmpty(id) == false)
			{
				curator = BaseDatos.Curators.Buscar.Uno(id);
			}
		}
    }
}
