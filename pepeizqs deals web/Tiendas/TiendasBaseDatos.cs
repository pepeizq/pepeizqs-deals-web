#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace Tiendas2
{
	public static class TiendasBaseDatos
	{
		public static void ActualizarTiempo(string tienda, DateTime fecha)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

			using (SqlConnection conexion = new SqlConnection(conexionTexto))
			{
				conexion.Open();

				bool insertar = false;
				bool actualizar = false;

				string seleccionarJuego = "SELECT * FROM adminTiendas WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(seleccionarJuego, conexion))
				{
					comando.Parameters.AddWithValue("@id", tienda);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == false)
						{
							insertar = true;
						}
						else
						{
							actualizar = true;
						}
					}
				}

				if (insertar == true)
				{
					string sqlAñadir = "INSERT INTO adminTiendas " +
						"(id, fecha) VALUES " +
						"(@id, @fecha)";

					using (SqlCommand comando = new SqlCommand(sqlAñadir, conexion))
					{
						comando.Parameters.AddWithValue("@id", tienda);
						comando.Parameters.AddWithValue("@fecha", fecha);

						try
						{
							comando.ExecuteNonQuery();
						}
						catch
						{

						}
					}
				}

				if (actualizar == true)
				{
					string sqlActualizar = "UPDATE adminTiendas " +
							"SET id=@id, fecha=@fecha WHERE id=@id";

					using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
					{
						comando.Parameters.AddWithValue("@id", tienda);
						comando.Parameters.AddWithValue("@fecha", fecha);

						try
						{
							comando.ExecuteNonQuery();
						}
						catch
						{

						}
					}
				}
			}
		}

		public static string SacarTiempo(string tienda)
		{
			string tiempo = string.Empty;

			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

			using (SqlConnection conexion = new SqlConnection(conexionTexto))
			{
				conexion.Open();

				string seleccionarJuego = "SELECT * FROM adminTiendas WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(seleccionarJuego, conexion))
				{
					comando.Parameters.AddWithValue("@id", tienda);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							tiempo = Calculadora.HaceTiempo(DateTime.Parse(lector.GetString(1)));
						}
					}
				}
			}

			return tiempo;
		}
	}
}
