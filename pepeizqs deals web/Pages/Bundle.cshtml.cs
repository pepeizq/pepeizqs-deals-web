#nullable disable

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages
{
    public class BundleModel : PageModel
    {
		public string idioma = string.Empty;

		public Bundles2.Bundle bundle = new Bundles2.Bundle();

		public void OnGet()
        {
			try
			{
				idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();
			}
			catch { }

			string id = Request.Query["id"];

			if (id != null)
			{
				bundle = BaseDatos.Bundles.Buscar.UnBundle(int.Parse(id));
			}
		}
	}
}
