#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Juegos
{
	public static class Limpiar
	{
		public static void Minimos(SqlConnection conexion = null)
		{
			if (conexion == null)
			{
				conexion = Herramientas.BaseDatos.Conectar();
			}

			using (conexion)
			{
				string sqlActualizar = "TRUNCATE TABLE seccionMinimos";

				using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
				{

					comando.ExecuteNonQuery();
					try
					{

					}
					catch
					{

					}
				}
			}
		}
	}
}
