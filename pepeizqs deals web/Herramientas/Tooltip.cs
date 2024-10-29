#nullable disable

using Juegos;

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
                UsuarioMensaje = null,
				BundlesActuales = null,
                BundlesPasados = null,
				GratisActuales = null,
                GratisPasados = null,
				SuscripcionesActuales = null,
                SuscripcionesPasadas = null
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
						datos.UsuarioMensaje = Idiomas.CogerCadena(idioma, "String8", "Tooltip");
					}
					else
					{
						if (usuarioDeseaJuego == true)
						{
							datos.UsuarioMensaje = Idiomas.CogerCadena(idioma, "String10", "Tooltip");
						}
						else
						{
							datos.UsuarioMensaje = Idiomas.CogerCadena(idioma, "String9", "Tooltip");
						}
					}
				}
			}
            else
            {
                datos.UsuarioMensaje = null;
			}

			if (juego.Bundles != null)
			{
				int bundlesActuales = 0;
                int bundlesPasados = 0;

                foreach (var bundle in juego.Bundles)
				{
					if (bundle.FechaEmpieza < DateTime.Now && bundle.FechaTermina > DateTime.Now)
					{
						bundlesActuales += 1;
					}
					else
					{
                        bundlesPasados += 1;
                    }
				}

                if (bundlesActuales == 1)
                {
                    datos.BundlesActuales = Herramientas.Idiomas.CogerCadena(idioma, "String2", "Tooltip");
                }
                else if (bundlesActuales > 1)
                {
                    datos.BundlesActuales = string.Format(Herramientas.Idiomas.CogerCadena(idioma, "String3", "Tooltip"), juego.Bundles.Count.ToString());
                }

                if (bundlesPasados == 1)
				{
					datos.BundlesPasados = Herramientas.Idiomas.CogerCadena(idioma, "String2", "Tooltip");
				}
				else if (bundlesPasados > 1)
				{
					datos.BundlesPasados = string.Format(Herramientas.Idiomas.CogerCadena(idioma, "String3", "Tooltip"), juego.Bundles.Count.ToString());
				}
			}

			if (juego.Gratis != null)
			{
				int gratisActuales = 0;
				int gratisPasados = 0;

				foreach (var gratis in juego.Gratis)
				{
					if (gratis.FechaEmpieza < DateTime.Now && gratis.FechaTermina > DateTime.Now)
					{
						gratisActuales += 1;
					}
					else
					{
						gratisPasados += 1;
					}
				}

				if (gratisActuales == 1)
				{
					datos.GratisActuales = Herramientas.Idiomas.CogerCadena(idioma, "String4", "Tooltip");
				}
				else if (gratisActuales > 1)
				{
					datos.GratisActuales = string.Format(Herramientas.Idiomas.CogerCadena(idioma, "String5", "Tooltip"), juego.Gratis.Count.ToString());
				}

				if (gratisPasados == 1)
				{
					datos.GratisPasados = Herramientas.Idiomas.CogerCadena(idioma, "String4", "Tooltip");
				}
				else if (gratisPasados > 1)
				{
					datos.GratisPasados = string.Format(Herramientas.Idiomas.CogerCadena(idioma, "String5", "Tooltip"), juego.Gratis.Count.ToString());
				}
			}

			if (juego.Suscripciones != null)
			{
				int suscripcionesActuales = 0;
				int suscripcionesPasados = 0;

				foreach (var suscripcion in juego.Suscripciones)
				{
					if (suscripcion.FechaEmpieza < DateTime.Now && suscripcion.FechaTermina > DateTime.Now)
					{
						suscripcionesActuales += 1;
					}
					else
					{
						suscripcionesPasados += 1;
					}
				}

				if (suscripcionesActuales == 1)
				{
					datos.SuscripcionesActuales = Herramientas.Idiomas.CogerCadena(idioma, "String6", "Tooltip");
				}
				else if (suscripcionesActuales > 1)
				{
					datos.SuscripcionesActuales = string.Format(Herramientas.Idiomas.CogerCadena(idioma, "String7", "Tooltip"), juego.Suscripciones.Count.ToString());
				}

				if (suscripcionesPasados == 1)
				{
					datos.SuscripcionesPasadas = Herramientas.Idiomas.CogerCadena(idioma, "String6", "Tooltip");
				}
				else if (suscripcionesPasados > 1)
				{
					datos.SuscripcionesPasadas = string.Format(Herramientas.Idiomas.CogerCadena(idioma, "String7", "Tooltip"), juego.Suscripciones.Count.ToString());
				}
			}

			return datos;
        }

        public static bool ComprobarUsuarioTieneJuego(List<string> juegosUsuario, Juego juego, JuegoDRM drm)
        {
			if (juego != null)
			{
				if (juegosUsuario != null)
				{
					if (juegosUsuario.Count > 0)
					{
						if (juego.Tipo == JuegoTipo.Game && drm == JuegoDRM.Steam)
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
        public string BundlesActuales { get; set; }
        public string GratisActuales { get; set; }
        public string SuscripcionesActuales { get; set; }
        public string BundlesPasados { get; set; }
		public string GratisPasados { get; set; }
		public string SuscripcionesPasadas { get; set; }
	}
}
