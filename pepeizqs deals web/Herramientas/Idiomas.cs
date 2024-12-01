//https://learn.microsoft.com/en-us/graph/search-concept-acceptlanguage-header

#nullable disable

using System.Text.Json;

namespace Herramientas
{
	public class IdiomaCadena
	{
		public string Id { get; set; }
		public string Valor { get; set; }
	}

	public static class Idiomas
	{
        public static string CogerCadena(string idiomaUsuario, string cadena, string nombreFichero = null)
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

				else
				{
					idiomaUsuario = "en";
				}
			}

			string rutaFichero = "Idiomas/" + idiomaUsuario + ".json";

			if (string.IsNullOrEmpty(nombreFichero) == false)
			{
				rutaFichero = "Idiomas/" + nombreFichero + "." + idiomaUsuario + ".json";
			}

			if (File.Exists(rutaFichero) == true)
			{
				string devolver = null;

				using (StreamReader r = new StreamReader(rutaFichero))
				{
					List<IdiomaCadena> items = new List<IdiomaCadena>();

					try
					{
						string json = r.ReadToEnd();
						items = JsonSerializer.Deserialize<List<IdiomaCadena>>(json);
					}
					catch { }

					if (items != null)
					{
						if (items.Count > 0)
						{
							foreach (var item in items)
							{
								if (item.Id == cadena)
								{
									devolver = item.Valor;
									break;
								}
							}
						}
					}
				}

				if (string.IsNullOrEmpty(devolver) == false)
				{
					return devolver;
				}
				else
				{
					if (string.IsNullOrEmpty(cadena) == false)
					{
						if (idiomaUsuario != "en")
						{
							return CogerCadena("en", cadena, nombreFichero);
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
			}
			else
			{
				return rutaFichero;
			}
		}

		public static string CogerCadena2(string idiomaUsuario, string cadena, string nombreFichero)
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
				else if (ComprobarSueco(idiomaUsuario) == true)
				{
					idiomaUsuario = "sv";
				}
				else
				{
					idiomaUsuario = "en";
				}
			}

			string rutaFichero = "Idiomas/" + idiomaUsuario + ".json";

			if (string.IsNullOrEmpty(nombreFichero) == false)
			{
				rutaFichero = "Idiomas/" + nombreFichero + "." + idiomaUsuario + ".json";
			}

			if (File.Exists(rutaFichero) == true)
			{
				using (StreamReader r = new StreamReader(rutaFichero))
				{
					List<IdiomaCadena> items = new List<IdiomaCadena>();

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
						return CogerCadena("en", cadena, nombreFichero);
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

		public static List<IdiomaCadena> CogerTodasCadenas(string idiomaUsuario, string nombreFichero)
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
			}

			string rutaFichero = "Idiomas/" + idiomaUsuario + ".json";

			if (string.IsNullOrEmpty(nombreFichero) == false)
			{
				rutaFichero = "Idiomas/" + nombreFichero + "." + idiomaUsuario + ".json";
			}

			if (File.Exists(rutaFichero) == true)
			{
				using (StreamReader r = new StreamReader(rutaFichero))
				{
					List<IdiomaCadena> items = new List<IdiomaCadena>();

					try
					{
						string json = r.ReadToEnd();
						items = JsonSerializer.Deserialize<List<IdiomaCadena>>(json);
					}
					catch { }

					if (items != null)
					{
						return items;
					}
				}
			}

			return null;
		}

		public static string MirarTexto(string idiomaUsuario, string textoIngles, string textoEspañol)
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

		//https://partner.steamgames.com/doc/store/localization/languages

		public class SteamIdioma
		{
			public string Id { get; set; }
			public string Contenido { get; set; }
		}

		public static List<SteamIdioma> ListadoSteam(string idiomaUsuario)
		{
			List<string> idiomas = ["english", "spanish", "latam", "french", "german", "italian", "portuguese", "brazilian", "swedish", "greek", "polish", "norwegian", "romanian", "dutch", "danish", "czech", "finnish"];

			List<SteamIdioma> idiomasFinal = new List<SteamIdioma>();

			foreach (var idioma in idiomas)
			{
				SteamIdioma idiomaFinal = new SteamIdioma();
				idiomaFinal.Id = idioma;
				idiomaFinal.Contenido = Idiomas.CogerCadena2(idiomaUsuario, "Language." + idioma, "Reviews");

				idiomasFinal.Add(idiomaFinal);
			}

			if (idiomasFinal.Count > 0)
			{
				idiomasFinal = idiomasFinal.OrderBy(x => x.Contenido).ToList();
			}

			return idiomasFinal;
		}

		public static string FormatoSteamAPI(string idiomaUsuario)
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

		private static bool ComprobarEspañol(string idiomaUsuario)
		{
			if (idiomaUsuario == "es" || idiomaUsuario == "es-ES" || idiomaUsuario == "ca" || idiomaUsuario == "ca-ES-valencia" || idiomaUsuario == "eu" || idiomaUsuario == "gl")
			{
				return true;
			}

			return false;
		}

		private static bool ComprobarEspañolLatino(string idiomaUsuario)
		{
			if (idiomaUsuario == "es-MX" || idiomaUsuario == "es-US")
			{
				return true;
			}

			return false;
		}

		private static bool ComprobarFrances(string idiomaUsuario)
		{
			if (idiomaUsuario == "fr" || idiomaUsuario == "fr-FR" || idiomaUsuario == "fr-CA")
			{
				return true;
			}

			return false;
		}

		private static bool ComprobarAleman(string idiomaUsuario)
		{
			if (idiomaUsuario == "de" || idiomaUsuario == "de-de")
			{
				return true;
			}

			return false;
		}

		private static bool ComprobarItaliano(string idiomaUsuario)
		{
			if (idiomaUsuario == "it" || idiomaUsuario == "it-it")
			{
				return true;
			}

			return false;
		}

		private static bool ComprobarPortugues(string idiomaUsuario)
		{
			if (idiomaUsuario == "pt-PT")
			{
				return true;
			}

			return false;
		}

		private static bool ComprobarBrasileño(string idiomaUsuario)
		{
			if (idiomaUsuario == "pt-BR")
			{
				return true;
			}

			return false;
		}

		private static bool ComprobarSueco(string idiomaUsuario)
		{
			if (idiomaUsuario == "sv")
			{
				return true;
			}

			return false;
		}

		private static bool ComprobarGriego(string idiomaUsuario)
		{
			if (idiomaUsuario == "el")
			{
				return true;
			}

			return false;
		}

		private static bool ComprobarPolaco(string idiomaUsuario)
		{
			if (idiomaUsuario == "pl")
			{
				return true;
			}

			return false;
		}

		private static bool ComprobarNoruego(string idiomaUsuario)
		{
			if (idiomaUsuario == "nb" || idiomaUsuario == "nn")
			{
				return true;
			}

			return false;
		}

		private static bool ComprobarRumano(string idiomaUsuario)
		{
			if (idiomaUsuario == "ro")
			{
				return true;
			}

			return false;
		}

		private static bool ComprobarHolandes(string idiomaUsuario)
		{
			if (idiomaUsuario == "nl" || idiomaUsuario == "nl-BE")
			{
				return true;
			}

			return false;
		}

		private static bool ComprobarDanes(string idiomaUsuario)
		{
			if (idiomaUsuario == "da")
			{
				return true;
			}

			return false;
		}

		private static bool ComprobarCheco(string idiomaUsuario)
		{
			if (idiomaUsuario == "cs")
			{
				return true;
			}

			return false;
		}

		private static bool ComprobarFines(string idiomaUsuario)
		{
			if (idiomaUsuario == "fi")
			{
				return true;
			}

			return false;
		}
	}
}
