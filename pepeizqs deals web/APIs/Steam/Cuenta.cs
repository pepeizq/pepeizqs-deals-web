#nullable disable

using Herramientas;
using Newtonsoft.Json;

namespace APIs.Steam
{
    public static class Cuenta
    {
        public static async Task<SteamCuentaID64> CargarID64(string enlace)
        {
            string id64 = string.Empty;
            int cuentaTipo = 0;
            string usuario = string.Empty;

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

            if (string.IsNullOrEmpty(id64) == false)
            {
                SteamCuentaID64 nuevaCuenta = new SteamCuentaID64
                {
                    ID64 = id64,
                    CuentaTipo = cuentaTipo,
                    Usuario = usuario
                };

                return nuevaCuenta;
            }

            return null;
        }

        public static async Task<SteamUsuario> CargarDatos(string enlace)
        {
            SteamCuentaID64 nuevaCuenta = await CargarID64(enlace);

            if (nuevaCuenta != null)
            {
                string html = await Decompiladores.Estandar("https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key=41F2D73A0B5024E9101F8D4E8D8AC21E&steamids=" + nuevaCuenta.ID64);

                if (html != null)
                {
                    SteamCuentaAPI cuenta = JsonConvert.DeserializeObject<SteamCuentaAPI>(html);

                    if (cuenta != null)
                    {
                        string juegos = string.Empty;
                        string htmlJuegos = await Decompiladores.Estandar("https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/?key=41F2D73A0B5024E9101F8D4E8D8AC21E&steamid=" + cuenta.Datos.Jugador[0].ID64 + "&include_appinfo=1&include_played_free_games=1&include_extended_appinfo=1");

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

                        try
                        {
                            List<SteamDeseadoJuego> juegosDeseadosTodos = new List<SteamDeseadoJuego>();
                            int i = 0;
                            while (i < 100)
                            {
                                string htmlDeseados = string.Empty;

                                if (nuevaCuenta.CuentaTipo == 1)
                                {
                                    htmlDeseados = await Decompiladores.Estandar("https://store.steampowered.com/wishlist/id/" + nuevaCuenta.Usuario + "/wishlistdata/?p=" + i.ToString());
                                }
                                else if (nuevaCuenta.CuentaTipo == 2)
                                {
                                    htmlDeseados = await Decompiladores.Estandar("https://store.steampowered.com/wishlist/profiles/" + nuevaCuenta.ID64 + "/wishlistdata/?p=" + i.ToString());
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
                        }
                        catch { }

                        //----------------------------------------------

                        bool grupoPremium = false;
                        bool grupoNormal = false;
                        string htmlGrupos = await Decompiladores.Estandar("https://api.steampowered.com/ISteamUser/GetUserGroupList/v0001/?key=41F2D73A0B5024E9101F8D4E8D8AC21E&steamid=" + cuenta.Datos.Jugador[0].ID64);

                        if (htmlGrupos != null)
                        {
							SteamGruposAPI json = JsonConvert.DeserializeObject<SteamGruposAPI>(htmlGrupos);

                            if (json != null) 
                            { 
                                foreach (var grupo in json.Datos.Grupos) 
                                { 
                                    if (grupo.Id == "40604285")
                                    {
                                        grupoPremium = true;
                                    }
                                    
                                    if (grupo.Id == "33500256")
									{
										grupoNormal = true;
									}
								}
                            }
						}

                        //----------------------------------------------

                        SteamUsuario datos = new SteamUsuario
                        {
                            Juegos = juegos,
                            Deseados = deseados,
                            Avatar = cuenta.Datos.Jugador[0].Avatar,
                            Nombre = cuenta.Datos.Jugador[0].Nombre,
                            GrupoPremium = grupoPremium.ToString(),
                            GrupoNormal = grupoNormal.ToString(),
                            SteamId = nuevaCuenta.ID64
                        };

                        return datos;
                    }
                }
            }

            return null;
        }
    }

    //----------------------------------------------

    public class SteamUsuario
    {
        public string Juegos { get; set; }
        public string Deseados { get; set; }
		public string Avatar { get; set; }
		public string Nombre { get; set; }
        public string GrupoPremium { get; set; }
		public string GrupoNormal { get; set; }
        public string SteamId { get; set; }
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

    //----------------------------------------------

    public class SteamCuentaID64
    {
        public string ID64;
        public int CuentaTipo;
        public string Usuario;
    }

    //----------------------------------------------

    public class SteamGruposAPI
	{
		[JsonProperty("response")]
		public SteamGruposAPIDatos Datos { get; set; }
	}

	public class SteamGruposAPIDatos
	{
		[JsonProperty("groups")]
		public List<SteamGruposAPIGrupo> Grupos { get; set; }
	}

	public class SteamGruposAPIGrupo
	{
		[JsonProperty("gid")]
		public string Id { get; set; }
	}
}
