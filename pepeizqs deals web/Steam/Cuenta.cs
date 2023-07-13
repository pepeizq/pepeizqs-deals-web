﻿#nullable disable

using Herramientas;
using Newtonsoft.Json;

namespace Steam
{
    public static class Cuenta
    {
        public static async Task<SteamJuegosyDeseados> CargarDatos(string enlace)
        {
            int cuentaTipo = 0;
            string usuario = string.Empty;
            string id64 = string.Empty;

            if (enlace.Contains("https://steamcommunity.com/id/") == true)
            {
                cuentaTipo = 1;

                usuario = enlace;

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
                cuentaTipo = 2;

                id64 = enlace;

                id64 = id64.Replace("https://steamcommunity.com/profiles/", null);
                id64 = id64.Replace("http://steamcommunity.com/profiles/", null);

                if (id64.Contains("?") == true)
                {
                    int int1 = id64.IndexOf("?");
                    id64 = id64.Remove(int1, id64.Length - int1);
                }

                if (id64.Contains("/") == true)
                {
                    int int1 = id64.IndexOf("/");
                    id64 = id64.Remove(int1, id64.Length - int1);
                }
            }

            if (id64 != string.Empty)
            {
                string html = await Decompiladores.Estandar("https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key=41F2D73A0B5024E9101F8D4E8D8AC21E&steamids=" + id64);

                if (html != null)
                {
                    SteamCuentaAPI cuenta = JsonConvert.DeserializeObject<SteamCuentaAPI>(html);

                    if (cuenta != null)
                    {
                        string juegos = string.Empty;
                        string htmlJuegos = await Decompiladores.Estandar("https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/?key=41F2D73A0B5024E9101F8D4E8D8AC21E&steamid=" + cuenta.Datos.Jugador[0].ID64 + "&include_appinfo=1&include_played_free_games=1");

                        if (htmlJuegos != null) 
                        {
                            SteamJuegosAPI json = JsonConvert.DeserializeObject<SteamJuegosAPI>(htmlJuegos);

                            if (json.Datos != null)
                            {
                                if (json.Datos.Juegos != null)
                                {
                                    if (json.Datos.Juegos.Count > 0)
                                    {
                                        foreach (SteamJuegosAPIJuego juego in json.Datos.Juegos)
                                        {
                                            if (juegos == string.Empty)
                                            {
                                                juegos = juego.ID;
                                            }
                                            else
                                            {
                                                juegos = juegos + "," + juego.ID;
                                            }           
                                        }
                                    }
                                }
                            }
                        }

                        //----------------------------------------------

                        string deseados = string.Empty;

                        List<SteamDeseadoJuego> juegosDeseadosTodos = new List<SteamDeseadoJuego>();
                        int i = 0;
                        while (i < 100)
                        {
                            string htmlDeseados = string.Empty;

                            if (cuentaTipo == 1)
                            {
                                htmlDeseados = await Decompiladores.Estandar("https://store.steampowered.com/wishlist/id/" + usuario + "/wishlistdata/?p=" + i.ToString());
                            }
                            else if (cuentaTipo == 2)
                            {
                                htmlDeseados = await Decompiladores.Estandar("https://store.steampowered.com/wishlist/profiles/" + id64 + "/wishlistdata/?p=" + i.ToString());
                            }

                            if (htmlDeseados != null)
                            {
                                if (htmlDeseados == "[]")
                                {
                                    break;
                                }

                                var juegosDeseados = new Dictionary<int, SteamDeseadoJuego>();

                                try
                                {
                                    juegosDeseados = JsonConvert.DeserializeObject<Dictionary<int, SteamDeseadoJuego>>(htmlDeseados);
                                }
                                catch (Exception)
                                {
                                    break;
                                };

                                if (juegosDeseados != null)
                                {
                                    if (juegosDeseados.Count > 0)
                                    {
                                        foreach (var juegoDeseado in juegosDeseados)
                                        {
                                            juegosDeseadosTodos.Add(juegoDeseado.Value);
                                        }
                                    }
                                }
                            }
                            i += 1;
                        }

                        if (juegosDeseadosTodos.Count > 0) 
                        {
                            foreach (var deseado in juegosDeseadosTodos)
                            {
                                if (deseado.capsule != null)
                                {
                                    string id = deseado.capsule;

                                    int int1 = id.IndexOf("/apps/");
                                    id = id.Remove(0, int1 + 6);

                                    int int2 = id.IndexOf("/");
                                    id = id.Remove(int2, id.Length - int2);

                                    if (deseados == string.Empty)
                                    {
                                        deseados = id;
                                    }
                                    else
                                    {
                                        deseados = deseados + "," + id;
                                    }
                                }
                            }
                        }

                        //----------------------------------------------

                        SteamJuegosyDeseados datos = new SteamJuegosyDeseados
                        {
                            juegos = juegos,
                            deseados = deseados
                        };

                        return datos;
                    }
                }
            }

            return null;
        }

        public static string Mensaje(SteamJuegosyDeseados datos)
        {
            string mensaje = string.Empty;

            if (datos != null)
            {
                if (datos.juegos != string.Empty)
                {
                    string juegos = datos.juegos;

                    int i = 0;
                    int j = 100000;

                    while (i < j)
                    {
                        if (juegos.Contains(",") == true)
                        {
                            int int1 = juegos.IndexOf(",");
                            juegos = juegos.Remove(0, int1 + 1);
                        }
                        else 
                        {
                            break;
                        }

                        i += 1;
                    }

                    if (i > 0)
                    {
                        mensaje = i.ToString() + " games detected on your account";
                    }
                }

                if (datos.deseados != string.Empty)
                {
                    string deseados = datos.deseados;

                    int i = 0;
                    int j = 100000;

                    while (i < j)
                    {
                        if (deseados.Contains(",") == true)
                        {
                            int int1 = deseados.IndexOf(",");
                            deseados = deseados.Remove(0, int1 + 1);
                        }
                        else
                        {
                            break;
                        }

                        i += 1;
                    }

                    if (i > 0)
                    {
                        if (mensaje == string.Empty) 
                        {
                            mensaje = (i + 1).ToString() + " games detected in your wishlist";
                        }
                        else
                        {
                            mensaje = mensaje + " and " + (i + 1).ToString() + " games detected in your wishlist";
                        }                     
                    }
                }
            }

            return mensaje;
        }
    }

    //----------------------------------------------

    public class SteamJuegosyDeseados
    {
        public string juegos { get; set; }
        public string deseados { get; set; }
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

    //----------------------------------------------

    public class SteamDeseadoJuego
    {
        public string name;
        public string capsule;
    }
}