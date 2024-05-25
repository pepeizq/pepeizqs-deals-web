#nullable disable

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages
{
    public class HistoricalLowModel : PageModel
    {
        public string idioma = string.Empty;
        public string modo = string.Empty;

        public void OnGet()
        {
            try
            {
                idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();
            }
            catch { }

			modo = Request.Query["mode"];
        }
    }
}
