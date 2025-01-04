#nullable disable

using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace BaseDatos.Curators
{
	public static class Actualizar
	{
		public static void Ejecutar(Curator curator, SqlConnection conexion = null)
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

			string añadirImagenFondo = null;

			if (string.IsNullOrEmpty(curator.ImagenFondo) == false)
			{
				añadirImagenFondo = ", imagenFondo=@imagenFondo";
			}

			string sqlActualizar = "UPDATE curators " +
						"SET nombre=@nombre, imagen=@imagen, descripcion=@descripcion, slug=@slug, steamIds=@steamIds, web=@web, fecha=@fecha " + añadirImagenFondo + " WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", curator.Id);
				comando.Parameters.AddWithValue("@nombre", curator.Nombre);
				comando.Parameters.AddWithValue("@imagen", curator.Imagen);
				comando.Parameters.AddWithValue("@descripcion", curator.Descripcion);
				comando.Parameters.AddWithValue("@slug", curator.Slug);
				comando.Parameters.AddWithValue("@steamIds", JsonSerializer.Serialize(curator.SteamIds));
				comando.Parameters.AddWithValue("@web", JsonSerializer.Serialize(curator.Web));
				comando.Parameters.AddWithValue("@fecha", DateTime.Now);

				if (string.IsNullOrEmpty(curator.ImagenFondo) == false)
				{
					comando.Parameters.AddWithValue("@imagenFondo", curator.ImagenFondo);
				}
				
				SqlDataReader lector = comando.ExecuteReader();

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void ImagenFondo(string imagenFondo, int id, SqlConnection conexion = null)
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

			string sqlActualizar = "UPDATE curators " +
						"SET imagenFondo=@imagenFondo WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);
				comando.Parameters.AddWithValue("@imagenFondo", imagenFondo);

				SqlDataReader lector = comando.ExecuteReader();

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
