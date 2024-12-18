#nullable disable

using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace BaseDatos.Usuarios
{
	public static class Buscar
	{
		public static bool RolDios(string id)
		{
			if (string.IsNullOrEmpty(id) == false) 
			{
				SqlConnection conexion = Herramientas.BaseDatos.Conectar();

				using (conexion)
				{
					string busqueda = "SELECT * FROM AspNetUsers WHERE Id=@Id";

					using (SqlCommand comando = new SqlCommand(busqueda, conexion))
					{
						comando.Parameters.AddWithValue("@Id", id);

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

		public static string UsuarioDeseados(string usuarioId, string juegoId, JuegoDRM drm, int juegoIdSteam = 0)
		{
			if (string.IsNullOrEmpty(usuarioId) == false)
			{
				SqlConnection conexion = Herramientas.BaseDatos.Conectar();

				using (conexion)
				{
					string busqueda = "SELECT NotificationLows, EmailConfirmed, SteamGames, SteamWishlist, Email, Wishlist FROM AspNetUsers WHERE Id=@Id";

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
                                if (lector.IsDBNull(0) == false)
								{
									if (lector.GetBoolean(0) == true)
									{
										notificaciones = true;
									}
								}

                                //Correo Confirmado
                                if (lector.IsDBNull(1) == false)
								{
									if (lector.GetBoolean(1) == true)
									{
										correo = true;
                                    }
                                }

                                //Juegos Steam
								if (juegoIdSteam > 0)
								{
                                    if (drm == JuegoDRM.Steam)
                                    {
                                        if (lector.IsDBNull(2) == false)
                                        {
											string tempSteam = string.Empty;

											try
											{
												tempSteam = lector.GetString(2);
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
									if (drm == JuegoDRM.Steam && lector.IsDBNull(3) == false)
									{
										if (string.IsNullOrEmpty(lector.GetString(3)) == false)
										{
											List<string> deseadosSteam = Listados.Generar(lector.GetString(3));

											if (deseadosSteam.Count > 0)
											{
												foreach (var deseadoSteam in deseadosSteam)
												{
													if (juegoIdSteam.ToString() == deseadoSteam)
													{
														//Correo
														if (lector.IsDBNull(4) == false)
														{
															if (string.IsNullOrEmpty(lector.GetString(4)) == false)
															{
																return lector.GetString(4);
															}
														}
													}
												}
											}
										}
									}

									//Deseados Web
									if (lector.IsDBNull(5) == false)
									{
										if (string.IsNullOrEmpty(lector.GetString(5)) == false)
										{
											string deseadosTexto = lector.GetString(5);
											List<JuegoDeseado> deseados = null;
                                          
                                            try
											{
												deseados = JsonSerializer.Deserialize<List<JuegoDeseado>>(deseadosTexto);
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
															if (lector.IsDBNull(4) == false)
															{
																if (string.IsNullOrEmpty(lector.GetString(4)) == false)
																{
																	return lector.GetString(4);
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
				string busqueda = "SELECT Id FROM AspNetUsers WHERE SteamId=@SteamId";

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

		public static bool CuentaGogUsada(string idGog, string idUsuario, SqlConnection conexion = null)
		{
			if (conexion == null)
			{
				conexion = Herramientas.BaseDatos.Conectar();
			}
			else
			{
				if (conexion.State != System.Data.ConnectionState.Open)
				{
					conexion = Herramientas.BaseDatos.Conectar();
				}
			}

			string busqueda = "SELECT Id FROM AspNetUsers WHERE GogId=@GogId";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				comando.Parameters.AddWithValue("@GogId", idGog);

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

			return false;
		}

		public static string UsuarioIdioma(string usuarioId, SqlConnection conexion = null)
		{
			if (string.IsNullOrEmpty(usuarioId) == false)
			{
				if (conexion == null)
				{
					conexion = Herramientas.BaseDatos.Conectar();
				}
				else
				{
					if (conexion.State != System.Data.ConnectionState.Open)
					{
						conexion = Herramientas.BaseDatos.Conectar();
					}
				}

				string busqueda = "SELECT * FROM AspNetUsers WHERE Id=@Id";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@Id", usuarioId);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							if (lector.IsDBNull(lector.GetOrdinal("Language")) == false)
							{
								return lector.GetString(lector.GetOrdinal("Language"));
							}
						}
					}
				}
			}
			
			return "en";
		}

		public static List<string> UsuariosCorreoSumario(SqlConnection conexion = null)
		{
			if (conexion == null)
			{
				conexion = Herramientas.BaseDatos.Conectar();
			}
			else
			{
				if (conexion.State != System.Data.ConnectionState.Open)
				{
					conexion = Herramientas.BaseDatos.Conectar();
				}
			}

			List<string> usuariosId = new List<string>();

			string busqueda = "SELECT Id FROM AspNetUsers WHERE MailSummary='true'";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						if (lector.IsDBNull(lector.GetOrdinal("Id")) == false)
						{
							usuariosId.Add(lector.GetString(lector.GetOrdinal("Id")));
						}
					}
				}
			}

			return usuariosId;
		}

		public static string UsuarioDeseadosNickname(string otroUsuario, SqlConnection conexion = null)
		{
			if (conexion == null)
			{
				conexion = Herramientas.BaseDatos.Conectar();
			}
			else
			{
				if (conexion.State != System.Data.ConnectionState.Open)
				{
					conexion = Herramientas.BaseDatos.Conectar();
				}
			}

			string busqueda = "SELECT Id FROM AspNetUsers WHERE WishlistPublic='true' AND WishlistNickname=@WishlistNickname";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				comando.Parameters.AddWithValue("@WishlistNickname", otroUsuario);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						if (lector.IsDBNull(lector.GetOrdinal("Id")) == false)
						{
							return lector.GetString(lector.GetOrdinal("Id"));
						}
					}
				}
			}

			return null;
		}
	}
}
