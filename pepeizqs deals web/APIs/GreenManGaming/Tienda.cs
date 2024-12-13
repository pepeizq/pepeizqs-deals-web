#nullable disable

using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace APIs.GreenManGaming
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "greenmangaming",
				Nombre = "Green Man Gaming",
				ImagenLogo = "/imagenes/tiendas/greenmangaming_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/greenmangaming_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/greenmangaming_icono.ico",
				Color = "#97ff9a",
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

			return "https://greenmangaming.sjv.io/c/1382810/1219987/15105?u=" + enlace;

			//return enlace + "?tap_a=1964-996bbb&tap_s=608263-a851ee";
		}

		public static Tiendas2.Tienda GenerarGold()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "greenmangaminggold",
				Nombre = "Green Man Gaming Gold",
				ImagenLogo = "/imagenes/tiendas/greenmangaminggold_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/greenmangaminggold_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/greenmangaming_icono.ico",
				Color = "#97ff9a",
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador)
		{
			BaseDatos.Admin.Actualizar.Tiendas(Generar().Id, DateTime.Now, "0 ofertas detectadas", conexion);

			string html = await Decompiladores.Estandar("https://api.greenmangaming.com/api/productfeed/prices/current?cc=es&cur=eur&lang=en");

			if (html != null)
			{
                GreenManGamingJuegos listaJuegos = new GreenManGamingJuegos();

                XmlSerializer xml = new XmlSerializer(typeof(GreenManGamingJuegos));
                listaJuegos = (GreenManGamingJuegos)xml.Deserialize(new StringReader(html));

                if (listaJuegos != null)
				{
					if (listaJuegos.Juegos != null)
					{
						if (listaJuegos.Juegos.Count > 0)
						{
							int juegos2 = 0;

							foreach (GreenManGamingJuego juego in listaJuegos.Juegos)
							{
								string nombre = WebUtility.HtmlDecode(juego.Nombre);

								string enlace = juego.Enlace;

								string imagen = juego.Imagen;

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
										FechaActualizacion = DateTime.Now,
										SteamID = juego.SteamId
									};

									try
									{
										BaseDatos.Tiendas.Comprobar.Resto(oferta, conexion);
									}
									catch (Exception ex)
									{
                                        BaseDatos.Errores.Insertar.Mensaje(Generar().Id, ex, conexion);
                                    }

									juegos2 += 1;

									try
									{
										BaseDatos.Admin.Actualizar.Tiendas(Generar().Id, DateTime.Now, juegos2.ToString() + " ofertas detectadas", conexion);
									}
									catch (Exception ex)
									{
                                        BaseDatos.Errores.Insertar.Mensaje(Generar().Id, ex, conexion);
                                    }
								}
							}
						}
					}
				}
			}
		}

		public static async Task BuscarOfertasGold(SqlConnection conexion, IDecompiladores decompilador)
		{
			BaseDatos.Admin.Actualizar.Tiendas(GenerarGold().Id, DateTime.Now, "0 ofertas detectadas", conexion);

			int juegos2 = 0;

			int i = 0;
			while (i < 10000)
			{
				HttpClient cliente = new HttpClient();
				cliente.BaseAddress = new Uri("https://www.greenmangaming.com/");
				cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				string peticionEnBruto = "{\"requests\":[{\"indexName\":\"prod_ProductSearch_ES_LK\",\"params\":\"query=hot-deals-view-all&ruleContexts=%5B%22EUR%22%2C%22EUR_ES%22%2C%22ES%22%5D&filters=IsSellable%3Atrue%20AND%20AvailableRegions%3AES%20AND%20NOT%20ExcludeCountryCodes%3AES&hitsPerPage=24&distinct=true&analytics=false&clickAnalytics=true&maxValuesPerFacet=10&facets=%5B%22Franchise%22%2C%22IsEarlyAccess%22%2C%22Genre%22%2C%22PlatformName%22%2C%22PublisherName%22%2C%22SupportedVrs%22%2C%22Regions.ES.ReleaseDateStatus%22%2C%22Regions.ES.Mrp%22%2C%22Regions.ES.IsOnSaleVip%22%2C%22Regions.ES.IsXpOffer%22%2C%22DrmName%22%5D&tagFilters=&facetFilters=%5B%5B%22Regions.ES.IsXpOffer%3Atrue%22%5D%5D\"},{\"indexName\":\"prod_ProductSearch_ES_LK\",\"params\":\"query=hot-deals-view-all&ruleContexts=%5B%22EUR%22%2C%22EUR_ES%22%2C%22ES%22%5D&filters=IsSellable%3Atrue%20AND%20AvailableRegions%3AES%20AND%20NOT%20ExcludeCountryCodes%3AES&hitsPerPage=1&distinct=true&analytics=false&clickAnalytics=false&maxValuesPerFacet=10&page=0&attributesToRetrieve=%5B%5D&attributesToHighlight=%5B%5D&attributesToSnippet=%5B%5D&tagFilters=&facets=Regions.ES.IsXpOffer\"},{\"indexName\":\"prod_ProductSearch_ES_LK\",\"params\":\"query=hot-deals-view-all&ruleContexts=%5B%22EUR%22%2C%22EUR_ES%22%2C%22ES%22%5D&filters=IsSellable%3Atrue%20AND%20AvailableRegions%3AES%20AND%20NOT%20ExcludeCountryCodes%3AES%20AND%20IsDlc%3Atrue&hitsPerPage=24&distinct=true&analytics=false&clickAnalytics=true&maxValuesPerFacet=10&highlightPreTag=__ais-highlight__&highlightPostTag=__%2Fais-highlight__&page=0&facets=%5B%22Franchise%22%2C%22IsEarlyAccess%22%2C%22Genre%22%2C%22PlatformName%22%2C%22PublisherName%22%2C%22SupportedVrs%22%2C%22Regions.ES.ReleaseDateStatus%22%2C%22Regions.ES.Mrp%22%2C%22Regions.ES.IsOnSaleVip%22%2C%22Regions.ES.IsXpOffer%22%2C%22DrmName%22%5D&tagFilters=&facetFilters=%5B%5B%22Regions.ES.IsXpOffer%3Atrue%22%5D%5D\"},{\"indexName\":\"prod_ProductSearch_ES_LK\",\"params\":\"query=hot-deals-view-all&ruleContexts=%5B%22EUR%22%2C%22EUR_ES%22%2C%22ES%22%5D&filters=IsSellable%3Atrue%20AND%20AvailableRegions%3AES%20AND%20NOT%20ExcludeCountryCodes%3AES%20AND%20IsDlc%3Atrue&hitsPerPage=1&distinct=true&analytics=false&clickAnalytics=false&maxValuesPerFacet=10&highlightPreTag=__ais-highlight__&highlightPostTag=__%2Fais-highlight__&page=0&attributesToRetrieve=%5B%5D&attributesToHighlight=%5B%5D&attributesToSnippet=%5B%5D&tagFilters=&facets=Regions.ES.IsXpOffer\"},{\"indexName\":\"prod_ProductSearch_ES_LK\",\"params\":\"query=hot-deals-view-all&ruleContexts=%5B%22EUR%22%2C%22EUR_ES%22%2C%22ES%22%5D&filters=IsSellable%3Atrue%20AND%20AvailableRegions%3AES%20AND%20NOT%20ExcludeCountryCodes%3AES%20AND%20IsDlc%3Afalse&hitsPerPage=24&distinct=true&analytics=false&clickAnalytics=true&maxValuesPerFacet=10&highlightPreTag=__ais-highlight__&highlightPostTag=__%2Fais-highlight__&page=0&facets=%5B%22Franchise%22%2C%22IsEarlyAccess%22%2C%22Genre%22%2C%22PlatformName%22%2C%22PublisherName%22%2C%22SupportedVrs%22%2C%22Regions.ES.ReleaseDateStatus%22%2C%22Regions.ES.Mrp%22%2C%22Regions.ES.IsOnSaleVip%22%2C%22Regions.ES.IsXpOffer%22%2C%22DrmName%22%5D&tagFilters=&facetFilters=%5B%5B%22Regions.ES.IsXpOffer%3Atrue%22%5D%5D\"},{\"indexName\":\"prod_ProductSearch_ES_LK\",\"params\":\"query=hot-deals-view-all&ruleContexts=%5B%22EUR%22%2C%22EUR_ES%22%2C%22ES%22%5D&filters=IsSellable%3Atrue%20AND%20AvailableRegions%3AES%20AND%20NOT%20ExcludeCountryCodes%3AES%20AND%20IsDlc%3Afalse&hitsPerPage=1&distinct=true&analytics=false&clickAnalytics=false&maxValuesPerFacet=10&highlightPreTag=__ais-highlight__&highlightPostTag=__%2Fais-highlight__&page=0&attributesToRetrieve=%5B%5D&attributesToHighlight=%5B%5D&attributesToSnippet=%5B%5D&tagFilters=&facets=Regions.ES.IsXpOffer\"}]}";

				if (peticionEnBruto.Contains("&page=0") == true)
				{
					int int1 = peticionEnBruto.LastIndexOf("&page=0");
					string temp1 = peticionEnBruto.Remove(int1, peticionEnBruto.Length - int1);

					int int2 = temp1.LastIndexOf("&page=0");
					peticionEnBruto = peticionEnBruto.Remove(int2, 7);
					peticionEnBruto = peticionEnBruto.Insert(int2, "&page=" + i.ToString());
				}
				
				HttpRequestMessage peticion = new HttpRequestMessage(HttpMethod.Post, "https://sczizsp09z-dsn.algolia.net/1/indexes/*/queries?x-algolia-agent=Algolia%20for%20JavaScript%20(4.5.1)%3B%20Browser%20(lite)%3B%20instantsearch.js%20(4.8.3)%3B%20JS%20Helper%20(3.2.2)&x-algolia-api-key=3bc4cebab2aa8cddab9e9a3cfad5aef3&x-algolia-application-id=SCZIZSP09Z");
				peticion.Content = new StringContent(peticionEnBruto, Encoding.UTF8, "application/json");

				HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

				string html = string.Empty;

				try
				{
					html = await respuesta.Content.ReadAsStringAsync();
				}
				catch { }

				if (string.IsNullOrEmpty(html) == false)
				{
					GreenManGamingGold datos = JsonSerializer.Deserialize<GreenManGamingGold>(html);

					if (datos != null)
					{
						if (datos.Resultados[4].Juegos.Count == 0)
						{
							break;
						}
						else
						{
							foreach (var juego in datos.Resultados[4].Juegos)
							{
								decimal precioBase = juego.Regiones.ES.PrecioBase;
								decimal precioRebajado = juego.Regiones.ES.PrecioRebajado;

								int descuento = Calculadora.SacarDescuento(precioBase, precioRebajado);

								if (descuento > 0)
								{
									JuegoDRM drm = JuegoDRM2.Traducir(juego.DRM, Generar().Id);

									JuegoPrecio oferta = new JuegoPrecio
									{
										Nombre = WebUtility.HtmlDecode(juego.Nombre),
										Enlace = "https://www.greenmangaming.com" + juego.Enlace,
										Imagen = "https://images.greenmangaming.com" + juego.Imagen,
										Moneda = JuegoMoneda.Euro,
										Precio = precioRebajado,
										Descuento = descuento,
										Tienda = GenerarGold().Id,
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
										BaseDatos.Errores.Insertar.Mensaje(GenerarGold().Id, ex, conexion);
									}

									juegos2 += 1;

									try
									{
										BaseDatos.Admin.Actualizar.Tiendas(GenerarGold().Id, DateTime.Now, juegos2.ToString() + " ofertas detectadas", conexion);
									}
									catch (Exception ex)
									{
										BaseDatos.Errores.Insertar.Mensaje(GenerarGold().Id, ex, conexion);
									}
								}									
							}
						}
					}
				}
				else
				{
					break;
				}

				i += 1;
			}
		}
	}

	#region Normal

	[XmlRoot("products")]
	public class GreenManGamingJuegos
	{
		[XmlElement("product")]
		public List<GreenManGamingJuego> Juegos { get; set; }
	}

	public class GreenManGamingJuego
	{
		[XmlElement("product_name")]
		public string Nombre { get; set; }

		[XmlElement("deep_link")]
		public string Enlace { get; set; }

		[XmlElement("image_url")]
		public string Imagen { get; set; }

		[XmlElement("price")]
		public string PrecioRebajado { get; set; }

		[XmlElement("rrp_price")]
		public string PrecioBase { get; set; }

		[XmlElement("drm")]
		public string DRM { get; set; }

		[XmlElement("steamapp_id")]
		public string SteamId { get; set; }
	}

	#endregion

	#region Gold

	public class GreenManGamingGold
	{
		[JsonPropertyName("results")]
		public List<GreenManGamingGoldResultado> Resultados { get; set; }
	}

	public class GreenManGamingGoldResultado
	{
		[JsonPropertyName("hits")]
		public List<GreenManGamingGoldJuego> Juegos { get; set; }
	}

	public class GreenManGamingGoldJuego
	{
		[JsonPropertyName("DisplayName")]
		public string Nombre { get; set; }

		[JsonPropertyName("Url")]
		public string Enlace { get; set; }

		[JsonPropertyName("ImageUrl")]
		public string Imagen { get; set; }

		[JsonPropertyName("DrmName")]
		public string DRM { get; set; }

		[JsonPropertyName("Regions")]
		public GreenManGamingGoldJuegoRegiones Regiones { get; set; }
	}

	public class GreenManGamingGoldJuegoRegiones
	{
		[JsonPropertyName("ES")]
		public GreenManGamingGoldJuegoPrecios ES { get; set; }
	}

	public class GreenManGamingGoldJuegoPrecios
	{
		[JsonPropertyName("Mrp")]
		public decimal PrecioRebajado { get; set; }

		[JsonPropertyName("Rrp")]
		public decimal PrecioBase { get; set; }
	}

	#endregion
}
