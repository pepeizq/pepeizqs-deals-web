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
    }
}
