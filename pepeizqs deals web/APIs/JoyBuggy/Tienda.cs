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
                ImagenIcono = "/imagenes/tiendas/joybuggy_icono.ico",
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
			BaseDatos.Admin.Actualizar.Tiendas(Tienda.Generar().Id, DateTime.Now, 0, conexion);

            string html = await Decompiladores.Estandar("https://www.joybuggy.com/module/xmlfeeds/api?id=70");

            if (string.IsNullOrEmpty(html) == false)
            {
                html = html.Replace("g:title", "title");
                html = html.Replace("g:link", "link");
                html = html.Replace("g:image_link", "image_link");
                html = html.Replace("g:id", "id");
                html = html.Replace("g:sale_price", "sale_price");
                html = html.Replace("g:price", "price");

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
                                if (string.IsNullOrEmpty(juego.PrecioBase) == false && string.IsNullOrEmpty(juego.PrecioRebajado) == false)
                                {
                                    decimal precioBase = decimal.Parse(juego.PrecioBase);
                                    decimal precioRebajado = decimal.Parse(juego.PrecioRebajado);

                                    int descuento = Calculadora.SacarDescuento(precioBase, precioRebajado);

                                    if (descuento > 0)
                                    {
                                        string nombre = WebUtility.HtmlDecode(juego.Nombre);

                                        string enlace = juego.Enlace;

                                        string imagen = juego.Imagen;

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

										try
										{
											BaseDatos.Tiendas.Comprobar.Resto(oferta, conexion);
										}
										catch (Exception ex)
										{
                                            BaseDatos.Errores.Insertar.Mensaje(Tienda.Generar().Id, ex, conexion);
                                        }

										juegos2 += 1;

										try
										{
											BaseDatos.Admin.Actualizar.Tiendas(Tienda.Generar().Id, DateTime.Now, juegos2, conexion);
										}
										catch (Exception ex)
										{
                                            BaseDatos.Errores.Insertar.Mensaje(Tienda.Generar().Id, ex, conexion);
                                        }
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

        [XmlElement("created_at")]
        public string Creado { get; set; }
    }

    public class JoyBuggyJuego
    {
        [XmlElement("title")]
        public string Nombre { get; set; }

        [XmlElement("link")]
        public string Enlace { get; set; }

        [XmlElement("sale_price")]
        public string PrecioRebajado { get; set; }

        [XmlElement("price")]
        public string PrecioBase { get; set; }

        [XmlElement("id")]
        public string ID { get; set; }

        [XmlElement("image_link")]
        public string Imagen { get; set; }

        [XmlElement("Platform")]
        public string DRM { get; set; }
    }
}
