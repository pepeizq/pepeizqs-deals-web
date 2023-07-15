#nullable disable

using Herramientas;
using Newtonsoft.Json;
using System.Globalization;

namespace APIs.Steam
{
	public static class Juego
	{
		public static string dominioImagenes = "https://cdn.cloudflare.steamstatic.com";

		public static async Task<Juegos.Juego> CargarDatos(string enlace)
		{
			string id = LimpiarID(enlace);

			string html = await Decompiladores.Estandar("https://store.steampowered.com/api/appdetails/?appids=" + id + "&l=english");

			if (html != null) 
			{
				int int1 = html.IndexOf(":");
				html = html.Remove(0, int1 + 1);
				html = html.Remove(html.Length - 1, 1);

				SteamJuegoAPI datos = JsonConvert.DeserializeObject<SteamJuegoAPI>(html);

				if (datos != null)
				{
					string descuento = datos.Datos.Precio.Descuento;

					if (descuento != null)
					{
						descuento = descuento.Replace("%", null);
						descuento = descuento.Replace("-", null);
					}

					string precioFormateado = datos.Datos.Precio.Formateado;
					precioFormateado = precioFormateado.Replace("€", null);
					precioFormateado = precioFormateado.Replace(",", ".");

					string enlacePrecio = "https://store.steampowered.com/app/" + datos.Datos.Id;

					Juegos.JuegoPrecio precio = new Juegos.JuegoPrecio
					{
						Descuento = int.Parse(descuento),
						DRM = Juegos.JuegoDRM.Steam,
						Precio = decimal.Parse(precioFormateado, CultureInfo.InvariantCulture),
						Moneda = Juegos.JuegoMoneda.Euro,
						FechaDetectado = DateTime.Now,
						Enlace = enlacePrecio,
						Tienda = "steam"
					};

					//------------------------------------------------------

					Juegos.JuegoImagenes imagenes = new Juegos.JuegoImagenes
					{
						Header_460x215 = datos.Datos.ImagenHeader_460x215,
						Capsule_231x87 = datos.Datos.ImagenCapsule_231x87,
						Logo = dominioImagenes + "/steam/apps/" + datos.Datos.Id + "/logo.png",
						Library_1920x620 = dominioImagenes + "/steam/apps/" + datos.Datos.Id + "/library_hero.jpg",
						Library_600x900 = dominioImagenes + "/steam/apps/" + datos.Datos.Id + "/library_600x900.jpg"
					};

					//------------------------------------------------------

					Juegos.JuegoCaracteristicas caracteristicas = new Juegos.JuegoCaracteristicas
					{
						Windows = datos.Datos.Sistemas.Windows,
						Mac = datos.Datos.Sistemas.Mac,
						Linux = datos.Datos.Sistemas.Linux,
						Desarrolladores = datos.Datos.Desarrolladores,
						Publishers = datos.Datos.Publishers,
						Descripcion = datos.Datos.DescripcionCorta
					};

					//------------------------------------------------------

					Juegos.JuegoMedia media = new Juegos.JuegoMedia();

					if (datos.Datos.Capturas != null)
					{
						if (datos.Datos.Capturas.Count > 0)
						{
							List<string> capturas = new List<string>();

							foreach (SteamJuegoAPICaptura captura in datos.Datos.Capturas)
							{
								capturas.Add(captura.Enlace);
							}

							media.Capturas = capturas;
						}
					}

					if (datos.Datos.Videos != null)
					{
						if (datos.Datos.Videos.Count > 0)
						{
							media.Video = datos.Datos.Videos[0].Mp4.Enlace;
						}
					}

					//------------------------------------------------------

					Juegos.Juego juego = new Juegos.Juego
					{
						IdSteam = int.Parse(datos.Datos.Id),
						Nombre = datos.Datos.Nombre,
						Imagenes = imagenes,
						PrecioActualesTiendas = new List<Juegos.JuegoPrecio> { precio },
						PrecioMinimoActual = new List<Juegos.JuegoPrecio> { precio },
						PrecioMinimoHistorico = new List<Juegos.JuegoPrecio> { precio },
						Caracteristicas = caracteristicas,
						Media = media,
						FechaSteamAPIComprobacion = DateTime.Now
					};

					if (datos.Datos.Tipo == "dlc")
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

	//----------------------------------------------

	public class SteamJuegoAPI
	{
		[JsonProperty("data")]
		public SteamJuegoAPIDatos Datos { get; set; }
	}

	public class SteamJuegoAPIDatos
	{
		[JsonProperty("type")]
		public string Tipo { get; set; }

		[JsonProperty("name")]
		public string Nombre { get; set; }

		[JsonProperty("steam_appid")]
		public string Id { get; set; }

		[JsonProperty("short_description")]
		public string DescripcionCorta { get; set; }

		[JsonProperty("header_image")]
		public string ImagenHeader_460x215 { get; set; }

		[JsonProperty("capsule_image")]
		public string ImagenCapsule_231x87 { get; set; }

		[JsonProperty("publishers")]
		public List<string> Publishers { get; set; }

		[JsonProperty("developers")]
		public List<string> Desarrolladores { get; set; }

		[JsonProperty("price_overview")]
		public SteamJuegoAPIPrecio Precio { get; set; }

		[JsonProperty("platforms")]
		public SteamJuegoAPISistemas Sistemas { get; set; }

		[JsonProperty("screenshots")]
		public List<SteamJuegoAPICaptura> Capturas { get; set; }

		[JsonProperty("movies")]
		public List<SteamJuegoAPIVideo> Videos { get; set; }
	}

	public class SteamJuegoAPIPrecio
	{
		[JsonProperty("final_formatted")]
		public string Formateado { get; set; }

		[JsonProperty("discount_percent")]
		public string Descuento { get; set; }
	}

	public class SteamJuegoAPISistemas
	{
		[JsonProperty("windows")]
		public bool Windows { get; set; }

		[JsonProperty("mac")]
		public bool Mac { get; set; }

		[JsonProperty("linux")]
		public bool Linux { get; set; }
	}

	public class SteamJuegoAPICaptura
	{

		[JsonProperty("path_thumbnail")]
		public string VistaPrevia { get; set; }

		[JsonProperty("path_full")]
		public string Enlace { get; set; }
	}

	public class SteamJuegoAPIVideo
	{

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("mp4")]
		public SteamJuegoAPIVideoDatos Mp4 { get; set; }

		[JsonProperty("webm")]
		public SteamJuegoAPIVideoDatos Webm { get; set; }
	}

	public class SteamJuegoAPIVideoDatos
	{
		[JsonProperty("max")]
		public string Enlace { get; set; }
	}
}
