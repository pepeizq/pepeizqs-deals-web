using Microsoft.Data.SqlClient;

namespace BaseDatos.Pendientes
{
	public static class ActualizarJuego
	{
		public static void Ejecutar(string idTienda, string enlace, string idJuegos)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
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

			conexion.Dispose();
		}
	}
}
