#nullable disable

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

		public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador, ViewDataDictionary objeto = null)
		{
			BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, "0 ofertas detectadas", conexion);

			string html = await decompilador.Estandar("https://2game.com/feeds/GoogleShopping_EU.xml");

			if (string.IsNullOrEmpty(html) == false)
			{
                html = html.Replace("g:image_link", "image_link");
                html = html.Replace("g:sale_price", "sale_price");
                html = html.Replace("g:price", "price");

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
                                    decimal precioBase = decimal.Parse(juego.PrecioBase);
                                    decimal precioRebajado = decimal.Parse(juego.PrecioRebajado);

                                    int descuento = Calculadora.SacarDescuento(precioBase, precioRebajado);

                                    if (descuento > 0)
                                    {
                                        string nombre = WebUtility.HtmlDecode(juego.Nombre);

                                        string enlace = juego.Enlace;

                                        string imagen = juego.Imagen;

                                        JuegoDRM drm = JuegoDRM.NoEspecificado;

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

                                        //try
                                        //{
                                        //    BaseDatos.Tiendas.Comprobar.Resto(oferta, objeto, conexion);
                                        //}
                                        //catch (Exception ex)
                                        //{
                                        //    BaseDatos.Errores.Insertar.Ejecutar(Tienda.Generar().Id, ex);
                                        //}

                                        juegos2 += 1;

                                        try
                                        {
                                            BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, juegos2.ToString() + " ofertas detectadas", conexion);
                                        }
                                        catch (Exception ex)
                                        {
                                            BaseDatos.Errores.Insertar.Ejecutar(Tienda.Generar().Id, ex);
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
