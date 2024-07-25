#nullable disable

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace pepeizqs_deals_web.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        public string idioma = string.Empty;
        public string codigo = string.Empty;

        public void OnGet()
        {
            try
            {
                idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();

                codigo = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Request.Query["code"]));
            }
            catch { }
        }
    }
}
