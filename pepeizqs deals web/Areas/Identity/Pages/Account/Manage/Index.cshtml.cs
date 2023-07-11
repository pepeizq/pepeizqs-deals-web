#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pepeizqs_deals_web.Areas.Identity.Data;
using Steam;
using System.ComponentModel.DataAnnotations;

namespace pepeizqs_deals_web.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public IndexModel(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Nickname")]
            public string Nickname { get; set; }

            [Display(Name = "Steam Account")]
            [DataType(DataType.Url)]
            public string SteamAccount { get; set; }
        }

        private Task LoadAsync(Usuario usuario)
		{
			string nickName = usuario.Nickname;
            string cuentaSteam = usuario.SteamAccount;

            Input = new InputModel
            {
                Nickname = nickName,
                SteamAccount = cuentaSteam
            };

            SteamJuegosyDeseados datos = new SteamJuegosyDeseados
            {
                juegos = usuario.SteamGames,
                deseados = usuario.SteamWishlist
            };

            ViewData["SteamData"] = Cuenta.Mensaje(datos);

			return Task.CompletedTask;
		}

		public async Task<IActionResult> OnGetAsync()
        {
            Usuario usuario = await _userManager.GetUserAsync(User);

            if (usuario == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(usuario);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
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

            try
            {
                usuario.Nickname = Input.Nickname;
                usuario.SteamAccount = Input.SteamAccount;

                SteamJuegosyDeseados datos = await Cuenta.CargarDatos(usuario.SteamAccount);
                usuario.SteamGames = datos.juegos;
                usuario.SteamWishlist = datos.deseados;

                ViewData["SteamData"] = Cuenta.Mensaje(datos);

                await _userManager.UpdateAsync(usuario);
            }
            catch (Exception ex) 
            {
                StatusMessage = ex.Message;
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(usuario);

            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
