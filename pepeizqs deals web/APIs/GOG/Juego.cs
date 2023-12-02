

//https://www.gog.com/games/ajax/filtered?mediaType=game&page=1&price=discounted&sort=popularity
//https://www.gog.com/games/feed?format=xml&country=ES&currency=EUR&page=0
//https://catalog.gog.com/v1/catalog?countryCode=ES&currencyCode=EUR&limit=20&locale=en-US&order=desc:score&page=1&productType=in:game,pack,dlc,extras&query=like:warcraft
//https://www.gog.com/api/products/1418669891
//https://www.gog.com/api/products/1418669891/prices?countryCode=ES&currency=EUR

#nullable disable

using Herramientas;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Globalization;

namespace APIs.GOG
{
	public static class Juego
	{
		public static async Task<Juegos.Juego> CargarDatos(string enlace)
		{
			if (Detectar(enlace) == false)
			{
				enlace = "https://www.gog.com/en/game/" + enlace;
			}

			string html = await Decompiladores.Estandar(enlace);

			if (html != null)
			{
				if (html.Contains(Strings.ChrW(34) + "sku" + Strings.ChrW(34)) == true)
				{
					int int1 = html.IndexOf(Strings.ChrW(34) + "sku" + Strings.ChrW(34));
					string temp1 = html.Remove(0, int1 + 6);

					int int2 = temp1.IndexOf(Strings.ChrW(34));
					string temp2 = temp1.Remove(0, int2 + 1);

					int int3 = temp2.IndexOf(Strings.ChrW(34));
					string temp3 = temp2.Remove(int3, temp2.Length - int3);

					string id = temp3.Trim();

					string fondo = string.Empty;

					if (html.Contains("_bg_crop_") == true)
					{
						int int4 = html.IndexOf("_bg_crop_");
						string temp4 = html.Remove(int4, html.Length - int4);

						int int5 = temp4.IndexOf("url(");
						string temp5 = temp4.Remove(0, int5 + 4);

						fondo = temp5.Trim() + ".jpg";
					}				

					if (id != null)
					{
						string htmlAPI = await Decompiladores.Estandar("https://www.gog.com/api/products/" + id);

						if (htmlAPI != null) 
						{
							GOGJuegoAPI datos = JsonConvert.DeserializeObject<GOGJuegoAPI>(htmlAPI);

							if (datos != null)
							{
								string htmlBusquedaAPI = await Decompiladores.Estandar("https://catalog.gog.com/v1/catalog?countryCode=ES&currencyCode=EUR&limit=20&locale=en-US&order=desc:score&page=1&productType=in:game,pack,dlc,extras&query=like:" + datos.Nombre);

								if (htmlBusquedaAPI != null)
								{
									GOGBusquedaAPI busqueda = JsonConvert.DeserializeObject<GOGBusquedaAPI>(htmlBusquedaAPI);

									if (busqueda != null)
									{
										foreach (GOGBusquedaAPIResultado resultado in busqueda.Resultados)
										{
											if (resultado.Id == datos.Id)
											{
												string descuentoTemp = resultado.Precio.Descuento;
												int descuento = 0;

												if (descuentoTemp != null)
												{
													descuentoTemp = descuentoTemp.Replace("%", null);
													descuentoTemp = descuentoTemp.Replace("-", null);
													descuento = int.Parse(descuentoTemp);
												}

												string precioFormateado = resultado.Precio.Cantidad;
												precioFormateado = precioFormateado.Replace("€", null);
												precioFormateado = precioFormateado.Replace(",", ".");

												string enlacePrecio = "https://www.gog.com/en/game/" + resultado.Slug;

												Juegos.JuegoPrecio precio = new Juegos.JuegoPrecio
												{
													Descuento = descuento,
													DRM = Juegos.JuegoDRM.GOG,
													Precio = decimal.Parse(precioFormateado, CultureInfo.InvariantCulture),
													Moneda = JuegoMoneda.Euro,
													FechaDetectado = DateTime.Now,
													Enlace = enlacePrecio,
													Tienda = "gog"
												};

												//------------------------------------------------------

												Juegos.JuegoImagenes imagenes = new Juegos.JuegoImagenes
												{
													Header_460x215 = resultado.CoverHorizontal,
													Capsule_231x87 = "https:" + datos.Imagenes.Logo,
													Library_600x900 = resultado.CoverVertical,
													Library_1920x620 = fondo
												};

												//------------------------------------------------------

												Juegos.JuegoCaracteristicas caracteristicas = new Juegos.JuegoCaracteristicas
												{
													Windows = datos.Sistemas.Windows,
													Mac = datos.Sistemas.Mac,
													Linux = datos.Sistemas.Linux,
													Desarrolladores = resultado.Desarrolladores,
													Publishers = resultado.Publishers
												};

												if (resultado.Sistemas.Count > 0)
												{
													foreach (var sistema in resultado.Sistemas)
													{
														if (sistema == "windows")
														{
															caracteristicas.Windows = true;
														}
														else if (sistema == "osx")
														{
															caracteristicas.Mac = true;
														}
														else if (sistema == "linux")
														{
															caracteristicas.Linux = true;
														}
													}
												}

												//------------------------------------------------------

												List<string> capturas = new List<string>();

												foreach (string captura in resultado.Capturas)
												{
													string captura2 = captura.Replace("_{formatter}", null);
													capturas.Add(captura2);
												}

												Juegos.JuegoMedia media = new Juegos.JuegoMedia
												{
													Capturas = capturas
												};

												//------------------------------------------------------

												Juegos.Juego juego = new Juegos.Juego
												{
													IdGog = int.Parse(datos.Id),
													Nombre = datos.Nombre,
													Media = media,
													Caracteristicas = caracteristicas,
													Imagenes = imagenes,
													PrecioActualesTiendas = new List<Juegos.JuegoPrecio> { precio },
													PrecioMinimosHistoricos = new List<Juegos.JuegoPrecio> { precio },
													FechaSteamAPIComprobacion = new DateTime(2000, 1, 1)
												};

												if (datos.Tipo == "dlc")
												{
													juego.Tipo = Juegos.JuegoTipo.DLC;
												}
												else
												{
													juego.Tipo = Juegos.JuegoTipo.Game;
												}

												return juego;
											}
										}
									}
								}
							}
						}
					}
				}
			}

			return null;
		}

		public static bool Detectar(string enlace)
		{
			bool resultado = false;

			if (enlace.Contains("https://www.gog.com/en/game/") == true)
			{
				resultado = true;
			}
			else if (enlace.Contains("https://www.gog.com/game/") == true)
			{
				resultado = true;
			}

			return resultado;
		}

		public static string LimpiarSlug(string enlace)
		{
			enlace = enlace.Replace("https://www.gog.com/en/game/", null);
			enlace = enlace.Replace("https://www.gog.com/game/", null);

			if (enlace.Contains("/") == true)
			{
				int int1 = enlace.IndexOf("/");
				enlace = enlace.Remove(int1, enlace.Length - int1);
			}

			if (enlace.Contains("?") == true)
			{
				int int1 = enlace.IndexOf("?");
				enlace = enlace.Remove(int1, enlace.Length - int1);
			}

			string id = enlace.Trim();

			return id;
		}
	}

	//----------------------------------------------

	public class GOGJuegoAPI
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("title")]
		public string Nombre { get; set; }

		[JsonProperty("game_type")]
		public string Tipo { get; set; }

		[JsonProperty("content_system_compatibility")]
		public GOGJuegoAPISistemas Sistemas { get; set; }

		[JsonProperty("images")]
		public GOGJuegoAPIImagenes Imagenes { get; set; }
	}

	public class GOGJuegoAPISistemas
	{
		[JsonProperty("windows")]
		public bool Windows { get; set; }

		[JsonProperty("osx")]
		public bool Mac { get; set; }

		[JsonProperty("linux")]
		public bool Linux { get; set; }
	}

	public class GOGJuegoAPIImagenes
	{
		[JsonProperty("background")]
		public string Background { get; set; }

		[JsonProperty("logo")]
		public string Logo { get; set; }

		[JsonProperty("icon")]
		public string Icon { get; set; }

		[JsonProperty("sidebarIcon")]
		public string SidebarIcon { get; set; }

		[JsonProperty("menuNotificationAv")]
		public string MenuNotificationAv { get; set; }
	}

	//----------------------------------------------

	public class GOGBusquedaAPI
	{
		[JsonProperty("products")]
		public List<GOGBusquedaAPIResultado> Resultados { get; set; }
	}

	public class GOGBusquedaAPIResultado
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("slug")]
		public string Slug { get; set; }

		[JsonProperty("screenshots")]
		public List<string> Capturas { get; set; }

		[JsonProperty("coverHorizontal")]
		public string CoverHorizontal { get; set; }

		[JsonProperty("coverVertical")]
		public string CoverVertical { get; set; }

		[JsonProperty("developers")]
		public List<string> Desarrolladores { get; set; }

		[JsonProperty("publishers")]
		public List<string> Publishers { get; set; }

		[JsonProperty("operatingSystems")]
		public List<string> Sistemas { get; set; }

		[JsonProperty("price")]
		public GOGBusquedaAPIResultadoPrecio Precio { get; set; }
	}

	public class GOGBusquedaAPIResultadoPrecio
	{
		[JsonProperty("discount")]
		public string Descuento { get; set; }

		[JsonProperty("final")]
		public string Cantidad { get; set; }
	}
}
