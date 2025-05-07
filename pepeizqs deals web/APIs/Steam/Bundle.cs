#nullable disable

using Herramientas;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIs.Steam
{
	public static class Bundle
	{
		public static async Task<string> CargarDatosBundle(string enlace)
		{
			string id = Juego.LimpiarID(enlace);

			if (string.IsNullOrEmpty(id) == true)
			{
				return null;
			}

			string html2 = await Decompiladores.Estandar(@"https://api.steampowered.com/IStoreBrowseService/GetItems/v1?input_json={""ids"":[{""bundleid"":" + id + @"}],""context"":{""language"":""english"",""country_code"":""ES"",""steam_realm"":1},""data_request"":{""include_reviews"":true,""include_basic_info"":true, ""include_assets"": true, ""include_links"": true, ""include_tag_count"": 20, ""include_release"": true, ""include_platforms"": true, ""include_screenshots"": true, ""include_trailers"": true, ""include_supported_languages"": true}}");

			if (string.IsNullOrEmpty(html2) == false)
			{
				SteamBundleAPI datos2 = null;

				try
				{
					datos2 = JsonSerializer.Deserialize<SteamBundleAPI>(html2);
				}
				catch { }

				if (datos2 != null)
				{
					if (datos2.Respuesta.Bundles.Count == 1)
					{
						if (datos2.Respuesta.Bundles[0].Apps != null)
						{
							if (datos2.Respuesta.Bundles[0].Apps.Count > 0)
							{
								string idsBuscar = string.Empty;

								foreach (int idApp in datos2.Respuesta.Bundles[0].Apps)
								{
									if (idApp > 0)
									{
										Juegos.Juego juego = BaseDatos.Juegos.Buscar.UnJuego(null, idApp.ToString());

										if (juego != null)
										{
											if (idsBuscar.Length > 0)
											{
												idsBuscar = idsBuscar + ",";
											}

											idsBuscar = idsBuscar + juego.Id.ToString();
										}
										else
										{
											Juegos.Juego nuevoJuego = await APIs.Steam.Juego.CargarDatosJuego(idApp.ToString());

											if (nuevoJuego != null)
											{
												BaseDatos.Juegos.Insertar.Ejecutar(nuevoJuego);
												Juegos.Juego nuevoJuego2 = BaseDatos.Juegos.Buscar.UnJuego(null, idApp.ToString());

												if (idsBuscar.Length > 0)
												{
													idsBuscar = idsBuscar + ",";
												}

												idsBuscar = idsBuscar + nuevoJuego2.Id.ToString();
											}
										}
									}
								}

								if (string.IsNullOrEmpty(idsBuscar) == false)
								{
									return idsBuscar;
								}
							}
						}
					}
				}
			}

			return null;
		}
	}

	#region Clases API Bundle

	public class SteamBundleAPI
	{
		[JsonPropertyName("response")]
		public SteamBundleAPIResultados Respuesta { get; set; }
	}

	public class SteamBundleAPIResultados
	{
		[JsonPropertyName("store_items")]
		public List<SteamBundleAPIBundle> Bundles { get; set; }
	}

	public class SteamBundleAPIBundle
	{
		[JsonPropertyName("name")]
		public string Nombre { get; set; }

		[JsonPropertyName("type")]
		public int Tipo { get; set; }

		[JsonPropertyName("included_appids")]
		public List<int> Apps { get; set; }
	}

	#endregion
}
