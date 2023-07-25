//https://www.humblebundle.com/store/api/recommend?recommendation_attempt=1&machine_name=americanfugitive_storefront
//https://www.humblebundle.com/store/api/search?sort=discount&filter=all&search=american&request=1
//https://www.humblebundle.com/api/v1/trove/chunk?index=4
//https://www.humblebundle.com/api/v1/subscriptions/humble_monthly/subscription_products_with_gamekeys/
//https://www.humblebundle.com/api/v1/subscriptions/humble_monthly/history?from_product=july_2020_choice
//https://www.humblebundle.com/store/api/lookup?products[]=sonic-mania&request=1

//https://scrapfly.io/
//https://www.zenrows.com

#nullable disable

using Herramientas;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Net;

namespace APIs.Humble
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "humblestore",
				Nombre = "Humble Store",
				ImagenLogo = "/imagenes/tiendas/humblestore_logo.png",
				Imagen300x80 = "/imagenes/tiendas/humblestore_300x80.png",
				ImagenIcono = "/imagenes/tiendas/humblestore_icono.ico",
				Color = "#ea9192",
				EnseñarAdmin = true
			};

			return tienda;
		}

		public static Tiendas2.Tienda GenerarChoice()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "humblechoice",
				Nombre = "Humble Choice",
				ImagenLogo = "/imagenes/tiendas/humblechoice_logo.png",
				Imagen300x80 = "/imagenes/tiendas/humblechoice_300x80.png",
				ImagenIcono = "/imagenes/tiendas/humblestore_icono.ico",
				Color = "#ea9192",
				EnseñarAdmin = false
			};

			return tienda;
		}

		public static void BuscarOfertas(ViewDataDictionary objeto, string html)
		{
			int numPaginas = 0;

			//Task<string> htmlPaginas2 = Decompiladores.Estandar("https://api.scrapfly.io/scrape?key=scp-live-9d23586400df4474a28c9075ea0b5ebc&url=https%3A%2F%2Fwww.humblebundle.com%2Fstore%2Fapi%2Fsearch%3Ffilter%3Donsale%26sort%3Ddiscount%26request%3D2%26page_size%3D20%26page%3D0&tags=player%2Cproject%3Adefault&country=es");
			//htmlPaginas2.Wait();

			//string htmlPaginas = Decompiladores.GZipFormato("https://www.humblebundle.com/store/api/search?filter=onsale&sort=discount&request=2&page_size=20&page=1");

			//if (htmlPaginas != null)
			//{
			//	Scraper scraper = JsonConvert.DeserializeObject<Scraper>(htmlPaginas);

			//	if (scraper != null)
			//	{
			//		HumblePaginas paginas = JsonConvert.DeserializeObject<HumblePaginas>(scraper.Resultado.Contenido);

			//		if (paginas != null)
			//		{
			//			numPaginas = int.Parse(paginas.Numero);
			//		}
			//	}			
			//}

			//if (numPaginas > 0)
			//{
			//	int i = 0;
			//	while (i <= numPaginas)
			//	{
			//		Task<string> html2 = Decompiladores.Estandar("https://api.scrapfly.io/scrape?key=scp-live-9d23586400df4474a28c9075ea0b5ebc&url=https%3A%2F%2Fwww.humblebundle.com%2Fstore%2Fapi%2Fsearch%3Ffilter%3Donsale%26sort%3Ddiscount%26request%3D2%26page_size%3D20%26page%3D" + i.ToString() + "&tags=project%3Adefault&country=es&asp=true");
			//		html2.Wait();

			//		string html = html2.Result;

			//		if (html != null)
			//		{
			//			Scraper scraper = JsonConvert.DeserializeObject<Scraper>(html);

			//			if (scraper != null)
			//			{
			HumbleJuegos juegos = JsonConvert.DeserializeObject<HumbleJuegos>(html);

			if (juegos != null)
			{
				foreach (HumbleJuego juego in juegos.Resultados)
				{
					string nombre = WebUtility.HtmlDecode(juego.Titulo);

					string imagen = juego.ImagenGrande;

					string enlace = "https://www.humblebundle.com/store/" + juego.Enlace;

					double precioChoice = 0;
					double precioRebajado = 0;

					if (juego.PrecioRebajado != null)
					{

					}
				}
			}

            objeto["Mensaje"] = objeto["Mensaje"] + "Humble Store: " + juegos.Resultados.Count();


            //			}
            //		}

            //		i += 1;
            //	}
            //}

            //objeto["Mensaje"] = objeto["Mensaje"] + "Humble Store: " + numPaginas.ToString() + " páginas detectadas" + Environment.NewLine;
        }

		private static double DescuentoChoice(double cantidad)
		{
			double descuento = 0;

			if (cantidad == 0.1)
			{
				descuento = 0.2;
			}
			else if (cantidad == 0.05)
			{
				descuento = 0.15;
			}
			else if (cantidad == 0.03)
			{
				descuento = 0.13;
			}
			else if (cantidad == 0.02)
			{
				descuento = 0.12;
			}
			else if (cantidad == 0)
			{
				descuento = 0.10;
			}

			return descuento;
		}
	}

	public class Scraper
	{
		[JsonProperty("result")]
		public ScraperContenido Resultado { get; set; }
	}

	public class ScraperContenido
	{
		[JsonProperty("content")]
		public string Contenido { get; set; }
	}

	public class HumblePaginas
	{
		[JsonProperty("num_pages")]
		public string Numero { get; set; }
	}

	public class HumbleJuegos
	{
		[JsonProperty("results")]
		public List<HumbleJuego> Resultados { get; set; }
	}

	public class HumbleJuego
	{
		[JsonProperty("human_name")]
		public string Titulo { get; set; }

		[JsonProperty("machine_name")]
		public string Id { get; set; }

		[JsonProperty("standard_carousel_image")]
		public string ImagenPequeña { get; set; }

		[JsonProperty("large_capsule")]
		public string ImagenGrande { get; set; }

		[JsonProperty("current_price")]
		public HumbleJuegoPrecio PrecioRebajado { get; set; }

		[JsonProperty("full_price")]
		public HumbleJuegoPrecio PrecioBase { get; set; }

		[JsonProperty("human_url")]
		public string Enlace { get; set; }

		[JsonProperty("delivery_methods")]
		public List<string> DRM { get; set; }

		[JsonProperty("platforms")]
		public List<string> Sistemas { get; set; }

		[JsonProperty("sale_end")]
		public double FechaTermina { get; set; }

		[JsonProperty("rewards_split")]
		public double DescuentoChoice { get; set; }

		[JsonProperty("incompatible_features")]
		public List<string> CosasIncompatibles { get; set; }
	}

	public class HumbleJuegoPrecio
	{
		[JsonProperty("currency")]
		public string Moneda { get; set; }

		[JsonProperty("amount")]
		public string Cantidad { get; set; }
	}
}
