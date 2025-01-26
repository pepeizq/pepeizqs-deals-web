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

		public static bool UsuarioTieneJuego(string usuarioId, JuegoDRM drm, int juegoIdSteam = 0, int juegoIdGog = 0, SqlConnection conexion = null)
		{
			if (drm == JuegoDRM.Steam || drm == JuegoDRM.GOG)
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

				string busqueda = string.Empty;
				
				if (drm == JuegoDRM.Steam)
				{
					busqueda = "SELECT SteamGames FROM AspNetUsers WHERE Id=@Id";
				}
				
				if (drm == JuegoDRM.GOG)
				{
					busqueda = "SELECT GogGames FROM AspNetUsers WHERE Id=@Id";
				}

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@Id", usuarioId);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							if (juegoIdSteam > 0)
							{
								if (lector.IsDBNull(0) == false)
								{
									string temp = string.Empty;

									try
									{
										temp = lector.GetString(0);
									}
									catch { }

									if (string.IsNullOrEmpty(temp) == false)
									{
										List<string> juegos = Listados.Generar(temp);

										if (juegos != null)
										{
											if (juegos.Count > 0)
											{
												foreach (var juego in juegos)
												{
													if (drm == JuegoDRM.Steam)
													{
														if (juego == juegoIdSteam.ToString())
														{
															return true;
														}
													}
													
													if (drm == JuegoDRM.GOG)
													{
														if (juego == juegoIdGog.ToString())
														{
															return true;
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

			return false;
		}

		public static string UsuarioDeseados(string usuarioId, string juegoId, JuegoDRM drm, int juegoIdSteam = 0, int juegoIdGog = 0)
		{
			if (string.IsNullOrEmpty(usuarioId) == false)
			{
				SqlConnection conexion = Herramientas.BaseDatos.Conectar();

				using (conexion)
				{
					string busqueda = "SELECT NotificationLows, EmailConfirmed, SteamWishlist, Email, Wishlist, GogWishlist FROM AspNetUsers WHERE Id=@Id";

					using (SqlCommand comando = new SqlCommand(busqueda, conexion))
					{
						comando.Parameters.AddWithValue("@Id", usuarioId);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							while (lector.Read())
							{
								bool notificaciones = false;
								bool correo = false;
                            
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

								if (correo == true && notificaciones == true)
								{
									//Deseados Steam
									if (drm == JuegoDRM.Steam && lector.IsDBNull(2) == false)
									{
										if (string.IsNullOrEmpty(lector.GetString(2)) == false)
										{
											List<string> deseadosSteam = Listados.Generar(lector.GetString(2));

											if (deseadosSteam.Count > 0)
											{
												foreach (var deseadoSteam in deseadosSteam)
												{
													if (juegoIdSteam.ToString() == deseadoSteam)
													{
														//Correo
														if (lector.IsDBNull(3) == false)
														{
															if (string.IsNullOrEmpty(lector.GetString(3)) == false)
															{
																return lector.GetString(3);
															}
														}
													}
												}
											}
										}
									}

									//Deseados Gog
									if (drm == JuegoDRM.GOG && lector.IsDBNull(5) == false)
									{
										if (string.IsNullOrEmpty(lector.GetString(5)) == false)
										{
											List<string> deseadosGog = Listados.Generar(lector.GetString(5));

											if (deseadosGog.Count > 0)
											{
												foreach (var deseadoGog in deseadosGog)
												{
													if (juegoIdGog.ToString() == deseadoGog)
													{
														//Correo
														if (lector.IsDBNull(3) == false)
														{
															if (string.IsNullOrEmpty(lector.GetString(3)) == false)
															{
																return lector.GetString(3);
															}
														}
													}
												}
											}
										}
									}

									//Deseados Web
									if (lector.IsDBNull(4) == false)
									{
										if (string.IsNullOrEmpty(lector.GetString(4)) == false)
										{
											string deseadosTexto = lector.GetString(4);
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
															if (lector.IsDBNull(3) == false)
															{
																if (string.IsNullOrEmpty(lector.GetString(3)) == false)
																{
																	return lector.GetString(3);
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

		public static List<NotificacionSuscripcion> TodosUsuariosNotificacionesPush(SqlConnection conexion = null)
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

			List<NotificacionSuscripcion> usuarios = new List<NotificacionSuscripcion>();

			string busqueda = "SELECT * FROM usuariosNotificaciones";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						NotificacionSuscripcion usuario = new NotificacionSuscripcion();
						usuario.UserId = lector.GetString(0);
						usuario.NotificationSubscriptionId = lector.GetInt32(1);
						usuario.Url = lector.GetString(2);
						usuario.P256dh = lector.GetString(3);
						usuario.Auth = lector.GetString(4);
						usuario.UserAgent = lector.GetString(5);

						usuarios.Add(usuario);
					}
				}
			}

			return usuarios;
		}

		public static NotificacionSuscripcion UnUsuarioNotificacionesPush(string usuarioId, SqlConnection conexion = null)
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

			string busqueda = "SELECT * FROM usuariosNotificaciones WHERE usuarioId=@usuarioId";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				comando.Parameters.AddWithValue("@usuarioId", usuarioId);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					if (lector.Read() == true)
					{
						NotificacionSuscripcion usuario = new NotificacionSuscripcion();
						usuario.UserId = lector.GetString(0);
						usuario.NotificationSubscriptionId = lector.GetInt32(1);
						usuario.Url = lector.GetString(2);
						usuario.P256dh = lector.GetString(3);
						usuario.Auth = lector.GetString(4);
						usuario.UserAgent = lector.GetString(5);

						return usuario;
					}
				}
			}

			return null;
		}

		public static bool UnUsuarioNotificacionesPushMinimos(string usuarioId, SqlConnection conexion = null)
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

			string busqueda = "SELECT NotificationPushLows FROM AspNetUsers WHERE Id=@Id";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				comando.Parameters.AddWithValue("@Id", usuarioId);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					if (lector.Read() == true)
					{
						if (lector.IsDBNull(0) == false)
						{
							return lector.GetBoolean(0);
						}
					}
				}
			}

			return false;
		}
	}
}
