using Microsoft.Data.SqlClient;

namespace BaseDatos.Error
{
	public static class Insertar
	{
		public static void Ejecutar(string mensaje)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string sqlInsertar = "INSERT INTO errores " +
					"(mensaje) VALUES " +
					"(@mensaje) ";

				using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
				{
					comando.Parameters.AddWithValue("@mensaje", mensaje);

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
