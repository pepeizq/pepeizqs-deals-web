//Luna Amazon
//https://catalog.gog.com/v1/filtered-catalog?limit=48&order=desc:discount&productType=in:game,pack,dlc,extras&page=2&pageId=2f70726f6d6f2f706c61792d6f6e2d6c756e61&sectionId=7efd2a6a-0831-43af-bc1f-616509912d24&countryCode=ES&locale=en-US&currencyCode=EUR

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

		public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador, ViewDataDictionary objeto = null)
		{
			BaseDatos.Admin.Actualizar.Tiendas(Tienda.Generar().Id, DateTime.Now, "0 ofertas detectadas", conexion);

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
											BaseDatos.Admin.Actualizar.Tiendas(Tienda.Generar().Id, DateTime.Now, juegos2.ToString() + " ofertas detectadas", conexion);
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

		public static async Task<JuegoGalaxyGOG> GalaxyDatos(string id)
		{
            JuegoGalaxyGOG galaxy = new JuegoGalaxyGOG();
            string html = await Decompiladores.Estandar("https://api.gog.com/products/" + id + "?expand=downloads,expanded_dlcs,description,screenshots,videos,related_products,changelog");

            if (string.IsNullOrEmpty(html) == false)
			{
                GOGGalaxy datos = JsonSerializer.Deserialize<GOGGalaxy>(html);

				if (datos != null)
				{
					galaxy.Windows = datos.Sistemas.Windows;
					galaxy.Mac = datos.Sistemas.Mac;
					galaxy.Linux = datos.Sistemas.Linux;
				}
            }

            string html2 = await Decompiladores.Estandar("https://api.gog.com/v2/games/" + id);

            if (string.IsNullOrEmpty(html2) == false)
            {
                GOGGalaxy2 datos = JsonSerializer.Deserialize<GOGGalaxy2>(html2);

                if (datos != null)
                {
                    foreach (var caracteristica in datos.Caracteristicas.Datos)
					{
						if (caracteristica.Id == "achievements")
						{
							galaxy.Logros = true;
						}

						if (caracteristica.Id == "cloud_saves")
						{
							galaxy.GuardadoNube = true;
						}
					}

					foreach (var propiedad in datos.Caracteristicas.Propiedades)
					{
						if (propiedad.Slug == "good-old-game")
						{
							galaxy.Preservacion = true;
						}
					}
                }
            }

			galaxy.Fecha = DateTime.Now;

			return galaxy;
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
    }

    public class GOGGalaxy2Caracteristicas
    {
        [JsonPropertyName("features")]
        public List<GOGGalaxy2Caracteristica> Datos { get; set; }

		[JsonPropertyName("properties")]
		public List<GOGGalaxy2Propiedad> Propiedades { get; set; }
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
}
