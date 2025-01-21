//https://learn.microsoft.com/en-us/graph/search-concept-acceptlanguage-header
//https://partner.steamgames.com/doc/store/localization/languages

#nullable disable

using APIs.GOG;
using APIs.XboxGamePass;
using Juegos;
using System.Text.Json;

namespace Herramientas
{
	public class IdiomaClase
	{
		public string Id { get; set; }
		public List<string> Contenidos { get; set; }
		public List<string> Codigos { get; set; }
		public string ImagenBandera { get; set; }
		public string SteamAPI { get; set; }
		public bool SteamReseñas { get; set; }
		public string Traduccion { get; set; }
		public bool WebOfrece { get; set; }
	}

	public static class Idiomas
	{
		public static List<IdiomaClase> ListadoIdiomasGenerar()
		{
			List<IdiomaClase> idiomas = [
				new IdiomaClase
				{
					Id = "en",
					Contenidos = new List<string>() { "English" },
					Codigos = new List<string>() { "en", "en-US", "en-GB" },
					ImagenBandera = "english",
					SteamAPI = "english",
					SteamReseñas = true,
					WebOfrece = true
				},
				new IdiomaClase
				{
					Id = "es",
					Contenidos = new List<string>() { "Spanish (Spain)", "Spanish - Spain", "Spanish" },
					Codigos = new List<string>() { "es", "es-ES", "ca", "ca-ES-valencia", "eu", "gl" },
					ImagenBandera = "spanish",
					SteamAPI = "spanish",
					SteamReseñas = true,
					WebOfrece = true
				},
				new IdiomaClase
				{
					Id = "de",
					Contenidos = new List<string>() { "German" },
					Codigos = new List<string>() { "de", "de-DE" },
					ImagenBandera = "german",
					SteamAPI = "german",
					SteamReseñas = true,
					WebOfrece = true
				},
				new IdiomaClase
				{
					Id = "fr",
					Contenidos = new List<string>() { "French" },
					Codigos = new List<string>() { "fr", "fr-FR", "fr-CA" },
					ImagenBandera = "french",
					SteamAPI = "french",
					SteamReseñas = true,
					WebOfrece = true
				},
				new IdiomaClase
				{
					Id = "it",
					Contenidos = new List<string>() { "Italian" },
					Codigos = new List<string>() { "it", "it-IT" },
					ImagenBandera = "italian",
					SteamAPI = "italian",
					SteamReseñas = true,
					WebOfrece = true
				},
				new IdiomaClase
				{
					Id = "pt",
					Contenidos = new List<string>() { "Portuguese - Portugal", "Portuguese (Portugal)", "Portuguese" },
					Codigos = new List<string>() { "pt", "pt-PT" },
					ImagenBandera = "portuguese",
					SteamAPI = "portuguese",
					SteamReseñas = true,
					WebOfrece = true
				},
				new IdiomaClase
				{
					Id = "da",
					Contenidos = new List<string>() { "Danish" },
					Codigos = new List<string>() { "da", "da-DK" },
					ImagenBandera = "danish",
					SteamAPI = "danish",
					SteamReseñas = true,
					WebOfrece = false
				},
				new IdiomaClase
				{
					Id = "nl",
					Contenidos = new List<string>() { "Dutch" },
					Codigos = new List<string>() { "nl", "nl-NL", "nl-BE" },
					ImagenBandera = "dutch",
					SteamAPI = "dutch",
					SteamReseñas = true,
					WebOfrece = false
				},
				new IdiomaClase
				{
					Id = "nn",
					Contenidos = new List<string>() { "Norwegian" },
					Codigos = new List<string>() { "nn", "nb", "nb-NO" },
					ImagenBandera = "norwegian",
					SteamAPI = "norwegian",
					SteamReseñas = true,
					WebOfrece = false
				},
				new IdiomaClase
				{
					Id = "pl",
					Contenidos = new List<string>() { "Polish" },
					Codigos = new List<string>() { "pl", "pl-PL" },
					ImagenBandera = "polish",
					SteamAPI = "polish",
					SteamReseñas = true,
					WebOfrece = false
				},
				new IdiomaClase
				{
					Id = "sv",
					Contenidos = new List<string>() { "Swedish" },
					Codigos = new List<string>() { "sv", "sv-SE" },
					ImagenBandera = "swedish",
					SteamAPI = "swedish",
					SteamReseñas = true,
					WebOfrece = false
				},
				new IdiomaClase
				{
					Id = "ko",
					Contenidos = new List<string>() { "Korean" },
					Codigos = new List<string>() { "ko", "ko-KR" },
					ImagenBandera = "korean",
					SteamAPI = "korean",
					SteamReseñas = false,
					WebOfrece = false
				},
				new IdiomaClase
				{
					Id = "zhs",
					Contenidos = new List<string>() { "Simplified Chinese", "Chinese - Simplified" },
					Codigos = new List<string>() { "zhs", "zh-Hans", "zh-CN", "cn" },
					ImagenBandera = "chinese",
					SteamAPI = "schinese",
					SteamReseñas = false,
					WebOfrece = false
				},
				new IdiomaClase
				{
					Id = "zht",
					Contenidos = new List<string>() { "Traditional Chinese", "Chinese - Traditional" },
					Codigos = new List<string>() { "zht", "zh-Hant", "zh-TW", "zh" },
					ImagenBandera = "chinese",
					SteamAPI = "tchinese",
					SteamReseñas = false,
					WebOfrece = false
				},
				new IdiomaClase
				{
					Id = "lat",
					Contenidos = new List<string>() { "Spanish - Latin America", "Spanish (Latin America)" },
					Codigos = new List<string>() { "lat", "es-MX", "es_mx", "es-US" },
					ImagenBandera = "latam",
					SteamAPI = "latam",
					SteamReseñas = true,
					WebOfrece = false
				},
				new IdiomaClase
				{
					Id = "br",
					Contenidos = new List<string>() { "Portuguese (Brazil)", "Portuguese - Brazil" },
					Codigos = new List<string>() { "br", "pt-BR" },
					ImagenBandera = "brazilian",
					SteamAPI = "brazilian",
					SteamReseñas = true,
					WebOfrece = false
				},
				new IdiomaClase
				{
					Id = "ja",
					Contenidos = new List<string>() { "Japanese" },
					Codigos = new List<string>() { "ja", "jp", "ja-JP" },
					ImagenBandera = "japanese",
					SteamAPI = "japanese",
					SteamReseñas = false,
					WebOfrece = false
				},
				new IdiomaClase
				{
					Id = "hu",
					Contenidos = new List<string>() { "Hungarian" },
					Codigos = new List<string>() { "hu" },
					ImagenBandera = "hungarian",
					SteamAPI = "hungarian",
					SteamReseñas = false,
					WebOfrece = false
				},
				new IdiomaClase
				{
					Id = "cz",
					Contenidos = new List<string>() { "Czech" },
					Codigos = new List<string>() { "cs" },
					ImagenBandera = "czech",
					SteamAPI = "czech",
					SteamReseñas = false,
					WebOfrece = false
				},
				new IdiomaClase
				{
					Id = "tr",
					Contenidos = new List<string>() { "Turkish" },
					Codigos = new List<string>() { "tr-TR" },
					ImagenBandera = "turkish",
					SteamAPI = "turkish",
					SteamReseñas = false,
					WebOfrece = false
				},
				new IdiomaClase
				{
					Id = "uk",
					Contenidos = new List<string>() { "Ukrainian" },
					Codigos = new List<string>() { "uk" },
					ImagenBandera = "ukrainian",
					SteamAPI = "ukrainian",
					SteamReseñas = false,
					WebOfrece = false
				},
				new IdiomaClase
				{
					Id = "fi",
					Contenidos = new List<string>() { "Finnish" },
					Codigos = new List<string>() { "fi", "fi-FI" },
					ImagenBandera = "finnish",
					SteamAPI = "finnish",
					SteamReseñas = true,
					WebOfrece = false
				},
				new IdiomaClase
				{
					Id = "ro",
					Contenidos = new List<string>() { "Romanian" },
					Codigos = new List<string>() { "ro" },
					ImagenBandera = "romanian",
					SteamAPI = "romanian",
					SteamReseñas = true,
					WebOfrece = false
				},
				new IdiomaClase
				{
					Id = "el",
					Contenidos = new List<string>() { "Greek" },
					Codigos = new List<string>() { "el" },
					ImagenBandera = "greek",
					SteamAPI = "greek",
					SteamReseñas = true,
					WebOfrece = false
				},
				new IdiomaClase
				{
					Id = "ru",
					Contenidos = new List<string>() { "Russian" },
					Codigos = new List<string>() { "ru", "ru-RU" },
					ImagenBandera = "russian",
					SteamAPI = "russian",
					SteamReseñas = false,
					WebOfrece = false
				},
				new IdiomaClase
				{
					Id = "ar",
					Contenidos = new List<string>() { "Arabic" },
					Codigos = new List<string>() { "ar", "ar-SA" },
					ImagenBandera = "arabic",
					SteamAPI = "arabic",
					SteamReseñas = false,
					WebOfrece = false
				}
			];

			return idiomas;
		}

		public static string SacarIdiomaUsuarioWeb(string idiomaUsuario)
		{
			if (string.IsNullOrEmpty(idiomaUsuario) == true)
			{
				return "en";
			}
			else
			{
				foreach (var idioma in ListadoIdiomasGenerar())
				{
					if (idioma.WebOfrece == true)
					{
						if (idioma.Id == idiomaUsuario)
						{
							return idioma.Id;
						}

						foreach (var codigo in idioma.Codigos)
						{
							if (codigo.ToLower() == idiomaUsuario.ToLower())
							{
								return idioma.Id;
							}
						}
					}
				}

				return "en";
			}
		}

		public static bool ComprobarIdiomaUso(string id, string idiomaUso)
		{
			foreach (var idioma in ListadoIdiomasGenerar())
			{
				if (id == idioma.Id)
				{
					if (idiomaUso == idioma.Id)
					{
						return true;
					}

					foreach (var codigo in idioma.Codigos)
					{
						if (codigo.ToLower() == idiomaUso.ToLower())
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		public static string BuscarTexto(string idiomaUsuario, string cadena, string carpeta)
		{
			idiomaUsuario = SacarIdiomaUsuarioWeb(idiomaUsuario);

			string rutaFichero = "Idiomas/" + idiomaUsuario + ".json";

			if (string.IsNullOrEmpty(carpeta) == false)
			{
				rutaFichero = "Idiomas/" + carpeta + "/" + idiomaUsuario + ".json";
			}

			if (File.Exists(rutaFichero) == true)
			{
				using (StreamReader r = new StreamReader(rutaFichero))
				{
					try
					{
						string json = r.ReadToEnd();
						JsonElement elementos = JsonSerializer.Deserialize<JsonElement>(json);

						if (elementos.TryGetProperty(cadena, out var resultado))
						{
							return resultado.GetString();
						}
					}
					catch (Exception ex)
					{
						global::BaseDatos.Errores.Insertar.Mensaje("traduccion", ex);
					}
				}

				if (string.IsNullOrEmpty(cadena) == false)
				{
					if (idiomaUsuario != "en")
					{
						return BuscarTexto("en", cadena, carpeta);
					}
					else
					{
						return null;
					}
				}
				else
				{
					return null;
				}
			}
			else
			{
				return BuscarTexto("en", cadena, carpeta);
			}
		}

		public static string CogerTodo(string idiomaUsuario, string carpeta)
		{
			idiomaUsuario = SacarIdiomaUsuarioWeb(idiomaUsuario);

			string rutaFichero = "Idiomas/" + idiomaUsuario + ".json";

			if (string.IsNullOrEmpty(carpeta) == false)
			{
				rutaFichero = "Idiomas/" + carpeta + "/" + idiomaUsuario + ".json";
			}

			if (File.Exists(rutaFichero) == true)
			{
				using (StreamReader r = new StreamReader(rutaFichero))
				{
					try
					{
						return r.ReadToEnd();
					}
					catch { }
				}
			}

			return null;
		}

		public static string ElegirTexto(string idiomaUsuario, string textoIngles, string textoEspañol)
		{
			if (idiomaUsuario != null)
			{
				if (ComprobarIdiomaUso("es", idiomaUsuario) == true)
				{
					return textoEspañol;
				}
				else
				{
					return textoIngles;
				}
			}
			else
			{
				return textoIngles;
			}
		}

		#region Juegos Razor

		public static string EncontrarIdiomaJuego(string idiomaUsuario, string idiomaJuego)
		{
			string imagenBandera = string.Empty;

			foreach (var idioma in ListadoIdiomasGenerar())
			{
				bool okUsuario = false;
				bool okJuego = false;

				if (idioma.Id == idiomaUsuario)
				{
					okUsuario = true;
				}

				if (idioma.Id == idiomaJuego)
				{
					okJuego = true;
				}

				foreach (var codigo in idioma.Codigos)
				{
					if (codigo.ToLower() == idiomaUsuario.ToLower())
					{
						okUsuario = true;
					}

					if (codigo.ToLower() == idiomaJuego.ToLower())
					{
						okJuego = true;
					}
				}

				if (okUsuario == true && okJuego == true)
				{
					return idioma.ImagenBandera;
				}
			}

			return null;
		}

		public static string EncontrarIdiomaImagen(string idiomaBandera)
		{
			foreach (var idioma in ListadoIdiomasGenerar())
			{
				if (idioma.Id == idiomaBandera)
				{
					if (string.IsNullOrEmpty(idioma.ImagenBandera) == false)
					{
						return idioma.ImagenBandera;
					}
				}
			}

			return null;
		}

		#endregion

		#region Steam Juegos

		public static List<JuegoIdioma> SteamSacarIdiomas(string contenido)
		{
			List<JuegoIdioma> idiomas = new List<JuegoIdioma>();

			if (string.IsNullOrEmpty(contenido) == false)
			{
				foreach (var idioma in ListadoIdiomasGenerar())
				{
					foreach (var idiomaContenido in idioma.Contenidos)
					{
						if (contenido.Contains(idiomaContenido + "<strong>*</strong>") == true)
						{
							JuegoIdioma nuevoIdioma = new JuegoIdioma
							{
								DRM = JuegoDRM.Steam,
								Idioma = idioma.Id,
								Audio = true,
								Texto = true
							};

							bool añadir = true;

							if (idiomas.Count > 0)
							{
								foreach (var idioma2 in idiomas)
								{
									if (idioma2.Idioma == nuevoIdioma.Idioma)
									{
										añadir = false;
									}
								}
							}

							if (añadir == true)
							{
								idiomas.Add(nuevoIdioma);
							}
						}

						if (contenido.Contains(idiomaContenido) == true)
						{
							JuegoIdioma nuevoIdioma = new JuegoIdioma
							{
								DRM = JuegoDRM.Steam,
								Idioma = idioma.Id,
								Audio = false,
								Texto = true
							};

							bool añadir = true;

							if (idiomas.Count > 0)
							{
								foreach (var idioma2 in idiomas)
								{
									if (idioma2.Idioma == nuevoIdioma.Idioma)
									{
										añadir = false;
									}
								}
							}

							if (añadir == true)
							{
								idiomas.Add(nuevoIdioma);
							}
						}
					}
				}
			}

			return idiomas;
		}

		#endregion

		#region GOG Juegos

		public static List<JuegoIdioma> GogSacarIdiomas(List<GOGGalaxy2Idioma> contenido)
		{
			List<JuegoIdioma> idiomas = new List<JuegoIdioma>();

			foreach (var idioma2 in ListadoIdiomasGenerar())
			{
				foreach (var idioma in contenido)
				{
					string id = idioma.Datos.Idioma.Codigo;
					bool existe = false;

					if (idioma2.Id == id)
					{
						existe = true;
						break;
					}

					foreach (var codigo in idioma2.Codigos)
					{
						if (codigo == id)
						{
							existe = true;
							id = idioma2.Id;
							break;
						}
					}

					if (existe == true)
					{
						JuegoIdioma nuevoIdioma = new JuegoIdioma
						{
							DRM = JuegoDRM.GOG,
							Idioma = id
						};

						if (idioma.Datos.Tipo.Nombre == "text")
						{
							nuevoIdioma.Texto = true;
						}

						if (idioma.Datos.Tipo.Nombre == "audio")
						{
							nuevoIdioma.Audio = true;
						}

						bool añadir = true;

						if (idiomas.Count > 0)
						{
							foreach (var idioma3 in idiomas)
							{
								if (idioma3.Idioma == nuevoIdioma.Idioma)
								{
									if (nuevoIdioma.Audio == true)
									{
										idioma3.Audio = true;
									}

									if (nuevoIdioma.Texto == true)
									{
										idioma3.Texto = true;
									}

									añadir = false;
								}
							}
						}

						if (añadir == true)
						{
							idiomas.Add(nuevoIdioma);
						}
					}
				}
			}
				
			return idiomas;
		}

		#endregion

		#region Epic Games Juegos

		public static List<JuegoIdioma> EpicGamesSacarIdiomas(List<string> audios, List<string> textos)
		{
			List<JuegoIdioma> idiomas = new List<JuegoIdioma>();

			if (audios != null)
			{
				if (audios.Count > 0)
				{
					foreach (var audio in audios)
					{
						foreach (var idioma in ListadoIdiomasGenerar())
						{
							foreach (var idiomaContenido in idioma.Contenidos)
							{
								if (idiomaContenido == audio)
								{
									JuegoIdioma nuevoIdioma = new JuegoIdioma
									{
										DRM = JuegoDRM.Epic,
										Idioma = idioma.Id,
										Audio = true,
										Texto = false
									};

									bool añadir = true;

									if (idiomas.Count > 0)
									{
										foreach (var idioma2 in idiomas)
										{
											if (idioma2.Idioma == nuevoIdioma.Idioma)
											{
												añadir = false;
											}
										}
									}

									if (añadir == true)
									{
										idiomas.Add(nuevoIdioma);
									}
								}
							}
							
						}
					}
				}
			}
			
			if (textos != null)
			{
				if (textos.Count > 0)
				{
					foreach (var texto in textos)
					{
						foreach (var idioma in ListadoIdiomasGenerar())
						{
							foreach (var idiomaContenido in idioma.Contenidos)
							{
								if (idiomaContenido == texto)
								{
									JuegoIdioma nuevoIdioma = new JuegoIdioma
									{
										DRM = JuegoDRM.Epic,
										Idioma = idioma.Id,
										Audio = false,
										Texto = true
									};

									bool añadir = true;

									if (idiomas.Count > 0)
									{
										foreach (var idioma2 in idiomas)
										{
											if (idioma2.Idioma == nuevoIdioma.Idioma)
											{
												idioma2.Texto = true;

												añadir = false;
											}
										}
									}

									if (añadir == true)
									{
										idiomas.Add(nuevoIdioma);
									}
								}
							}
						}
					}
				}
			}
			
			return idiomas;
		}

		public static List<JuegoIdioma> EpicGamesSacarIdiomas(string contenido1, string contenido2)
		{
			List<JuegoIdioma> idiomas = new List<JuegoIdioma>();

			string contenidoAudio = string.Empty;

			if (string.IsNullOrEmpty(contenido1) == false)
			{
				if (contenido1.ToLower().Contains("audio") == true)
				{
					contenidoAudio = contenido1;
				}
			}

			if (string.IsNullOrEmpty(contenido2) == false)
			{
				if (contenido2.ToLower().Contains("audio") == true)
				{
					contenidoAudio = contenido2;
				}
			}

			if (string.IsNullOrEmpty(contenidoAudio) == false)
			{
				foreach (var idioma in ListadoIdiomasGenerar())
				{
					foreach (var idiomaContenido in idioma.Contenidos)
					{
						if (contenidoAudio.Contains(idiomaContenido) == true)
						{
							JuegoIdioma nuevoIdioma = new JuegoIdioma
							{
								DRM = JuegoDRM.Epic,
								Idioma = idioma.Id,
								Audio = true,
								Texto = true
							};

							bool añadir = true;

							if (idiomas.Count > 0)
							{
								foreach (var idioma2 in idiomas)
								{
									if (idioma2.Idioma == nuevoIdioma.Idioma)
									{
										añadir = false;
									}
								}
							}

							if (añadir == true)
							{
								idiomas.Add(nuevoIdioma);
							}
						}
					}
				}
			}

			string contenidoTexto = string.Empty;

			if (string.IsNullOrEmpty(contenido1) == false)
			{
				if (contenido1.ToLower().Contains("text") == true)
				{
					contenidoTexto = contenido1;
				}

				if (contenido1.ToLower().Contains("interface") == true)
				{
					contenidoTexto = contenido1;
				}

				if (contenido1.ToLower().Contains("menus + subtitles") == true)
				{
					contenidoTexto = contenido1;
				}
			}

			if (string.IsNullOrEmpty(contenido2) == false)
			{
				if (contenido2.ToLower().Contains("text") == true)
				{
					contenidoTexto = contenido2;
				}

				if (contenido2.ToLower().Contains("interface") == true)
				{
					contenidoTexto = contenido2;
				}

				if (contenido2.ToLower().Contains("menus + subtitles") == true)
				{
					contenidoTexto = contenido2;
				}
			}

			if (string.IsNullOrEmpty(contenidoTexto) == false)
			{
				foreach (var idioma in ListadoIdiomasGenerar())
				{
					foreach (var idiomaContenido in idioma.Contenidos)
					{
						if (contenidoTexto.Contains(idiomaContenido) == true)
						{
							JuegoIdioma nuevoIdioma = new JuegoIdioma
							{
								DRM = JuegoDRM.Epic,
								Idioma = idioma.Id,
								Audio = false,
								Texto = true
							};

							bool añadir = true;

							if (idiomas.Count > 0)
							{
								foreach (var idioma2 in idiomas)
								{
									if (idioma2.Idioma == nuevoIdioma.Idioma)
									{
										idioma2.Texto = true;

										añadir = false;
									}
								}
							}

							if (añadir == true)
							{
								idiomas.Add(nuevoIdioma);
							}
						}
					}
				}
			}

			return idiomas;
		}

		#endregion

		#region Xbox Juegos

		public static List<JuegoIdioma> XboxSacarIdiomas(Dictionary<string, XboxJuegoAPIIdioma> idiomas2)
		{
			List<JuegoIdioma> idiomas = new List<JuegoIdioma>();

			foreach (var idioma in ListadoIdiomasGenerar())
			{
				foreach (var idiomaCodigo in idioma.Codigos)
				{
					if (string.IsNullOrEmpty(idiomaCodigo) == false)
					{
						if (idiomas2.ContainsKey(idiomaCodigo) == true)
						{
							if (idiomas2[idiomaCodigo].Audio == 1)
							{
								JuegoIdioma nuevoIdioma = new JuegoIdioma
								{
									DRM = JuegoDRM.Microsoft,
									Idioma = idioma.Id,
									Audio = true
								};

								bool añadir = true;

								if (idiomas.Count > 0)
								{
									foreach (var idioma2 in idiomas)
									{
										if (idioma2.Idioma == nuevoIdioma.Idioma)
										{
											idioma2.Audio = true;

											añadir = false;
										}
									}
								}

								if (añadir == true)
								{
									idiomas.Add(nuevoIdioma);
								}
							}

							if (idiomas2[idiomaCodigo].Interfaz == 1)
							{
								JuegoIdioma nuevoIdioma = new JuegoIdioma
								{
									DRM = JuegoDRM.Microsoft,
									Idioma = idioma.Id,
									Texto = true
								};

								bool añadir = true;

								if (idiomas.Count > 0)
								{
									foreach (var idioma2 in idiomas)
									{
										if (idioma2.Idioma == nuevoIdioma.Idioma)
										{
											idioma2.Texto = true;

											añadir = false;
										}
									}
								}

								if (añadir == true)
								{
									idiomas.Add(nuevoIdioma);
								}
							}

							if (idiomas2[idiomaCodigo].Subtitulos == 1)
							{
								JuegoIdioma nuevoIdioma = new JuegoIdioma
								{
									DRM = JuegoDRM.Microsoft,
									Idioma = idioma.Id,
									Texto = true
								};

								bool añadir = true;

								if (idiomas.Count > 0)
								{
									foreach (var idioma2 in idiomas)
									{
										if (idioma2.Idioma == nuevoIdioma.Idioma)
										{
											idioma2.Texto = true;

											añadir = false;
										}
									}
								}

								if (añadir == true)
								{
									idiomas.Add(nuevoIdioma);
								}
							}
						}
					}
				}
			}

			return idiomas;
		}

		#endregion
	}
}
