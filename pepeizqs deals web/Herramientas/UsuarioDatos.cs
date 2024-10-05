#nullable disable

using APIs.Steam;
using Microsoft.AspNetCore.Identity;
using pepeizqs_deals_web.Areas.Identity.Data;
using System.Security.Claims;

namespace Herramientas
{
    public static class UsuarioDatos
    {
        public static async Task<Usuario> Actualizar(ClaimsPrincipal contexto, Usuario usuario, UserManager<Usuario> UserManager, string idioma)
        {
            

            return usuario;
        }
    }
}
