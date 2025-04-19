#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pepeizqs_deals_web.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace pepeizqs_deals_web.Pages.Account
{
	public class LoginModel : PageModel
	{
		public string idioma = string.Empty;

		public string mensaje = string.Empty;

		private readonly SignInManager<Usuario> _signInManager;
		private readonly ILogger<LoginModel> _logger;

		public LoginModel(SignInManager<Usuario> signInManager, ILogger<LoginModel> logger)
		{
			_signInManager = signInManager;
			_logger = logger;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public string ReturnUrl { get; set; }

		[TempData]
		public string ErrorMessage { get; set; }

		public class InputModel
		{
			[Required]
			[DataType(DataType.EmailAddress)]
			public string Email { get; set; }

			[Required]
			[DataType(DataType.Password)]
			public string Password { get; set; }

			public bool RememberMe { get; set; }
		}

		public async Task OnGetAsync(string returnUrl = null)
		{
			try
			{
				idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();
			}
			catch { }

			if (!string.IsNullOrEmpty(ErrorMessage))
			{
				ModelState.AddModelError(string.Empty, ErrorMessage);
			}

			returnUrl ??= Url.Content("~/");

			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			ReturnUrl = returnUrl;
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl ??= Url.Content("~/");

			if (ModelState.IsValid == true)
			{
				Microsoft.AspNetCore.Identity.SignInResult resultado = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

				if (resultado.Succeeded == true)
				{
					return Redirect("https://pepeizqdeals.com/");

					//if (string.IsNullOrEmpty(returnUrl) == false)
					//{
					//	if (returnUrl.Contains("/account/login") == true)
					//	{
					//		return RedirectToPage("./");
					//	}
					//	else
					//	{
					//		return LocalRedirect(returnUrl);
					//	}
					//}
					//else
					//{
					//	return RedirectToPage("./");
					//}
				}

				if (resultado.RequiresTwoFactor)
				{
					return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, Input.RememberMe });
				}

				if (resultado.IsLockedOut)
				{
					return RedirectToPage("./Lockout");
				}
				else
				{
					ModelState.AddModelError(string.Empty, resultado.ToString());
					return Page();
				}
			}

			return Page();
		}
	}
}
