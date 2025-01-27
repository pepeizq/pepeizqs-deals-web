#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using pepeizqs_deals_web.Areas.Identity.Data;

namespace pepeizqs_deals_web.Pages.Account
{
    public class RegisterModel : PageModel
    {
        public string idioma = string.Empty;

        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly IUserStore<Usuario> _userStore;
        private readonly IUserEmailStore<Usuario> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public string errorMensaje = string.Empty;

        public RegisterModel(UserManager<Usuario> userManager, IUserStore<Usuario> userStore, SignInManager<Usuario> signInManager,
            ILogger<RegisterModel> logger, IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            try
            {
                idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();
            }
            catch { }

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string token, string returnUrl = null)
        {
			returnUrl ??= Url.Content("~/");
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

			if (ModelState.IsValid == true)
			{
				Usuario usuario = CrearUsuario();

				usuario.Role = "Peasent";

				await _userStore.SetUserNameAsync(usuario, Input.Email, CancellationToken.None);
				await _emailStore.SetEmailAsync(usuario, Input.Email, CancellationToken.None);

				try
				{
					IdentityResult resultado = await _userManager.CreateAsync(usuario, Input.Password);

					if (resultado.Succeeded)
					{
						_logger.LogInformation("User created a new account with password.");

						string usuarioId = await _userManager.GetUserIdAsync(usuario);
						string codigo = await _userManager.GenerateEmailConfirmationTokenAsync(usuario);
						codigo = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(codigo));
						var callbackUrl = Url.Page(
							"/Account/ConfirmEmail",
							pageHandler: null,
							values: new { area = "Identity", userId = usuarioId, code = codigo, returnUrl },
							protocol: Request.Scheme);

						await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
							$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

						if (_userManager.Options.SignIn.RequireConfirmedAccount)
						{
							return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });
						}
						else
						{
							await _signInManager.SignInAsync(usuario, isPersistent: false);
							//return LocalRedirect(returnUrl);
							return Redirect("~/account");
						}
					}

					foreach (var error in resultado.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
				catch (Exception ex)
				{
					errorMensaje = ex.Message;
				}
			}

			return Page();
        }

        private Usuario CrearUsuario()
        {
            try
            {
                return Activator.CreateInstance<Usuario>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Usuario)}'. " +
                    $"Ensure that '{nameof(Usuario)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<Usuario> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<Usuario>)_userStore;
        }
    }
}
