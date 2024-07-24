#nullable disable

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages.Account
{
    public class ForgotPasswordModel : PageModel
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
