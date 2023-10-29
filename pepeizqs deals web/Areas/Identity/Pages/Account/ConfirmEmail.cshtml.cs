#nullable disable

using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using pepeizqs_deals_web.Areas.Identity.Data;

namespace pepeizqs_deals_web.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        public string idioma = string.Empty;
        public string mensaje = string.Empty;

        private readonly UserManager<Usuario> _userManager;

        public ConfirmEmailModel(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string usuarioId, string codigo)
        {
            try
            {
                idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();
            }
            catch { }

            if (usuarioId == null || codigo == null)
            {
                return RedirectToPage("/Index");
            }

            Usuario usuario = await _userManager.FindByIdAsync(usuarioId);

            if (usuario == null)
            {
                return NotFound($"Unable to load user with ID '{usuarioId}'.");
            }

            codigo = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(codigo));

            IdentityResult resultado = await _userManager.ConfirmEmailAsync(usuario, codigo);

            if (resultado.Succeeded == true)
            {
                mensaje = Herramientas.Idiomas.CogerCadena(idioma, "Settings.String12");
            }
            else
            {
                mensaje = Herramientas.Idiomas.CogerCadena(idioma, "Settings.String13");
            }

            return Page();
        }
    }
}
