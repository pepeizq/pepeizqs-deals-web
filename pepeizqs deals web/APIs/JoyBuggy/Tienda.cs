#nullable disable

using Herramientas;
using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Xml.Serialization;

namespace APIs.JoyBuggy
{
    public static class Tienda
    {
        public static Tiendas2.Tienda Generar()
        {
            Tiendas2.Tienda tienda = new Tiendas2.Tienda
            {
                Id = "joybuggy",
                Nombre = "JoyBuggy",
                ImagenLogo = "/imagenes/tiendas/joybuggy_logo.webp",
                Imagen300x80 = "/imagenes/tiendas/joybuggy_300x80.webp",
                ImagenIcono = "/imagenes/tiendas/joybuggy_icono.icon",
                Color = "#39f2d3",
                AdminEnseñar = true,
                AdminInteractuar = true
            };

            return tienda;
        }

        public static string Referido(string enlace)
        {
            return enlace + "?ref=253";
        }

        public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador, ViewDataDictionary objeto = null)
        {
            BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, "0 ofertas detectadas", conexion);

            string html = await decompilador.Estandar("https://www.joybuggy.com/module/xmlfeeds/api?id=70");

            if (string.IsNullOrEmpty(html) == false)
            {
                XmlSerializer xml = new XmlSerializer(typeof(JoyBuggyCanal));
                JoyBuggyCanal listaJuegos = null;

                using (TextReader lector = new StringReader(html))
                {
                    listaJuegos = (JoyBuggyCanal)xml.Deserialize(lector);
                }

                if (listaJuegos != null)
                {
                    if (listaJuegos.Datos.Juegos != null)
                    {
                        if (listaJuegos.Datos.Juegos.Count > 0)
                        {
                            int juegos2 = 0;

                            foreach (JoyBuggyJuego juego in listaJuegos.Datos.Juegos)
                            {
                                string nombre = WebUtility.HtmlDecode(juego.Nombre);
                                BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, juegos2.ToString() + " ofertas detectadas", conexion);
                                string enlace = juego.Enlace;

                                string imagen = juego.Imagen;

                                if (string.IsNullOrEmpty(juego.PrecioBase) == false && string.IsNullOrEmpty(juego.PrecioRebajado) == false)
                                {
                                    decimal precioBase = decimal.Parse(juego.PrecioBase);
                                    decimal precioRebajado = decimal.Parse(juego.PrecioRebajado);

                                    int descuento = Calculadora.SacarDescuento(precioBase, precioRebajado);

                                    if (descuento > 0)
                                    {
                                        JuegoDRM drm = JuegoDRM2.Traducir(juego.DRM, Generar().Id);

                                        JuegoPrecio oferta = new JuegoPrecio
                                        {
                                            Nombre = nombre,
                                            Enlace = enlace,
                                            Imagen = imagen,
                                            Moneda = JuegoMoneda.Euro,
                                            Precio = precioRebajado,
                                            Descuento = descuento,
                                            Tienda = Generar().Id,
                                            DRM = drm,
                                            FechaDetectado = DateTime.Now,
                                            FechaActualizacion = DateTime.Now
                                        };

                                        //BaseDatos.Tiendas.Comprobar.Resto(oferta, objeto, conexion);

                                        juegos2 += 1;
                                        BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, juegos2.ToString() + " ofertas detectadas", conexion);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    [XmlRoot("rss")]
    public class JoyBuggyCanal
    {
        [XmlElement("channel")]
        public JoyBuggyJuegos Datos { get; set; }
    }

    public class JoyBuggyJuegos
    {
        [XmlElement("item")]
        public List<JoyBuggyJuego> Juegos { get; set; }
    }

    public class JoyBuggyJuego
    {
        [XmlElement("g:title")]
        public string Nombre { get; set; }

        [XmlElement("g:link")]
        public string Enlace { get; set; }

        [XmlElement("g:sale_price")]
        public string PrecioRebajado { get; set; }

        [XmlElement("g:price")]
        public string PrecioBase { get; set; }

        [XmlElement("g:id")]
        public string ID { get; set; }

        [XmlElement("g:image_link")]
        public string Imagen { get; set; }

        [XmlElement("Platform")]
        public string DRM { get; set; }
    }
}
