#nullable disable

using Newtonsoft.Json;

namespace Herramientas
{
	public static class Github
	{
		public static async Task<string> UltimaModificacion()
		{
			string html = await Decompiladores.Estandar("https://api.github.com/repos/pepeizq/pepeizqs-deals-web");

			if (string.IsNullOrEmpty(html) == false)
			{
				GithubAPI api = JsonConvert.DeserializeObject<GithubAPI>(html);

				if (api != null) 
				{
					return api.UltimaModificacion;
				}
			}

			return null;
		}
	}

	public class GithubAPI
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("updated_at")]
		public string UltimaModificacion { get; set; }
	}
}
