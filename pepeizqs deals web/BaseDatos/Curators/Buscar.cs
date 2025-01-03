#nullable disable

using APIs.Steam;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace BaseDatos.Curators
{
	public static class Buscar
	{
		public static Curator Cargar(SqlDataReader lector)
		{
			Curator curator = new Curator
			{
				Id = lector.GetInt32(0),
				Nombre = lector.GetString(1),
				Imagen = lector.GetString(2),
				Descripcion = lector.GetString(3),
				Slug = lector.GetString(4),
				SteamIds = JsonSerializer.Deserialize<List<int>>(lector.GetString(5)),
				Web = JsonSerializer.Deserialize<SteamCuratorAPIWeb>(lector.GetString(6))
			};

			if (lector.IsDBNull(7) == false)
			{
				curator.ImagenFondo = lector.GetString(7);
			}

			return curator;
		}

		public static Curator Uno(int id, SqlConnection conexion = null)
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

			string busqueda = "SELECT * FROM curators WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					if (lector.Read() == true)
					{
						return Cargar(lector);
					}
				}
			}

			return null;
		}

		public static Curator Uno(string slug, SqlConnection conexion = null)
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

			string busqueda = "SELECT * FROM curators WHERE slug=@slug";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				comando.Parameters.AddWithValue("@slug", slug);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					if (lector.Read() == true)
					{
						return Cargar(lector);
					}
				}
			}

			return null;
		}
	}

	public class Curator
	{
		public int Id;
		public string Nombre;
		public string Imagen;
		public string Descripcion;
		public string Slug;
		public List<int> SteamIds;
		public SteamCuratorAPIWeb Web;
		public string ImagenFondo;
	}

	public class CuratorFicha
	{
		public Curator Curator;
		public int Posicion;
	}
}
