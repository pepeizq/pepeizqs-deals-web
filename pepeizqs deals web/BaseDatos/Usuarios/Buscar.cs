#nullable disable

using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;

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

		public static bool UsuarioTieneJuego(string usuarioId, int juegoId, JuegoDRM drm, SqlConnection conexion = null)
		{
			string busqueda = string.Empty;

			if (drm == JuegoDRM.Steam)
			{
				busqueda = "SELECT id FROM AspNetUsers WHERE EXISTS(SELECT * FROM STRING_SPLIT(SteamGames, ',') WHERE VALUE IN ((SELECT idSteam FROM juegos WHERE id=@juegoId))) AND id=@id";
			}

			if (drm == JuegoDRM.GOG)
			{
				busqueda = "SELECT id FROM AspNetUsers WHERE EXISTS(SELECT * FROM STRING_SPLIT(GogGames, ',') WHERE VALUE IN ((SELECT idGog FROM juegos WHERE id=@juegoId))) AND id=@id";
			}

			if (drm == JuegoDRM.Amazon)
			{
				busqueda = "SELECT id FROM AspNetUsers WHERE EXISTS(SELECT * FROM STRING_SPLIT(AmazonGames, ',') WHERE VALUE IN ((SELECT idAmazon FROM juegos WHERE id=@juegoId))) AND id=@id";
			}

			if (drm == JuegoDRM.Epic)
			{
				busqueda = "SELECT id FROM AspNetUsers WHERE EXISTS(SELECT * FROM STRING_SPLIT(EpicGames, ',') WHERE VALUE IN ((SELECT exeEpic FROM juegos WHERE id=@juegoId))) AND id=@id";
			}

			if (drm == JuegoDRM.Ubisoft)
			{
				busqueda = "SELECT id FROM AspNetUsers WHERE EXISTS(SELECT * FROM STRING_SPLIT(UbisoftGames, ',') WHERE VALUE IN ((SELECT exeUbisoft FROM juegos WHERE id=@juegoId))) AND id=@id";
			}

			if (drm == JuegoDRM.EA)
			{
				busqueda = "SELECT id FROM AspNetUsers WHERE EXISTS(SELECT * FROM STRING_SPLIT(EaGames, ',') WHERE VALUE IN ((SELECT exeEa FROM juegos WHERE id=@juegoId))) AND id=@id";
			}

			if (string.IsNullOrEmpty(busqueda) == false)
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

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@id", usuarioId);
					comando.Parameters.AddWithValue("@juegoId", juegoId);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							if (lector.IsDBNull(0) == false)
							{
								return true;
							}
						}
					}
				}
			}

			return false;
		}

		public static bool UsuarioTieneDeseado(string usuarioId, string juegoId, JuegoDRM drm, SqlConnection conexion = null)
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

				if (drm == JuegoDRM.Steam)
				{
					string busquedaSteam = "SELECT id FROM AspNetUsers WHERE EXISTS(SELECT * FROM STRING_SPLIT(SteamWishlist, ',') WHERE VALUE IN ((SELECT idSteam FROM juegos WHERE id=@juegoId))) AND id=@id";

					using (SqlCommand comando = new SqlCommand(busquedaSteam, conexion))
					{
						comando.Parameters.AddWithValue("@id", usuarioId);
						comando.Parameters.AddWithValue("@juegoId", juegoId);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							if (lector.Read() == true)
							{
								if (lector.IsDBNull(0) == false)
								{
									return true;
								}
							}
						}
					}
				}

				if (drm == JuegoDRM.GOG)
				{
					string busquedaGog = "SELECT id FROM AspNetUsers WHERE EXISTS(SELECT * FROM STRING_SPLIT(GogWishlist, ',') WHERE VALUE IN ((SELECT idGog FROM juegos WHERE id=@juegoId))) AND id=@id";

					using (SqlCommand comando = new SqlCommand(busquedaGog, conexion))
					{
						comando.Parameters.AddWithValue("@id", usuarioId);
						comando.Parameters.AddWithValue("@juegoId", juegoId);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							if (lector.Read() == true)
							{
								if (lector.IsDBNull(0) == false)
								{
									return true;
								}
							}
						}
					}
				}

				string busqueda = @"DECLARE @array NVARCHAR(MAX);
									SET @array = (SELECT Wishlist FROM AspNetUsers WHERE id=@id);

									SELECT * FROM OPENJSON(@array)
									WITH (IdBaseDatos int,
									DRM int) WHERE IdBaseDatos=@juegoId AND DRM=@drm";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@id", usuarioId);
					comando.Parameters.AddWithValue("@juegoId", juegoId);
					comando.Parameters.AddWithValue("@drm", (int) drm);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							if (lector.IsDBNull(0) == false)
							{
								return true;
							}
						}
					}
				}
			}

			return false;
		}

		public static string UsuarioQuiereCorreos(string usuarioId, SqlConnection conexion = null)
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

				string busqueda = "SELECT NotificationLows, EmailConfirmed, Email FROM AspNetUsers WHERE Id=@Id";

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
								//Correo
								if (lector.IsDBNull(2) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(2)) == false)
									{
										return lector.GetString(2);
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

		public static bool UsuarioQuiereNotificacionesPushMinimos(string usuarioId, SqlConnection conexion = null)
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

		public static bool UsuarioNombreRepetido(string nombre, SqlConnection sqlConnection = null)
		{
			if (string.IsNullOrEmpty(nombre) == false)
			{
				if (sqlConnection == null)
				{
					sqlConnection = Herramientas.BaseDatos.Conectar();
				}
				else
				{
					if (sqlConnection.State != System.Data.ConnectionState.Open)
					{
						sqlConnection = Herramientas.BaseDatos.Conectar();
					}
				}

				string busqueda = "SELECT Id FROM AspNetUsers WHERE Nickname=@Nickname";

				using (SqlCommand comando = new SqlCommand(busqueda, sqlConnection))
				{
					comando.Parameters.AddWithValue("@Nickname", nombre);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							if (lector.IsDBNull(0) == false)
							{
								return true;
							}
						}
					}
				}
			}

			return false;
		}
	}
}
