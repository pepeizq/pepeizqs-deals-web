#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIs.Allyouplay
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "allyouplay",
				Nombre = "Allyouplay",
				ImagenLogo = "/imagenes/tiendas/greenmangaming_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/greenmangaming_300x80.webp",
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

			int juegos2 = 0;

			int i = 1;
			while (i < 5)
			{
				HttpClient cliente = new HttpClient();
				cliente.BaseAddress = new Uri("https://www.allyouplay.com");
				cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				string peticionEnBruto = "{\"page_number\":" + i.ToString() + ",\"page_size\":24,\"specification_option_ids\":[4]}";

				HttpRequestMessage peticion = new HttpRequestMessage(HttpMethod.Post, "https://sefim.allyouplay.com/api-frontend/Catalog/GetCategory/69")
				{
					Content = new StringContent(peticionEnBruto, Encoding.UTF8, "application/json"),
					Headers = { { "POST", "/api-frontend/Catalog/GetCategory/69" } }
				}; 
			//(" HTTP/2\r\nHost: sefim.allyouplay.com\r\nUser-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:133.0) Gecko/20100101 Firefox/133.0\r\nAccept: application/json, text/plain, */*\r\nAccept-Language: es-ES,es;q=0.8,en-US;q=0.5,en;q=0.3\r\nAccept-Encoding: gzip, deflate, br, zstd\r\nContent-Type: application/json\r\nAuthorization: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOiIxNzMzOTUwNjM2IiwiZXhwIjoiMTczNDU1NTQzNiIsIkN1c3RvbWVySWQiOiI1NjIyMTk1MyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiMmNkODI0MzUtNjRiMS00YmIwLTg1OWYtZjMzMzk3YzU5NTJlIn0.EUOoslhtBROeaHxBr5qbshw9EazbSq9bz0t7TuwHH9M\r\nContent-Length: 63\r\nOrigin: https://allyouplay.com\r\nDNT: 1\r\nSec-GPC: 1\r\nConnection: keep-alive\r\nReferer: https://allyouplay.com/\r\nCookie: soundestID=20241211205715-T0d2Dm0lrpDbKX6EkVpfp6v7Gooz58jdx5h0gvBNE7wlpV6gZ; .Nop.Customer=2cd82435-64b1-4bb0-859f-f33397c5952e; .Nop.Culture=c%3Den-GB%7Cuic%3Den-GB\r\nSec-Fetch-Dest: empty\r\nSec-Fetch-Mode: cors\r\nSec-Fetch-Site: same-site")}
				HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

				string html = string.Empty;

				try
				{
					html = await respuesta.Content.ReadAsStringAsync();
				}
				catch { }

				if (string.IsNullOrEmpty(html) == false)
				{
					BaseDatos.Errores.Insertar.Mensaje("test", html);
					AllyouplayAPI datos = JsonSerializer.Deserialize<AllyouplayAPI>(html);

					if (datos != null)
					{
						//if (datos.Datos.Catalogo.Juegos.Count == 0)
						//{
						//	break;
						//}
						//else
						//{
						//	BaseDatos.Errores.Insertar.Mensaje("test", JsonSerializer.Serialize(datos));
						//	foreach (var juego in datos.Datos.Catalogo.Juegos)
						//	{

						//	}
						//}
					}
					else
					{
						break;
					}
				}

				i += 1;
			}
		}
	}

	public class AllyouplayAPI
	{
		[JsonPropertyName("category_model_dto")]
		public AllyouplayAPIDatos Datos { get; set; }

		[JsonPropertyName("template_view_path")]
		public string Nombre { get; set; }
	}

	public class AllyouplayAPIDatos
	{
		[JsonPropertyName("catalog_products_model")]
		public AllyouplayAPICatalogo Catalogo { get; set; }

		[JsonPropertyName("name")]
		public string Nombre { get; set; }
	}

	public class AllyouplayAPICatalogo
	{
		[JsonPropertyName("products")]
		public List<AllyouplayAPIJuego> Juegos { get; set; }

		[JsonPropertyName("total_pages")]
		public int Paginas { get; set; }
	}

	public class AllyouplayAPIJuego
	{
		[JsonPropertyName("name")]
		public string Nombre { get; set; }

		[JsonPropertyName("se_name")]
		public string Slug { get; set; }

		[JsonPropertyName("image_url")]
		public string Imagen { get; set; }

		[JsonPropertyName("product_price")]
		public AllyouplayAPIJuegoPrecios Precios { get; set; }
	}

	public class AllyouplayAPIJuegoPrecios
	{
		[JsonPropertyName("old_price_value")]
		public decimal PrecioBase { get; set; }

		[JsonPropertyName("price_value")]
		public decimal PrecioRebajado { get; set; }
	}
}
