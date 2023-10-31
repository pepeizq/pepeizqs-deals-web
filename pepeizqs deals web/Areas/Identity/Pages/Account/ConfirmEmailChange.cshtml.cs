﻿#nullable disable

using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using pepeizqs_deals_web.Areas.Identity.Data;

namespace pepeizqs_deals_web.Areas.Identity.Pages.Account
{
    public class ConfirmEmailChangeModel : PageModel
    {
        public string idioma = string.Empty;
        public IdentityResult resultado = null;

        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public ConfirmEmailChangeModel(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();
            }
            catch { }

            string usuarioId = Request.Query["userId"];
            string nuevoCorreo = Request.Query["email"];
            string codigo = Request.Query["code"];

            if (usuarioId == null || nuevoCorreo == null || codigo == null)
            {
                return RedirectToPage("/Index");
            }

            Usuario usuario = await _userManager.FindByIdAsync(usuarioId);

            if (usuario == null)
            {
                return NotFound($"Unable to load user with ID '{usuarioId}'.");
            }

            codigo = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(codigo));

            var result = await _userManager.ChangeEmailAsync(usuario, nuevoCorreo, codigo);

            if (!result.Succeeded)
            {
                //StatusMessage = "Error changing email.";
                return Page();
            }

            var setUserNameResult = await _userManager.SetUserNameAsync(usuario, nuevoCorreo);
            if (!setUserNameResult.Succeeded)
            {
                //StatusMessage = "Error changing user name.";
                return Page();
            }

            await _signInManager.RefreshSignInAsync(usuario);

            return Page();
        }
    }
}
