#nullable disable

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIs.XboxGamePass
{
	public static class Juego
	{
		public static async void XboxDatos(string id)
		{
			HttpClient cliente = new HttpClient();
			cliente.BaseAddress = new Uri("https://www.xbox.com/xbox-game-pass");
			cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			HttpRequestMessage peticion = new HttpRequestMessage(HttpMethod.Post, "https://catalog.gamepass.com/products?market=US&language=en-US&hydration=MobileDetailsForConsole");
			peticion.Content = new StringContent("{\r\n  \"Products\": [ \"" + id + "\" ]\r\n}",
												Encoding.UTF8, "application/json");

			HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

			string html = string.Empty;

			try
			{
				html = await respuesta.Content.ReadAsStringAsync();
			}
			catch { }

			if (string.IsNullOrEmpty(html) == false)
			{
				XboxJuegoAPI datos = JsonSerializer.Deserialize<XboxJuegoAPI>(html);

				if (datos != null)
				{
					BaseDatos.Errores.Insertar.Mensaje("test", datos.Productos[id].Nombre);
				}


				
			}
		}
	}

	public class XboxJuegoAPI
	{
		[JsonPropertyName("Products")]
		public Dictionary<string, XboxJuegoAPIProducto> Productos { get; set; }
	}

	public class XboxJuegoAPIProducto
	{
		[JsonPropertyName("ProductTitle")]
		public string Nombre { get; set; }

		[JsonPropertyName("Attributes")]
		public List<XboxJuegoAPIAtributo> Atributos { get; set; }

		[JsonPropertyName("AllowedPlatforms")]
		public List<string> Plataformas { get; set; }

		[JsonPropertyName("LanguageSupport")]
		public Dictionary<string, XboxJuegoAPIProducto> Idiomas { get; set; }
	}

	public class XboxJuegoAPIAtributo
	{
		[JsonPropertyName("Name")]
		public string Nombre { get; set; }
	}

	public class XboxJuegoAPIIdioma
	{
		[JsonPropertyName("InterfaceLanguageSupport")]
		public int Interfaz { get; set; }

		[JsonPropertyName("GamePlayAudioLanguageSupport")]
		public int Audio { get; set; }

		[JsonPropertyName("SubtitlesLanguageSupport")]
		public int Subtitulos { get; set; }
	}
}
