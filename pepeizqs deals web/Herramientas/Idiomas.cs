//https://learn.microsoft.com/en-us/graph/search-concept-acceptlanguage-header

#nullable disable

using AngleSharp.Dom;
using APIs.GOG;
using APIs.XboxGamePass;
using Juegos;
using System.Text.Json;

namespace Herramientas
{
	public static class Idiomas
	{
		public static string SacarIdiomaUsuario(string idiomaUsuario)
		{
			if (string.IsNullOrEmpty(idiomaUsuario) == true)
			{
				idiomaUsuario = "en";
			}
			else
			{
				if (ComprobarEspañol(idiomaUsuario) == true || ComprobarEspañolLatino(idiomaUsuario) == true)
				{
					idiomaUsuario = "es";
				}
				else if (ComprobarFrances(idiomaUsuario) == true)
				{
					idiomaUsuario = "fr";
				}
				else if (ComprobarAleman(idiomaUsuario) == true)
				{
					idiomaUsuario = "de";
				}
				else if (ComprobarItaliano(idiomaUsuario) == true)
				{
					idiomaUsuario = "it";
				}
				else if (ComprobarPortugues(idiomaUsuario) == true || ComprobarBrasileño(idiomaUsuario) == true)
				{
					idiomaUsuario = "pt";
				}
				//else if (ComprobarSueco(idiomaUsuario) == true)
				//{
				//	idiomaUsuario = "sv";
				//}
				else
				{
					idiomaUsuario = "en";
				}
			}

			return idiomaUsuario;
		}

		public static string BuscarTexto(string idiomaUsuario, string cadena, string carpeta)
		{
			idiomaUsuario = SacarIdiomaUsuario(idiomaUsuario);

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
				return rutaFichero;
			}
		}

		public static string CogerTodo(string idiomaUsuario, string carpeta)
		{
			idiomaUsuario = SacarIdiomaUsuario(idiomaUsuario);

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
				if (ComprobarEspañol(idiomaUsuario) == true || ComprobarEspañolLatino(idiomaUsuario) == true)
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

		#region Steam Reseñas

		//https://partner.steamgames.com/doc/store/localization/languages

		public class IdiomaClase
		{
			public string Id { get; set; }
			public string Contenido { get; set; }
			public string Codigo { get; set; }
		}

		public static List<IdiomaClase> ListadoSteamReseñas(string idiomaUsuario)
		{
			List<string> idiomas = ["english", "spanish", "latam", "french", "german", "italian", "portuguese", "brazilian", "swedish", "greek", "polish", "norwegian", "romanian", "dutch", "danish", "czech", "finnish"];

			List<IdiomaClase> idiomasFinal = new List<IdiomaClase>();

			foreach (var idioma in idiomas)
			{
				IdiomaClase idiomaFinal = new IdiomaClase();
				idiomaFinal.Id = idioma;
				idiomaFinal.Contenido = Idiomas.BuscarTexto(idiomaUsuario, "Language." + idioma, "Reviews");

				idiomasFinal.Add(idiomaFinal);
			}

			if (idiomasFinal.Count > 0)
			{
				idiomasFinal = idiomasFinal.OrderBy(x => x.Contenido).ToList();
			}

			return idiomasFinal;
		}

		public static string SacarIdiomaSteamAPI(string idiomaUsuario)
		{
			string idiomaSteam = string.Empty;

			if (string.IsNullOrEmpty(idiomaUsuario) == false)
			{
				if (ComprobarEspañol(idiomaUsuario) == true)
				{
					idiomaSteam = "spanish";
				}
				else if (ComprobarEspañolLatino(idiomaUsuario) == true)
				{
					idiomaSteam = "latam";
				}
				else if (ComprobarFrances(idiomaUsuario) == true)
				{
					idiomaSteam = "french";
				}
				else if (ComprobarAleman(idiomaUsuario) == true)
				{
					idiomaSteam = "german";
				}
				else if (ComprobarItaliano(idiomaUsuario) == true)
				{
					idiomaSteam = "italian";
				}
				else if (ComprobarPortugues(idiomaUsuario) == true)
				{
					idiomaSteam = "portuguese";
				}
				else if (ComprobarBrasileño(idiomaUsuario) == true)
				{
					idiomaSteam = "brazilian";
				}
				else if (ComprobarSueco(idiomaUsuario) == true)
				{
					idiomaSteam = "swedish";
				}
				else if (ComprobarGriego(idiomaUsuario) == true)
				{
					idiomaSteam = "greek";
				}
				else if (ComprobarPolaco(idiomaUsuario) == true)
				{
					idiomaSteam = "polish";
				}
				else if (ComprobarNoruego(idiomaUsuario) == true)
				{
					idiomaSteam = "norwegian";
				}
				else if (ComprobarRumano(idiomaUsuario) == true)
				{
					idiomaSteam = "romanian";
				}
				else if (ComprobarHolandes(idiomaUsuario) == true)
				{
					idiomaSteam = "dutch";
				}
				else if (ComprobarDanes(idiomaUsuario) == true)
				{
					idiomaSteam = "danish";
				}
				else if (ComprobarCheco(idiomaUsuario) == true)
				{
					idiomaSteam = "czech";
				}
				else if (ComprobarFines(idiomaUsuario) == true)
				{
					idiomaSteam = "finnish";
				}
			}

			if (string.IsNullOrEmpty(idiomaSteam) == true)
			{
				idiomaSteam = "english";
			}

			return idiomaSteam;
		}

		public static bool ComprobarIngles(string idiomaUsuario)
		{
			if (idiomaUsuario == "en" || idiomaUsuario == "en-US" || idiomaUsuario == "en-GB")
			{
				return true;
			}

			return false;
		}

		public static bool ComprobarEspañol(string idiomaUsuario)
		{
			if (idiomaUsuario == "es" || idiomaUsuario == "es-ES" || idiomaUsuario == "ca" || idiomaUsuario == "ca-ES-valencia" || idiomaUsuario == "eu" || idiomaUsuario == "gl")
			{
				return true;
			}

			return false;
		}

		public static bool ComprobarEspañolLatino(string idiomaUsuario)
		{
			if (idiomaUsuario == "es-MX" || idiomaUsuario == "es-US")
			{
				return true;
			}

			return false;
		}

		public static bool ComprobarFrances(string idiomaUsuario)
		{
			if (idiomaUsuario == "fr" || idiomaUsuario == "fr-FR" || idiomaUsuario == "fr-CA")
			{
				return true;
			}

			return false;
		}

		public static bool ComprobarAleman(string idiomaUsuario)
		{
			if (idiomaUsuario == "de" || idiomaUsuario == "de-de")
			{
				return true;
			}

			return false;
		}

		public static bool ComprobarItaliano(string idiomaUsuario)
		{
			if (idiomaUsuario == "it" || idiomaUsuario == "it-it")
			{
				return true;
			}

			return false;
		}

		public static bool ComprobarPortugues(string idiomaUsuario)
		{
			if (idiomaUsuario == "pt" || idiomaUsuario == "pt-PT")
			{
				return true;
			}

			return false;
		}

		public static bool ComprobarBrasileño(string idiomaUsuario)
		{
			if (idiomaUsuario == "pt-BR")
			{
				return true;
			}

			return false;
		}

		public static bool ComprobarSueco(string idiomaUsuario)
		{
			if (idiomaUsuario == "sv")
			{
				return true;
			}

			return false;
		}

		public static bool ComprobarGriego(string idiomaUsuario)
		{
			if (idiomaUsuario == "el")
			{
				return true;
			}

			return false;
		}

		public static bool ComprobarPolaco(string idiomaUsuario)
		{
			if (idiomaUsuario == "pl")
			{
				return true;
			}

			return false;
		}

		public static bool ComprobarNoruego(string idiomaUsuario)
		{
			if (idiomaUsuario == "nb" || idiomaUsuario == "nn")
			{
				return true;
			}

			return false;
		}

		public static bool ComprobarRumano(string idiomaUsuario)
		{
			if (idiomaUsuario == "ro")
			{
				return true;
			}

			return false;
		}

		public static bool ComprobarHolandes(string idiomaUsuario)
		{
			if (idiomaUsuario == "nl" || idiomaUsuario == "nl-BE")
			{
				return true;
			}

			return false;
		}

		public static bool ComprobarDanes(string idiomaUsuario)
		{
			if (idiomaUsuario == "da")
			{
				return true;
			}

			return false;
		}

		public static bool ComprobarCheco(string idiomaUsuario)
		{
			if (idiomaUsuario == "cs")
			{
				return true;
			}

			return false;
		}

		public static bool ComprobarFines(string idiomaUsuario)
		{
			if (idiomaUsuario == "fi")
			{
				return true;
			}

			return false;
		}

		#endregion

		#region Juegos Razor

		public static string EncontrarIdiomaJuego(string idiomaUsuario, string idiomaJuego)
		{
			string imagenBandera = string.Empty;

			if (ComprobarIngles(idiomaUsuario) == true && ComprobarIngles(idiomaJuego) == true)
			{
				imagenBandera = "english";
			}

			if (ComprobarEspañol(idiomaUsuario) == true && ComprobarEspañol(idiomaJuego) == true)
			{
				imagenBandera = "spanish";
			}

			if (ComprobarAleman(idiomaUsuario) == true && ComprobarAleman(idiomaJuego) == true)
			{
				imagenBandera = "german";
			}

			if (ComprobarFrances(idiomaUsuario) == true && ComprobarFrances(idiomaJuego) == true)
			{
				imagenBandera = "french";
			}

			if (ComprobarItaliano(idiomaUsuario) == true && ComprobarItaliano(idiomaJuego) == true)
			{
				imagenBandera = "italian";
			}

			if (ComprobarPortugues(idiomaUsuario) == true && ComprobarPortugues(idiomaJuego) == true)
			{
				imagenBandera = "portuguese";
			}

			return imagenBandera;
		}

		public static string EncontrarIdiomaUsuario(string idiomaUsuario)
		{
			string imagenBandera = string.Empty;

			if (ComprobarIngles(idiomaUsuario) == true)
			{
				imagenBandera = "english";
			}

			if (ComprobarEspañol(idiomaUsuario) == true)
			{
				imagenBandera = "spanish";
			}

			if (ComprobarAleman(idiomaUsuario) == true)
			{
				imagenBandera = "german";
			}

			if (ComprobarFrances(idiomaUsuario) == true)
			{
				imagenBandera = "french";
			}

			if (ComprobarItaliano(idiomaUsuario) == true)
			{
				imagenBandera = "italian";
			}

			if (ComprobarPortugues(idiomaUsuario) == true)
			{
				imagenBandera = "portuguese";
			}

			return imagenBandera;
		}

		#endregion

		private static List<IdiomaClase> ListadoIdiomasBuscar()
		{
			List<IdiomaClase> idiomas = [
				new IdiomaClase
				{
					Id = "en",
					Contenido = "English",
					Codigo = "en-US"
				},
				new IdiomaClase
				{
					Id = "es",
					Contenido = "Spanish (Spain)",
					Codigo = "es-ES"
				},
				new IdiomaClase
				{
					Id = "es",
					Contenido = "Spanish - Spain",
					Codigo = "es-ES"
				},
				new IdiomaClase
				{
					Id = "de",
					Contenido = "German",
					Codigo = "de-DE"
				},
				new IdiomaClase
				{
					Id = "fr",
					Contenido = "French",
					Codigo = "fr-FR"
				},
				new IdiomaClase
				{
					Id = "it",
					Contenido = "Italian",
					Codigo = "it-IT"
				},
				new IdiomaClase
				{
					Id = "pt",
					Contenido = "Portuguese - Portugal",
					Codigo = "pt-PT"
				},
				new IdiomaClase
				{
					Id = "pt",
					Contenido = "Portuguese (Portugal)",
					Codigo = "pt-PT"
				},
				new IdiomaClase
				{
					Id = "da",
					Contenido = "Danish"
				},
				new IdiomaClase
				{
					Id = "nl",
					Contenido = "Dutch",
					Codigo = "nl-NL"
				},
				new IdiomaClase
				{
					Id = "nn",
					Contenido = "Norwegian"
				},
				new IdiomaClase
				{
					Id = "pl",
					Contenido = "Polish",
					Codigo = "pl-PL"
				},
				new IdiomaClase
				{
					Id = "sv",
					Contenido = "Swedish"
				},
				new IdiomaClase
				{
					Id = "ko",
					Contenido = "Korean"
				},
				new IdiomaClase
				{
					Id = "zhs",
					Contenido = "Simplified Chinese"
				},
				new IdiomaClase
				{
					Id = "zht",
					Contenido = "Traditional Chinese"
				},
				new IdiomaClase
				{
					Id = "lat",
					Contenido = "Spanish (Latin America)",
					Codigo = "es-MX"
				},
				new IdiomaClase
				{
					Id = "lat",
					Contenido = "Spanish - Latin America",
					Codigo = "es-MX"
				},
				new IdiomaClase
				{
					Id = "br",
					Contenido = "Portuguese (Brazil)",
					Codigo = "pt-BR"
				},
				new IdiomaClase
				{
					Id = "br",
					Contenido = "Portuguese - Brazil",
					Codigo = "pt-BR"
				},
				new IdiomaClase
				{
					Id = "ja",
					Contenido = "Japanese",
					Codigo = "ja-JP"
				},
				new IdiomaClase
				{
					Id = "hu",
					Contenido = "Hungarian"
				},
				new IdiomaClase
				{
					Id = "cz",
					Contenido = "Czech"
				},
				new IdiomaClase
				{
					Id = "tr",
					Contenido = "Turkish",
					Codigo = "tr-TR"
				},
				new IdiomaClase
				{
					Id = "uk",
					Contenido = "Ukrainian"
				},
				new IdiomaClase
				{
					Id = "fi",
					Contenido = "Finnish"
				},
				new IdiomaClase
				{
					Id = "ro",
					Contenido = "Romanian"
				},
				new IdiomaClase
				{
					Id = "ro",
					Contenido = "Romanian"
				},
				new IdiomaClase
				{
					Id = "el",
					Contenido = "Greek"
				}
			];

			return idiomas;
		}

		#region Steam Juegos

		public static List<JuegoIdioma> SteamSacarIdiomas(string contenido)
		{
			List<JuegoIdioma> idiomas = new List<JuegoIdioma>();

			if (string.IsNullOrEmpty(contenido) == false)
			{
				foreach (var idioma in ListadoIdiomasBuscar())
				{
					if (contenido.Contains(idioma.Contenido + "<strong>*</strong>") == true)
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

					if (contenido.Contains(idioma.Contenido) == true)
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

			return idiomas;
		}

		#endregion

		#region GOG Juegos

		public static List<JuegoIdioma> GogSacarIdiomas(List<GOGGalaxy2Idioma> contenido)
		{
			List<JuegoIdioma> idiomas = new List<JuegoIdioma>();

			foreach (var idioma in contenido)
			{
				JuegoIdioma nuevoIdioma = new JuegoIdioma
				{
					DRM = JuegoDRM.GOG,
					Idioma = idioma.Datos.Idioma.Codigo
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
					foreach (var idioma2 in idiomas)
					{
						if (idioma2.Idioma == nuevoIdioma.Idioma)
						{
							if (nuevoIdioma.Audio == true)
							{
								idioma2.Audio = true;
							}

							if (nuevoIdioma.Texto == true)
							{
								idioma2.Texto = true;
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
						foreach (var idioma in ListadoIdiomasBuscar())
						{
							if (idioma.Contenido == audio)
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
			
			if (textos != null)
			{
				if (textos.Count > 0)
				{
					foreach (var texto in textos)
					{
						foreach (var idioma in ListadoIdiomasBuscar())
						{
							if (idioma.Contenido == texto)
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

			if (string.IsNullOrEmpty(contenido2) == false && string.IsNullOrEmpty(contenidoAudio) == true)
			{
				if (contenido2.ToLower().Contains("audio") == true)
				{
					contenidoAudio = contenido2;
				}
			}

			if (string.IsNullOrEmpty(contenidoAudio) == false)
			{
				foreach (var idioma in ListadoIdiomasBuscar())
				{
					if (contenidoAudio.Contains(idioma.Contenido) == true)
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
			}

			if (string.IsNullOrEmpty(contenido2) == false && string.IsNullOrEmpty(contenidoTexto) == true)
			{
				if (contenido2.ToLower().Contains("text") == true)
				{
					contenidoTexto = contenido2;
				}

				if (contenido2.ToLower().Contains("interface") == true)
				{
					contenidoTexto = contenido2;
				}
			}

			if (string.IsNullOrEmpty(contenidoTexto) == false)
			{
				foreach (var idioma in ListadoIdiomasBuscar())
				{
					if (contenidoTexto.Contains(idioma.Contenido) == true)
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

			return idiomas;
		}

		#endregion

		#region Xbox Juegos

		public static List<JuegoIdioma> XboxSacarIdiomas(Dictionary<string, XboxJuegoAPIIdioma> idiomas2)
		{
			List<JuegoIdioma> idiomas = new List<JuegoIdioma>();

			foreach (var idioma in ListadoIdiomasBuscar())
			{
				if (string.IsNullOrEmpty(idioma.Codigo) == false)
				{
					if (idiomas2.ContainsKey(idioma.Codigo) == true)
					{
						if (idiomas2[idioma.Codigo].Audio == 1)
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

						if (idiomas2[idioma.Codigo].Interfaz == 1)
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

						if (idiomas2[idioma.Codigo].Subtitulos == 1)
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

			return idiomas;
		}

		#endregion
	}
}
