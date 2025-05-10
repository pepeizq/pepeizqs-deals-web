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
					 "(idSteam, nombre, imagen, descripcion, slug, steamIds, web, fecha) VALUES " +
					 "(@idSteam, @nombre, @imagen, @descripcion, @slug, @steamIds, @web, @fecha) ";

				using (SqlCommand comando = new SqlCommand(sqlAñadir, conexion))
				{
					comando.Parameters.AddWithValue("@idSteam", api.Id);
					comando.Parameters.AddWithValue("@nombre", api.Nombre);
					comando.Parameters.AddWithValue("@imagen", api.Imagen);
					comando.Parameters.AddWithValue("@descripcion", api.Descripcion);
					comando.Parameters.AddWithValue("@steamIds", JsonSerializer.Serialize(api.SteamIds));
					comando.Parameters.AddWithValue("@web", JsonSerializer.Serialize(api.Web));
					comando.Parameters.AddWithValue("@fecha", DateTime.Now);

					if (string.IsNullOrEmpty(api.Slug) == false)
					{
						comando.Parameters.AddWithValue("@slug", api.Slug);
					}
					else
					{
						comando.Parameters.AddWithValue("@slug", Herramientas.EnlaceAdaptador.Nombre(api.Nombre));
					}

					try
					{
						comando.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Mensaje("Insertar Curator", ex);
					}
				}
			}
		}
	}
}
