#nullable disable

using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace pepeizqs_deals_web.Areas.Identity.Data
{
    public static class UsuarioCoger
    {
        public static async Task<string> Nickname(UserManager<Usuario> userManager, ClaimsPrincipal user) 
        {
            Usuario usuario = await userManager.GetUserAsync(user);

            string nombre = usuario.Email;

            if (usuario.Nickname != null) 
            { 
                nombre = usuario.Nickname;
            }

            return nombre;
        }

        public static async Task<bool> RolDios(UserManager<Usuario> userManager, ClaimsPrincipal user)
        {
            bool god = false;

            Usuario usuario = await userManager.GetUserAsync(user);

            string rol = usuario.Role;

            if (rol.ToLower() == "god")
            {
                god = true;
            }

            return god;
        }
    }
}
