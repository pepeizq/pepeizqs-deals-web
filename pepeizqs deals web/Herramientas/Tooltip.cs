#nullable disable

using Juegos;

namespace Herramientas
{
    public static class Tooltip
    {
        public static ToolTipDatos Generar(string idioma, Juego juego, JuegoDRM drm, bool usuarioConectado, bool usuarioTieneJuego, bool usuarioDeseaJuego, int idBundleDescartar = 0)
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
				if (juego.Media.Videos != null)
				{
					if (juego.Media.Videos.Count > 0)
					{
						if (string.IsNullOrEmpty(juego.Media.Videos[0].Micro) == false)
						{
							datos.Video = juego.Media.Videos[0].Micro;
						}
					}
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
						datos.UsuarioMensaje = Idiomas.BuscarTexto(idioma, "String8", "Tooltip");
					}
					else
					{
						if (usuarioDeseaJuego == true)
						{
							datos.UsuarioMensaje = Idiomas.BuscarTexto(idioma, "String10", "Tooltip");
						}
						else
						{
							datos.UsuarioMensaje = Idiomas.BuscarTexto(idioma, "String9", "Tooltip");
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
				string bundleExtraActual = null;
				string bundleExtraPasado = null;

				foreach (var bundle in juego.Bundles)
				{
					bool contar = true;

					if (idBundleDescartar > 0 && bundle.BundleId == idBundleDescartar)
					{
						contar = false;
					}

					if (contar == true)
					{
						if (bundle.FechaEmpieza < DateTime.Now && bundle.FechaTermina > DateTime.Now)
						{
							bundlesActuales += 1;

							if (bundlesActuales == 1)
							{
								bundleExtraActual = Bundles2.BundlesCargar.DevolverBundle(bundle.Tipo).NombreTienda;
							}
							else if (bundlesActuales > 1)
							{
								if (Bundles2.BundlesCargar.DevolverBundle(bundle.Tipo).NombreTienda != bundleExtraActual)
								{
									bundleExtraActual = null;
								}
							}
						}
						else
						{
							bundlesPasados += 1;

							if (bundlesPasados == 1)
							{
								bundleExtraPasado = Bundles2.BundlesCargar.DevolverBundle(bundle.Tipo).NombreTienda;
							}
							else if (bundlesPasados > 1)
							{
								if (Bundles2.BundlesCargar.DevolverBundle(bundle.Tipo).NombreTienda != bundleExtraPasado)
								{
									bundleExtraPasado = null;
								}
							}
						}
					}
				}

                if (bundlesActuales == 1)
                {
                    datos.BundlesActuales = Herramientas.Idiomas.BuscarTexto(idioma, "String2", "Tooltip");
                }
                else if (bundlesActuales > 1)
                {
                    datos.BundlesActuales = string.Format(Herramientas.Idiomas.BuscarTexto(idioma, "String3", "Tooltip"), juego.Bundles.Count.ToString());
                }

				if (string.IsNullOrEmpty(bundleExtraActual) == false)
				{
					datos.BundlesActuales = datos.BundlesActuales + " (" + bundleExtraActual + ")";
				}

                if (bundlesPasados == 1)
				{
					datos.BundlesPasados = Herramientas.Idiomas.BuscarTexto(idioma, "String2", "Tooltip");
				}
				else if (bundlesPasados > 1)
				{
					datos.BundlesPasados = string.Format(Herramientas.Idiomas.BuscarTexto(idioma, "String3", "Tooltip"), juego.Bundles.Count.ToString());
				}

				if (string.IsNullOrEmpty(bundleExtraPasado) == false)
				{
					datos.BundlesPasados = datos.BundlesPasados + " (" + bundleExtraPasado + ")";
				}
			}

			if (juego.Gratis != null)
			{
				int gratisActuales = 0;
				int gratisPasados = 0;
				string gratisExtraActual = null;
				string gratisExtraPasado = null;

				foreach (var gratis in juego.Gratis)
				{
					if (gratis.FechaEmpieza < DateTime.Now && gratis.FechaTermina > DateTime.Now)
					{
						gratisActuales += 1;

						if (gratisActuales == 1)
						{
							gratisExtraActual = Gratis2.GratisCargar.DevolverGratis(gratis.Tipo).Nombre;
						}
						else if (gratisActuales > 1)
						{
							if (Gratis2.GratisCargar.DevolverGratis(gratis.Tipo).Nombre != gratisExtraActual)
							{
								gratisExtraActual = null;
							}
						}
					}
					else
					{
						gratisPasados += 1;

						if (gratisPasados == 1)
						{
							gratisExtraPasado = Gratis2.GratisCargar.DevolverGratis(gratis.Tipo).Nombre;
						}
						else if (gratisPasados > 1)
						{
							if (Gratis2.GratisCargar.DevolverGratis(gratis.Tipo).Nombre != gratisExtraPasado)
							{
								gratisExtraPasado = null;
							}
						}
					}
				}

				if (gratisActuales == 1)
				{
					datos.GratisActuales = Herramientas.Idiomas.BuscarTexto(idioma, "String4", "Tooltip");
				}
				else if (gratisActuales > 1)
				{
					datos.GratisActuales = string.Format(Herramientas.Idiomas.BuscarTexto(idioma, "String5", "Tooltip"), juego.Gratis.Count.ToString());
				}

				if (string.IsNullOrEmpty(gratisExtraActual) == false)
				{
					datos.GratisActuales = datos.GratisActuales + " (" + gratisExtraActual + ")";
				}

				if (gratisPasados == 1)
				{
					datos.GratisPasados = Herramientas.Idiomas.BuscarTexto(idioma, "String4", "Tooltip");
				}
				else if (gratisPasados > 1)
				{
					datos.GratisPasados = string.Format(Herramientas.Idiomas.BuscarTexto(idioma, "String5", "Tooltip"), juego.Gratis.Count.ToString());
				}

				if (string.IsNullOrEmpty(gratisExtraPasado) == false)
				{
					datos.GratisPasados = datos.GratisPasados + " (" + gratisExtraPasado + ")";
				}
			}

			if (juego.Suscripciones != null)
			{
				int suscripcionesActuales = 0;
				int suscripcionesPasados = 0;
				string suscripcionExtraActual = null;
				string suscripcionExtraPasada = null;

				foreach (JuegoSuscripcion suscripcion in juego.Suscripciones)
				{
					if (suscripcion != null)
					{
						bool contar = true;

						if (Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(suscripcion.Tipo) != null)
						{
							if (Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(suscripcion.Tipo).IncluyeSuscripcion != null)
							{
								foreach (JuegoSuscripcion suscripcion2 in juego.Suscripciones)
								{
									if (Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(suscripcion.Tipo).IncluyeSuscripcion == suscripcion2.Tipo)
									{
										contar = false;
										break;
									}
								}
							}

							if (contar == true)
							{
								if (suscripcion.FechaEmpieza < DateTime.Now && suscripcion.FechaTermina > DateTime.Now)
								{
									suscripcionesActuales += 1;

									if (suscripcionesActuales == 1)
									{
										suscripcionExtraActual = Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(suscripcion.Tipo).Nombre;
									}
									else if (suscripcionesActuales > 1)
									{
										if (Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(suscripcion.Tipo).Nombre != suscripcionExtraActual)
										{
											suscripcionExtraActual = null;
										}
									}
								}
								else
								{
									suscripcionesPasados += 1;

									if (suscripcionesPasados == 1)
									{
										suscripcionExtraPasada = Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(suscripcion.Tipo).Nombre;
									}
									else if (suscripcionesPasados > 1)
									{
										if (Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(suscripcion.Tipo).Nombre != suscripcionExtraPasada)
										{
											suscripcionExtraPasada = null;
										}
									}
								}
							}
						}					
					}
				}

				if (suscripcionesActuales == 1)
				{
					datos.SuscripcionesActuales = Herramientas.Idiomas.BuscarTexto(idioma, "String6", "Tooltip");
				}
				else if (suscripcionesActuales > 1)
				{
					datos.SuscripcionesActuales = string.Format(Herramientas.Idiomas.BuscarTexto(idioma, "String7", "Tooltip"), juego.Suscripciones.Count.ToString());
				}

				if (string.IsNullOrEmpty(suscripcionExtraActual) == false)
				{
					datos.SuscripcionesActuales = datos.SuscripcionesActuales + " (" + suscripcionExtraActual + ")";
				}

				if (suscripcionesPasados == 1)
				{
					datos.SuscripcionesPasadas = Herramientas.Idiomas.BuscarTexto(idioma, "String6", "Tooltip");
				}
				else if (suscripcionesPasados > 1)
				{
					datos.SuscripcionesPasadas = string.Format(Herramientas.Idiomas.BuscarTexto(idioma, "String7", "Tooltip"), juego.Suscripciones.Count.ToString());
				}

				if (string.IsNullOrEmpty(suscripcionExtraPasada) == false)
				{
					datos.SuscripcionesPasadas = datos.SuscripcionesPasadas + " (" + suscripcionExtraPasada + ")";
				}
			}

			return datos;
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
