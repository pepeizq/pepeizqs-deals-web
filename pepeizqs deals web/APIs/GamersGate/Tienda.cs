﻿#nullable disable

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
		}

		public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador, ViewDataDictionary objeto = null)
		{
			BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, "0 ofertas detectadas", conexion);

			string html = await decompilador.Estandar("https://www.gamersgate.com/feeds/products?country=DEU");

			if (html != null)
			{
				XmlSerializer xml = new XmlSerializer(typeof(GamersGateJuegos));
				GamersGateJuegos listaJuegos = null;

				using (TextReader lector = new StringReader(html))
				{
					listaJuegos = (GamersGateJuegos)xml.Deserialize(lector);
				}

				if (listaJuegos != null)
				{
					if (listaJuegos.Juegos != null)
					{
						if (listaJuegos.Juegos.Count > 0)
						{
							int juegos2 = 0;

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

									try
									{
										BaseDatos.Tiendas.Comprobar.Resto(oferta, objeto, conexion);
									}
									catch (Exception ex)
									{
										BaseDatos.Errores.Insertar.Ejecutar(Tienda.Generar().Id + " Actualizando - " + ex.Message + " - " + DateTime.Now.ToString());
									}

									juegos2 += 1;

									try
									{
										BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, juegos2.ToString() + " ofertas detectadas", conexion);
									}
									catch (Exception ex)
									{
										BaseDatos.Errores.Insertar.Ejecutar(Tienda.Generar().Id + " Detectando - " + ex.Message + " - " + DateTime.Now.ToString());
									}
								}
							}
						}
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
