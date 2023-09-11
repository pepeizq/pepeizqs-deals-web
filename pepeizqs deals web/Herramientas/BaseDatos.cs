#nullable disable

using Microsoft.Data.SqlClient;
using System.Data;

namespace Herramientas
{
	public static class BaseDatos
	{
		public static SqlConnection Conectar()
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");
			SqlConnection conexion = new SqlConnection(conexionTexto);

            ConnectionState estado = conexion.State;
            
            if (estado != ConnectionState.Open)
            {
				try
				{
					conexion.Close();
				}
				catch { }

                conexion.Open();
            }

			return conexion;	
        }
	}
}
