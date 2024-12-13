#nullable disable

//https://app.impact.com/secure/productservices/apps/catalog/download.irps?p=46x%7B%22networkId%22%3A%221%22%2C%22id%22%3A%221813947%22%2C%22mpId%22%3A%221382810%22%2C%22version%22%3A%22standard%22%7DRdI6oFUr5Kb9Dw0tR8044psFyU8%3D

using Herramientas;
using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Xml.Serialization;

namespace APIs._2Game
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "2game",
				Nombre = "2Game",
				ImagenLogo = "/imagenes/tiendas/2game_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/2game_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/2game_icono.webp",
				Color = "#beb2f1",
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

        public static string Referido(string enlace)
        {
            enlace = enlace.Replace(":", "%3A");
            enlace = enlace.Replace("/", "%2F");
            enlace = enlace.Replace("/", "%2F");
            enlace = enlace.Replace("?", "%3F");
            enlace = enlace.Replace("=", "%3D");

            return "https://2game-europe.sjv.io/c/1382810/1697067/19711?u=" + enlace;
        }

        public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador, ViewDataDictionary objeto = null)
		{
			BaseDatos.Admin.Actualizar.Tiendas(Tienda.Generar().Id, DateTime.Now, "0 ofertas detectadas", conexion);

            string html2 = await Decompiladores.GZipFormato("https://app.impact.com/secure/productservices/apps/catalog/download.irps?p=46x%7B%22networkId%22%3A%221%22%2C%22id%22%3A%221813947%22%2C%22mpId%22%3A%221382810%22%2C%22version%22%3A%22standard%22%7DRdI6oFUr5Kb9Dw0tR8044psFyU8%3D");

            string html = await Decompiladores.Estandar("https://2game.com/feeds/GoogleShopping_EU.xml");

			if (string.IsNullOrEmpty(html) == false)
			{
                html = html.Replace("g:image_link", "image_link");
                html = html.Replace("g:sale_price", "sale_price");
                html = html.Replace("g:price", "price");
                html = html.Replace("g:id", "id");

                XmlSerializer xml = new XmlSerializer(typeof(_2GameJuegos));
                _2GameJuegos listaJuegos = null;

                using (TextReader lector = new StringReader(html))
                {
                    listaJuegos = (_2GameJuegos)xml.Deserialize(lector);
                }

                if (listaJuegos != null)
                {
                    if (listaJuegos.Canal.Juegos != null)
                    {
                        if (listaJuegos.Canal.Juegos.Count > 0)
                        {
                            int juegos2 = 0;

                            foreach (var juego in listaJuegos.Canal.Juegos)
                            {
                                if (string.IsNullOrEmpty(juego.PrecioBase) == false && string.IsNullOrEmpty(juego.PrecioRebajado) == false)
                                {
                                    string precioBase = juego.PrecioBase;
                                    precioBase = precioBase.Replace("EUR", null);

                                    string precioRebajado = juego.PrecioRebajado;
                                    precioRebajado = precioRebajado.Replace("EUR", null);

                                    decimal dprecioBase = decimal.Parse(precioBase);
                                    decimal dprecioRebajado = decimal.Parse(precioRebajado);

                                    int descuento = Calculadora.SacarDescuento(dprecioBase, dprecioRebajado);

                                    if (descuento > 0)
                                    {
                                        string nombre = WebUtility.HtmlDecode(juego.Nombre);

                                        string enlace = juego.Enlace;

                                        enlace = enlace.Replace("/fr/", "/");

                                        string imagen = juego.Imagen;

                                        JuegoDRM drm = JuegoDRM.NoEspecificado;

                                        JuegoPrecio oferta = new JuegoPrecio
                                        {
                                            Nombre = nombre,
                                            Enlace = enlace,
                                            Imagen = imagen,
                                            Moneda = JuegoMoneda.Euro,
                                            Precio = dprecioRebajado,
                                            Descuento = descuento,
                                            Tienda = Generar().Id,
                                            DRM = drm,
                                            FechaDetectado = DateTime.Now,
                                            FechaActualizacion = DateTime.Now
                                        };

                                        if (html2.Contains(juego.Id) == true)
                                        {
                                            StringReader lector = new StringReader(html2);

                                            string linea = string.Empty;

                                            do
                                            {
                                                linea = lector.ReadLine();

                                                if (string.IsNullOrEmpty(linea) == false)
                                                {
                                                    if (linea.Contains(juego.Id) == true)
                                                    {
                                                        if (linea.Contains("Steam,,") == true)
                                                        {
                                                            oferta.DRM = JuegoDRM.Steam;
                                                        }
                                                    }
                                                }
                                            }
                                            while (linea != null);
                                        }

                                        if (oferta.DRM != JuegoDRM.NoEspecificado)
                                        {
                                            try
                                            {
                                                BaseDatos.Tiendas.Comprobar.Resto(oferta, conexion);
                                            }
                                            catch (Exception ex)
                                            {
                                                BaseDatos.Errores.Insertar.Mensaje(Tienda.Generar().Id, ex);
                                            }
                                        }
                                        
                                        juegos2 += 1;

                                        try
                                        {
											BaseDatos.Admin.Actualizar.Tiendas(Tienda.Generar().Id, DateTime.Now, juegos2.ToString() + " ofertas detectadas", conexion);
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
    public class _2GameJuegos
    {
        [XmlElement("channel")]
        public _2GameCanal Canal { get; set; }
    }

    public class _2GameCanal
    {
        [XmlElement("item")]
        public List<_2GameJuego> Juegos { get; set; }
    }

    public class _2GameJuego
    {
        [XmlElement("id")]
        public string Id { get; set; }

        [XmlElement("title")]
        public string Nombre { get; set; }

        [XmlElement("link")]
        public string Enlace { get; set; }

        [XmlElement("sale_price")]
        public string PrecioRebajado { get; set; }

        [XmlElement("price")]
        public string PrecioBase { get; set; }

        [XmlElement("image_link")]
        public string Imagen { get; set; }
    }
}
