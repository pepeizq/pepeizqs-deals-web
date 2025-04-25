//https://www.gog.com/games/ajax/filtered?mediaType=game&page=1&price=discounted&sort=popularity
//https://www.gog.com/games/feed?format=xml&country=ES&currency=EUR&page=0
//https://catalog.gog.com/v1/catalog?countryCode=ES&currencyCode=EUR&limit=20&locale=en-US&order=desc:score&page=1&productType=in:game,pack,dlc,extras&query=like:warcraft
//https://www.gog.com/api/products/1418669891
//https://www.gog.com/api/products/1418669891/prices?countryCode=ES&currency=EUR
//https://api.gog.com/products/1453375253?expand=downloads,expanded_dlcs,description,screenshots,videos,related_products,changelog
//https://reviews.gog.com/v1/products/1841195376/reviews?language=in:en-US&limit=5&order=desc:votes
//https://reviews.gog.com/v1/products/1841195376/averageRating

#nullable disable

using Herramientas;
using Juegos;
using Microsoft.VisualBasic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIs.GOG
{
	public static class Juego
	{
		public static string dominioImagenes = "https://images.gog-statics.com";

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

						if (string.IsNullOrEmpty(htmlAPI) == false) 
						{
							GOGJuegoAPI datos = JsonSerializer.Deserialize<GOGJuegoAPI>(htmlAPI);

							if (datos != null)
							{
								string htmlBusquedaAPI = await Decompiladores.Estandar("https://catalog.gog.com/v1/catalog?countryCode=ES&currencyCode=EUR&limit=20&locale=en-US&order=desc:score&page=1&productType=in:game,pack,dlc,extras&query=like:" + datos.Nombre);

								if (htmlBusquedaAPI != null)
								{
									GOGBusquedaAPI busqueda = JsonSerializer.Deserialize<GOGBusquedaAPI>(htmlBusquedaAPI);

									if (busqueda != null)
									{
										foreach (GOGBusquedaAPIResultado resultado in busqueda.Resultados)
										{
											if (resultado.Id == datos.Id.ToString())
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

												JuegoPrecio precio = new JuegoPrecio
												{
													Descuento = descuento,
													DRM = JuegoDRM.GOG,
													Precio = decimal.Parse(precioFormateado, CultureInfo.InvariantCulture),
													Moneda = JuegoMoneda.Euro,
													FechaDetectado = DateTime.Now,
													Enlace = enlacePrecio,
													Tienda = Tienda.Generar().Id
												};

												//------------------------------------------------------

												JuegoImagenes imagenes = new JuegoImagenes
												{
													Header_460x215 = resultado.CoverHorizontal,
													Capsule_231x87 = "https:" + datos.Imagenes.Logo,
													Library_600x900 = resultado.CoverVertical,
													Library_1920x620 = fondo
												};

												//------------------------------------------------------

												JuegoCaracteristicas caracteristicas = new JuegoCaracteristicas
												{
													Windows = datos.Sistemas.Windows,
													Mac = datos.Sistemas.Mac,
													Linux = datos.Sistemas.Linux
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

												JuegoMedia media = new JuegoMedia
												{
													Capturas = capturas
												};

												//------------------------------------------------------

												Juegos.Juego juego = new Juegos.Juego
												{
													IdSteam = 0,
													IdGog = datos.Id,
													Nombre = datos.Nombre,
													Media = media,
													Caracteristicas = caracteristicas,
													Imagenes = imagenes,
													PrecioActualesTiendas = new List<Juegos.JuegoPrecio> { precio },
													PrecioMinimosHistoricos = new List<Juegos.JuegoPrecio> { precio },
													FechaSteamAPIComprobacion = new DateTime(2000, 1, 1),
													MayorEdad = "false"
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

		public static async Task<List<JuegoIdioma>> GalaxyIdiomas(string id, List<JuegoIdioma> listadoIdiomas)
		{
			string html2 = await Decompiladores.Estandar("https://api.gog.com/v2/games/" + id);

			if (string.IsNullOrEmpty(html2) == false)
			{
				GOGGalaxy2 datos = JsonSerializer.Deserialize<GOGGalaxy2>(html2);

				if (datos != null)
				{
					List<JuegoIdioma> idiomas = Herramientas.Idiomas.GogSacarIdiomas(datos.Caracteristicas.Idiomas);

					if (listadoIdiomas == null)
					{
						listadoIdiomas = idiomas;
					}
					else
					{
						List<JuegoIdioma> listadoActualizar = listadoIdiomas;

						//Limpiar en un futuro

						int i = 0;
						while (i < 30)
						{
							int j = 0;
							bool borrar = false;

							foreach (var viejoIdioma in listadoActualizar)
							{
								if (viejoIdioma.DRM == JuegoDRM.GOG)
								{
									borrar = true;
									break;
								}

								j += 1;
							}

							if (borrar == true)
							{
								listadoActualizar.RemoveAt(j);
							}

							i += 1;
						}

						//----------------------------

						foreach (var nuevoIdioma in idiomas)
						{
							bool existe = false;

							foreach (var viejoIdioma in listadoActualizar)
							{
								if (viejoIdioma.DRM == nuevoIdioma.DRM && nuevoIdioma.Idioma == viejoIdioma.Idioma)
								{
									existe = true;

									viejoIdioma.Audio = nuevoIdioma.Audio;
									viejoIdioma.Texto = nuevoIdioma.Texto;

									break;
								}
							}

							if (existe == false)
							{
								listadoActualizar.Add(nuevoIdioma);
							}
						}

						return listadoActualizar;
					}
				}
			}

			return null;
		}

		public static async Task<string> CargarIdiomasAdmin(string id)
		{
			string html2 = await Decompiladores.Estandar("https://api.gog.com/v2/games/" + id);

			if (string.IsNullOrEmpty(html2) == false)
			{
				GOGGalaxy2 datos = JsonSerializer.Deserialize<GOGGalaxy2>(html2);

				if (datos != null)
				{
					return JsonSerializer.Serialize(datos.Caracteristicas.Idiomas);
				}
			}

			return null;
		}

		public static async Task<string> BuscarReferencia(string id)
		{
			string html2 = await Decompiladores.Estandar("https://api.gog.com/v2/games/" + id);

			if (string.IsNullOrEmpty(html2) == false)
			{
				GOGGalaxy2 datos = JsonSerializer.Deserialize<GOGGalaxy2>(html2);

				if (datos != null)
				{
					if (datos.SeHaLanzado == "unavailable")
					{
						if (datos.Enlaces != null)
						{
							if (datos.Enlaces.Listado != null)
							{
								if (datos.Enlaces.Listado.Count > 0)
								{
									foreach (var enlace in datos.Enlaces.Listado)
									{
										if (enlace.Estado == "default" && enlace.Id > 0)
										{
											return enlace.Id.ToString();
										}
									}
								}
							}
						}
					}
				}
			}

			return id;
		}
	}

    #region Clases

    public class GOGJuegoAPI
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("title")]
		public string Nombre { get; set; }

		[JsonPropertyName("game_type")]
		public string Tipo { get; set; }

		[JsonPropertyName("content_system_compatibility")]
		public GOGJuegoAPISistemas Sistemas { get; set; }

		[JsonPropertyName("images")]
		public GOGJuegoAPIImagenes Imagenes { get; set; }
	}

	public class GOGJuegoAPISistemas
	{
		[JsonPropertyName("windows")]
		public bool Windows { get; set; }

		[JsonPropertyName("osx")]
		public bool Mac { get; set; }

		[JsonPropertyName("linux")]
		public bool Linux { get; set; }
	}

	public class GOGJuegoAPIImagenes
	{
		[JsonPropertyName("background")]
		public string Background { get; set; }

		[JsonPropertyName("logo")]
		public string Logo { get; set; }

		[JsonPropertyName("icon")]
		public string Icon { get; set; }

		[JsonPropertyName("sidebarIcon")]
		public string SidebarIcon { get; set; }

		[JsonPropertyName("menuNotificationAv")]
		public string MenuNotificationAv { get; set; }
	}

	//----------------------------------------------

	public class GOGBusquedaAPI
	{
		[JsonPropertyName("products")]
		public List<GOGBusquedaAPIResultado> Resultados { get; set; }
	}

	public class GOGBusquedaAPIResultado
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("slug")]
		public string Slug { get; set; }

		[JsonPropertyName("screenshots")]
		public List<string> Capturas { get; set; }

		[JsonPropertyName("coverHorizontal")]
		public string CoverHorizontal { get; set; }

		[JsonPropertyName("coverVertical")]
		public string CoverVertical { get; set; }

		[JsonPropertyName("developers")]
		public List<string> Desarrolladores { get; set; }

		[JsonPropertyName("publishers")]
		public List<string> Publishers { get; set; }

		[JsonPropertyName("operatingSystems")]
		public List<string> Sistemas { get; set; }

		[JsonPropertyName("price")]
		public GOGBusquedaAPIResultadoPrecio Precio { get; set; }
	}

	public class GOGBusquedaAPIResultadoPrecio
	{
		[JsonPropertyName("discount")]
		public string Descuento { get; set; }

		[JsonPropertyName("final")]
		public string Cantidad { get; set; }
	}

    #endregion
}
