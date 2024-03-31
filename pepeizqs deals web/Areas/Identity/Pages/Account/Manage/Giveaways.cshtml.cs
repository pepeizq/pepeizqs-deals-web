#nullable disable

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Areas.Identity.Pages.Account.Manage
{
    public class GiveawaysModel : PageModel
    {
        public string idioma = string.Empty;

        public void OnGet()
        {
            try
            {
                idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();
            }
            catch { }
        }
    }
}
