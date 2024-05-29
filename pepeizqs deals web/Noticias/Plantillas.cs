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

                plantilla.Fecha = bundle.FechaTermina.ToString("yyyy-MM-ddTHH:mm:ss");
                plantilla.Enlace = EnlaceAcortador.Generar(bundle.Enlace, bundle.Tipo);

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
                                plantilla.ContenidoEn = plantilla.ContenidoEn + "<li><a href=" + Strings.ChrW(34) + EnlaceAcortador.Generar(bundle.Enlace, bundle.Tipo) + Strings.ChrW(34) + " target=" + Strings.ChrW(34) + "_blank" + Strings.ChrW(34) + ">" + juego.Nombre + " (" + Juegos.JuegoDRM2.DevolverDRM(juego.DRM) + ")</a></li>";
                                plantilla.ContenidoEs = plantilla.ContenidoEs + "<li><a href=" + Strings.ChrW(34) + EnlaceAcortador.Generar(bundle.Enlace, bundle.Tipo) + Strings.ChrW(34) + " target=" + Strings.ChrW(34) + "_blank" + Strings.ChrW(34) + ">" + juego.Nombre + " (" + Juegos.JuegoDRM2.DevolverDRM(juego.DRM) + ")</a></li>";
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
                        plantilla.ContenidoEn = plantilla.ContenidoEn + "<li><a href=" + Strings.ChrW(34) + EnlaceAcortador.Generar(bundle.Enlace, bundle.Tipo) + Strings.ChrW(34) + " target=" + Strings.ChrW(34) + "_blank" + Strings.ChrW(34) + ">" + juego.Nombre + " (" + Juegos.JuegoDRM2.DevolverDRM(juego.DRM) + ")</a></li>";
                        plantilla.ContenidoEs = plantilla.ContenidoEs + "<li><a href=" + Strings.ChrW(34) + EnlaceAcortador.Generar(bundle.Enlace, bundle.Tipo) + Strings.ChrW(34) + " target=" + Strings.ChrW(34) + "_blank" + Strings.ChrW(34) + ">" + juego.Nombre + " (" + Juegos.JuegoDRM2.DevolverDRM(juego.DRM) + ")</a></li>";
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

            return plantilla;
        }

        public static Plantilla Gratis(Plantilla plantilla, int juegoId, string tipoSeleccionado)
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
                        if (lista.Count == 1)
                        {
                            plantilla.Enlace = EnlaceAcortador.Generar(gratis.Enlace, gratis.Tipo);
                        }
                        else
                        {
                            plantilla.Enlace = null;
                        }

                        if (GratisCargar.DevolverGratis(tipoSeleccionado).DRMEnseñar == true)
                        {
                            plantilla.ContenidoEn = plantilla.ContenidoEn + "<li><a href=" + Strings.ChrW(34) + EnlaceAcortador.Generar(gratis.Enlace, gratis.Tipo) + Strings.ChrW(34) + " target=" + Strings.ChrW(34) + "_blank" + Strings.ChrW(34) + ">" + gratis.Nombre + " (" + Juegos.JuegoDRM2.DevolverDRM(gratis.DRM) + ")</a></li>" + Environment.NewLine;
                            plantilla.ContenidoEs = plantilla.ContenidoEs + "<li><a href=" + Strings.ChrW(34) + EnlaceAcortador.Generar(gratis.Enlace, gratis.Tipo) + Strings.ChrW(34) + " target=" + Strings.ChrW(34) + "_blank" + Strings.ChrW(34) + ">" + gratis.Nombre + " (" + Juegos.JuegoDRM2.DevolverDRM(gratis.DRM) + ")</a></li>" + Environment.NewLine;
                        }
                        else
                        {
                            plantilla.ContenidoEn = plantilla.ContenidoEn + "<li><a href=" + Strings.ChrW(34) + EnlaceAcortador.Generar(gratis.Enlace, gratis.Tipo) + Strings.ChrW(34) + " target=" + Strings.ChrW(34) + "_blank" + Strings.ChrW(34) + ">" + gratis.Nombre + "</a></li>" + Environment.NewLine;
                            plantilla.ContenidoEs = plantilla.ContenidoEs + "<li><a href=" + Strings.ChrW(34) + EnlaceAcortador.Generar(gratis.Enlace, gratis.Tipo) + Strings.ChrW(34) + " target=" + Strings.ChrW(34) + "_blank" + Strings.ChrW(34) + ">" + gratis.Nombre + "</a></li>" + Environment.NewLine;
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

                plantilla.Fecha = juegoGratis1.FechaTermina.ToString("yyyy-MM-ddTHH:mm:ss");
            }
            else
            {
                plantilla = new Plantilla();
            }

            return plantilla;
        }

        public static Plantilla Suscripciones(Plantilla plantilla, int juegoId, string tipoSeleccionado)
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

                bool mismoEnlace = true;
                string enlaceSuscripcion = string.Empty;

                foreach (var juego in lista)
                {
                    Juegos.JuegoSuscripcion suscripcion = BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(juego));

                    if (suscripcion != null)
                    {
                        if (enlaceSuscripcion == string.Empty)
                        {
                            enlaceSuscripcion = EnlaceAcortador.Generar(suscripcion.Enlace, suscripcion.Tipo);
                        }
                        else
                        {
                            if (enlaceSuscripcion != EnlaceAcortador.Generar(suscripcion.Enlace, suscripcion.Tipo))
                            {
                                mismoEnlace = false;
                            }
                        }

                        plantilla.ContenidoEn = plantilla.ContenidoEn + "<li><a href=" + Strings.ChrW(34) + EnlaceAcortador.Generar(suscripcion.Enlace, suscripcion.Tipo) + Strings.ChrW(34) + " target=" + Strings.ChrW(34) + "_blank" + Strings.ChrW(34) + ">" + suscripcion.Nombre + " (" + Juegos.JuegoDRM2.DevolverDRM(suscripcion.DRM) + ")</a></li>" + Environment.NewLine;
                        plantilla.ContenidoEs = plantilla.ContenidoEs + "<li><a href=" + Strings.ChrW(34) + EnlaceAcortador.Generar(suscripcion.Enlace, suscripcion.Tipo) + Strings.ChrW(34) + " target=" + Strings.ChrW(34) + "_blank" + Strings.ChrW(34) + ">" + suscripcion.Nombre + " (" + Juegos.JuegoDRM2.DevolverDRM(suscripcion.DRM) + ")</a></li>" + Environment.NewLine;
                    }
                }

                if (mismoEnlace == true)
                {
                    plantilla.Enlace = enlaceSuscripcion;
                }
                else
                {
                    plantilla.Enlace = null;
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

                plantilla.Fecha = BaseDatos.Suscripciones.Buscar.UnJuego(int.Parse(lista[0])).FechaTermina.ToString("yyyy-MM-ddTHH:mm:ss");

                #endregion
            }
            else
            {
                plantilla = new Plantilla();
            }

            return plantilla;
        }

        public static Plantilla Sorteos(List<Sorteos2.Sorteo> sorteosActivos)
        {
            Plantilla plantilla = new Plantilla();

            plantilla.TituloEn = string.Format("{0} {1}", sorteosActivos.Count.ToString(), Idiomas.CogerCadena("en-US", "News.Giveaways1"));
            plantilla.TituloEs = string.Format("{0} {1}", sorteosActivos.Count.ToString(), Idiomas.CogerCadena("es-ES", "News.Giveaways1"));

            plantilla.Imagen = BaseDatos.Juegos.Buscar.UnJuego(sorteosActivos[0].JuegoId.ToString()).Imagenes.Header_460x215;

            plantilla.Fecha = sorteosActivos[0].FechaTermina.ToString("yyyy-MM-ddTHH:mm:ss");

            plantilla.Enlace = "https://pepeizqdeals.com/Giveaways";

            plantilla.ContenidoEn = "<div>" + Idiomas.CogerCadena("en-US", "News.Giveaways2") + "</div><br/>";
            plantilla.ContenidoEs = "<div>" + Idiomas.CogerCadena("es-ES", "News.Giveaways2") + "</div><br/>";

            List<Sorteos2.Sorteo> sorteosPremium = new List<Sorteos2.Sorteo>();
            List<Sorteos2.Sorteo> sorteosBasico = new List<Sorteos2.Sorteo>();

            foreach (var activo in sorteosActivos)
            {
                if (activo.GrupoId == "40604285")
                {
                    sorteosPremium.Add(activo);
                }
                else if (activo.GrupoId == "33500256")
                {
                    sorteosBasico.Add(activo);
                }
            }

            if (sorteosPremium.Count > 0)
            {
                plantilla.ContenidoEn = plantilla.ContenidoEn + "<div>" + Idiomas.CogerCadena("en-US", "News.Giveaways3") + "</div><ul>";
                plantilla.ContenidoEs = plantilla.ContenidoEs + "<div>" + Idiomas.CogerCadena("es-ES", "News.Giveaways3") + "</div><ul>";

                foreach (var premium in sorteosPremium)
                {
                    plantilla.ContenidoEn = plantilla.ContenidoEn + "<li><a href=" + Strings.ChrW(34) + "https://pepeizqdeals.com/Giveaways#premium" + Strings.ChrW(34) + " target=" + Strings.ChrW(34) + "_blank" + Strings.ChrW(34) + ">" + BaseDatos.Juegos.Buscar.UnJuego(premium.JuegoId.ToString()).Nombre + "</a></li>";
                    plantilla.ContenidoEs = plantilla.ContenidoEs + "<li><a href=" + Strings.ChrW(34) + "https://pepeizqdeals.com/Giveaways#premium" + Strings.ChrW(34) + " target=" + Strings.ChrW(34) + "_blank" + Strings.ChrW(34) + ">" + BaseDatos.Juegos.Buscar.UnJuego(premium.JuegoId.ToString()).Nombre + "</a></li>";
                }

                plantilla.ContenidoEn = plantilla.ContenidoEn + "</ul>";
                plantilla.ContenidoEs = plantilla.ContenidoEs + "</ul>";
            }

            if (sorteosBasico.Count > 0)
            {
                plantilla.ContenidoEn = plantilla.ContenidoEn + "<div>" + Idiomas.CogerCadena("en-US", "News.Giveaways4") + "</div><ul>";
                plantilla.ContenidoEs = plantilla.ContenidoEs + "<div>" + Idiomas.CogerCadena("es-ES", "News.Giveaways4") + "</div><ul>";

                foreach (var basico in sorteosBasico)
                {
                    plantilla.ContenidoEn = plantilla.ContenidoEn + "<li><a href=" + Strings.ChrW(34) + "https://pepeizqdeals.com/Giveaways#basic" + Strings.ChrW(34) + " target=" + Strings.ChrW(34) + "_blank" + Strings.ChrW(34) + ">" + BaseDatos.Juegos.Buscar.UnJuego(basico.JuegoId.ToString()).Nombre + "</a></li>";
                    plantilla.ContenidoEs = plantilla.ContenidoEs + "<li><a href=" + Strings.ChrW(34) + "https://pepeizqdeals.com/Giveaways#basic" + Strings.ChrW(34) + " target=" + Strings.ChrW(34) + "_blank" + Strings.ChrW(34) + ">" + BaseDatos.Juegos.Buscar.UnJuego(basico.JuegoId.ToString()).Nombre + "</a></li>";
                }

                plantilla.ContenidoEn = plantilla.ContenidoEn + "</ul>";
                plantilla.ContenidoEs = plantilla.ContenidoEs + "</ul>";
            }

            return plantilla;
        }

        public static Plantilla Rumores(Plantilla plantilla, string juegoId)
        {
            if (plantilla.Juegos == null)
            {
                plantilla.Juegos = juegoId;
            }
            else
            {
                plantilla.Juegos = plantilla.Juegos + "," + juegoId;
            }

            plantilla.ContenidoEn = "<ul>" + Environment.NewLine;
            plantilla.ContenidoEs = "<ul>" + Environment.NewLine;

            foreach (string juego in Herramientas.Listados.Generar(plantilla.Juegos))
            {
                Juegos.Juego juegob = BaseDatos.Juegos.Buscar.UnJuego(juego);

                if (juegob != null)
                {
                    plantilla.ContenidoEn = plantilla.ContenidoEn + "<li><a href=" + Strings.ChrW(34) + "/game/" + juegob.Id.ToString() + Strings.ChrW(34) + ">" + juegob.Nombre + "</a></li>" + Environment.NewLine;
                    plantilla.ContenidoEs = plantilla.ContenidoEs + "<li><a href=" + Strings.ChrW(34) + "/game/" + juegob.Id.ToString() + Strings.ChrW(34) + ">" + juegob.Nombre + "</a></li>" + Environment.NewLine;
                }
            }

            plantilla.ContenidoEn = Environment.NewLine + plantilla.ContenidoEn + "</ul>";
            plantilla.ContenidoEs = Environment.NewLine + plantilla.ContenidoEs + "</ul>";

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
        public string Fecha;
        public string Enlace;
        public string Imagen;
        public string BundleId;
    }
}
