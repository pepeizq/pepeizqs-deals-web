#nullable disable

using Newtonsoft.Json;

namespace Herramientas
{
	public class Idioma
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
				idiomaUsuario = "en-US";
			}
			else
			{
				if (idiomaUsuario == "es")
				{
					idiomaUsuario = "es-ES";
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
					List<Idioma> items = new List<Idioma>();

					try
					{
						string json = r.ReadToEnd();
						items = JsonConvert.DeserializeObject<List<Idioma>>(json);
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
						return CogerCadena("en-US", cadena, nombreFichero);
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

		public static string MirarTexto(string idiomaUsuario, string textoIngles, string textoEspañol)
		{
			if (idiomaUsuario != null)
			{
				if (idiomaUsuario == "es" || idiomaUsuario == "es-ES")
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

		public static string CogerRSS(string idiomaUsuario)
		{
			string rss = "/rss-en.xml";

			if (idiomaUsuario != null)
			{
				if (idiomaUsuario == "es" || idiomaUsuario == "es-ES")
				{
					rss = "/rss-es.xml";
				}
			}

			return rss;
		}
	}
	
}
