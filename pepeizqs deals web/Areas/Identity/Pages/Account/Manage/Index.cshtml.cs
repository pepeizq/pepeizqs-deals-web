﻿#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pepeizqs_deals_web.Areas.Identity.Data;
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
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Display(Name = "Steam Account")]
            public string SteamAccount { get; set; }
        }

        private async Task LoadAsync(Usuario usuario)
        {
            string userName = await _userManager.GetUserNameAsync(usuario);

            Usuario usuarioSteam = await _userManager.GetUserAsync(User);
            string cuentaSteam = usuarioSteam.SteamAccount;

            Input = new InputModel
            {
                Username = userName,
                SteamAccount = cuentaSteam
            };
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
                usuario.UserName = Input.Username;
                usuario.SteamAccount = Input.SteamAccount;

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