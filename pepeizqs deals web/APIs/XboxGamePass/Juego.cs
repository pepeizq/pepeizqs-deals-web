#nullable disable

using Juegos;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIs.XboxGamePass
{
	public static class Juego
	{
		public static async Task<JuegoXbox> XboxDatos(string id)
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
					JuegoXbox xbox = new JuegoXbox();

					if (datos.Productos[id].Plataformas != null)
					{
						if (datos.Productos[id].Plataformas.Count > 0)
						{
							foreach (var plataforma in datos.Productos[id].Plataformas)
							{
								if (plataforma == "Windows.Desktop")
								{
									xbox.Windows = true;
								}
							}
						}
					}

					if (datos.Productos[id].Atributos != null)
					{
						if (datos.Productos[id].Atributos.Count > 0)
						{
							foreach (var atributo in datos.Productos[id].Atributos)
							{
								if (atributo.Nombre == "XblAchievements")
								{
									xbox.Logros = true;
								}

								if (atributo.Nombre == "XblCloudSaves")
								{
									xbox.GuardadoNube = true;
								}

								if (atributo.Nombre == "GameStreaming")
								{
									xbox.Streaming = true;
								}
							}
						}
					}

					xbox.Fecha = DateTime.Now;

					return xbox;
				}
			}

			return null;
		}

		public static async Task<List<JuegoIdioma>> XboxIdiomas(string id, List<JuegoIdioma> listadoIdiomas)
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
					List<JuegoIdioma> idiomas = Herramientas.Idiomas.XboxSacarIdiomas(datos.Productos[id].Idiomas);

					if (listadoIdiomas == null)
					{
						listadoIdiomas = idiomas;
					}
					else
					{
						List<JuegoIdioma> listadoActualizar = listadoIdiomas;

						foreach (var nuevoIdioma in idiomas)
						{
							bool existe = false;

							foreach (var viejoIdioma in listadoActualizar)
							{
								if (viejoIdioma.DRM == nuevoIdioma.DRM && nuevoIdioma.Idioma == viejoIdioma.Idioma)
								{
									existe = true;

									viejoIdioma.Audio = nuevoIdioma.Audio;
									viejoIdioma.Texto = nuevoIdioma.Texto;

									break;
								}
							}

							if (existe == false)
							{
								listadoActualizar.Add(nuevoIdioma);
							}
						}

						return listadoActualizar;
					}
				}
			}

			return listadoIdiomas;
		}

		public static async Task<string> CargarIdiomasAdmin(string id)
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
					return JsonSerializer.Serialize(datos.Productos[id].Idiomas);
				}
			}

			return null;
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
		public Dictionary<string, XboxJuegoAPIIdioma> Idiomas { get; set; }
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
