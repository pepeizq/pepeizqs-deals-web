using Microsoft.Data.SqlClient;

namespace BaseDatos.Pendientes
{
	public static class DescartarJuego
	{
		public static void Ejecutar(string idTienda, string enlace)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			if (conexion.State == System.Data.ConnectionState.Open)
			{
				using (conexion)
				{
					string sqlActualizar = "UPDATE tienda" + idTienda + " " +
						"SET descartado=@descartado WHERE enlace=@enlace";

					using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
					{
						comando.Parameters.AddWithValue("@descartado", "si");
						comando.Parameters.AddWithValue("@enlace", enlace);

						comando.ExecuteNonQuery();
					}
				}
			}		
		}
	}
}
