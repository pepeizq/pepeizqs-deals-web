﻿#nullable disable

using Herramientas;
using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Xml.Serialization;

namespace APIs.GOG
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "gog",
				Nombre = "GOG",
				ImagenLogo = "/imagenes/tiendas/gog_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/gog_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/gog_icono.ico",
				Color = "#7f3694",
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static async Task BuscarOfertas(ViewDataDictionary objeto = null)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				conexion.Open();

				int juegos2 = 0;

				int i = 1;
				while (i < 200)
				{
					string html = await Decompiladores.Estandar("https://www.gog.com/games/feed?format=xml&country=ES&currency=EUR&page=" + i.ToString());

					if (html != null)
					{
						XmlSerializer xml = new XmlSerializer(typeof(GOGJuegos));
						GOGJuegos listaJuegos = null;

						using (TextReader lector = new StringReader(html))
						{
							listaJuegos = (GOGJuegos)xml.Deserialize(lector);
						}

						if (listaJuegos != null)
						{
							if (listaJuegos.Catalogo != null)
							{
								if (listaJuegos.Catalogo.Juegos.Count > 0)
								{
									foreach (GOGJuego juego in listaJuegos.Catalogo.Juegos)
									{
										string nombre = WebUtility.HtmlDecode(juego.Nombre);

										string enlace = juego.Enlace;

										string imagen = "https:" + juego.ImagenVertical;

										string tempPrecio = juego.Precio;
										tempPrecio = tempPrecio.Replace("€", null);

										decimal precioRebajado = decimal.Parse(tempPrecio);

										int descuento = int.Parse(juego.Descuento);

										if (descuento > 0)
										{
											JuegoPrecio oferta = new JuegoPrecio
											{
												Nombre = nombre,
												Enlace = enlace,
												Imagen = imagen,
												Moneda = JuegoMoneda.Euro,
												Precio = precioRebajado,
												Descuento = descuento,
												Tienda = Generar().Id,
												DRM = JuegoDRM.GOG,
												FechaDetectado = DateTime.Now,
												FechaActualizacion = DateTime.Now
											};

											BaseDatos.Tiendas.Comprobar.Resto(oferta, objeto, conexion, juego.Id);

											juegos2 += 1;
											BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, juegos2.ToString() + " ofertas detectadas", conexion);
										}
									}
								}
								else
								{
									break;
								}
							}	
							else
							{
								break;
							}
						}
						else
						{
							break;
						}
					}

					i += 1;
				}
			}

			conexion.Dispose();
		}
	}

	[XmlRoot("catalogue")]
	public class GOGJuegos
	{
		[XmlElement("products")]
		public GOGJuegosCatalogo Catalogo { get; set; }
	}

	public class GOGJuegosCatalogo
	{
		[XmlElement("product")]
		public List<GOGJuego> Juegos { get; set; }
	}

	public class GOGJuego
	{
		[XmlElement("id")]
		public string Id { get; set; }

		[XmlElement("title")]
		public string Nombre { get; set; }

		[XmlElement("price")]
		public string Precio { get; set; }

		[XmlElement("discount")]
		public string Descuento { get; set; }

		[XmlElement("img_icon")]
		public string ImagenHorizontal { get; set; }

		[XmlElement("img_cover")]
		public string ImagenVertical { get; set; }

		[XmlElement("link")]
		public string Enlace { get; set; }
	}
}
