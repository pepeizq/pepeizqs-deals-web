using Microsoft.Data.SqlClient;

namespace BaseDatos.Pendientes
{
	public static class Actualizar
	{
		public static void Juego(string idTienda, string enlace, string idJuegos, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE tienda" + idTienda + " " +
					"SET idJuegos=@idJuegos WHERE enlace=@enlace";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@idJuegos", idJuegos);
				comando.Parameters.AddWithValue("@enlace", enlace);

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void Descartar(string idTienda, string enlace, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE tienda" + idTienda + " " +
					"SET descartado=@descartado WHERE enlace=@enlace";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@descartado", "si");
				comando.Parameters.AddWithValue("@enlace", enlace);

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
