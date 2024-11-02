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

			string html = await Decompiladores.Estandar("https://files.channable.com/FDPJ7_Cg8Pqi90kXICkb3g==.xml");

			if (string.IsNullOrEmpty(html) == false) 
			{
                VoiduJuegos listaJuegos = new VoiduJuegos();

                XmlSerializer xml = new XmlSerializer(typeof(VoiduJuegos));
                listaJuegos = (VoiduJuegos)xml.Deserialize(new StringReader(html));

				if (listaJuegos != null)
				{
					if (listaJuegos.Juegos != null)
					{
						if (listaJuegos.Juegos.Count > 0)
						{
							int juegos2 = 0;

							foreach (VoiduJuego juego in listaJuegos.Juegos)
							{
								string tempBase = juego.PrecioBase;
								string tempRebajado = juego.PrecioRebajado;

								tempBase = tempBase.Replace("NA", null);
								tempBase = tempBase.Trim();

								tempRebajado = tempRebajado.Replace("NA", null);
								tempRebajado = tempRebajado.Trim();

								if (string.IsNullOrEmpty(tempBase) == false && string.IsNullOrEmpty(tempRebajado) == false)
								{
									decimal precioBase = decimal.Parse(tempBase);
									decimal precioRebajado = decimal.Parse(tempRebajado);

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

											bool enlaceCorrecto = false;

                                            if (enlace.Contains("voidu.com/en/") == true)
                                            {
												enlaceCorrecto = true;
                                            }

                                            if (enlaceCorrecto == true)
											{
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
                                                    BaseDatos.Tiendas.Comprobar.Resto(oferta, conexion);
                                                }
                                                catch (Exception ex)
                                                {
                                                    BaseDatos.Errores.Insertar.Mensaje(Tienda.Generar().Id, ex, conexion);
                                                }

                                                juegos2 += 1;

                                                try
                                                {
                                                    BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, juegos2.ToString() + " ofertas detectadas", conexion);
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
