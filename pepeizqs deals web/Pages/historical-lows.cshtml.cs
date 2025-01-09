#nullable disable

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages
{
    public class HistoricalLowModel : PageModel
    {
        public string idioma = string.Empty;
        public string drm = string.Empty;
		public string tienda = string.Empty;

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

			drm = Request.Query["drm"];
			tienda = Request.Query["store"];
        }
    }
}
