#nullable disable

using Newtonsoft.Json;

namespace Herramientas
{
	public class Idioma
	{
		public string Id { get; set; }
		public string Valor { get; set; }
	}

	public static class IdiomaBuscar
	{
		public static string CogerCadena(string idiomaUsuario, string cadena)
		{
			if (File.Exists("Idiomas/" + idiomaUsuario + ".json") == false)
			{
				idiomaUsuario = "en-US";
			}

			using (StreamReader r = new StreamReader("Idiomas/" + idiomaUsuario + ".json"))
			{
				string json = r.ReadToEnd();
				List<Idioma> items = JsonConvert.DeserializeObject<List<Idioma>>(json);

				foreach (var item in items)
				{
					if (item.Id == cadena)
					{
						return item.Valor;
					}
				}
			}

			return null;
		}
	}
	
}
