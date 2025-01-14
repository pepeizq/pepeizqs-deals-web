#nullable disable

using Herramientas;
using System.Text.Json;
using System.Text.Json.Serialization;

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

                if (string.IsNullOrEmpty(html) == false)
                {
					SteamSacarID id = JsonSerializer.Deserialize<SteamSacarID>(html);

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

                if (string.IsNullOrEmpty(html) == false)
                {
                    SteamCuentaAPI cuenta = JsonSerializer.Deserialize<SteamCuentaAPI>(html);

                    if (cuenta != null)
                    {
                        string juegos = string.Empty;
                        string htmlJuegos = await Decompiladores.Estandar("https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/?key=41F2D73A0B5024E9101F8D4E8D8AC21E&steamid=" + cuenta.Datos.Jugador[0].ID64 + "&include_appinfo=1&include_played_free_games=1&include_extended_appinfo=1");

                        if (htmlJuegos != null)
                        {
                            SteamJuegosAPI json = JsonSerializer.Deserialize<SteamJuegosAPI>(htmlJuegos);

                            if (json != null)
                            {
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
                                                    juegos = juego.ID.ToString();
                                                }
                                                else
                                                {
                                                    juegos = juegos + "," + juego.ID.ToString();
                                                }
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
							string htmlDeseados = string.Empty;

							htmlDeseados = await Decompiladores.Estandar("https://api.steampowered.com/IWishlistService/GetWishlist/v1/?input_json={%22steamid%22:%20%22" + nuevaCuenta.ID64 + "%22}");

							if (string.IsNullOrEmpty(htmlDeseados) == false)
							{
								if (htmlDeseados != "[]" || htmlDeseados != "null")
								{
									SteamDeseadosRespuesta juegosDeseados = new SteamDeseadosRespuesta();

									try
									{
										juegosDeseados = JsonSerializer.Deserialize<SteamDeseadosRespuesta>(htmlDeseados);
									}
									catch (Exception ex)
									{
                                        BaseDatos.Errores.Insertar.Mensaje("Steam deseados", ex);
									};

									if (juegosDeseados != null)
									{
										if (juegosDeseados.Datos.Juegos.Count > 0)
										{
											foreach (var juegoDeseado in juegosDeseados.Datos.Juegos)
											{
												if (deseados == string.Empty)
												{
													deseados = juegoDeseado.Id.ToString();
												}
												else
												{
													deseados = deseados + "," + juegoDeseado.Id.ToString();
												}
											}
										}
									}
								}
							}

							
						}
                        catch (Exception ex) 
                        { 
                            BaseDatos.Errores.Insertar.Mensaje("Steam deseados", ex);
                        }

						//----------------------------------------------

						bool grupoPremium = false;
                        bool grupoNormal = false;
                        string htmlGrupos = await Decompiladores.Estandar("https://api.steampowered.com/ISteamUser/GetUserGroupList/v0001/?key=41F2D73A0B5024E9101F8D4E8D8AC21E&steamid=" + cuenta.Datos.Jugador[0].ID64);

                        if (string.IsNullOrEmpty(htmlGrupos) == false)
                        {
                            SteamGruposAPI json = JsonSerializer.Deserialize<SteamGruposAPI>(htmlGrupos);

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
                            Avatar = cuenta.Datos.Jugador[0].Avatar,
                            Nombre = cuenta.Datos.Jugador[0].Nombre,
                            GrupoPremium = grupoPremium.ToString(),
                            GrupoNormal = grupoNormal.ToString(),
                            SteamId = nuevaCuenta.ID64
                        };

                        if (string.IsNullOrEmpty(juegos) == false)
                        {
                            datos.Juegos = juegos;
                        }

						if (string.IsNullOrEmpty(deseados) == false)
						{
							datos.Deseados = deseados;
						}

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
        [JsonPropertyName("response")]
        public SteamCuentaAPIDatos Datos { get; set; }
    }

    public class SteamCuentaAPIDatos
    {
        [JsonPropertyName("players")]
        public List<SteamCuentaAPIJugador> Jugador { get; set; }
    }

    public class SteamCuentaAPIJugador
    {
        [JsonPropertyName("steamid")]
        public string ID64 { get; set; }

        [JsonPropertyName("personaname")]
        public string Nombre { get; set; }

        [JsonPropertyName("avatarfull")]
        public string Avatar { get; set; }
    }

    //----------------------------------------------

    public class SteamSacarID
    {
        [JsonPropertyName("response")]
        public SteamSacarID2 Datos { get; set; }
    }

    public class SteamSacarID2
    {
        [JsonPropertyName("steamid")]
        public string ID64 { get; set; }
    }

    //----------------------------------------------

    public class SteamJuegosAPI
    {
        [JsonPropertyName("response")]
        public SteamJuegosAPIDatos Datos { get; set; }
    }

    public class SteamJuegosAPIDatos
    {
        [JsonPropertyName("game_count")]
        public int CantidadJuegos { get; set; }

        [JsonPropertyName("games")]
        public List<SteamJuegosAPIJuego> Juegos { get; set; }
    }

    public class SteamJuegosAPIJuego
    {
        [JsonPropertyName("appid")]
        public int ID { get; set; }

        [JsonPropertyName("name")]
        public string Titulo { get; set; }

        [JsonPropertyName("img_icon_url")]
        public string Icono { get; set; }
    }

    //----------------------------------------------

	public class SteamDeseadosRespuesta
	{
		[JsonPropertyName("response")]
		public SteamDeseadosRespuestaJuegos Datos { get; set; }
	}

	public class SteamDeseadosRespuestaJuegos
	{
		[JsonPropertyName("items")]
		public List<SteamDeseadosRespuestaJuego> Juegos { get; set; }
	}

	public class SteamDeseadosRespuestaJuego
	{
		[JsonPropertyName("appid")]
		public int Id { get; set; }
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
		[JsonPropertyName("response")]
		public SteamGruposAPIDatos Datos { get; set; }
	}

	public class SteamGruposAPIDatos
	{
		[JsonPropertyName("groups")]
		public List<SteamGruposAPIGrupo> Grupos { get; set; }
	}

	public class SteamGruposAPIGrupo
	{
		[JsonPropertyName("gid")]
		public string Id { get; set; }
	}
}
