#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Enlaces
{
	public static class Insertar
	{
		public static Enlace Ejecutar(string enlace)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

			using (SqlConnection conexion = new SqlConnection(conexionTexto))
			{
				conexion.Open();

				string sqlAñadir = "INSERT INTO enlaces " +
					"(base) VALUES " +
					"(@base) ";

				using (SqlCommand comando = new SqlCommand(sqlAñadir, conexion))
				{
					comando.Parameters.AddWithValue("@base", enlace);

					try
					{
						comando.ExecuteNonQuery();
					}
					catch
					{

					}
				}
			}

			return Buscar.Base(enlace);
		}
	}
}
