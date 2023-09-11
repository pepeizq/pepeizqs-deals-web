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
				SqlConnection conexion = Herramientas.BaseDatos.Conectar();

				using (conexion)
				{
					string busqueda = "SELECT * FROM AspNetUsers WHERE UserName=@UserName";

					using (SqlCommand comando = new SqlCommand(busqueda, conexion))
					{
						comando.Parameters.AddWithValue("@UserName", username);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							while (lector.Read())
							{
								if (lector.GetString(2) == "God")
								{
									return true;
								}
							}
						}
					}
				}

				conexion.Dispose();
			}

			return false;
		}
	}
}
