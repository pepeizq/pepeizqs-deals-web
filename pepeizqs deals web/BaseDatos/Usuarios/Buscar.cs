#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Usuarios
{
	public static class Buscar
	{
		public static bool RolDios(string username)
		{
			if (string.IsNullOrEmpty(username) == false) 
			{
				WebApplicationBuilder builder = WebApplication.CreateBuilder();
				string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");
				SqlConnection conexion = new SqlConnection(conexionTexto);

				using (conexion)
				{
					conexion.Open();
					string busqueda = "SELECT * FROM AspNetUsers WHERE UserName=@UserName";
					SqlCommand comando = new SqlCommand(busqueda, conexion);

					using (comando)
					{
						comando.Parameters.AddWithValue("@UserName", username);

						SqlDataReader lector = comando.ExecuteReader();

						using (lector)
						{
							while (lector.Read())
							{
								if (lector.GetString(2) == "God")
								{
									return true;
								}
							}
						}

						lector.Close();
					}

					comando.Dispose();
				}

				conexion.Dispose();
			}

			return false;
		}
	}
}
