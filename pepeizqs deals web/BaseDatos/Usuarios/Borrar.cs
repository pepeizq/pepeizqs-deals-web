#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Usuarios
{
	public static class Borrar
	{
		public static void NotificacionesPush(string usuarioId, SqlConnection conexion = null)
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

			string borrar = "DELETE FROM usuariosNotificaciones WHERE usuarioId=@usuarioId";

			using (SqlCommand comando = new SqlCommand(borrar, conexion))
			{
				comando.Parameters.AddWithValue("@usuarioId", usuarioId);

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
