#nullable disable

using Herramientas;
using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIs.Battlenet
{
    public class Tienda
    {
        public static Tiendas2.Tienda Generar()
        {
            Tiendas2.Tienda tienda = new Tiendas2.Tienda
            {
                Id = "battlenet",
                Nombre = "Battlenet Store",
                ImagenLogo = "/imagenes/tiendas/battlenet_logo.webp",
                Imagen300x80 = "/imagenes/tiendas/battlenet_300x80.webp",
                ImagenIcono = "/imagenes/tiendas/battlenet_icono.webp",
                Color = "#005aad",
                AdminEnseñar = true,
                AdminInteractuar = true
            };

            return tienda;
        }

        private static List<string> ListaSlugs()
        {
            List<string> slugs = [
				"avowed",
				"call-of-duty-black-ops-4",
				"call-of-duty-black-ops-6",
				"call-of-duty-black-ops-cold-war",
				"call-of-duty-modern-warfare",
				"call-of-duty-modern-warfare-2-campaign-remastered",
				"call-of-duty-modern-warfare-ii", 
				"call-of-duty-modern-warfare-iii",
				"call-of-duty-vanguard",
				"crash-bandicoot-4",
				"diablo",
                "diablo_ii_resurrected",
				"diablo-iv",
				"doom-the-dark-ages",
				"sea-of-thieves",
				"warcraft-orcs-and-humans",
				"warcraft-ii-battle-net-edition"];

            return slugs;
        }

        public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador, ViewDataDictionary objeto = null)
        {
			BaseDatos.Admin.Actualizar.Tiendas(Generar().Id, DateTime.Now, 0, conexion);

			int juegos2 = 0;

			List<string> listaSlugs = ListaSlugs();

			foreach (var slug in listaSlugs)
			{
				string html = await Decompiladores.Estandar("https://eu.shop.battle.net/api/product/" + slug + "?platform=Web&locale=en-us");

				if (string.IsNullOrEmpty(html) == false)
				{
					BattlenetJuego juegobattle = null;

					try
					{
						juegobattle = JsonSerializer.Deserialize<BattlenetJuego>(html);
					}
					catch { }

					if (juegobattle != null)
					{
						string nombreFamilia = juegobattle.Nombre;

						if (juegobattle.Productos != null)
						{
							foreach (var producto in juegobattle.Productos)
							{
								if (producto.Precio.Precio != null)
								{
									string textoPrecioRebajado = producto.Precio.Precio.PrecioRebajado;

									if (string.IsNullOrEmpty(textoPrecioRebajado) == false)
									{
										textoPrecioRebajado = textoPrecioRebajado.Replace("€", null);
										textoPrecioRebajado = textoPrecioRebajado.Replace("EUR", null);
										textoPrecioRebajado = textoPrecioRebajado.Trim();
									}

									string textoPrecioBase = producto.Precio.Precio.PrecioBase;

									if (string.IsNullOrEmpty(textoPrecioBase) == false)
									{
										textoPrecioBase = textoPrecioBase.Replace("€", null);
										textoPrecioBase = textoPrecioBase.Replace("EUR", null);
										textoPrecioBase = textoPrecioBase.Trim();
									}

									if (string.IsNullOrEmpty(textoPrecioRebajado) == false && string.IsNullOrEmpty(textoPrecioBase) == false)
									{
										decimal precioRebajado = decimal.Parse(textoPrecioRebajado);
										decimal precioBase = decimal.Parse(textoPrecioBase);

										int descuento = Calculadora.SacarDescuento(precioBase, precioRebajado);

										if (descuento > 0)
										{
											string nombre = nombreFamilia;

											if (string.IsNullOrEmpty(producto.Subnombre) == false)
											{
												if (producto.Subnombre != nombre)
												{
													nombre = nombre + " - " + producto.Subnombre;
												}
											}

											nombre = WebUtility.HtmlDecode(nombre);

											string enlace = "https://eu.shop.battle.net/en-us/product/" + slug + "?p=" + producto.Id;

											string imagen = producto.Imagen;

											if (string.IsNullOrEmpty(imagen) == false)
											{
												if (imagen.Contains("https:") == false)
												{
													imagen = "https:" + imagen;
												}
											}

											JuegoDRM drm = JuegoDRM.BattleNet;

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
						}
					}
				}
			}
		}
    }

    #region Clases

    public class BattlenetJuego
    {
        [JsonPropertyName("title")]
        public string Nombre { get; set; }

        [JsonPropertyName("products")]
        public List<BattlenetJuegoProducto> Productos { get; set; }
    }

    public class BattlenetJuegoProducto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("quantificationLabel")]
        public string Subnombre { get; set; }

        [JsonPropertyName("imageUrl")]
        public string Imagen { get; set; }

        [JsonPropertyName("priceInfo")]
        public BattlenetJuegoProductoPrecio Precio { get; set; }
    }

    public class BattlenetJuegoProductoPrecio
    {
        [JsonPropertyName("price")]
        public BattlenetJuegoProductoPrecio2 Precio { get; set; }
    }

    public class BattlenetJuegoProductoPrecio2
    {
        [JsonPropertyName("fullAmount")]
        public string PrecioBase { get; set; }

        [JsonPropertyName("discountAmount")]
        public string PrecioRebajado { get; set; }
    }

    #endregion
}
