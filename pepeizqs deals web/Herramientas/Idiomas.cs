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
        public static string CogerCadena(string idiomaUsuario, string cadena)
		{
			if (idiomaUsuario != null)
			{
				if (idiomaUsuario == "es")
				{
					idiomaUsuario = "es-ES";
				}
            }		

			if (string.IsNullOrEmpty(idiomaUsuario) == true)
			{
                idiomaUsuario = "en-US";
            }
				
			if (File.Exists("Idiomas/" + idiomaUsuario + ".json") == false)
			{
				idiomaUsuario = "en-US";
			}

			string devolver = null;

			using (StreamReader r = new StreamReader("Idiomas/" + idiomaUsuario + ".json"))
			{
				string json = r.ReadToEnd();
				List<Idioma> items = new List<Idioma>();
				
				try
				{
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
							}
						}
					}
				}							
			}

			if (devolver != null)
			{
				return devolver;
			}
			else
			{
				if (cadena != null)
				{
                    return CogerCadena("en-US", cadena);
                }
				else
				{
					return null;
				}
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
