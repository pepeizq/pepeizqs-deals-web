#nullable disable

using Microsoft.Data.SqlClient;
using System.Data;

namespace Herramientas
{
	public static class BaseDatos
	{
		public static string cadenaConexion = "pepeizqs_deals_webContextConnection";

		public static SqlConnection Conectar(bool usarEstado = true)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string conexionTexto = builder.Configuration.GetConnectionString(cadenaConexion);
			SqlConnection conexion = new SqlConnection(conexionTexto);
			
            ConnectionState estado = conexion.State;
            
			if (usarEstado == true)
			{
				if (estado != ConnectionState.Open)
				{
					try
					{
						conexion.Close();
					}
					catch { }

					conexion.Open();
				}
			}

			return conexion;	
        }
	}
}
