﻿#nullable disable

using Herramientas;
using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Net;
using System.Xml.Serialization;
using Tiendas2;

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
				ImagenLogo = "/imagenes/tiendas/gamersgate_logo.png",
				Imagen300x80 = "/imagenes/tiendas/gamersgate_300x80.png",
				ImagenIcono = "/imagenes/tiendas/gamersgate_icono.ico",
				Color = "#232A3E",
				EnseñarAdmin = true
			};

			return tienda;
		}

		public static void BuscarOfertas(ViewDataDictionary objeto)
		{
            TiendasBaseDatos.ActualizarTiempo(Tienda.Generar().Id, DateTime.Now);

            Task<string> tarea = Decompiladores.Estandar("https://www.gamersgate.com/feeds/products?country=DEU");
			tarea.Wait();

			string html = tarea.Result;
		
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
							foreach (GamersGateJuego juego in listaJuegos.Juegos)
							{
								string nombre = WebUtility.HtmlDecode(juego.Titulo);
								
								string enlace = juego.Enlace;

								string imagen = juego.ImagenGrande;

								decimal precioBase = decimal.Parse(juego.PrecioBase);
								decimal precioRebajado = decimal.Parse(juego.PrecioDescontado);

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
										FechaDetectado = DateTime.Now
									};

									if (juego.Fecha != null)
									{
										DateTime fechaTermina = DateTime.Parse(juego.Fecha);
										oferta.FechaTermina = fechaTermina;
									}

									JuegoBaseDatos.ComprobarTienda(oferta, objeto);
								}	
							}
						}

						objeto["Mensaje"] = objeto["Mensaje"] + "GamersGate: " + listaJuegos.Juegos.Count.ToString() + " juegos detectados" + Environment.NewLine;
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
		public string Titulo { get; set; }

		[XmlElement("link")]
		public string Enlace { get; set; }

		[XmlElement("price")]
		public string PrecioDescontado { get; set; }

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
