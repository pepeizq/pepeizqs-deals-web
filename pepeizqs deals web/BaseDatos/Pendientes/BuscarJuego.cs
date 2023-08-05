#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Pendientes
{
	public static class BuscarJuego
	{
		public static string Ejecutar(string nombre)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				conexion.Open();

				string busqueda = "SELECT * FROM juegos WHERE nombre=@nombre";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@nombre", nombre);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							return lector.GetInt32(0).ToString();
						}
					}
				}
			}

			conexion.Dispose();

			return "0";
		}
	}
}
