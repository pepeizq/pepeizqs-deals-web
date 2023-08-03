#nullable disable

using Microsoft.Data.SqlClient;

namespace Herramientas
{
	public static class BaseDatos
	{
		public static SqlConnection Conectar()
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");
			SqlConnection conexion = new SqlConnection(conexionTexto);

			return conexion;
		}
	}
}
