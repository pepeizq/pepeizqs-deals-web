#nullable disable

using Juegos;
using pepeizqs_deals_web.Areas.Identity.Data;

namespace Herramientas
{
    public static class Tooltip
    {
        public static ToolTipDatos Generar(string idioma, Juego juego, JuegoDRM drm, bool usuarioConectado, bool usuarioTieneJuego, bool usuarioDeseaJuego)
        {
            ToolTipDatos datos = new ToolTipDatos
            {
                Nombre = juego.Nombre,
                Video = null,
                ReviewsIcono = null,
                ReviewsCantidad = null,
                UsuarioMensaje = null
            };

            if (juego.Media != null)
            {
                if (string.IsNullOrEmpty(juego.Media.Video) == false)
                {
                    int int1 = juego.Media.Video.LastIndexOf("/");
                    string temp1 = juego.Media.Video.Remove(int1, juego.Media.Video.Length - int1);

                    datos.Video = temp1 + "/microtrailer.webm";

                    datos.Video = datos.Video.Replace("cdn.akamai.steamstatic.com/", "cdn.cloudflare.steamstatic.com/");
                    datos.Video = datos.Video.Replace("http://", "https://");
                }
            }

            if (juego.Analisis != null)
            {
                if (string.IsNullOrEmpty(juego.Analisis.Porcentaje) == false && string.IsNullOrEmpty(juego.Analisis.Cantidad) == false)
                {
                    if (int.Parse(juego.Analisis.Porcentaje) > 74)
                    {
                        datos.ReviewsIcono = "/imagenes/analisis/positive.webp";
                    }
                    else if (int.Parse(juego.Analisis.Porcentaje) > 49 && int.Parse(juego.Analisis.Porcentaje) < 75)
                    {
                        datos.ReviewsIcono = "/imagenes/analisis/mixed.webp";
                    }
                    else if (int.Parse(juego.Analisis.Porcentaje) < 50)
                    {
                        datos.ReviewsIcono = "/imagenes/analisis/negative.webp";
                    }

                    datos.ReviewsCantidad = juego.Analisis.Porcentaje.ToString() + "% • " + Calculadora.RedondearAnalisis(idioma, juego.Analisis.Cantidad);
                }
            }

            if (usuarioConectado == true)
            {
				if (drm == JuegoDRM.Steam && juego.Tipo == JuegoTipo.Game)
				{
					if (usuarioTieneJuego == true)
					{
						datos.UsuarioMensaje = Idiomas.CogerCadena(idioma, "Tooltip.String1");
					}
					else
					{
						if (usuarioDeseaJuego == true)
						{
							datos.UsuarioMensaje = Idiomas.CogerCadena(idioma, "Tooltip.String3");
						}
						else
						{
							datos.UsuarioMensaje = Idiomas.CogerCadena(idioma, "Tooltip.String2");
						}
					}
				}
			}
            else
            {
                datos.UsuarioMensaje = null;
			}

            return datos;
        }

        public static bool ComprobarUsuarioTieneJuego(Usuario usuario, List<string> juegosUsuario, Juego juego, JuegoDRM drm)
        {
            if (usuario != null && juego != null && juegosUsuario.Count > 0 && drm == JuegoDRM.Steam)
            {
                if (juego.Tipo == JuegoTipo.Game)
                {
					foreach (var juegoUsuario in juegosUsuario)
					{
						if (juegoUsuario == juego.IdSteam.ToString())
						{
							return true;
						}
					}
				}
            }

            return false;
        }
    }

    public class ToolTipDatos
    {
        public string Nombre { get; set; }
        public string Video { get; set; }
		public string ReviewsIcono { get; set; }
		public string ReviewsCantidad { get; set; }
		public string UsuarioMensaje { get; set; }
	}
}
