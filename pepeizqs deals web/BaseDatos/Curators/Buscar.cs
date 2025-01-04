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

			if (lector.IsDBNull(8) == false)
			{
				curator.Fecha = lector.GetDateTime(8);
			}

			return curator;
		}

		public static List<Curator> Todos(SqlConnection conexion = null)
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

			List<Curator> curators = new List<Curator>();

			string busqueda = "SELECT * FROM curators";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read() == true)
					{
						curators.Add(Cargar(lector));
					}
				}
			}

			return curators;
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

		public static List<Curator> Nombre(string nombre, SqlConnection conexion = null)
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

			List<Curator> curators = new List<Curator>();

			string busqueda = string.Empty;
			string busquedaTodo = "*";

			if (nombre.Contains(" ") == true)
			{
				if (nombre.Contains("  ") == true)
				{
					nombre = nombre.Replace("  ", " ");
				}

				string[] palabras = nombre.Split(" ");

				int i = 0;
				foreach (var palabra in palabras)
				{
					if (string.IsNullOrEmpty(palabra) == false)
					{
						string palabraLimpia = Herramientas.Buscador.LimpiarNombre(palabra, true);

						if (palabraLimpia.Length > 0)
						{
							if (i == 0)
							{
								busqueda = "SELECT TOP 10 " + busquedaTodo + " FROM curators WHERE CHARINDEX('" + palabraLimpia + "', nombre) > 0 ";
							}
							else
							{
								bool buscar = true;

								if (palabra.ToLower() == "and")
								{
									buscar = false;
								}
								else if (palabra.ToLower() == "dlc")
								{
									buscar = false;
								}
								if (palabra.ToLower() == "expansion")
								{
									buscar = false;
								}

								if (buscar == true)
								{
									busqueda = busqueda + " AND CHARINDEX('" + palabraLimpia + "', nombre) > 0 ";
								}
							}

							i += 1;
						}
					}
				}
			}
			else
			{
				busqueda = "SELECT TOP 10 " + busquedaTodo + " FROM curators WHERE nombre LIKE '%" + Herramientas.Buscador.LimpiarNombre(nombre) + "%'";
			}

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					if (lector.Read() == true)
					{
						curators.Add(Cargar(lector));
					}
				}
			}

			return curators;
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
		public DateTime? Fecha;
	}

	public class CuratorFicha
	{
		public Curator Curator;
		public int Posicion;
	}
}
