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
				else if (ComprobarPortugues(idiomaUsuario) == true)
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

		public static List<string> ListadoSteam()
		{
			List<string> idiomas = ["english", "spanish", "latam", "french", "german", "italian", "portuguese", "swedish"];

			return idiomas;
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
			if (idiomaUsuario == "pt-PT" || idiomaUsuario == "pt-BR")
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
	}
}
