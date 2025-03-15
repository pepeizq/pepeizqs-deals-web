#nullable disable

using Gratis2;
using Herramientas;
using Microsoft.VisualBasic;

namespace Noticias
{
    public static class Plantillas
    {
        public static Plantilla Bundles(int bundleId)
        {
            Plantilla plantilla = new Plantilla();

            Bundles2.Bundle bundle = BaseDatos.Bundles.Buscar.UnBundle(bundleId);

            if (bundle != null)
            {
                if (bundle.Id > 0)
                {
					plantilla.BundleId = bundleId.ToString();
					plantilla.Juegos = null;

					for (int i = 0; i < bundle.Juegos.Count; i += 1)
					{
						if (i == 0)
						{
							plantilla.Juegos = bundle.Juegos[i].JuegoId;
						}
						else
						{
							plantilla.Juegos = plantilla.Juegos + "," + bundle.Juegos[i].JuegoId;
						}
					}

					plantilla.Fecha = bundle.FechaTermina;
					//plantilla.Enlace = EnlaceAcortador.Generar(bundle.Enlace, bundle.Tipo);

					#region Titulo

					if (bundle.NombreBundle.ToLower().Contains("bundle") == false)
					{
						plantilla.TituloEn = string.Format(Idiomas.BuscarTexto("en", "Bundle1", "NewsTemplates"), bundle.NombreBundle, bundle.NombreTienda);
						plantilla.TituloEs = string.Format(Idiomas.BuscarTexto("es", "Bundle1", "NewsTemplates"), bundle.NombreBundle, bundle.NombreTienda);
					}
					else
					{
						plantilla.TituloEn = string.Format(Idiomas.BuscarTexto("en", "Bundle5", "NewsTemplates"), bundle.NombreBundle, bundle.NombreTienda);
						plantilla.TituloEs = string.Format(Idiomas.BuscarTexto("es", "Bundle5", "NewsTemplates"), bundle.NombreBundle, bundle.NombreTienda);
					}

					#endregion

					#region Contenido

					if (bundle.Pick == false)
					{
						plantilla.ContenidoEn = "<div style=" + Strings.ChrW(34) + "margin-bottom: 15px;" + Strings.ChrW(34) + ">" + string.Format(Idiomas.BuscarTexto("en", "Bundle2", "NewsTemplates"), bundle.NombreTienda) + "</div>";
						plantilla.ContenidoEs = "<div style=" + Strings.ChrW(34) + "margin-bottom: 15px;" + Strings.ChrW(34) + ">" + string.Format(Idiomas.BuscarTexto("es", "Bundle2", "NewsTemplates"), bundle.NombreTienda) + "</div>";

						foreach (var tier in bundle.Tiers.OrderBy(x => x.Posicion))
						{
							string precio = tier.Precio;
							precio = precio.Replace(".", ",");
							precio = precio + "€";

							plantilla.ContenidoEn = plantilla.ContenidoEn + "<div>Tier " + tier.Posicion + " (" + precio + "):<ul>";
							plantilla.ContenidoEs = plantilla.ContenidoEs + "<div>Tier " + tier.Posicion + " (" + precio + "):<ul>";

							foreach (var juego in bundle.Juegos)
							{
								if (juego.Tier.Posicion == tier.Posicion)
								{
									plantilla.ContenidoEn = plantilla.ContenidoEn + "<li>" + juego.Nombre + " (" + Juegos.JuegoDRM2.DevolverDRM(juego.DRM) + ")</li>";
									plantilla.ContenidoEs = plantilla.ContenidoEs + "<li>" + juego.Nombre + " (" + Juegos.JuegoDRM2.DevolverDRM(juego.DRM) + ")</li>";
								}
							}

							plantilla.ContenidoEn = plantilla.ContenidoEn + "</ul></div>";
							plantilla.ContenidoEs = plantilla.ContenidoEs + "</ul></div>";
						}
					}
					else
					{
						plantilla.ContenidoEn = "<div style=" + Strings.ChrW(34) + "margin-bottom: 15px;" + Strings.ChrW(34) + ">" + string.Format(Idiomas.BuscarTexto("en", "Bundle3", "NewsTemplates"), bundle.NombreTienda) + "</div>";
						plantilla.ContenidoEs = "<div style=" + Strings.ChrW(34) + "margin-bottom: 15px;" + Strings.ChrW(34) + ">" + string.Format(Idiomas.BuscarTexto("es", "Bundle3", "NewsTemplates"), bundle.NombreTienda) + "</div>";

						foreach (var tier in bundle.Tiers)
						{
							string precio = tier.Precio;
							precio = precio.Replace(".", ",");
							precio = precio + "€";

							plantilla.ContenidoEn = plantilla.ContenidoEn + "<div style=" + Strings.ChrW(34) + "margin-bottom: 15px;" + Strings.ChrW(34) + ">" + tier.CantidadJuegos.ToString() + " " + Idiomas.BuscarTexto("en", "Bundle4", "NewsTemplates") + " (" + precio + ")</div>";
							plantilla.ContenidoEs = plantilla.ContenidoEs + "<div style=" + Strings.ChrW(34) + "margin-bottom: 15px;" + Strings.ChrW(34) + ">" + tier.CantidadJuegos.ToString() + " " + Idiomas.BuscarTexto("es", "Bundle4", "NewsTemplates") + " (" + precio + ")</div>";
						}

						plantilla.ContenidoEn = plantilla.ContenidoEn + "<div><ul>";
						plantilla.ContenidoEs = plantilla.ContenidoEs + "<div><ul>";

						foreach (var juego in bundle.Juegos)
						{
							plantilla.ContenidoEn = plantilla.ContenidoEn + "<li>" + juego.Nombre + " (" + Juegos.JuegoDRM2.DevolverDRM(juego.DRM) + ")</li>";
							plantilla.ContenidoEs = plantilla.ContenidoEs + "<li>" + juego.Nombre + " (" + Juegos.JuegoDRM2.DevolverDRM(juego.DRM) + ")</li>";
						}

						plantilla.ContenidoEn = plantilla.ContenidoEn + "</ul></div>";
						plantilla.ContenidoEs = plantilla.ContenidoEs + "</ul></div>";
					}

					#endregion

					#region Imagen

					if (bundle.ImagenNoticia != null)
					{
						plantilla.Imagen = bundle.ImagenNoticia;
					}

					#endregion
				}
			}

            return plantilla;
        }

        public static Plantilla Gratis(Plantilla plantilla, int juegoId, int id, string tipoSeleccionado)
        {
            if (string.IsNullOrEmpty(plantilla.Juegos) == true)
            {
                plantilla.Juegos = juegoId.ToString();
            }
            else
            {
                if (plantilla.Juegos.Contains(juegoId.ToString()) == false)
                {
                    plantilla.Juegos = plantilla.Juegos + "," + juegoId.ToString();
                }
                else
                {
                    int int1 = plantilla.Juegos.IndexOf(juegoId.ToString() + ",");

                    if (int1 == -1)
                    {
                        int1 = plantilla.Juegos.IndexOf(juegoId.ToString());
                        plantilla.Juegos = plantilla.Juegos.Remove(int1, juegoId.ToString().Length);
                    }
                    else
                    {
                        plantilla.Juegos = plantilla.Juegos.Remove(int1, juegoId.ToString().Length + 1);
                    }

                    if (plantilla.Juegos.Trim().Length == 1)
                    {
                        plantilla.Juegos = null;
                    }
                }
            }

            List<string> lista = Listados.Generar(plantilla.Juegos);

            if (lista != null)
            {
                if (lista.Count > 0)
                {
					Juegos.JuegoGratis juegoGratis1 = BaseDatos.Gratis.Buscar.UnJuego(lista[0]);

					if (juegoGratis1.Juego == null)
					{
						juegoGratis1.Juego = BaseDatos.Juegos.Buscar.UnJuego(juegoGratis1.JuegoId);
					}

					if (lista.Count == 1)
					{
						plantilla.TituloEn = juegoGratis1.Nombre + " " + Idiomas.BuscarTexto("en", "Free1", "NewsTemplates") + " " + GratisCargar.DevolverGratis(tipoSeleccionado).Nombre;
						plantilla.TituloEs = juegoGratis1.Nombre + " " + Idiomas.BuscarTexto("es", "Free1", "NewsTemplates") + " " + GratisCargar.DevolverGratis(tipoSeleccionado).Nombre;

						if (juegoGratis1.Juego.Tipo == Juegos.JuegoTipo.Game)
						{
							plantilla.ContenidoEn = GratisCargar.DevolverGratis(tipoSeleccionado).Nombre + " " + Idiomas.BuscarTexto("en", "Free5", "NewsTemplates");
							plantilla.ContenidoEs = GratisCargar.DevolverGratis(tipoSeleccionado).Nombre + " " + Idiomas.BuscarTexto("es", "Free5", "NewsTemplates");
						}
						else if (juegoGratis1.Juego.Tipo == Juegos.JuegoTipo.DLC)
						{
							plantilla.ContenidoEn = GratisCargar.DevolverGratis(tipoSeleccionado).Nombre + " " + Idiomas.BuscarTexto("en", "Free7", "NewsTemplates");
							plantilla.ContenidoEs = GratisCargar.DevolverGratis(tipoSeleccionado).Nombre + " " + Idiomas.BuscarTexto("es", "Free7", "NewsTemplates");
						}
					}
					else if (lista.Count > 1)
					{
						Juegos.JuegoGratis juegoGratis2 = BaseDatos.Gratis.Buscar.UnJuego(lista[1]);

						if (juegoGratis2.Juego == null)
						{
							juegoGratis2.Juego = BaseDatos.Juegos.Buscar.UnJuego(juegoGratis1.JuegoId);
						}

						if (lista.Count == 2)
						{
							plantilla.TituloEn = juegoGratis1.Nombre + " " + Idiomas.BuscarTexto("en", "Free2", "NewsTemplates") + " " +
								juegoGratis2.Nombre + " " + Idiomas.BuscarTexto("en", "Free3", "NewsTemplates") + " " + GratisCargar.DevolverGratis(tipoSeleccionado).Nombre;
							plantilla.TituloEs = juegoGratis1.Nombre + " " + Idiomas.BuscarTexto("es", "Free2", "NewsTemplates") + " " +
								juegoGratis2.Nombre + " " + Idiomas.BuscarTexto("es", "Free3", "NewsTemplates") + " " + GratisCargar.DevolverGratis(tipoSeleccionado).Nombre;
						}
						else if (lista.Count > 2)
						{
							if (juegoGratis1.Juego.Tipo == Juegos.JuegoTipo.Game && juegoGratis2.Juego.Tipo == Juegos.JuegoTipo.Game)
							{
								plantilla.TituloEn = juegoGratis1.Nombre + ", " + juegoGratis2.Nombre + " " +
									Idiomas.BuscarTexto("en", "Free4", "NewsTemplates") + " " + GratisCargar.DevolverGratis(tipoSeleccionado).Nombre;
								plantilla.TituloEs = juegoGratis1.Nombre + ", " + juegoGratis2.Nombre + " " +
									Idiomas.BuscarTexto("es", "Free4", "NewsTemplates") + " " + GratisCargar.DevolverGratis(tipoSeleccionado).Nombre;
							}
							else if (juegoGratis1.Juego.Tipo == Juegos.JuegoTipo.DLC && juegoGratis2.Juego.Tipo == Juegos.JuegoTipo.DLC)
							{
								plantilla.TituloEn = juegoGratis1.Nombre + ", " + juegoGratis2.Nombre + " " +
									Idiomas.BuscarTexto("en", "Free9", "NewsTemplates") + " " + GratisCargar.DevolverGratis(tipoSeleccionado).Nombre;
								plantilla.TituloEs = juegoGratis1.Nombre + ", " + juegoGratis2.Nombre + " " +
									Idiomas.BuscarTexto("es", "Free9", "NewsTemplates") + " " + GratisCargar.DevolverGratis(tipoSeleccionado).Nombre;
							}
						}

						if (juegoGratis1.Juego.Tipo == Juegos.JuegoTipo.Game && juegoGratis2.Juego.Tipo == Juegos.JuegoTipo.Game)
						{
							plantilla.ContenidoEn = GratisCargar.DevolverGratis(tipoSeleccionado).Nombre + " " + Idiomas.BuscarTexto("en", "Free6", "NewsTemplates");
							plantilla.ContenidoEs = GratisCargar.DevolverGratis(tipoSeleccionado).Nombre + " " + Idiomas.BuscarTexto("es", "Free6", "NewsTemplates");
						}
						else if (juegoGratis1.Juego.Tipo == Juegos.JuegoTipo.DLC && juegoGratis2.Juego.Tipo == Juegos.JuegoTipo.DLC)
						{
							plantilla.ContenidoEn = GratisCargar.DevolverGratis(tipoSeleccionado).Nombre + " " + Idiomas.BuscarTexto("en", "Free8", "NewsTemplates");
							plantilla.ContenidoEs = GratisCargar.DevolverGratis(tipoSeleccionado).Nombre + " " + Idiomas.BuscarTexto("es", "Free8", "NewsTemplates");
						}
					}

					plantilla.ContenidoEn = plantilla.ContenidoEn + Environment.NewLine + "<ul>" + Environment.NewLine;
					plantilla.ContenidoEs = plantilla.ContenidoEs + Environment.NewLine + "<ul>" + Environment.NewLine;

					foreach (var juego in lista)
					{
						Juegos.JuegoGratis gratis = BaseDatos.Gratis.Buscar.UnJuego(juego);

						if (gratis != null)
						{
							if (GratisCargar.DevolverGratis(tipoSeleccionado).DRMEnseñar == true)
							{
								plantilla.ContenidoEn = plantilla.ContenidoEn + "<li>" + gratis.Nombre + " (" + Juegos.JuegoDRM2.DevolverDRM(gratis.DRM) + ")</li>" + Environment.NewLine;
								plantilla.ContenidoEs = plantilla.ContenidoEs + "<li>" + gratis.Nombre + " (" + Juegos.JuegoDRM2.DevolverDRM(gratis.DRM) + ")</li>" + Environment.NewLine;
							}
							else
							{
								plantilla.ContenidoEn = plantilla.ContenidoEn + "<li>" + gratis.Nombre + "</li>" + Environment.NewLine;
								plantilla.ContenidoEs = plantilla.ContenidoEs + "<li>" + gratis.Nombre + "</li>" + Environment.NewLine;
							}
						}
					}

					plantilla.ContenidoEn = plantilla.ContenidoEn + "</ul>";
					plantilla.ContenidoEs = plantilla.ContenidoEs + "</ul>";

					if (juegoGratis1.ImagenNoticia != null)
					{
						plantilla.Imagen = juegoGratis1.ImagenNoticia;
					}
					else
					{
						plantilla.Imagen = juegoGratis1.Juego.Imagenes.Library_1920x620;
					}

					plantilla.Fecha = BaseDatos.Gratis.Buscar.UnJuego(lista[0]).FechaTermina;

					if (string.IsNullOrEmpty(plantilla.GratisIds) == true)
					{
						plantilla.GratisIds = id.ToString();
					}
					else
					{
						plantilla.GratisIds = plantilla.GratisIds + "," + id.ToString();
					}
				}
            }
            else
            {
                plantilla = new Plantilla();
            }

            return plantilla;
        }

        public static Plantilla Suscripciones(Plantilla plantilla, int juegoId, int id, string tipoSeleccionado)
        {
            if (string.IsNullOrEmpty(plantilla.Juegos) == true)
            {
                plantilla.Juegos = juegoId.ToString();
            }
            else
            {
                if (plantilla.Juegos.Contains(juegoId.ToString()) == false)
                {
                    plantilla.Juegos = plantilla.Juegos + "," + juegoId.ToString();
                }
                else
                {
                    int int1 = plantilla.Juegos.IndexOf(juegoId.ToString() + ",");

                    if (int1 == -1)
                    {
                        int1 = plantilla.Juegos.IndexOf(juegoId.ToString());
                        plantilla.Juegos = plantilla.Juegos.Remove(int1, juegoId.ToString().Length);
                    }
                    else
                    {
                        plantilla.Juegos = plantilla.Juegos.Remove(int1, juegoId.ToString().Length + 1);
                    }

                    if (plantilla.Juegos.Trim().Length == 1)
                    {
                        plantilla.Juegos = null;
                    }
                }
            }

            List<string> lista = Listados.Generar(plantilla.Juegos);

            if (lista != null)
            {
                #region Titulo

                if (lista.Count == 1)
                {
                    plantilla.TituloEn = BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[0])).Nombre + " " + string.Format(Idiomas.BuscarTexto("en", "Subscription1", "NewsTemplates"), Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(tipoSeleccionado).Nombre);
                    plantilla.TituloEs = BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[0])).Nombre + " " + string.Format(Idiomas.BuscarTexto("es", "Subscription1", "NewsTemplates"), Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(tipoSeleccionado).Nombre);
                }
                else if (lista.Count == 2)
                {
                    plantilla.TituloEn = BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[0])).Nombre + " " + Idiomas.BuscarTexto("en", "Subscription2", "NewsTemplates") + " " +
                        BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[1])).Nombre + " " + string.Format(Idiomas.BuscarTexto("en", "Subscription3", "NewsTemplates"), Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(tipoSeleccionado).Nombre);
                    plantilla.TituloEs = BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[0])).Nombre + " " + Idiomas.BuscarTexto("es", "Subscription2", "NewsTemplates") + " " +
                        BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[1])).Nombre + " " + string.Format(Idiomas.BuscarTexto("es", "Subscription3", "NewsTemplates"), Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(tipoSeleccionado).Nombre);
                }
                else if (lista.Count > 2)
                {
                    plantilla.TituloEn = BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[0])).Nombre + ", " +
                        BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[1])).Nombre + " " + string.Format(Idiomas.BuscarTexto("en", "Subscription4", "NewsTemplates"), Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(tipoSeleccionado).Nombre);
                    plantilla.TituloEs = BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[0])).Nombre + ", " +
                        BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[1])).Nombre + " " + string.Format(Idiomas.BuscarTexto("es", "Subscription4", "NewsTemplates"), Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(tipoSeleccionado).Nombre);
                }

                #endregion

                #region Contenido

                plantilla.ContenidoEn = Idiomas.BuscarTexto("en", "Subscription5", "NewsTemplates") + " " + Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(tipoSeleccionado).Nombre + " ";
                plantilla.ContenidoEs = Idiomas.BuscarTexto("es", "Subscription5", "NewsTemplates") + " " + Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(tipoSeleccionado).Nombre + " ";

                if (lista.Count == 1)
                {
                    plantilla.ContenidoEn = plantilla.ContenidoEn + Idiomas.BuscarTexto("en", "Subscription6", "NewsTemplates");
                    plantilla.ContenidoEs = plantilla.ContenidoEs + Idiomas.BuscarTexto("es", "Subscription6", "NewsTemplates");
                }
                else if (lista.Count > 1)
                {
                    plantilla.ContenidoEn = plantilla.ContenidoEn + Idiomas.BuscarTexto("en", "Subscription7", "NewsTemplates");
                    plantilla.ContenidoEs = plantilla.ContenidoEs + Idiomas.BuscarTexto("es", "Subscription7", "NewsTemplates");
                }

                plantilla.ContenidoEn = plantilla.ContenidoEn + Environment.NewLine + "<ul>" + Environment.NewLine;
                plantilla.ContenidoEs = plantilla.ContenidoEs + Environment.NewLine + "<ul>" + Environment.NewLine;

                foreach (var juego in lista)
                {
                    Juegos.JuegoSuscripcion suscripcion = BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(juego));

                    if (suscripcion != null)
                    {
                        plantilla.ContenidoEn = plantilla.ContenidoEn + "<li>" + suscripcion.Nombre + " (" + Juegos.JuegoDRM2.DevolverDRM(suscripcion.DRM) + ")</li>" + Environment.NewLine;
                        plantilla.ContenidoEs = plantilla.ContenidoEs + "<li>" + suscripcion.Nombre + " (" + Juegos.JuegoDRM2.DevolverDRM(suscripcion.DRM) + ")</li>" + Environment.NewLine;
                    }
                }

                plantilla.ContenidoEn = plantilla.ContenidoEn + "</ul>";
                plantilla.ContenidoEs = plantilla.ContenidoEs + "</ul>";

                #endregion

                #region Imagen

                if (BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[0])).ImagenNoticia != null)
                {
                    plantilla.Imagen = BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[0])).ImagenNoticia;
                }
                else
                {
                    Juegos.Juego juego = BaseDatos.Juegos.Buscar.UnJuego(BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[0])).JuegoId.ToString());
                    plantilla.Imagen = juego.Imagenes.Library_1920x620;
                }

                #endregion

                #region Fecha

                plantilla.Fecha = BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[0])).FechaTermina;

                #endregion

                if (string.IsNullOrEmpty(plantilla.SuscripcionesIds) == true)
                {
                    plantilla.SuscripcionesIds = id.ToString();
                }
                else
                {
                    plantilla.SuscripcionesIds = plantilla.SuscripcionesIds + "," + id.ToString();
                }
            }
            else
            {
                plantilla = new Plantilla();
            }

            return plantilla;
        }
    }

    public class Plantilla
    {
        public string TituloEn;
        public string TituloEs;
        public string ContenidoEn;
        public string ContenidoEs;
        public string Juegos;
        public DateTime Fecha;
        public string Enlace;
        public string Imagen;
        public string BundleId;
        public string GratisIds;
        public string SuscripcionesIds;
    }
}
