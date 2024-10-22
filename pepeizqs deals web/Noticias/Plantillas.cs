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
						plantilla.TituloEn = string.Format(Idiomas.CogerCadena("en-US", "News.BundleString1"), bundle.NombreBundle, bundle.NombreTienda);
						plantilla.TituloEs = string.Format(Idiomas.CogerCadena("es-ES", "News.BundleString1"), bundle.NombreBundle, bundle.NombreTienda);
					}
					else
					{
						plantilla.TituloEn = string.Format(Idiomas.CogerCadena("en-US", "News.BundleString5"), bundle.NombreBundle, bundle.NombreTienda);
						plantilla.TituloEs = string.Format(Idiomas.CogerCadena("es-ES", "News.BundleString5"), bundle.NombreBundle, bundle.NombreTienda);
					}

					#endregion

					#region Contenido

					if (bundle.Pick == false)
					{
						plantilla.ContenidoEn = "<div style=" + Strings.ChrW(34) + "margin-bottom: 15px;" + Strings.ChrW(34) + ">" + string.Format(Idiomas.CogerCadena("en-US", "News.BundleString2"), bundle.NombreTienda) + "</div>";
						plantilla.ContenidoEs = "<div style=" + Strings.ChrW(34) + "margin-bottom: 15px;" + Strings.ChrW(34) + ">" + string.Format(Idiomas.CogerCadena("es-ES", "News.BundleString2"), bundle.NombreTienda) + "</div>";

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
						plantilla.ContenidoEn = "<div style=" + Strings.ChrW(34) + "margin-bottom: 15px;" + Strings.ChrW(34) + ">" + string.Format(Idiomas.CogerCadena("en-US", "News.BundleString3"), bundle.NombreTienda) + "</div>";
						plantilla.ContenidoEs = "<div style=" + Strings.ChrW(34) + "margin-bottom: 15px;" + Strings.ChrW(34) + ">" + string.Format(Idiomas.CogerCadena("es-ES", "News.BundleString3"), bundle.NombreTienda) + "</div>";

						foreach (var tier in bundle.Tiers)
						{
							string precio = tier.Precio;
							precio = precio.Replace(".", ",");
							precio = precio + "€";

							plantilla.ContenidoEn = plantilla.ContenidoEn + "<div style=" + Strings.ChrW(34) + "margin-bottom: 15px;" + Strings.ChrW(34) + ">" + tier.CantidadJuegos.ToString() + " " + Idiomas.CogerCadena("en-US", "News.BundleString4") + " (" + precio + ")</div>";
							plantilla.ContenidoEs = plantilla.ContenidoEs + "<div style=" + Strings.ChrW(34) + "margin-bottom: 15px;" + Strings.ChrW(34) + ">" + tier.CantidadJuegos.ToString() + " " + Idiomas.CogerCadena("es-ES", "News.BundleString4") + " (" + precio + ")</div>";
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
                Juegos.JuegoGratis juegoGratis1 = BaseDatos.Gratis.Buscar.UnJuego(int.Parse(lista[0]));

                if (juegoGratis1.Juego == null)
                {
                    juegoGratis1.Juego = BaseDatos.Juegos.Buscar.UnJuego(juegoGratis1.JuegoId);
                }

                if (lista.Count == 1)
                { 
                    plantilla.TituloEn = juegoGratis1.Nombre + " " + Idiomas.CogerCadena("en-US", "News.FreeString1") + " " + GratisCargar.DevolverGratis(tipoSeleccionado).Nombre;
                    plantilla.TituloEs = juegoGratis1.Nombre + " " + Idiomas.CogerCadena("es-ES", "News.FreeString1") + " " + GratisCargar.DevolverGratis(tipoSeleccionado).Nombre;

                    if (juegoGratis1.Juego.Tipo == Juegos.JuegoTipo.Game)
                    {
                        plantilla.ContenidoEn = GratisCargar.DevolverGratis(tipoSeleccionado).Nombre + " " + Idiomas.CogerCadena("en-US", "News.FreeString5");
                        plantilla.ContenidoEs = GratisCargar.DevolverGratis(tipoSeleccionado).Nombre + " " + Idiomas.CogerCadena("es-ES", "News.FreeString5");
                    }
                    else if (juegoGratis1.Juego.Tipo == Juegos.JuegoTipo.DLC)
                    {
                        plantilla.ContenidoEn = GratisCargar.DevolverGratis(tipoSeleccionado).Nombre + " " + Idiomas.CogerCadena("en-US", "News.FreeString7");
                        plantilla.ContenidoEs = GratisCargar.DevolverGratis(tipoSeleccionado).Nombre + " " + Idiomas.CogerCadena("es-ES", "News.FreeString7");
                    }
                }
                else if (lista.Count > 1)
                {
                    Juegos.JuegoGratis juegoGratis2 = BaseDatos.Gratis.Buscar.UnJuego(int.Parse(lista[1]));

                    if (juegoGratis2.Juego == null)
                    {
                        juegoGratis2.Juego = BaseDatos.Juegos.Buscar.UnJuego(juegoGratis1.JuegoId);
                    }

                    if (lista.Count == 2)
                    {
                        plantilla.TituloEn = juegoGratis1.Nombre + " " + Idiomas.CogerCadena("en-US", "News.FreeString2") + " " +
                            juegoGratis2.Nombre + " " + Idiomas.CogerCadena("en-US", "News.FreeString3") + " " + GratisCargar.DevolverGratis(tipoSeleccionado).Nombre;
                        plantilla.TituloEs = juegoGratis1.Nombre + " " + Idiomas.CogerCadena("es-ES", "News.FreeString2") + " " +
                            juegoGratis2.Nombre + " " + Idiomas.CogerCadena("es-ES", "News.FreeString3") + " " + GratisCargar.DevolverGratis(tipoSeleccionado).Nombre;
                    }
                    else if (lista.Count > 2)
                    {
                        if (juegoGratis1.Juego.Tipo == Juegos.JuegoTipo.Game && juegoGratis2.Juego.Tipo == Juegos.JuegoTipo.Game)
                        {
                            plantilla.TituloEn = juegoGratis1.Nombre + ", " + juegoGratis2.Nombre + " " +
                                Idiomas.CogerCadena("en-US", "News.FreeString4") + " " + GratisCargar.DevolverGratis(tipoSeleccionado).Nombre;
                            plantilla.TituloEs = juegoGratis1.Nombre + ", " + juegoGratis2.Nombre + " " +
                                Idiomas.CogerCadena("es-ES", "News.FreeString4") + " " + GratisCargar.DevolverGratis(tipoSeleccionado).Nombre;
                        }
                        else if (juegoGratis1.Juego.Tipo == Juegos.JuegoTipo.DLC && juegoGratis2.Juego.Tipo == Juegos.JuegoTipo.DLC)
                        {
                            plantilla.TituloEn = juegoGratis1.Nombre + ", " + juegoGratis2.Nombre + " " +
                                Idiomas.CogerCadena("en-US", "News.FreeString9") + " " + GratisCargar.DevolverGratis(tipoSeleccionado).Nombre;
                            plantilla.TituloEs = juegoGratis1.Nombre + ", " + juegoGratis2.Nombre + " " +
                                Idiomas.CogerCadena("es-ES", "News.FreeString9") + " " + GratisCargar.DevolverGratis(tipoSeleccionado).Nombre;
                        }
                    }

                    if (juegoGratis1.Juego.Tipo == Juegos.JuegoTipo.Game && juegoGratis2.Juego.Tipo == Juegos.JuegoTipo.Game)
                    {
                        plantilla.ContenidoEn = GratisCargar.DevolverGratis(tipoSeleccionado).Nombre + " " + Idiomas.CogerCadena("en-US", "News.FreeString6");
                        plantilla.ContenidoEs = GratisCargar.DevolverGratis(tipoSeleccionado).Nombre + " " + Idiomas.CogerCadena("es-ES", "News.FreeString6");
                    }
                    else if (juegoGratis1.Juego.Tipo == Juegos.JuegoTipo.DLC && juegoGratis2.Juego.Tipo == Juegos.JuegoTipo.DLC)
                    {
                        plantilla.ContenidoEn = GratisCargar.DevolverGratis(tipoSeleccionado).Nombre + " " + Idiomas.CogerCadena("en-US", "News.FreeString8");
                        plantilla.ContenidoEs = GratisCargar.DevolverGratis(tipoSeleccionado).Nombre + " " + Idiomas.CogerCadena("es-ES", "News.FreeString8");
                    }
                }

                plantilla.ContenidoEn = plantilla.ContenidoEn + Environment.NewLine + "<ul>" + Environment.NewLine;
                plantilla.ContenidoEs = plantilla.ContenidoEs + Environment.NewLine + "<ul>" + Environment.NewLine;

                foreach (var juego in lista)
                {
                    Juegos.JuegoGratis gratis = BaseDatos.Gratis.Buscar.UnJuego(int.Parse(juego));

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

                plantilla.Fecha = BaseDatos.Gratis.Buscar.UnJuego(int.Parse(lista[0])).FechaTermina;

                if (string.IsNullOrEmpty(plantilla.GratisIds) == true)
                {
                    plantilla.GratisIds = id.ToString();
                }
                else
                {
                    plantilla.GratisIds = plantilla.GratisIds + "," + id.ToString();
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
                    plantilla.TituloEn = BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[0])).Nombre + " " + string.Format(Idiomas.CogerCadena("en-US", "News.SubscriptionString1"), Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(tipoSeleccionado).Nombre);
                    plantilla.TituloEs = BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[0])).Nombre + " " + string.Format(Idiomas.CogerCadena("es-ES", "News.SubscriptionString1"), Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(tipoSeleccionado).Nombre);
                }
                else if (lista.Count == 2)
                {
                    plantilla.TituloEn = BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[0])).Nombre + " " + Idiomas.CogerCadena("en-US", "News.SubscriptionString2") + " " +
                        BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[1])).Nombre + " " + string.Format(Idiomas.CogerCadena("en-US", "News.SubscriptionString3"), Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(tipoSeleccionado).Nombre);
                    plantilla.TituloEs = BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[0])).Nombre + " " + Idiomas.CogerCadena("es-ES", "News.SubscriptionString2") + " " +
                        BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[1])).Nombre + " " + string.Format(Idiomas.CogerCadena("es-ES", "News.SubscriptionString3"), Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(tipoSeleccionado).Nombre);
                }
                else if (lista.Count > 2)
                {
                    plantilla.TituloEn = BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[0])).Nombre + ", " +
                        BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[1])).Nombre + " " + string.Format(Idiomas.CogerCadena("en-US", "News.SubscriptionString4"), Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(tipoSeleccionado).Nombre);
                    plantilla.TituloEs = BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[0])).Nombre + ", " +
                        BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[1])).Nombre + " " + string.Format(Idiomas.CogerCadena("es-ES", "News.SubscriptionString4"), Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(tipoSeleccionado).Nombre);
                }

                #endregion

                #region Contenido

                plantilla.ContenidoEn = Idiomas.CogerCadena("en-US", "News.SubscriptionString5") + " " + Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(tipoSeleccionado).Nombre + " ";
                plantilla.ContenidoEs = Idiomas.CogerCadena("es-ES", "News.SubscriptionString5") + " " + Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(tipoSeleccionado).Nombre + " ";

                if (lista.Count == 1)
                {
                    plantilla.ContenidoEn = plantilla.ContenidoEn + Idiomas.CogerCadena("en-US", "News.SubscriptionString6");
                    plantilla.ContenidoEs = plantilla.ContenidoEs + Idiomas.CogerCadena("es-ES", "News.SubscriptionString6");
                }
                else if (lista.Count > 1)
                {
                    plantilla.ContenidoEn = plantilla.ContenidoEn + Idiomas.CogerCadena("en-US", "News.SubscriptionString7");
                    plantilla.ContenidoEs = plantilla.ContenidoEs + Idiomas.CogerCadena("es-ES", "News.SubscriptionString7");
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
