#nullable disable

using Herramientas;
using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Xml.Serialization;

namespace APIs.GamersGate
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "gamersgate",
				Nombre = "GamersGate",
				ImagenLogo = "/imagenes/tiendas/gamersgate_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/gamersgate_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/gamersgate_icono.ico",
				Color = "#232A3E",
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static string Referido(string enlace)
		{
			return enlace + "?aff=6704538";
			//return "https://www.anrdoezrs.net/click-101212497-15785566?url=" + enlace;
		}

		public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador, ViewDataDictionary objeto = null)
		{
			BaseDatos.Admin.Actualizar.Tiendas(Tienda.Generar().Id, DateTime.Now, 0, conexion);

			List<JuegoPrecio> ofertas = new List<JuegoPrecio>();

			string html = await Decompiladores.Estandar("https://www.gamersgate.com/feeds/products?country=DEU");

			if (string.IsNullOrEmpty(html) == false)
			{
                GamersGateJuegos listaJuegos = new GamersGateJuegos();
                XmlSerializer xml = new XmlSerializer(typeof(GamersGateJuegos));

                using (StringReader lector = new StringReader(html))
                {
                    listaJuegos = (GamersGateJuegos)xml.Deserialize(lector);
					lector.Dispose();
                }        

                if (listaJuegos != null)
				{
					if (listaJuegos.Juegos != null)
					{
						if (listaJuegos.Juegos.Count > 0)
						{
							foreach (GamersGateJuego juego in listaJuegos.Juegos)
							{
								string nombre = WebUtility.HtmlDecode(juego.Nombre);

								string enlace = juego.Enlace;

								string imagen = juego.ImagenGrande;

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

									if (juego.Fecha != null)
									{
										DateTime fechaTermina = DateTime.Parse(juego.Fecha);
										oferta.FechaTermina = fechaTermina;
									}

									ofertas.Add(oferta);
								}
							}
						}
					}
				}
			}

			int juegos2 = 0;

			if (ofertas.Count > 0)
			{
				foreach (var oferta in ofertas)
				{
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

    [XmlRoot("xml")]
    public class GamersGateJuegos
	{
		[XmlElement("item")]
		public List<GamersGateJuego> Juegos { get; set; }
	}

	public class GamersGateJuego
	{
		[XmlElement("title")]
		public string Nombre { get; set; }

		[XmlElement("link")]
		public string Enlace { get; set; }

		[XmlElement("price")]
		public string PrecioRebajado { get; set; }

		[XmlElement("srp")]
		public string PrecioBase { get; set; }

		[XmlElement("sku")]
		public string ID { get; set; }

		[XmlElement("boximg")]
		public string ImagenGrande { get; set; }

		[XmlElement("boximg_medium")]
		public string ImagenPequeña { get; set; }

		[XmlElement("drm")]
		public string DRM { get; set; }

		[XmlElement("publisher")]
		public string Desarrollador { get; set; }

		[XmlElement("discount_end")]
		public string Fecha { get; set; }

		[XmlElement("platforms")]
		public string Sistemas { get; set; }

		[XmlElement("type")]
		public string Tipo { get; set; }

		[XmlElement("state")]
		public string Estado { get; set; }
	}
}
