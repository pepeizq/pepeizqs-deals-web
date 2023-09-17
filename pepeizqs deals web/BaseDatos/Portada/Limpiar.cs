using Microsoft.Data.SqlClient;

namespace BaseDatos.Portada
{
	public static class Limpiar
	{
		public static void Ejecutar(string tabla, SqlConnection conexion)
		{
			string limpiar = "TRUNCATE TABLE " + tabla;

			using (SqlCommand comando = new SqlCommand(limpiar, conexion))
			{
				comando.ExecuteNonQuery();
			}
		}
	}
}
