#nullable disable

using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using pepeizqs_deals_web.Areas.Identity.Data;

namespace BaseDatos.Usuarios
{
	public static class Buscar
	{
		public static bool RolDios(string username)
		{
			if (string.IsNullOrEmpty(username) == false) 
			{
				SqlConnection conexion = Herramientas.BaseDatos.Conectar();

				using (conexion)
				{
					string busqueda = "SELECT * FROM AspNetUsers WHERE UserName=@UserName";

					using (SqlCommand comando = new SqlCommand(busqueda, conexion))
					{
						comando.Parameters.AddWithValue("@UserName", username);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							while (lector.Read())
							{
								if (lector.GetString(2) == "God")
								{
									return true;
								}
							}
						}
					}
				}
			}

			return false;
		}

		public static string UnUsuarioCorreo(SqlConnection conexion, string usuarioId)
		{
			if (string.IsNullOrEmpty(usuarioId) == false)
			{
                string busqueda = "SELECT * FROM AspNetUsers WHERE Id=@Id";

                using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
                    comando.Parameters.AddWithValue("@Id", usuarioId);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read()) 
						{
                            if (lector.IsDBNull(8) == false)
                            {
                                if (string.IsNullOrEmpty(lector.GetString(8)) == false)
                                {
                                    return lector.GetString(8);
                                }
                            }
                        }
					}
                }
            }

			return null;
		}

		public static string UnUsuarioDeseados(string usuarioId, string juegoId, JuegoDRM drm, int juegoIdSteam = 0)
		{
			if (string.IsNullOrEmpty(usuarioId) == false)
			{
				SqlConnection conexion = Herramientas.BaseDatos.Conectar();

				using (conexion)
				{
					string busqueda = "SELECT * FROM AspNetUsers WHERE Id=@Id";

					using (SqlCommand comando = new SqlCommand(busqueda, conexion))
					{
						comando.Parameters.AddWithValue("@Id", usuarioId);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							while (lector.Read())
							{
								bool notificaciones = false;
								bool correo = false;
								bool steam = true;
                            
                                //Enseñar Notificaciones Minimos
                                if (lector.IsDBNull(25) == false)
								{
									if (lector.GetBoolean(25) == true)
									{
										notificaciones = true;
									}
								}

                                //Correo Confirmado
                                if (lector.IsDBNull(10) == false)
								{
									if (lector.GetBoolean(10) == true)
									{
										correo = true;
                                    }
                                }

                                //Juegos Steam
								if (juegoIdSteam > 0)
								{
                                    if (drm == JuegoDRM.Steam)
                                    {
                                        if (lector.IsDBNull(4) == false)
                                        {
											string tempSteam = string.Empty;

											try
											{
												tempSteam = lector.GetString(4);
											}
											catch { }

											if (string.IsNullOrEmpty(tempSteam) == false)
											{
												List<string> juegosSteam = Listados.Generar(tempSteam);

												if (juegosSteam != null)
												{
													if (juegosSteam.Count > 0)
													{
														foreach (var juegoSteam in juegosSteam)
														{
															if (juegoSteam == juegoIdSteam.ToString())
															{
																steam = false;
															}
														}
													}
												}												
											}
										}
                                    }
                                }                             

                                if (correo == true && notificaciones == true && steam == true)
								{
									//Deseados Steam
									if (drm == JuegoDRM.Steam && lector.IsDBNull(5) == false)
									{
										if (string.IsNullOrEmpty(lector.GetString(5)) == false)
										{
											List<string> deseadosSteam = Listados.Generar(lector.GetString(5));

											if (deseadosSteam.Count > 0)
											{
												foreach (var deseadoSteam in deseadosSteam)
												{
													if (juegoIdSteam.ToString() == deseadoSteam)
													{
														//Correo
														if (lector.IsDBNull(8) == false)
														{
															if (string.IsNullOrEmpty(lector.GetString(8)) == false)
															{
																return lector.GetString(8);
															}
														}
													}
												}
											}
										}
									}

									//Deseados Web
									if (lector.IsDBNull(20) == false)
									{
										if (string.IsNullOrEmpty(lector.GetString(20)) == false)
										{
											string deseadosTexto = lector.GetString(20);
											List<JuegoDeseado> deseados = null;
                                          
                                            try
											{
												deseados = JsonConvert.DeserializeObject<List<JuegoDeseado>>(deseadosTexto);
											}
											catch { }

											if (deseados != null)
											{
												if (deseados.Count > 0)
												{
                                                    foreach (var deseado in deseados)
													{
														if (deseado.IdBaseDatos == juegoId && deseado.DRM == drm)
														{
															//Correo
															if (lector.IsDBNull(8) == false)
															{
																if (string.IsNullOrEmpty(lector.GetString(8)) == false)
																{
																	return lector.GetString(8);
																}
															}
														}
													}
												}
											}
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

        public static bool CuentaSteamUsada(string id64Steam, string idUsuario)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string busqueda = "SELECT * FROM AspNetUsers WHERE SteamId=@SteamId";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@SteamId", id64Steam);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							if (lector.IsDBNull(0) == false)
							{
								if (lector.GetString(0) != idUsuario)
								{
									return true;
								}							
							}
						}
					}
				}
			}

			return false;
        }
    }
}
