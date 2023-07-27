#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Juegos
{
	public static class Limpiar
	{
		public static void Ejecutar()
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

			using (SqlConnection conexion = new SqlConnection(conexionTexto))
			{
				conexion.Open();

				string limpiar = "TRUNCATE TABLE juegos";

				using (SqlCommand comando = new SqlCommand(limpiar, conexion))
				{
					comando.ExecuteNonQuery();
				}
			}
		}
	}
}
