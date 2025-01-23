#nullable disable

using Herramientas;
using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
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

		public static async Task BuscarOfertasAntiguo(SqlConnection conexion, IDecompiladores decompilador, ViewDataDictionary objeto = null)
		{
			BaseDatos.Admin.Actualizar.Tiendas(Tienda.Generar().Id, DateTime.Now, 0, conexion);

			int juegos2 = 0;

			int i = 1;
			while (i < 300)
			{
				string html = await Decompiladores.Estandar("https://www.gog.com/games/feed?format=xml&country=ES&currency=EUR&page=" + i.ToString());

				if (string.IsNullOrEmpty(html) == false)
				{
					XmlSerializer xml = new XmlSerializer(typeof(GOGJuegos));
					GOGJuegos listaJuegos = null;

					try
					{
						using (TextReader lector = new StringReader(html))
						{
							listaJuegos = (GOGJuegos)xml.Deserialize(lector);
						}
					}
					catch { }

					if (listaJuegos != null)
					{
						if (listaJuegos.Catalogo != null)
						{
							if (listaJuegos.Catalogo.Juegos.Count > 0)
							{
								foreach (GOGJuego juego in listaJuegos.Catalogo.Juegos)
								{
									int descuento = int.Parse(juego.Descuento);

									if (descuento > 0)
									{
                                        string nombre = WebUtility.HtmlDecode(juego.Nombre);

                                        string enlace = juego.Enlace;

                                        string slug = enlace;
                                        slug = slug.Replace("https://www.gog.com/en/game/", null);

                                        string imagen = "https:" + juego.ImagenVertical;

                                        string tempPrecio = juego.Precio;
                                        tempPrecio = tempPrecio.Replace("€", null);

                                        decimal precioRebajado = decimal.Parse(tempPrecio);

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

										try
										{
											BaseDatos.Tiendas.Comprobar.Resto(oferta, conexion, juego.Id, slug);
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

		public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador)
		{
			BaseDatos.Admin.Actualizar.Tiendas(Generar().Id, DateTime.Now, 0, conexion);

			int juegos2 = 0;

			int i = 1;
			int limite = 100;
			while (i < limite + 1)
			{
				string html = await Decompiladores.Estandar("https://catalog.gog.com/v1/catalog?limit=48&order=desc:trending&discounted=eq:true&productType=in:game,pack,dlc,extras&page=" + i.ToString() + "&countryCode=ES&locale=en-US&currencyCode=EUR");

				if (string.IsNullOrEmpty(html) == false)
				{
					GOGOfertas datos = JsonSerializer.Deserialize<GOGOfertas>(html);

					if (datos != null)
					{
						limite = datos.Paginas;

						foreach (var juego in datos.Juegos)
						{
							string precioBaseTexto = juego.Precios.PrecioBase;
							precioBaseTexto = precioBaseTexto.Replace("€", null);
							string precioRebajadoTexto = juego.Precios.PrecioRebajado;
							precioRebajadoTexto = precioRebajadoTexto.Replace("€", null);

							decimal precioBase = decimal.Parse(precioBaseTexto);
							decimal precioRebajado = decimal.Parse(precioRebajadoTexto);

							int descuento = Calculadora.SacarDescuento(precioBase, precioRebajado);

							if (descuento > 0)
							{
								string nombre = WebUtility.HtmlDecode(juego.Nombre);
								string enlace = "https://www.gog.com/en/game/" + juego.Slug;
								string imagen = juego.Imagen;

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

								try
								{
									if (juego.Tipo == "game" || juego.Tipo == "dlc")
									{
										BaseDatos.Tiendas.Comprobar.Resto(oferta, conexion, juego.Id, juego.Slug);
									}
									else
									{
										BaseDatos.Tiendas.Comprobar.Resto(oferta, conexion);
									}
								}
								catch (Exception ex)
								{
									BaseDatos.Errores.Insertar.Mensaje(Generar().Id, ex, conexion);
								}

								juegos2 += 1;

								try
								{
									BaseDatos.Admin.Actualizar.Tiendas(Generar().Id, DateTime.Now, juegos2, conexion);
								}
								catch (Exception ex)
								{
									BaseDatos.Errores.Insertar.Mensaje(Generar().Id, ex, conexion);
								}
							}
						}
					}
				}

				i += 1;
			}
		}
	}

	#region Ofertas (Antiguo)

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

	#endregion

	#region Ofertas (Nuevo)

	public class GOGOfertas
	{
		[JsonPropertyName("pages")]
		public int Paginas { get; set; }

		[JsonPropertyName("products")]
		public List<GOGOfertasJuego> Juegos { get; set; }
	}

	public class GOGOfertasJuego
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("slug")]
		public string Slug { get; set; }

		[JsonPropertyName("title")]
		public string Nombre { get; set; }

		[JsonPropertyName("coverHorizontal")]
		public string Imagen { get; set; }

		[JsonPropertyName("productType")]
		public string Tipo { get; set; }

		[JsonPropertyName("price")]
		public GOGOfertasJuegoPrecio Precios { get; set; }
	}

	public class GOGOfertasJuegoPrecio
	{
		[JsonPropertyName("final")]
		public string PrecioRebajado { get; set; }

		[JsonPropertyName("base")]
		public string PrecioBase { get; set; }
	}

	#endregion

	#region Datos

	public class GOGGalaxy
    {
        [JsonPropertyName("content_system_compatibility")]
        public GOGGalaxySistemas Sistemas { get; set; }
    }

    public class GOGGalaxySistemas
	{
        [JsonPropertyName("windows")]
        public bool Windows { get; set; }

        [JsonPropertyName("osx")]
        public bool Mac { get; set; }

        [JsonPropertyName("linux")]
        public bool Linux { get; set; }
    }

    public class GOGGalaxy2
    {
        [JsonPropertyName("_embedded")]
        public GOGGalaxy2Caracteristicas Caracteristicas { get; set; }

		[JsonPropertyName("_links")]
		public GOGGalaxy2Enlaces Enlaces { get; set; }

		[JsonPropertyName("releaseStatus")]
		public string SeHaLanzado { get; set; }
	}

    public class GOGGalaxy2Caracteristicas
    {
		[JsonPropertyName("localizations")]
		public List<GOGGalaxy2Idioma> Idiomas { get; set; }

		[JsonPropertyName("features")]
        public List<GOGGalaxy2Caracteristica> Datos { get; set; }

		[JsonPropertyName("properties")]
		public List<GOGGalaxy2Propiedad> Propiedades { get; set; }
	}

	public class GOGGalaxy2Idioma
	{
		[JsonPropertyName("_embedded")]
		public GOGGalaxy2IdiomaDatos Datos { get; set; }
	}

	public class GOGGalaxy2IdiomaDatos
	{
		[JsonPropertyName("language")]
		public GOGGalaxy2IdiomaDatosIdioma Idioma { get; set; }

		[JsonPropertyName("localizationScope")]
		public GOGGalaxy2IdiomaDatosTipo Tipo { get; set; }
	}

	public class GOGGalaxy2IdiomaDatosIdioma
	{
		[JsonPropertyName("code")]
		public string Codigo { get; set; }

		[JsonPropertyName("name")]
		public string Nombre { get; set; }
	}

	public class GOGGalaxy2IdiomaDatosTipo
	{
		[JsonPropertyName("type")]
		public string Nombre { get; set; }
	}

	public class GOGGalaxy2Caracteristica
	{
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

	public class GOGGalaxy2Propiedad
	{
		[JsonPropertyName("slug")]
		public string Slug { get; set; }
	}
	public class GOGGalaxy2Enlaces
	{
		[JsonPropertyName("isIncludedInGames")]
		public List<GOGGalaxy2Enlace> Listado { get; set; }
	}

	public class GOGGalaxy2Enlace
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("isSecret")]
		public bool Secreto { get; set; }

		[JsonPropertyName("releaseStatus")]
		public string Estado { get; set; }
	}

	#endregion
}
