//https://api.steampowered.com/IStoreBrowseService/GetItems/v1?input_json={"ids":[{"appid":1091500}],"context":{"language":"english","country_code":"ES","steam_realm":1},"data_request":{"include_reviews":true,"include_basic_info":true, "include_assets": true, "include_links": true, "include_tag_count": 20, "include_release": true, "include_platforms": true, "include_screenshots": true, "include_trailers": true, "include_supported_languages": true}}
//https://store.steampowered.com/api/appdetails/?appids=1091500&l=english

#nullable disable

using Herramientas;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIs.Steam
{
	public static class Juego
	{
		public static string dominioImagenes = "https://cdn.cloudflare.steamstatic.com";
		public static string dominioImagenes2 = "https://shared.cloudflare.steamstatic.com";
		public static string dominioVideos = "https://video.cloudflare.steamstatic.com/store_trailers/";

		public static async Task<Juegos.Juego> CargarDatosJuego(string enlace)
		{
			string id = LimpiarID(enlace);

			if (string.IsNullOrEmpty(id) == true)
			{
				return null;
			}

			string html2 = await Decompiladores.Estandar(@"https://api.steampowered.com/IStoreBrowseService/GetItems/v1?input_json={""ids"":[{""appid"":" + id + @"}],""context"":{""language"":""english"",""country_code"":""ES"",""steam_realm"":1},""data_request"":{""include_reviews"":true,""include_basic_info"":true, ""include_assets"": true, ""include_links"": true, ""include_tag_count"": 20, ""include_release"": true, ""include_platforms"": true, ""include_screenshots"": true, ""include_trailers"": true, ""include_supported_languages"": true}}");

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
						string nombre = string.Empty;
						Juegos.JuegoTipo tipo = Juegos.JuegoTipo.Game;
						Juegos.JuegoCaracteristicas caracteristicas = new Juegos.JuegoCaracteristicas();
						Juegos.JuegoImagenes imagenes = new Juegos.JuegoImagenes();
						Juegos.JuegoMedia media = new Juegos.JuegoMedia();
						Juegos.JuegoAnalisis reseñas = new Juegos.JuegoAnalisis();
						List<string> categorias = new List<string>();
						List<string> etiquetas = new List<string>();
						Juegos.JuegoDeck deck = Juegos.JuegoDeck.Desconocido;
						bool freeToPlay = false;
						List<Juegos.JuegoIdioma> idiomas = new List<Juegos.JuegoIdioma>();
						Juegos.JuegoPrecio precio = null;
						int idMaestro = 0;

						#region Nombre

						if (string.IsNullOrEmpty(datos2.Respuesta.Juegos[0].Nombre) == false)
						{
							Encoding Utf8 = Encoding.UTF8;
							byte[] utf8Bytes = Utf8.GetBytes(datos2.Respuesta.Juegos[0].Nombre);
							nombre = Utf8.GetString(utf8Bytes);
						}

						#endregion

						#region Tipo

						if (datos2.Respuesta.Juegos[0].Tipo == 0)
						{
							tipo = Juegos.JuegoTipo.Game;
						}
						else if (datos2.Respuesta.Juegos[0].Tipo == 4)
						{
							tipo = Juegos.JuegoTipo.DLC;
						}
						else if (datos2.Respuesta.Juegos[0].Tipo == 11)
						{
							tipo = Juegos.JuegoTipo.Music;
						}
						else if (datos2.Respuesta.Juegos[0].Tipo == 6)
						{
							tipo = Juegos.JuegoTipo.Software;
						}

						#endregion

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

							if (string.IsNullOrEmpty(datos2.Respuesta.Juegos[0].Info.DescripcionCorta) == false)
							{
								caracteristicas.Descripcion = datos2.Respuesta.Juegos[0].Info.DescripcionCorta;
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

						if (datos2.Respuesta.Juegos[0].AccesoAnticipado == true)
						{
							caracteristicas.AccesoAnticipado = true;
						}
						else
						{
							caracteristicas.AccesoAnticipado = false;
						}

						#endregion

						#region Imagenes

						if (string.IsNullOrEmpty(datos2.Respuesta.Juegos[0].Imagenes.Header_460x215) == false)
						{
							imagenes.Header_460x215 = dominioImagenes2 + "/store_item_assets/steam/apps/" + id + "/" + datos2.Respuesta.Juegos[0].Imagenes.Header_460x215;
						}

						if (string.IsNullOrEmpty(datos2.Respuesta.Juegos[0].Imagenes.Capsule_231x87) == false)
						{
							imagenes.Capsule_231x87 = dominioImagenes2 + "/store_item_assets/steam/apps/" + id + "/" + datos2.Respuesta.Juegos[0].Imagenes.Capsule_231x87;
						}

						if (string.IsNullOrEmpty(datos2.Respuesta.Juegos[0].Imagenes.Library_600x900) == false)
						{
							imagenes.Library_600x900 = dominioImagenes2 + "/store_item_assets/steam/apps/" + id + "/" + datos2.Respuesta.Juegos[0].Imagenes.Library_600x900;
						}

						if (string.IsNullOrEmpty(datos2.Respuesta.Juegos[0].Imagenes.Library_600x900) == false)
						{
							imagenes.Library_1920x620 = dominioImagenes2 + "/store_item_assets/steam/apps/" + id + "/" + datos2.Respuesta.Juegos[0].Imagenes.Library_1920x620;
						}

						imagenes.Logo = dominioImagenes2 + "/store_item_assets/steam/apps/" + id + "/logo.png";

						#endregion

						#region Reseñas

						reseñas.Porcentaje = datos2.Respuesta.Juegos[0].Reseñas.Filtrado.Porcentaje.ToString();
						reseñas.Cantidad = datos2.Respuesta.Juegos[0].Reseñas.Filtrado.Cantidad.ToString();

						#endregion

						#region Categorias

						if (datos2.Respuesta.Juegos[0].Categorias != null)
						{
							if (datos2.Respuesta.Juegos[0].Categorias.Tipo1 != null)
							{
								if (datos2.Respuesta.Juegos[0].Categorias.Tipo1.Count > 0)
								{
									foreach (var nuevaCategoria in datos2.Respuesta.Juegos[0].Categorias.Tipo1)
									{
										if (nuevaCategoria > 0)
										{
											categorias.Add(nuevaCategoria.ToString());
										}
									}
								}
							}

							if (datos2.Respuesta.Juegos[0].Categorias.Tipo2 != null)
							{
								if (datos2.Respuesta.Juegos[0].Categorias.Tipo2.Count > 0)
								{
									foreach (var nuevaCategoria in datos2.Respuesta.Juegos[0].Categorias.Tipo2)
									{
										if (nuevaCategoria > 0)
										{
											categorias.Add(nuevaCategoria.ToString());
										}
									}
								}
							}

							if (datos2.Respuesta.Juegos[0].Categorias.Tipo3 != null)
							{
								if (datos2.Respuesta.Juegos[0].Categorias.Tipo3.Count > 0)
								{
									foreach (var nuevaCategoria in datos2.Respuesta.Juegos[0].Categorias.Tipo3)
									{
										if (nuevaCategoria > 0)
										{
											categorias.Add(nuevaCategoria.ToString());
										}
									}
								}
							}
						}

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

						#region Plataformas

						if (datos2.Respuesta.Juegos[0].Plataformas != null)
						{
							caracteristicas.Windows = false;
							caracteristicas.Mac = false;
							caracteristicas.Linux = false;

							if (datos2.Respuesta.Juegos[0].Plataformas.Windows == true)
							{
								caracteristicas.Windows = true;
							}

							if (datos2.Respuesta.Juegos[0].Plataformas.Mac == true)
							{
								caracteristicas.Mac = true;
							}

							if (datos2.Respuesta.Juegos[0].Plataformas.Linux == true)
							{
								caracteristicas.Linux = true;
							}

							if (datos2.Respuesta.Juegos[0].Plataformas.Deck > 0)
							{
								deck = (Juegos.JuegoDeck)datos2.Respuesta.Juegos[0].Plataformas.Deck;
							}

							if (datos2.Respuesta.Juegos[0].Plataformas.RV != null)
							{
								if (datos2.Respuesta.Juegos[0].Plataformas.RV.Vrhmd == true)
								{
									if (caracteristicas.RealidadVirtual == null)
									{
										caracteristicas.RealidadVirtual = new Juegos.JuegoCaracteristicasRealidadVirtual();
									}

									caracteristicas.RealidadVirtual.Vrhmd = true;
								}

								if (datos2.Respuesta.Juegos[0].Plataformas.RV.VrhmdOnly == true)
								{
									if (caracteristicas.RealidadVirtual == null)
									{
										caracteristicas.RealidadVirtual = new Juegos.JuegoCaracteristicasRealidadVirtual();
									}

									caracteristicas.RealidadVirtual.VrhmdOnly = true;
								}

								if (datos2.Respuesta.Juegos[0].Plataformas.RV.HtcVive == true)
								{
									if (caracteristicas.RealidadVirtual == null)
									{
										caracteristicas.RealidadVirtual = new Juegos.JuegoCaracteristicasRealidadVirtual();
									}

									caracteristicas.RealidadVirtual.HtcVive = true;
								}

								if (datos2.Respuesta.Juegos[0].Plataformas.RV.OculusRift == true)
								{
									if (caracteristicas.RealidadVirtual == null)
									{
										caracteristicas.RealidadVirtual = new Juegos.JuegoCaracteristicasRealidadVirtual();
									}

									caracteristicas.RealidadVirtual.OculusRift = true;
								}

								if (datos2.Respuesta.Juegos[0].Plataformas.RV.WindowsMr == true)
								{
									if (caracteristicas.RealidadVirtual == null)
									{
										caracteristicas.RealidadVirtual = new Juegos.JuegoCaracteristicasRealidadVirtual();
									}

									caracteristicas.RealidadVirtual.WindowsMr = true;
								}

								if (datos2.Respuesta.Juegos[0].Plataformas.RV.ValveIndex == true)
								{
									if (caracteristicas.RealidadVirtual == null)
									{
										caracteristicas.RealidadVirtual = new Juegos.JuegoCaracteristicasRealidadVirtual();
									}

									caracteristicas.RealidadVirtual.ValveIndex = true;
								}
							}
						}

						#endregion

						#region Capturas

						if (datos2.Respuesta.Juegos[0].Capturas != null)
						{
							media.Capturas2 = new List<Juegos.JuegoMediaCaptura>();

							if (datos2.Respuesta.Juegos[0].Capturas.TodasEdades != null)
							{
								if (datos2.Respuesta.Juegos[0].Capturas.TodasEdades.Count > 0)
								{
									foreach (var captura in datos2.Respuesta.Juegos[0].Capturas.TodasEdades)
									{
										Juegos.JuegoMediaCaptura captura2 = new Juegos.JuegoMediaCaptura
										{
											Posicion = captura.Posicion,
											Imagen = dominioImagenes2 + "/store_item_assets/" + captura.Nombre,
											MayorEdad = false
										};

										media.Capturas2.Add(captura2);
									}
								}
							}

							if (datos2.Respuesta.Juegos[0].Capturas.MayorEdad != null)
							{
								if (datos2.Respuesta.Juegos[0].Capturas.MayorEdad.Count > 0)
								{
									foreach (var captura in datos2.Respuesta.Juegos[0].Capturas.MayorEdad)
									{
										Juegos.JuegoMediaCaptura captura2 = new Juegos.JuegoMediaCaptura
										{
											Posicion = captura.Posicion,
											Imagen = dominioImagenes2 + "/store_item_assets/" + captura.Nombre,
											MayorEdad = true
										};

										media.Capturas2.Add(captura2);
									}
								}
							}

							if (media.Capturas2.Count > 0)
							{
								media.Capturas2 = media.Capturas2.OrderBy(x => x.Posicion).ToList();
							}
						}

						#endregion

						#region Videos

						if (datos2.Respuesta.Juegos[0].Trailers != null)
						{
							if (datos2.Respuesta.Juegos[0].Trailers.Videos != null)
							{
								if (datos2.Respuesta.Juegos[0].Trailers.Videos.Count > 0)
								{
									List<Juegos.JuegoMediaVideo> videos = new List<Juegos.JuegoMediaVideo>();

									foreach (var video in datos2.Respuesta.Juegos[0].Trailers.Videos)
									{
										string videoEnlace = string.Empty;

										foreach (var videoDatos in video.Datos)
										{
											if (videoDatos.Tipo.Contains("mp4") == true)
											{
												videoEnlace = videoDatos.Fichero;
											}
										}

										string microEnlace = string.Empty;

										foreach (var microDatos in video.DatosMicro)
										{
											if (microDatos.Tipo.Contains("mp4") == true)
											{
												microEnlace = microDatos.Fichero;
											}
										}

										Juegos.JuegoMediaVideo nuevoVideo = new Juegos.JuegoMediaVideo
										{
											Nombre = video.Nombre,
											Enlace = dominioVideos + video.EnlaceFormato.Replace("steam/apps/", null).Replace("${FILENAME}", videoEnlace),
											MayorEdad = !video.MayorEdad,
											Captura = dominioImagenes2 + "/store_item_assets/" + video.EnlaceFormato.Replace("${FILENAME}", video.Captura),
											CapturaPequeña = dominioImagenes2 + "/store_item_assets/" + video.EnlaceFormato.Replace("${FILENAME}", video.CapturaPequeña),
											Micro = dominioVideos + video.EnlaceFormato.Replace("steam/apps/", null).Replace("${FILENAME}", microEnlace)
										};

										videos.Add(nuevoVideo);

										if (videos.Count > 4)
										{
											break;
										}
									}

									if (videos.Count > 0)
									{
										media.Videos = videos;
									}
								}
							}
						}

						#endregion

						#region FreeToPlay

						if (datos2.Respuesta.Juegos[0].FreeToPlay == true)
						{
							freeToPlay = true;
						}

						#endregion

						#region Idiomas

						if (datos2.Respuesta.Juegos[0].Idiomas != null)
						{
							if (datos2.Respuesta.Juegos[0].Idiomas.Count > 0)
							{
								foreach (var idioma in datos2.Respuesta.Juegos[0].Idiomas)
								{
									List<IdiomaClase> idiomas2 = Idiomas.ListadoIdiomasGenerar();

									foreach (var idioma2 in idiomas2)
									{
										if (idioma2.SteamID == idioma.Id)
										{
											bool texto = false;

											if (idioma.Soportado == true || idioma.Texto == true)
											{
												texto = true;
											}

											Juegos.JuegoIdioma nuevoIdioma = new Juegos.JuegoIdioma
											{
												Idioma = idioma2.Id,
												Audio = idioma.Audio,
												Texto = texto,
												DRM = Juegos.JuegoDRM.Steam
											};

											idiomas.Add(nuevoIdioma);
										}
									}
								}
							}
						}

						#endregion

						#region Enlaces

						if (datos2.Respuesta.Juegos[0].Enlaces != null)
						{
							if (datos2.Respuesta.Juegos[0].Enlaces.Count > 0)
							{
								List<string> nuevosEnlaces = new List<string>();

								foreach (var enlace2 in datos2.Respuesta.Juegos[0].Enlaces)
								{
									if (string.IsNullOrEmpty(enlace2.Enlace) == false)
									{
										nuevosEnlaces.Add(enlace2.Enlace);
									}
								}

								if (nuevosEnlaces.Count > 0)
								{
									caracteristicas.Enlaces = nuevosEnlaces;
								}
							}
						}

						#endregion

						#region Precio 

						if (datos2.Respuesta.Juegos[0].Precio != null)
						{
							string enlacePrecio = "https://store.steampowered.com/app/" + id;

							int descuento = 0;

							if (datos2.Respuesta.Juegos[0].Precio.Descuento > 0)
							{
								descuento = datos2.Respuesta.Juegos[0].Precio.Descuento;
							}

							string precioFormateado = datos2.Respuesta.Juegos[0].Precio.PrecioRebajado;
							precioFormateado = precioFormateado.Replace("€", null);
							precioFormateado = precioFormateado.Replace(",", ".");
							precioFormateado = precioFormateado.Replace(".--", ".00");
							precioFormateado = precioFormateado.Trim();

							precio = new Juegos.JuegoPrecio
							{
								Descuento = descuento,
								DRM = Juegos.JuegoDRM.Steam,
								Precio = decimal.Parse(precioFormateado),
								Moneda = JuegoMoneda.Euro,
								FechaDetectado = DateTime.Now,
								FechaActualizacion = DateTime.Now,
								Enlace = enlacePrecio,
								Tienda = "steam"
							};
						}

						#endregion

						#region DLC

						if (datos2.Respuesta.Juegos[0].DLC != null)
						{
							idMaestro = datos2.Respuesta.Juegos[0].DLC.MaestroId;
						}

						#endregion

						Juegos.Juego juego = new Juegos.Juego
						{
							IdSteam = int.Parse(id),
							Nombre = nombre,
							Imagenes = imagenes,
							Caracteristicas = caracteristicas,
							Media = media,
							FechaSteamAPIComprobacion = DateTime.Now,
							FreeToPlay = freeToPlay.ToString(),
							Tipo = tipo,
							MayorEdad = "false",
							Analisis = reseñas
						};

						if (tipo == Juegos.JuegoTipo.DLC)
						{
							juego.Imagenes.Logo = null;
							juego.Imagenes.Library_600x900 = null;
							juego.Imagenes.Library_1920x620 = null;

							if (idMaestro > 0)
							{
								Juegos.Juego maestro = BaseDatos.Juegos.Buscar.UnJuego(null, idMaestro.ToString());

								if (maestro != null)
								{
									if (maestro.IdSteam > 0)
									{
										juego.Maestro = maestro.Id.ToString();
									}
								}
							}
						}
						else if (tipo == Juegos.JuegoTipo.Music)
						{
							juego.Imagenes.Logo = null;
							juego.Imagenes.Library_600x900 = null;
							juego.Imagenes.Library_1920x620 = null;
						}
						else if (tipo == Juegos.JuegoTipo.Software)
						{
							juego.Imagenes.Logo = null;
							juego.Imagenes.Library_600x900 = null;
							juego.Imagenes.Library_1920x620 = null;
						}
						else
						{
							juego.Deck = deck;
						}

						if (categorias.Count > 0)
						{
							juego.Categorias = categorias;
						}

						if (idiomas.Count > 0)
						{
							juego.Idiomas = idiomas;
						}

						if (etiquetas.Count > 0)
						{
							juego.Etiquetas = etiquetas;
						}

						if (precio != null)
						{
							juego.PrecioActualesTiendas = new List<Juegos.JuegoPrecio> { precio };
							juego.PrecioMinimosHistoricos = new List<Juegos.JuegoPrecio> { precio };
						}

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
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Mensaje("Deck API", ex);
					}
					
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
			enlace = enlace.Replace("http://store.steampowered.com/bundle/", null);
			enlace = enlace.Replace("https://store.steampowered.com/app/", null);
			enlace = enlace.Replace("https://store.steampowered.com/dlc/", null);
			enlace = enlace.Replace("https://store.steampowered.com/bundle/", null);
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
		[JsonPropertyName("name")]
		public string Nombre { get; set; }

		[JsonPropertyName("type")]
		public int Tipo { get; set; }

		[JsonPropertyName("is_free")]
		public bool FreeToPlay { get; set; }

		[JsonPropertyName("is_early_access")]
		public bool AccesoAnticipado { get; set; }

		[JsonPropertyName("assets")]
		public SteamJuegoAPI2JuegoImagenes Imagenes { get; set; }

		[JsonPropertyName("reviews")]
		public SteamJuegoAPI2JuegoReseñas Reseñas { get; set; }

		[JsonPropertyName("tagids")]
		public List<int> Etiquetas { get; set; }

		[JsonPropertyName("categories")]
		public SteamJuegoAPI2JuegoCategorias Categorias { get; set; }

		[JsonPropertyName("basic_info")]
		public SteamJuegoAPI2JuegoInfo Info { get; set; }

		[JsonPropertyName("release")]
		public SteamJuegoAPI2JuegoLanzamiento Lanzamiento { get; set; }

		[JsonPropertyName("platforms")]
		public SteamJuegoAPI2JuegoPlataformas Plataformas { get; set; }

		[JsonPropertyName("screenshots")]
		public SteamJuegoAPI2JuegoCapturas Capturas { get; set; }

		[JsonPropertyName("trailers")]
		public SteamJuegoAPI2JuegoVideos Trailers { get; set; }

		[JsonPropertyName("supported_languages")]
		public List<SteamJuegoAPI2JuegoIdioma> Idiomas { get; set; }

		[JsonPropertyName("links")]
		public List<SteamJuegoAPI2JuegoEnlace> Enlaces { get; set; }

		[JsonPropertyName("best_purchase_option")]
		public SteamJuegoAPI2JuegoPrecio Precio { get; set; }

		[JsonPropertyName("related_items")]
		public SteamJuegoAPI2JuegoDLC DLC { get; set; }
	}

	public class SteamJuegoAPI2JuegoImagenes
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
		[JsonPropertyName("short_description")]
		public string DescripcionCorta { get; set; }

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

	public class SteamJuegoAPI2JuegoPlataformas
	{
		[JsonPropertyName("windows")]
		public bool Windows { get; set; }

		[JsonPropertyName("mac")]
		public bool Mac { get; set; }

		[JsonPropertyName("steamos_linux")]
		public bool Linux { get; set; }

		[JsonPropertyName("steam_deck_compat_category")]
		public int Deck { get; set; }

		[JsonPropertyName("vr_support")]
		public SteamJuegoAPI2JuegoPlataformasRV RV { get; set; }
	}

	public class SteamJuegoAPI2JuegoPlataformasRV
	{
		[JsonPropertyName("vrhmd")]
		public bool Vrhmd { get; set; }

		[JsonPropertyName("vrhmd_only")]
		public bool VrhmdOnly { get; set; }

		[JsonPropertyName("htc_vive")]
		public bool HtcVive { get; set; }

		[JsonPropertyName("oculus_rift")]
		public bool OculusRift { get; set; }

		[JsonPropertyName("windows_mr")]
		public bool WindowsMr { get; set; }

		[JsonPropertyName("valve_index")]
		public bool ValveIndex { get; set; }
	}

	public class SteamJuegoAPI2JuegoCapturas
	{
		[JsonPropertyName("all_ages_screenshots")]
		public List<SteamJuegoAPI2JuegoCaptura> TodasEdades { get; set; }

		[JsonPropertyName("mature_content_screenshots")]
		public List<SteamJuegoAPI2JuegoCaptura> MayorEdad { get; set; }
	}

	public class SteamJuegoAPI2JuegoCaptura
	{
		[JsonPropertyName("filename")]
		public string Nombre { get; set; }

		[JsonPropertyName("ordinal")]
		public int Posicion { get; set; }
	}

	public class SteamJuegoAPI2JuegoVideos
	{
		[JsonPropertyName("highlights")]
		public List<SteamJuegoAPI2JuegoVideo> Videos { get; set; }
	}

	public class SteamJuegoAPI2JuegoVideo
	{
		[JsonPropertyName("trailer_name")]
		public string Nombre { get; set; }

		[JsonPropertyName("trailer_url_format")]
		public string EnlaceFormato { get; set; }

		[JsonPropertyName("trailer_max")]
		public List<SteamJuegoAPI2JuegoVideoDatos> Datos { get; set; }

		[JsonPropertyName("microtrailer")]
		public List<SteamJuegoAPI2JuegoVideoDatos> DatosMicro { get; set; }

		[JsonPropertyName("screenshot_full")]
		public string Captura { get; set; }

		[JsonPropertyName("screenshot_medium")]
		public string CapturaPequeña { get; set; }

		[JsonPropertyName("all_ages")]
		public bool MayorEdad { get; set; }
	}

	public class SteamJuegoAPI2JuegoVideoDatos
	{
		[JsonPropertyName("filename")]
		public string Fichero { get; set; }

		[JsonPropertyName("type")]
		public string Tipo { get; set; }
	}

	public class SteamJuegoAPI2JuegoIdioma
	{
		[JsonPropertyName("elanguage")]
		public int Id { get; set; }

		[JsonPropertyName("supported")]
		public bool Soportado { get; set; }

		[JsonPropertyName("full_audio")]
		public bool Audio { get; set; }

		[JsonPropertyName("subtitles")]
		public bool Texto { get; set; }
	}

	public class SteamJuegoAPI2JuegoEnlace
	{
		[JsonPropertyName("url")]
		public string Enlace { get; set; }
	}

	public class SteamJuegoAPI2JuegoCategorias
	{
		[JsonPropertyName("supported_player_categoryids")]
		public List<int> Tipo1 { get; set; }

		[JsonPropertyName("feature_categoryids")]
		public List<int> Tipo2 { get; set; }

		[JsonPropertyName("controller_categoryids")]
		public List<int> Tipo3 { get; set; }
	}

	public class SteamJuegoAPI2JuegoPrecio
	{
		[JsonPropertyName("formatted_final_price")]
		public string PrecioRebajado { get; set; }

		[JsonPropertyName("formatted_original_price")]
		public string PrecioBase { get; set; }

		[JsonPropertyName("discount_pct")]
		public int Descuento { get; set; } = 0;
	}

	public class SteamJuegoAPI2JuegoDLC
	{
		[JsonPropertyName("parent_appid")]
		public int MaestroId { get; set; }
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
