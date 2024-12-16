#nullable disable

using Herramientas;
using Juegos;
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
				ImagenLogo = "/imagenes/tiendas/allyouplay_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/allyouplay_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/allyouplay_icono.webp",
				Color = "#ff4081",
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
			int total = 10;
			while (i < total)
			{
				HttpClient cliente = new HttpClient();
				cliente.BaseAddress = new Uri("https://www.allyouplay.com");
				cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				string peticionEnBruto = "{\"page_number\":" + i.ToString() + ",\"page_size\":24,\"specification_option_ids\":[4]}";

				HttpRequestMessage peticion = new HttpRequestMessage(HttpMethod.Post, "https://sefim.allyouplay.com/api-frontend/Catalog/GetCategory/69")
				{
					Content = new StringContent(peticionEnBruto, Encoding.UTF8, "application/json"),
					Headers = { { "POST", "/api-frontend/Catalog/GetCategory/69" },
								{ "Host", "sefim.allyouplay.com" },
								{ "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:133.0) Gecko/20100101 Firefox/133.0" },
								{ "Accept", "application/json, text/plain, */*" },
								{ "Accept-Language", "es-ES,es;q=0.8,en-US;q=0.5,en;q=0.3"},
								{ "Authorization", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOiIxNzMzOTUwNjM2IiwiZXhwIjoiMTczNDU1NTQzNiIsIkN1c3RvbWVySWQiOiI1NjIyMTk1MyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiMmNkODI0MzUtNjRiMS00YmIwLTg1OWYtZjMzMzk3YzU5NTJlIn0.EUOoslhtBROeaHxBr5qbshw9EazbSq9bz0t7TuwHH9M" },
								{ "Origin", "https://allyouplay.com" },
								{ "DNT", "1" },
								{ "Sec-GPC", "1" },
								{ "Connection", "keep-alive" },
								{ "Referer", "https://allyouplay.com/" },
								{ "Cookie", "soundestID=20241211205715-T0d2Dm0lrpDbKX6EkVpfp6v7Gooz58jdx5h0gvBNE7wlpV6gZ; .Nop.Customer=2cd82435-64b1-4bb0-859f-f33397c5952e; .Nop.Culture=c%3Den-GB%7Cuic%3Den-GB" },
								{ "Sec-Fetch-Dest", "empty" },
								{ "Sec-Fetch-Mode", "cors" }
					}
				}; 

				HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

				string html = string.Empty;

				try
				{
					html = await respuesta.Content.ReadAsStringAsync();
				}
				catch { }

				if (string.IsNullOrEmpty(html) == false)
				{
					AllyouplayAPI datos = JsonSerializer.Deserialize<AllyouplayAPI>(html);

					if (datos != null)
					{
						total = datos.Datos.Catalogo.Paginas;

						foreach (var juego in datos.Datos.Catalogo.Juegos)
						{
							if (juego.Precios != null)
							{
								if (juego.Precios.PrecioRebajado != null && juego.Precios.PrecioBase != null)
								{
									decimal precioBase = juego.Precios.PrecioBase.Value;
									decimal precioRebajado = juego.Precios.PrecioRebajado.Value;

									int descuento = Calculadora.SacarDescuento(precioBase, precioRebajado);

									if (descuento > 0)
									{
										JuegoPrecio oferta = new JuegoPrecio
										{
											Nombre = juego.Nombre,
											Enlace = "https://allyouplay.com/" + juego.Slug,
											Imagen = juego.Imagen,
											Moneda = JuegoMoneda.Euro,
											Precio = precioRebajado,
											Descuento = descuento,
											Tienda = Generar().Id,
											DRM = JuegoDRM.Steam,
											FechaDetectado = DateTime.Now,
											FechaActualizacion = DateTime.Now
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

				i += 1;
			}
		}
	}

	public class AllyouplayAPI
	{
		[JsonPropertyName("category_model_dto")]
		public AllyouplayAPIDatos Datos { get; set; }
	}

	public class AllyouplayAPIDatos
	{
		[JsonPropertyName("catalog_products_model")]
		public AllyouplayAPICatalogo Catalogo { get; set; }
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
		public AllyouplayAPIJuegoPrecios? Precios { get; set; }
	}

	public class AllyouplayAPIJuegoPrecios
	{
		[JsonPropertyName("old_price_value")]
		public decimal? PrecioBase { get; set; }

		[JsonPropertyName("price_value")]
		public decimal? PrecioRebajado { get; set; }
	}
}
