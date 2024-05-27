#nullable disable

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages
{
    public class BundleModel : PageModel
    {
		public string idioma = string.Empty;

        public Bundles2.Bundle bundle = new Bundles2.Bundle();

		public string id = string.Empty;

        public void OnGet()
        {
            try
            {
                idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();
            }
            catch { }

            //id = Request.Query["id"].ToString();

            if (string.IsNullOrEmpty(id) == false)
            {
                bundle = BaseDatos.Bundles.Buscar.UnBundle(int.Parse(id));
            }
		}
    }

    public class BundleModel2
    {
        public int Id { get; set; }
    }
}
