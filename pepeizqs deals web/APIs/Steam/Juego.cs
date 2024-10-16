﻿#nullable disable

using Herramientas;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIs.Steam
{
	public static class Juego
	{
		public static string dominioImagenes = "https://cdn.cloudflare.steamstatic.com";

		public static async Task<Juegos.Juego> CargarDatos(string enlace)
		{
			string id = LimpiarID(enlace);

			string html = await Decompiladores.Estandar("https://store.steampowered.com/api/appdetails/?appids=" + id + "&l=english");

			if (string.IsNullOrEmpty(html) == false) 
			{
				int int1 = html.IndexOf(":");

				html = html.Remove(0, int1 + 1);
				html = html.Remove(html.Length - 1, 1);

				SteamJuegoAPI datos = null;

				try
				{
					datos = JsonSerializer.Deserialize<SteamJuegoAPI>(html);
				}
				catch { }

				if (datos != null)
				{
					Juegos.JuegoImagenes imagenes = new Juegos.JuegoImagenes();

					if (datos.Datos != null) 
					{
						imagenes.Header_460x215 = datos.Datos.ImagenHeader_460x215;
						imagenes.Capsule_231x87 = datos.Datos.ImagenCapsule_231x87;
						imagenes.Logo = dominioImagenes + "/steam/apps/" + datos.Datos.Id + "/logo.png";
						imagenes.Library_1920x620 = dominioImagenes + "/steam/apps/" + datos.Datos.Id + "/library_hero.jpg";
						imagenes.Library_600x900 = dominioImagenes + "/steam/apps/" + datos.Datos.Id + "/library_600x900.jpg";
					}

					//------------------------------------------------------

					Juegos.JuegoCaracteristicas caracteristicas = new Juegos.JuegoCaracteristicas();

					if (datos.Datos != null)
					{
						caracteristicas.Windows = datos.Datos.Sistemas.Windows;
						caracteristicas.Mac = datos.Datos.Sistemas.Mac;
						caracteristicas.Linux = datos.Datos.Sistemas.Linux;
						caracteristicas.Desarrolladores = datos.Datos.Desarrolladores;
						caracteristicas.Publishers = datos.Datos.Publishers;

						if (string.IsNullOrEmpty(datos.Datos.DescripcionCorta) == false)
						{
							caracteristicas.Descripcion = datos.Datos.DescripcionCorta;
						}
					}

					//------------------------------------------------------

					Juegos.JuegoMedia media = new Juegos.JuegoMedia();

					if (datos.Datos != null)
					{
						if (datos.Datos.Capturas != null)
						{
							if (datos.Datos.Capturas.Count > 0)
							{
								List<string> capturas = new List<string>();
								List<string> miniaturas = new List<string>();

								foreach (SteamJuegoAPICaptura captura in datos.Datos.Capturas)
								{
									capturas.Add(captura.Enlace);
									miniaturas.Add(captura.Miniatura);
								}

								media.Capturas = capturas;
								media.Miniaturas = miniaturas;
							}
						}

						if (datos.Datos.Videos != null)
						{
							if (datos.Datos.Videos.Count > 0)
							{
								media.Video = datos.Datos.Videos[0].Mp4.Enlace;
							}
						}
					}

					//------------------------------------------------------

					string nombre = string.Empty;

					if (datos.Datos != null)
					{
						if (string.IsNullOrEmpty(datos.Datos.Nombre) == false)
						{
							Encoding Utf8 = Encoding.UTF8;
							byte[] utf8Bytes = Utf8.GetBytes(datos.Datos.Nombre);
							nombre = Utf8.GetString(utf8Bytes);
						}
					}

					if (string.IsNullOrEmpty(nombre) == false)
					{
						Juegos.Juego juego = new Juegos.Juego
						{
							IdSteam = int.Parse(datos.Datos.Id),
							Nombre = nombre,
							Imagenes = imagenes,
							Caracteristicas = caracteristicas,
							Media = media,
							FechaSteamAPIComprobacion = DateTime.Now
						};

						if (datos.Datos.Tipo == "dlc")
						{
							juego.Tipo = Juegos.JuegoTipo.DLC;

							Juegos.Juego maestro = BaseDatos.Juegos.Buscar.UnJuego(null, datos.Datos.Maestro.Id);

							if (maestro != null)
							{
								if (maestro.IdSteam > 0)
								{
									juego.Maestro = maestro.Id.ToString();
								}
							}
						}
						else if (datos.Datos.Tipo == "music")
						{
							juego.Tipo = Juegos.JuegoTipo.Music;
						}
						else if (datos.Datos.Tipo == "software" || datos.Datos.Tipo == "application")
						{
							juego.Tipo = Juegos.JuegoTipo.Software;
						}
						else
						{
							juego.Tipo = Juegos.JuegoTipo.Game;
						}

						if (string.IsNullOrEmpty(datos.Datos.Free2Play) == false)
						{
							juego.FreeToPlay = datos.Datos.Free2Play;
						}

						juego.MayorEdad = "false";

						//if (string.IsNullOrEmpty(datos.Datos.MayorEdad) == false)
						//{
						//	try
						//	{
						//		if (int.Parse(datos.Datos.MayorEdad) >= 18)
						//		{
						//			juego.MayorEdad = "true";
						//		}
						//	}
						//	catch {}
						//}

						if (datos.Datos.Categorias != null)
						{
							if (datos.Datos.Categorias.Count > 0)
							{
								List<string> categorias = new List<string>();

								foreach (var categoria in datos.Datos.Categorias)
								{
									categorias.Add(categoria.Id);
								}

								juego.Categorias = categorias;
							}
						}

						if (datos.Datos.Generos != null)
						{
							if (datos.Datos.Generos.Count > 0)
							{
								List<string> generos = new List<string>();

								foreach (var categoria in datos.Datos.Generos)
								{
									generos.Add(categoria.Id);
								}

								juego.Generos = generos;
							}
						}

						try
						{
							if (datos.Datos.Precio != null)
							{
								string descuento = datos.Datos.Precio.Descuento;

								if (descuento != null)
								{
									descuento = descuento.Replace("%", null);
									descuento = descuento.Replace("-", null);
								}

								string precioFormateado = datos.Datos.Precio.Formateado;

								if (precioFormateado != null)
								{
									if (precioFormateado == "Free")
									{
										precioFormateado = "0.00";
									}

									precioFormateado = precioFormateado.Replace("€", null);
									precioFormateado = precioFormateado.Replace(",", ".");
									precioFormateado = precioFormateado.Replace(".--", ".00");
								}

								string enlacePrecio = "https://store.steampowered.com/app/" + datos.Datos.Id;

								Juegos.JuegoPrecio precio = new Juegos.JuegoPrecio
								{
									Descuento = int.Parse(descuento),
									DRM = Juegos.JuegoDRM.Steam,
									Precio = decimal.Parse(precioFormateado, CultureInfo.InvariantCulture),
									Moneda = JuegoMoneda.Euro,
									FechaDetectado = DateTime.Now,
									FechaActualizacion = DateTime.Now,
									Enlace = enlacePrecio,
									Tienda = "steam"
								};

								juego.PrecioActualesTiendas = new List<Juegos.JuegoPrecio> { precio };
								juego.PrecioMinimosHistoricos = new List<Juegos.JuegoPrecio> { precio };
							}
						}
						catch { }

						return juego;
					}
				}
			}

			return null;
		}

		public static async Task<SteamDeckAPI> CargarDatosDeck(int id2)
		{
			string id = id2.ToString();

			if (string.IsNullOrEmpty(id) == false)
			{
				string html = await Decompiladores.Estandar("https://store.steampowered.com/saleaction/ajaxgetdeckappcompatibilityreport?nAppID=" + id + "&l=english&cc=en");
			
				if (string.IsNullOrEmpty(html) == false)
				{
					SteamDeckAPI api = null;
						
					try
					{
						api = JsonSerializer.Deserialize<SteamDeckAPI>(html);
					}
					catch { }
					
					if (api != null)
					{
						return api;
					}
				}
			}	
				
			return null;
		}

		public static bool Detectar(string enlace)
		{
			bool resultado = false;

            if (enlace.Contains("https://store.steampowered.com/app/") == true)
            {
				resultado = true;
            }
			else if (enlace.Contains("https://store.steampowered.com/dlc/") == true)
			{
				resultado = true;
			}
			else if (enlace.Contains("https://steamdb.info/app/") == true)
			{
				resultado = true;
			}

			return resultado;
		}

		public static string LimpiarID(string enlace)
		{
			enlace = enlace.Replace("http://store.steampowered.com/app/", null);
			enlace = enlace.Replace("http://store.steampowered.com/dlc/", null);
			enlace = enlace.Replace("https://store.steampowered.com/app/", null);
			enlace = enlace.Replace("https://store.steampowered.com/dlc/", null);
			enlace = enlace.Replace("https://steamdb.info/app/", null);

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

	#region Clases Juego

	public class SteamJuegoAPI
	{
		[JsonPropertyName("data")]
		public SteamJuegoAPIDatos Datos { get; set; }
	}

	public class SteamJuegoAPIDatos
	{
		[JsonPropertyName("type")]
		public string Tipo { get; set; }

		[JsonPropertyName("name")]
		public string Nombre { get; set; }

		[JsonPropertyName("steam_appid")]
		public string Id { get; set; }

		[JsonPropertyName("is_free")]
		public string Free2Play { get; set; }

		[JsonPropertyName("required_age")]
		public string MayorEdad {  get; set; }

		[JsonPropertyName("short_description")]
		public string DescripcionCorta { get; set; }

		[JsonPropertyName("header_image")]
		public string ImagenHeader_460x215 { get; set; }

		[JsonPropertyName("capsule_image")]
		public string ImagenCapsule_231x87 { get; set; }

		[JsonPropertyName("publishers")]
		public List<string> Publishers { get; set; }

		[JsonPropertyName("developers")]
		public List<string> Desarrolladores { get; set; }

		[JsonPropertyName("dlc")]
		public List<string> DLCs { get; set; }

		[JsonPropertyName("price_overview")]
		public SteamJuegoAPIPrecio Precio { get; set; }

		[JsonPropertyName("platforms")]
		public SteamJuegoAPISistemas Sistemas { get; set; }

		[JsonPropertyName("screenshots")]
		public List<SteamJuegoAPICaptura> Capturas { get; set; }

		[JsonPropertyName("movies")]
		public List<SteamJuegoAPIVideo> Videos { get; set; }

		[JsonPropertyName("fullgame")]
		public SteamJuegoAPIMaestro Maestro { get; set; }

		[JsonPropertyName("categories")]
		public List<SteamJuegoAPICategoria> Categorias { get; set; }

		[JsonPropertyName("genres")]
		public List<SteamJuegoAPIGenero> Generos { get; set; }
	}

	public class SteamJuegoAPIPrecio
	{
		[JsonPropertyName("final_formatted")]
		public string Formateado { get; set; }

		[JsonPropertyName("discount_percent")]
		public string Descuento { get; set; }
	}

	public class SteamJuegoAPISistemas
	{
		[JsonPropertyName("windows")]
		public bool Windows { get; set; }

		[JsonPropertyName("mac")]
		public bool Mac { get; set; }

		[JsonPropertyName("linux")]
		public bool Linux { get; set; }
	}

	public class SteamJuegoAPICaptura
	{

		[JsonPropertyName("path_thumbnail")]
		public string Miniatura { get; set; }

		[JsonPropertyName("path_full")]
		public string Enlace { get; set; }
	}

	public class SteamJuegoAPIVideo
	{

		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("mp4")]
		public SteamJuegoAPIVideoDatos Mp4 { get; set; }

		[JsonPropertyName("webm")]
		public SteamJuegoAPIVideoDatos Webm { get; set; }
	}

	public class SteamJuegoAPIVideoDatos
	{
		[JsonPropertyName("max")]
		public string Enlace { get; set; }
	}

	public class SteamJuegoAPIMaestro
	{
		[JsonPropertyName("appid")]
		public string Id { get; set; }

		[JsonPropertyName("name")]
		public string Nombre { get; set; }
	}

	public class SteamJuegoAPICategoria
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("description")]
		public string Descripcion { get; set; }
	}

	public class SteamJuegoAPIGenero
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("description")]
		public string Descripcion { get; set; }
	}

	#endregion

	#region Clases Deck

	public class SteamDeckAPI
	{
		[JsonPropertyName("results")]
		public SteamDeckAPIResultado Datos { get; set; }
	}

	public class SteamDeckAPIResultado
	{
		[JsonPropertyName("appid")]
		public string Id { get; set; }

		[JsonPropertyName("resolved_category")]
		public int Resultado { get; set; }

		[JsonPropertyName("resolved_items")]
		public List<SteamDeckAPIToken> Tokens { get; set; }
	}

	public class SteamDeckAPIToken
	{
		[JsonPropertyName("display_type")]
		public int Tipo { get; set; }

		[JsonPropertyName("loc_token")]
		public string Token { get; set; }
	}

	#endregion
}
