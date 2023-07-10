#nullable disable

using Herramientas;
using Newtonsoft.Json;

namespace Steam
{
    public static class Cuenta
    {
        public static async Task<string>CargarDatos(string enlace)
        {
            string id64 = string.Empty;

            if (enlace.Contains("https://steamcommunity.com/id/") == true)
            {
                string usuario = enlace;

                usuario = usuario.Replace("https://steamcommunity.com/id/", null);
                usuario = usuario.Replace("http://steamcommunity.com/id/", null);

                if (usuario.Contains("?") == true)
                {
                    int int1 = usuario.IndexOf("?");
                    usuario = usuario.Remove(int1, usuario.Length - int1);
                }

                if (usuario.Contains("/") == true)
                {
                    int int1 = usuario.IndexOf("/");
                    usuario = usuario.Remove(int1, usuario.Length - int1);
                }

                string html = await Decompiladores.Estandar("https://api.steampowered.com/ISteamUser/ResolveVanityURL/v1/?key=41F2D73A0B5024E9101F8D4E8D8AC21E&vanityurl=" + usuario);

                if (html != null)
                {
                    SteamSacarID id = JsonConvert.DeserializeObject<SteamSacarID>(html);

                    if (id != null)
                    {
                        id64 = id.Datos.ID64;
                    }
                }
            }
            else if (enlace.Contains("https://steamcommunity.com/profiles/") == true)
            {
                string usuario = enlace;

                usuario = usuario.Replace("https://steamcommunity.com/profiles/", null);
                usuario = usuario.Replace("http://steamcommunity.com/profiles/", null);

                if (usuario.Contains("?") == true)
                {
                    int int1 = usuario.IndexOf("?");
                    usuario = usuario.Remove(int1, usuario.Length - int1);
                }

                if (usuario.Contains("/") == true)
                {
                    int int1 = usuario.IndexOf("/");
                    usuario = usuario.Remove(int1, usuario.Length - int1);
                }

                id64 = usuario;
            }

            if (id64 != string.Empty)
            {
                string html = await Decompiladores.Estandar("https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key=41F2D73A0B5024E9101F8D4E8D8AC21E&steamids=" + id64);

                if (html != null)
                {
                    SteamCuentaAPI cuenta = JsonConvert.DeserializeObject<SteamCuentaAPI>(html);

                    if (cuenta != null)
                    {
                        string htmlJuegos = await Decompiladores.Estandar("https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/?key=41F2D73A0B5024E9101F8D4E8D8AC21E&steamid=" + cuenta.Datos.Jugador[0].ID64 + "&include_appinfo=1&include_played_free_games=1");

                        if (htmlJuegos != null) 
                        {
                            SteamJuegosAPI json = JsonConvert.DeserializeObject<SteamJuegosAPI>(html);

                            if (json.Datos != null)
                            {
                                if (json.Datos.Juegos != null)
                                {
                                    if (json.Datos.Juegos.Count > 0)
                                    {


                                        foreach (SteamJuegosAPIJuego juego in json.Datos.Juegos)
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }
    }

    //----------------------------------------------

    public class SteamCuentaAPI
    {
        [JsonProperty("response")]
        public SteamCuentaAPIDatos Datos { get; set; }
    }

    public class SteamCuentaAPIDatos
    {
        [JsonProperty("players")]
        public List<SteamCuentaAPIJugador> Jugador { get; set; }
    }

    public class SteamCuentaAPIJugador
    {
        [JsonProperty("steamid")]
        public string ID64 { get; set; }

        [JsonProperty("personaname")]
        public string Nombre { get; set; }

        [JsonProperty("avatarfull")]
        public string Avatar { get; set; }
    }

    //----------------------------------------------

    public class SteamSacarID
    {
        [JsonProperty("response")]
        public SteamSacarID2 Datos { get; set; }
    }

    public class SteamSacarID2
    {
        [JsonProperty("steamid")]
        public string ID64 { get; set; }
    }

    //----------------------------------------------

    public class SteamJuegosAPI
    {
        [JsonProperty("response")]
        public SteamJuegosAPIDatos Datos { get; set; }
    }

    public class SteamJuegosAPIDatos
    {
        [JsonProperty("game_count")]
        public string CantidadJuegos { get; set; }

        [JsonProperty("games")]
        public List<SteamJuegosAPIJuego> Juegos { get; set; }
    }

    public class SteamJuegosAPIJuego
    {
        [JsonProperty("appid")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Titulo { get; set; }

        [JsonProperty("img_icon_url")]
        public string Icono { get; set; }
    }
}
