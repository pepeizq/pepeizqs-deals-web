#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using pepeizqs_deals_web.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

namespace pepeizqs_deals_web.Areas.Identity.Pages.Account.Manage
{
    public class EmailModel : PageModel
    {
        public string idioma = string.Empty;

        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IEmailSender _emailSender;

        public EmailModel(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "New email")]
            public string NewEmail { get; set; }
        }

        private async Task LoadAsync(Usuario user)
        {
            string correo = await _userManager.GetEmailAsync(user);
            Email = correo;

            Input = new InputModel
            {
                NewEmail = correo,
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();
            }
            catch { }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            Usuario usuario = await _userManager.GetUserAsync(User);

            if (usuario == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(usuario);
                return Page();
            }

            string correo = await _userManager.GetEmailAsync(usuario);

            if (Input.NewEmail != correo)
            {
                string usuarioId = await _userManager.GetUserIdAsync(usuario);
                string codigo = await _userManager.GenerateChangeEmailTokenAsync(usuario, Input.NewEmail);
                codigo = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(codigo));
                
                string enlaceFinal = Url.Page(
                    "/Account/ConfirmEmailChange",
                    pageHandler: null,
                    values: new { area = "Identity", userId = usuarioId, email = Input.NewEmail, code = codigo },
                    protocol: Request.Scheme);

                //await _emailSender.SendEmailAsync(
                //    Input.NewEmail,
                //    "Confirm your email",
                //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                Herramientas.Correos.EnviarCambioCorreo(HtmlEncoder.Default.Encode(enlaceFinal), correo);

                StatusMessage = Herramientas.Idiomas.CogerCadena(idioma, "Settings.String9");
                return RedirectToPage();
            }

            StatusMessage = Herramientas.Idiomas.CogerCadena(idioma, "Settings.String10");
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            Usuario usuario = await _userManager.GetUserAsync(User);

            if (usuario == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(usuario);
                return Page();
            }

            string usuarioId = await _userManager.GetUserIdAsync(usuario);
            string correo = await _userManager.GetEmailAsync(usuario);
            string codigo = await _userManager.GenerateEmailConfirmationTokenAsync(usuario);
            codigo = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(codigo));
            
            string enlaceFinal = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = usuarioId, code = codigo },
                protocol: Request.Scheme);

            Herramientas.Correos.EnviarConfirmacionCorreo(HtmlEncoder.Default.Encode(enlaceFinal), correo);

            StatusMessage = Herramientas.Idiomas.CogerCadena(idioma, "Settings.String8");
            return RedirectToPage();
        }
    }
}
