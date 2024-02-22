#nullable disable

using Herramientas;
using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Xml.Serialization;

namespace APIs.Voidu
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "voidu",
				Nombre = "Voidu",
				ImagenLogo = "/imagenes/tiendas/voidu_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/voidu_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/voidu_icono.ico",
				Color = "#f58331",
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador, ViewDataDictionary objeto = null)
		{
			BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, "0 ofertas detectadas", conexion);

			string html = await decompilador.Estandar("https://files.channable.com/FDPJ7_Cg8Pqi90kXICkb3g==.xml");

			if (string.IsNullOrEmpty(html) == false) 
			{
				XmlSerializer xml = new XmlSerializer(typeof(VoiduJuegos));
				VoiduJuegos listaJuegos = null;

				using (TextReader lector = new StringReader(html))
				{
					listaJuegos = (VoiduJuegos)xml.Deserialize(lector);
				}

				if (listaJuegos != null)
				{
					if (listaJuegos.Juegos != null)
					{
						if (listaJuegos.Juegos.Count > 0)
						{
							int juegos2 = 0;

							foreach (VoiduJuego juego in listaJuegos.Juegos)
							{
								decimal precioBase = decimal.Parse(juego.PrecioBase);
								decimal precioRebajado = decimal.Parse(juego.PrecioRebajado);

								int descuento = Calculadora.SacarDescuento(precioBase, precioRebajado);

								if (descuento > 0)
								{
									string nombre = WebUtility.HtmlDecode(juego.Nombre);

									string enlace = juego.Enlace;

                                    if (string.IsNullOrEmpty(enlace) == false)
                                    {
                                        if (enlace.Contains("?") == true)
										{
											int int1 = enlace.IndexOf("?");
											enlace = enlace.Remove(int1, enlace.Length - int1);
										}
                                    }

                                    string imagen = "vacio";

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

	[XmlRoot("items")]
	public class VoiduJuegos
	{
		[XmlElement("item")]
		public List<VoiduJuego> Juegos { get; set; }
	}

	public class VoiduJuego 
	{
		[XmlElement("title")]
		public string Nombre { get; set; }

		[XmlElement("link")]
		public string Enlace { get; set; }

		[XmlElement("price")]
		public string PrecioRebajado { get; set; }

		[XmlElement("price_old")]
		public string PrecioBase { get; set; }

		[XmlElement("id")]
		public string ID { get; set; }

		[XmlElement("drm")]
		public string DRM { get; set; }
	}
}
