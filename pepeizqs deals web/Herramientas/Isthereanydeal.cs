#nullable disable

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Herramientas
{
	public static class Isthereanydeal
	{
		public static List<string> ImportarDeseados(string jsonContenido)
		{
			List<string> listaJuegos = new List<string>();

			if (string.IsNullOrEmpty(jsonContenido) == false)
			{
				IsthereanydealAPI datos = JsonSerializer.Deserialize<IsthereanydealAPI>(jsonContenido);

				if (datos != null)
				{
					foreach (var grupo in datos.Datos.Grupos)
					{
						foreach (var juego in grupo.Juegos)
						{
							listaJuegos.Add(juego.Nombre);
						}
					}
				}
			}

			return listaJuegos;
		}
	}

	public class IsthereanydealAPI
	{
		[JsonPropertyName("data")]
		public IsthereanydealAPIDatos Datos { get; set; }
	}

	public class IsthereanydealAPIDatos
	{
		[JsonPropertyName("data")]
		public List<IsthereanydealAPIGrupo> Grupos { get; set; }
	}

	public class IsthereanydealAPIGrupo
	{
		[JsonPropertyName("games")]
		public List<IsthereanydealAPIJuego> Juegos { get; set; }
	}

	public class IsthereanydealAPIJuego
	{
		[JsonPropertyName("title")]
		public string Nombre { get; set; }
	}
}
