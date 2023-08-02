#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Enlaces
{
	public static class Buscar
	{
		public static Enlace Id(string id)
		{
			if (string.IsNullOrEmpty(id) == false)
			{
				WebApplicationBuilder builder = WebApplication.CreateBuilder();
				string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

				using (SqlConnection conexion = new SqlConnection(conexionTexto))
				{
					conexion.Open();
					string sqlBuscar = "SELECT * FROM enlaces WHERE id=@id";

					using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
					{
						comando.Parameters.AddWithValue("@id", id);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							if (lector.Read())
							{
								Enlace enlaceFinal = new Enlace
								{
									Id = lector.GetInt32(0).ToString(),
									Base = lector.GetString(1)
								};

								return enlaceFinal;
							}
						}
					}
				}
			}

			return null;
		}

		public static Enlace Base(string enlace)
		{
			if (string.IsNullOrEmpty(enlace) == false)
			{
				WebApplicationBuilder builder = WebApplication.CreateBuilder();
				string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

				using (SqlConnection conexion = new SqlConnection(conexionTexto))
				{
					conexion.Open();
					string sqlBuscar = "SELECT * FROM enlaces WHERE base=@base";

					using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
					{
						comando.Parameters.AddWithValue("@base", enlace);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							if (lector.Read())
							{
								Enlace enlaceFinal = new Enlace
								{
									Id = lector.GetInt32(0).ToString(),
									Base = lector.GetString(1)
								};

								return enlaceFinal;
							}
						}
					}
				}
			}

			return null;
		}
	}
}
