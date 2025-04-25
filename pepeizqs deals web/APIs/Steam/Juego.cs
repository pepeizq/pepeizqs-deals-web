//https://api.steampowered.com/IStoreBrowseService/GetItems/v1?input_json={"ids":[{"appid":1091500}],"context":{"language":"english","country_code":"ES","steam_realm":1},"data_request":{"include_reviews":true,"include_basic_info":true, "include_assets": true, "include_links": true, "include_tag_count": 20, "include_release": true, "include_platforms": true}}

#nullable disable

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
		public static string dominioImagenes2 = "https://shared.cloudflare.steamstatic.com";

		public static async Task<Juegos.Juego> CargarDatosJuego(string enlace)
		{
			string id = LimpiarID(enlace);

			if (string.IsNullOrEmpty(id) == true)
			{
				return null;
			}

			Juegos.JuegoCaracteristicas caracteristicas = new Juegos.JuegoCaracteristicas();
			Juegos.JuegoImagenes imagenes = new Juegos.JuegoImagenes();
			Juegos.JuegoAnalisis reseñas = new Juegos.JuegoAnalisis();
			List<string> etiquetas = new List<string>();

			string html2 = await Decompiladores.Estandar(@"https://api.steampowered.com/IStoreBrowseService/GetItems/v1?input_json={""ids"":[{""appid"":" + id + @"}],""context"":{""language"":""english"",""country_code"":""ES"",""steam_realm"":1},""data_request"":{""include_reviews"":true,""include_basic_info"":true, ""include_assets"": true, ""include_links"": true, ""include_tag_count"": 20, ""include_release"": true, ""include_platforms"": true}}");

			if (string.IsNullOrEmpty(html2) == false)
			{
				SteamJuegoAPI2 datos2 = null;

				try
				{
					datos2 = JsonSerializer.Deserialize<SteamJuegoAPI2>(html2);
				}
				catch { }

				if (datos2 != null)
				{
					if (datos2.Respuesta.Juegos.Count == 1)
					{
						#region Caracteristicas

						if (datos2.Respuesta.Juegos[0].Info != null)
						{
							if (datos2.Respuesta.Juegos[0].Info.Desarrolladores != null)
							{
								if (datos2.Respuesta.Juegos[0].Info.Desarrolladores.Count > 0)
								{
									List<Juegos.JuegoCaracteristicasCurator> desarrolladores = new List<Juegos.JuegoCaracteristicasCurator>();

									foreach (var desarrollador in datos2.Respuesta.Juegos[0].Info.Desarrolladores)
									{
										Juegos.JuegoCaracteristicasCurator desarrollador2 = new Juegos.JuegoCaracteristicasCurator
										{
											Id = desarrollador.Id,
											Nombre = desarrollador.Nombre
										};

										desarrolladores.Add(desarrollador2);
									}

									caracteristicas.Desarrolladores2 = desarrolladores;
								}
							}

							if (datos2.Respuesta.Juegos[0].Info.Editores != null)
							{
								if (datos2.Respuesta.Juegos[0].Info.Editores.Count > 0)
								{
									List<Juegos.JuegoCaracteristicasCurator> editores = new List<Juegos.JuegoCaracteristicasCurator>();

									foreach (var editor in datos2.Respuesta.Juegos[0].Info.Editores)
									{
										Juegos.JuegoCaracteristicasCurator editor2 = new Juegos.JuegoCaracteristicasCurator
										{
											Id = editor.Id,
											Nombre = editor.Nombre
										};

										editores.Add(editor2);
									}

									caracteristicas.Editores2 = editores;
								}
							}

							if (datos2.Respuesta.Juegos[0].Info.Franquicias != null)
							{
								if (datos2.Respuesta.Juegos[0].Info.Franquicias.Count > 0)
								{
									List<Juegos.JuegoCaracteristicasCurator> franquicias = new List<Juegos.JuegoCaracteristicasCurator>();

									foreach (var franquicia in datos2.Respuesta.Juegos[0].Info.Franquicias)
									{
										Juegos.JuegoCaracteristicasCurator franquicia2 = new Juegos.JuegoCaracteristicasCurator
										{
											Id = franquicia.Id,
											Nombre = franquicia.Nombre
										};

										franquicias.Add(franquicia2);
									}

									caracteristicas.Franquicias = franquicias;
								}
							}
						}

						if (datos2.Respuesta.Juegos[0].Lanzamiento != null)
						{
							if (datos2.Respuesta.Juegos[0].Lanzamiento.Steam > 0)
							{
								caracteristicas.FechaLanzamientoSteam = DateTime.UnixEpoch.AddSeconds(datos2.Respuesta.Juegos[0].Lanzamiento.Steam);
							}

							if (datos2.Respuesta.Juegos[0].Lanzamiento.Original > 0)
							{
								caracteristicas.FechaLanzamientoOriginal = DateTime.UnixEpoch.AddSeconds(datos2.Respuesta.Juegos[0].Lanzamiento.Original);
							}
						}

						#endregion

						#region Imagenes

						if (string.IsNullOrEmpty(datos2.Respuesta.Juegos[0].Media.Header_460x215) == false)
						{
							imagenes.Header_460x215 = dominioImagenes2 + "/store_item_assets/steam/apps/" + id + "/" + datos2.Respuesta.Juegos[0].Media.Header_460x215;
						}

						if (string.IsNullOrEmpty(datos2.Respuesta.Juegos[0].Media.Capsule_231x87) == false)
						{
							imagenes.Capsule_231x87 = dominioImagenes2 + "/store_item_assets/steam/apps/" + id + "/" + datos2.Respuesta.Juegos[0].Media.Capsule_231x87;
						}

						if (string.IsNullOrEmpty(datos2.Respuesta.Juegos[0].Media.Library_600x900) == false)
						{
							imagenes.Library_600x900 = dominioImagenes2 + "/store_item_assets/steam/apps/" + id + "/" + datos2.Respuesta.Juegos[0].Media.Library_600x900;
						}

						if (string.IsNullOrEmpty(datos2.Respuesta.Juegos[0].Media.Library_600x900) == false)
						{
							imagenes.Library_1920x620 = dominioImagenes2 + "/store_item_assets/steam/apps/" + id + "/" + datos2.Respuesta.Juegos[0].Media.Library_1920x620;
						}

						#endregion

						#region Reseñas

						reseñas.Porcentaje = datos2.Respuesta.Juegos[0].Reseñas.Filtrado.Porcentaje.ToString();
						reseñas.Cantidad = datos2.Respuesta.Juegos[0].Reseñas.Filtrado.Cantidad.ToString();

						#endregion

						#region Etiquetas

						if (datos2.Respuesta.Juegos[0].Etiquetas != null)
						{
							if (datos2.Respuesta.Juegos[0].Etiquetas.Count > 0)
							{
								foreach (int etiqueta in datos2.Respuesta.Juegos[0].Etiquetas)
								{
									if (etiqueta > 0)
									{
										etiquetas.Add(etiqueta.ToString());
									}
								}
							}
						}

						#endregion
					}
				}
			}

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
					if (datos.Datos != null) 
					{
						if (string.IsNullOrEmpty(imagenes.Header_460x215) == true)
						{
							imagenes.Header_460x215 = datos.Datos.ImagenHeader_460x215;
						}

						if (string.IsNullOrEmpty(imagenes.Capsule_231x87) == true)
						{
							imagenes.Capsule_231x87 = datos.Datos.ImagenCapsule_231x87;
						}

						if (string.IsNullOrEmpty(imagenes.Logo) == true)
						{
							imagenes.Logo = dominioImagenes + "/steam/apps/" + datos.Datos.Id + "/logo.png";
						}

						if (string.IsNullOrEmpty(imagenes.Library_1920x620) == true)
						{
							imagenes.Library_1920x620 = dominioImagenes + "/steam/apps/" + datos.Datos.Id + "/library_hero.jpg";
						}

						if (string.IsNullOrEmpty(imagenes.Library_600x900) == true)
						{
							imagenes.Library_600x900 = dominioImagenes + "/steam/apps/" + datos.Datos.Id + "/library_600x900.jpg";
						}
					}

					//------------------------------------------------------

					if (datos.Datos != null)
					{
						caracteristicas.Windows = datos.Datos.Sistemas.Windows;
						caracteristicas.Mac = datos.Datos.Sistemas.Mac;
						caracteristicas.Linux = datos.Datos.Sistemas.Linux;

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
							IdSteam = int.Parse(datos.Datos.Id.ToString()),
							Nombre = nombre,
							Imagenes = imagenes,
							Caracteristicas = caracteristicas,
							Media = media,
							FechaSteamAPIComprobacion = DateTime.Now
						};

						if (datos.Datos.Tipo == "dlc")
						{
							juego.Tipo = Juegos.JuegoTipo.DLC;

							juego.Imagenes.Logo = null;
							juego.Imagenes.Library_600x900 = null;
							juego.Imagenes.Library_1920x620 = null;

							Juegos.Juego maestro = BaseDatos.Juegos.Buscar.UnJuego(null, datos.Datos.Maestro.Id.ToString());

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

							juego.Imagenes.Logo = null;
							juego.Imagenes.Library_600x900 = null;
							juego.Imagenes.Library_1920x620 = null;
						}
						else if (datos.Datos.Tipo == "software" || datos.Datos.Tipo == "application")
						{
							juego.Tipo = Juegos.JuegoTipo.Software;

							juego.Imagenes.Logo = null;
							juego.Imagenes.Library_600x900 = null;
							juego.Imagenes.Library_1920x620 = null;
						}
						else
						{
							juego.Tipo = Juegos.JuegoTipo.Game;

							juego.Analisis = reseñas;
						}

						if (string.IsNullOrEmpty(datos.Datos.Free2Play.ToString()) == false)
						{
							juego.FreeToPlay = datos.Datos.Free2Play.ToString();
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
									categorias.Add(categoria.Id.ToString());
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
									generos.Add(categoria.Id.ToString());
								}

								juego.Generos = generos;
							}
						}

						if (string.IsNullOrEmpty(datos.Datos.Idiomas) == false)
						{
							juego.Idiomas = Herramientas.Idiomas.SteamSacarIdiomas(datos.Datos.Idiomas);
						}

						if (etiquetas.Count > 0)
						{
							juego.Etiquetas = etiquetas;
						}

						try
						{
							if (datos.Datos.Precio != null)
							{
								string descuento = datos.Datos.Precio.Descuento.ToString();

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

		public static async Task<string> CargarIdiomasAdmin(string id)
		{
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
					return JsonSerializer.Serialize(datos.Datos.Idiomas);
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

		public static async Task<int> CargarCantidadJugadores(string id)
		{
			string html = await Decompiladores.Estandar("https://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1/?appid=" + id);
		
			if (string.IsNullOrEmpty(html) == false)
			{
				SteamCantidadJugadoresAPI api = null;

				try
				{
					api = JsonSerializer.Deserialize<SteamCantidadJugadoresAPI>(html);
				}
				catch { }

				if (api != null)
				{
					if (api.Datos != null)
					{
						return api.Datos.Cantidad;
					}
				}
			}

			return 0;
		}

		public static async Task<SteamAnalisisAPI> CargarDatosAnalisis(int id2, string idioma)
		{
			string id = id2.ToString();

			if (string.IsNullOrEmpty(id) == false)
			{
				string html = await Decompiladores.Estandar("https://store.steampowered.com/appreviews/" + id + "?json=1&language=" + idioma + "&num_per_page=50&filter_offtopic_activity=0");

				if (string.IsNullOrEmpty(html) == false)
				{
					SteamAnalisisAPI api = null;
					
					try
					{
						api = JsonSerializer.Deserialize<SteamAnalisisAPI>(html);
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
		public object Id { get; set; }

		[JsonPropertyName("is_free")]
		public object Free2Play { get; set; }

		[JsonPropertyName("required_age")]
		public object MayorEdad { get; set; }

		[JsonPropertyName("short_description")]
		public string DescripcionCorta { get; set; }

		[JsonPropertyName("supported_languages")]
		public string Idiomas { get; set; }

		[JsonPropertyName("header_image")]
		public string ImagenHeader_460x215 { get; set; }

		[JsonPropertyName("capsule_image")]
		public string ImagenCapsule_231x87 { get; set; }

		[JsonPropertyName("publishers")]
		public List<string> Publishers { get; set; }

		[JsonPropertyName("developers")]
		public List<string> Desarrolladores { get; set; }

		[JsonPropertyName("dlc")]
		public List<object> DLCs { get; set; }

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
		public object Descuento { get; set; }
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
		public object Id { get; set; }

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
		public object Id { get; set; }

		[JsonPropertyName("name")]
		public string Nombre { get; set; }
	}

	public class SteamJuegoAPICategoria
	{
		[JsonPropertyName("id")]
		public object Id { get; set; }

		[JsonPropertyName("description")]
		public string Descripcion { get; set; }
	}

	public class SteamJuegoAPIGenero
	{
		[JsonPropertyName("id")]
		public object Id { get; set; }

		[JsonPropertyName("description")]
		public string Descripcion { get; set; }
	}

	#endregion

	#region Clases Juego2

	public class SteamJuegoAPI2
	{
		[JsonPropertyName("response")]
		public SteamJuegoAPI2Resultados Respuesta { get; set; }
	}

	public class SteamJuegoAPI2Resultados
	{
		[JsonPropertyName("store_items")]
		public List<SteamJuegoAPI2Juego> Juegos { get; set; }
	}

	public class SteamJuegoAPI2Juego
	{
		[JsonPropertyName("assets")]
		public SteamJuegoAPI2JuegoMedia Media { get; set; }

		[JsonPropertyName("reviews")]
		public SteamJuegoAPI2JuegoReseñas Reseñas { get; set; }

		[JsonPropertyName("tagids")]
		public List<int> Etiquetas { get; set; }

		[JsonPropertyName("basic_info")]
		public SteamJuegoAPI2JuegoInfo Info { get; set; }

		[JsonPropertyName("release")]
		public SteamJuegoAPI2JuegoLanzamiento Lanzamiento { get; set; }
	}

	public class SteamJuegoAPI2JuegoMedia
	{
		[JsonPropertyName("small_capsule")]
		public string Capsule_231x87 { get; set; }

		[JsonPropertyName("header")]
		public string Header_460x215 { get; set; }

		[JsonPropertyName("library_capsule")]
		public string Library_600x900 { get; set; }

		[JsonPropertyName("library_hero")]
		public string Library_1920x620 { get; set; }
	}

	public class SteamJuegoAPI2JuegoReseñas
	{
		[JsonPropertyName("summary_filtered")]
		public SteamJuegoAPI2JuegoReseñasFiltrado Filtrado { get; set; }
	}

	public class SteamJuegoAPI2JuegoReseñasFiltrado
	{
		[JsonPropertyName("review_count")]
		public int Cantidad { get; set; }

		[JsonPropertyName("percent_positive")]
		public int Porcentaje { get; set; }
	}

	public class SteamJuegoAPI2JuegoInfo
	{
		[JsonPropertyName("publishers")]
		public List<SteamJuegoAPI2JuegoInfoDesarrollador> Editores { get; set; }

		[JsonPropertyName("developers")]
		public List<SteamJuegoAPI2JuegoInfoDesarrollador> Desarrolladores { get; set; }

		[JsonPropertyName("franchises")]
		public List<SteamJuegoAPI2JuegoInfoDesarrollador> Franquicias { get; set; }
	}

	public class SteamJuegoAPI2JuegoInfoDesarrollador
	{
		[JsonPropertyName("name")]
		public string Nombre { get; set; }

		[JsonPropertyName("creator_clan_account_id")]
		public int Id { get; set; }
	}

	public class SteamJuegoAPI2JuegoLanzamiento
	{
		[JsonPropertyName("steam_release_date")]
		public int Steam { get; set; }

		[JsonPropertyName("original_release_date")]
		public int Original { get; set; }
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
		public object Id { get; set; }

		[JsonPropertyName("resolved_category")]
		public object Resultado { get; set; }

		[JsonPropertyName("resolved_items")]
		public List<SteamDeckAPIToken> Tokens { get; set; }
	}

	public class SteamDeckAPIToken
	{
		[JsonPropertyName("display_type")]
		public object Tipo { get; set; }

		[JsonPropertyName("loc_token")]
		public string Token { get; set; }
	}

	#endregion

	#region Clases Cantidad Jugadores

	public class SteamCantidadJugadoresAPI
	{
		[JsonPropertyName("response")]
		public SteamCantidadJugadoresAPIRespuesta Datos { get; set; }
	}

	public class SteamCantidadJugadoresAPIRespuesta
	{
		[JsonPropertyName("player_count")]
		public int Cantidad { get; set; }
	}

	#endregion

	#region Clases Analisis

	public class SteamAnalisisAPI
	{
		[JsonPropertyName("query_summary")]
		public SteamAnalisisAPISumario Sumario { get; set; }

		[JsonPropertyName("reviews")]
		public List<SteamAnalisisAPIAnalisis> Analisis { get; set; }
	}

	public class SteamAnalisisAPISumario
	{
		[JsonPropertyName("total_positive")]
		public int TotalPositivas { get; set; }

		[JsonPropertyName("total_negative")]
		public int TotalNegativas { get; set; }

		[JsonPropertyName("total_reviews")]
		public int TotalCantidad { get; set; }
	}

	public class SteamAnalisisAPIAnalisis
	{
		[JsonPropertyName("recommendationid")]
		public string Id { get; set; }

		[JsonPropertyName("author")]
		public SteamAnalisisAPIAnalisisAutor Autor { get; set; }

		[JsonPropertyName("language")]
		public string Idioma { get; set; }

		[JsonPropertyName("voted_up")]
		public bool Positiva { get; set; }

		[JsonPropertyName("timestamp_created")]
		public int TicksCreado { get; set; }

		[JsonPropertyName("timestamp_updated")]
		public int TicksActualizado { get; set; }

		[JsonPropertyName("review")]
		public string Contenido { get; set; }

		[JsonPropertyName("votes_up")]
		public int CantidadVotosPositivos { get; set; }

		[JsonPropertyName("votes_funny")]
		public int CantidadVotosDivertidos { get; set; }

		[JsonPropertyName("steam_purchase")]
		public bool FueComprado { get; set; }

		[JsonPropertyName("received_for_free")]
		public bool FueGratis { get; set; }

		[JsonPropertyName("written_during_early_access")]
		public bool FueAccesoAnticipado { get; set; }

		[JsonPropertyName("primarily_steam_deck")]
		public bool FueSteamDeck { get; set; }
	}

	public class SteamAnalisisAPIAnalisisAutor
	{
		[JsonPropertyName("steamid")]
		public string SteamID { get; set; }

		[JsonPropertyName("playtime_forever")]
		public int TiempoJugadoSiempre { get; set; }

		[JsonPropertyName("playtime_last_two_weeks")]
		public int TiempoJugado2Semanas { get; set; }

		[JsonPropertyName("playtime_at_review")]
		public int TiempoJugadoCuandoHizoAnalisis { get; set; }
	}

	#endregion
}
