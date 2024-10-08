﻿#nullable disable

using Newtonsoft.Json;
using System.Collections.Generic;

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
					List<IdiomaCadena> items = new List<IdiomaCadena>();

					try
					{
						string json = r.ReadToEnd();
						items = JsonConvert.DeserializeObject<List<IdiomaCadena>>(json);
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
						if (idiomaUsuario != "en-US")
						{
							return CogerCadena("en-US", cadena, nombreFichero);
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

		public static List<IdiomaCadena> CogerTodasCadenas(string idiomaUsuario, string nombreFichero)
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
				using (StreamReader r = new StreamReader(rutaFichero))
				{
					List<IdiomaCadena> items = new List<IdiomaCadena>();

					try
					{
						string json = r.ReadToEnd();
						items = JsonConvert.DeserializeObject<List<IdiomaCadena>>(json);
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
