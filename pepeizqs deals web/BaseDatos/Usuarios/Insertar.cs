#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Usuarios
{
	public static class Insertar
	{
		public static void Notificaciones(NotificacionSuscripcion datos, SqlConnection conexion = null)
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

			bool añadir = false;

			string sqlBusqueda = "SELECT * FROM usuariosNotificaciones WHERE usuarioId=@usuarioId";

			using (SqlCommand comando = new SqlCommand(sqlBusqueda, conexion))
			{
				comando.Parameters.AddWithValue("@usuarioId", datos.UserId);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					if (lector.Read() == false)
					{
						añadir = true;
					}
				}
			}

			if (añadir == true)
			{
				string sqlAñadir = "INSERT INTO usuariosNotificaciones " +
					 "(usuarioId, notificacionId, enlace, p256dh, auth) VALUES " +
					 "(@usuarioId, @notificacionId, @enlace, @p256dh, @auth) ";

				using (SqlCommand comando = new SqlCommand(sqlAñadir, conexion))
				{
					comando.Parameters.AddWithValue("@usuarioId", datos.UserId);
					comando.Parameters.AddWithValue("@notificacionId", datos.NotificationSubscriptionId);
					comando.Parameters.AddWithValue("@enlace", datos.Url);
					comando.Parameters.AddWithValue("@p256dh", datos.P256dh);
					comando.Parameters.AddWithValue("@auth", datos.Auth);

					try
					{
						comando.ExecuteNonQuery();
					}
					catch
					{

					}
				}
			}			
		}
	}
}
