#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages
{
    public class BundleModel : PageModel
    {
		public string idioma = string.Empty;

        public Bundles2.Bundle bundle = new Bundles2.Bundle();

		[BindProperty(SupportsGet = true)]
		public int id { get; set; }

		[BindProperty(SupportsGet = true)]
		public string nombre { get; set; }

		public void OnGet()
        {
            try
            {
                idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();
            }
            catch { }

            if (id > 0)
            {
                bundle = BaseDatos.Bundles.Buscar.UnBundle(id);
			}
		}
    }
}
