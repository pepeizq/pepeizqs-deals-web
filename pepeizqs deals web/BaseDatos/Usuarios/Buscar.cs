#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

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

		public static string UnUsuarioDeseados(string usuarioId, string juegoId, JuegoDRM drm)
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

								if (correo == true && notificaciones == true)
								{
									if (lector.IsDBNull(20) == false)
									{
										if (lector.GetString(20) != null)
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
															if (lector.IsDBNull(8) == false)
															{
																if (lector.GetString(8) != null)
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
	}
}
