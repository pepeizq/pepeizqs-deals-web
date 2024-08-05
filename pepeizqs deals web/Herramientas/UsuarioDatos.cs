#nullable disable

using APIs.Steam;
using Microsoft.AspNetCore.Identity;
using pepeizqs_deals_web.Areas.Identity.Data;
using System.Security.Claims;

namespace Herramientas
{
    public static class UsuarioDatos
    {
        public static async Task<Usuario> Actualizar(ClaimsPrincipal User, Usuario usuario, UserManager<Usuario> UserManager)
        {
			usuario = UserManager.GetUserAsync(User).Result;

            if (usuario != null)
            {
                if (usuario.EmailConfirmed == true && string.IsNullOrEmpty(usuario.SteamAccount) == false && string.IsNullOrEmpty(usuario.SteamAccountLastCheck) == false)
                {
                    bool tiempo = true;

                    if (string.IsNullOrEmpty(usuario.SteamAccountLastCheck) == false)
                    {
                        if (Convert.ToDateTime(usuario.SteamAccountLastCheck) + TimeSpan.FromDays(7) > DateTime.Now)
                        {
                            tiempo = false;
                        }
                    }

                    if (tiempo == true)
                    {
                        SteamUsuario datos = await APIs.Steam.Cuenta.CargarDatos(usuario.SteamAccount);

                        usuario.SteamGames = datos.Juegos;
                        usuario.SteamWishlist = datos.Deseados;
                        usuario.Avatar = datos.Avatar;
                        usuario.Nickname = datos.Nombre;
                        usuario.SteamAccountLastCheck = DateTime.Now.ToString();
                        usuario.OfficialGroup = datos.GrupoPremium;
                        usuario.OfficialGroup2 = datos.GrupoNormal;                     
                    }
                    else
                    {
						if (string.IsNullOrEmpty(usuario.RewardsLastLogin) == true)
						{
							if (Listados.Generar(usuario.SteamGames).Count() > 50)
							{
								usuario.RewardsLastLogin = DateTime.Now.ToString();
								usuario.RewardsCoins = 1;

								global::BaseDatos.Recompensas.Historial.Insertar(usuario.Id, 1, "Daily", DateTime.Now);
							}
						}
						else
						{
							if (Convert.ToDateTime(usuario.RewardsLastLogin).DayOfYear != DateTime.Now.DayOfYear)
							{
								if (Listados.Generar(usuario.SteamGames).Count() > 50)
								{
									usuario.RewardsLastLogin = DateTime.Now.ToString();
									usuario.RewardsCoins = usuario.RewardsCoins + 1;

									global::BaseDatos.Recompensas.Historial.Insertar(usuario.Id, 1, "Daily", DateTime.Now);
								}
							}
						}
					}
					       
                    try
                    {
                        await UserManager.UpdateAsync(usuario);
                    }
                    catch { }
                }
            }

            return usuario;
        }
    }
}
