#nullable disable

using Herramientas;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIs.Steam
{
    public static class Curator
    {
        public static async Task<SteamCuratorAPI> Extraer(string id)
        {
            string html = await Decompiladores.Estandar("https://store.steampowered.com/curator/" + id + "/ajaxgetcreatorhomeinfo?get_appids=true");
        
            if (string.IsNullOrEmpty(html) == false)
            {
                SteamCuratorAPI api = JsonSerializer.Deserialize<SteamCuratorAPI>(html);

                if (api != null)
                {
                    return api;
                }
			}

            return null;
        }

		public static async Task<SteamCuratorAPIVanidad> ExtraerVanidad(string id)
		{
			string html = await Decompiladores.Estandar("https://steamcommunity.com/gid/" + id + "/ajaxgetvanityandclanid/");

			if (string.IsNullOrEmpty(html) == false)
			{
				SteamCuratorAPIVanidad api = JsonSerializer.Deserialize<SteamCuratorAPIVanidad>(html);

				if (api != null)
				{
					return api;
				}
			}

			return null;
		}
	}

    public class SteamCuratorAPI
    {
		[JsonPropertyName("creator_clan_id")]
		public int Id { get; set; }

		[JsonPropertyName("name")]
		public string Nombre { get; set; }

		[JsonPropertyName("avatar_url_full_size")]
		public string Imagen { get; set; }

		[JsonPropertyName("tag_line_localized")]
		public string Descripcion { get; set; }

		[JsonPropertyName("vanity")]
		public string Slug { get; set; }

		[JsonPropertyName("appids")]
		public List<int> SteamIds { get; set; }

		[JsonPropertyName("weblink")]
		public SteamCuratorAPIWeb Web { get; set; }
	}

	public class SteamCuratorAPIWeb
    {
		[JsonPropertyName("url")]
		public string Enlace { get; set; }

		[JsonPropertyName("title")]
		public string Nombre { get; set; }
	}

	public class SteamCuratorAPIVanidad
	{
		[JsonPropertyName("clanAccountID")]
		public int Id { get; set; }

		[JsonPropertyName("creator_page_bg_url")]
		public string Imagen { get; set; }
	}
}
