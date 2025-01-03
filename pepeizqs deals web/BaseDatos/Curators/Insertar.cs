#nullable disable

using APIs.Steam;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace BaseDatos.Curators
{
	public static class Insertar
	{
		public static void Ejecutar(SteamCuratorAPI api, SqlConnection conexion = null)
		{
			if (conexion == null)
			{
				conexion = Herramientas.BaseDatos.Conectar();
			}
			else
			{
				if (conexion.State != System.Data.ConnectionState.Open)
				{
					conexion = Herramientas.BaseDatos.Conectar();
				}
			}

			if (api != null)
			{
				string sqlAñadir = "INSERT INTO curators " +
					 "(id, nombre, imagen, descripcion, slug, steamIds, web) VALUES " +
					 "(@id, @nombre, @imagen, @descripcion, @slug, @steamIds, @web) ";

				using (SqlCommand comando = new SqlCommand(sqlAñadir, conexion))
				{
					comando.Parameters.AddWithValue("@id", api.Id);
					comando.Parameters.AddWithValue("@nombre", api.Nombre);
					comando.Parameters.AddWithValue("@imagen", api.Imagen);
					comando.Parameters.AddWithValue("@descripcion", api.Descripcion);
					comando.Parameters.AddWithValue("@slug", api.Slug);
					comando.Parameters.AddWithValue("@steamIds", JsonSerializer.Serialize(api.SteamIds));
					comando.Parameters.AddWithValue("@web", JsonSerializer.Serialize(api.Web));

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
}
