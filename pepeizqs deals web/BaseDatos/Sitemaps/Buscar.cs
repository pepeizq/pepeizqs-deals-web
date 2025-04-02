#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Sitemaps
{
	public static class Buscar
	{
		public static List<string> JuegosAzar(SqlConnection conexion = null)
		{
			List<string> enlaces = new List<string>();

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

			using (conexion)
			{
				string buscar = "SELECT TOP 1000 id, nombre FROM juegos ORDER BY NEWID()";

				using (SqlCommand comando = new SqlCommand(buscar, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							int id = 0;
							string nombre = string.Empty;

							try
							{
								if (lector.IsDBNull(0) == false)
								{
									id = lector.GetInt32(0);
								}
							}
							catch { }

							try
							{
								if (lector.IsDBNull(1) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(1)) == false)
									{
										nombre = lector.GetString(1);
									}
								}
							}
							catch { }

							if (id > 0 && string.IsNullOrEmpty(nombre) == false)
							{
								enlaces.Add("https://pepeizqdeals.com/game/" + id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(nombre) + "/");
							}
						}
					}
				}
			}

			return enlaces;
		}

		public static List<string> JuegosMinimos(SqlConnection conexion = null)
		{
			List<string> enlaces = new List<string>();

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

			using (conexion)
			{
				string buscar = "SELECT id, nombre FROM seccionMinimos";

				using (SqlCommand comando = new SqlCommand(buscar, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							int id = 0;
							string nombre = string.Empty;

							try
							{
								if (lector.IsDBNull(0) == false)
								{
									id = lector.GetInt32(0);
								}
							}
							catch { }

							try
							{
								if (lector.IsDBNull(1) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(1)) == false)
									{
										nombre = lector.GetString(1);
									}
								}
							}
							catch { }

							if (id > 0 && string.IsNullOrEmpty(nombre) == false)
							{
								enlaces.Add("https://pepeizqdeals.com/game/" + id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(nombre) + "/");
							}
						}
					}
				}
			}

			return enlaces;
		}

		public static List<string> JuegosUltimos(SqlConnection conexion = null)
		{
			List<string> enlaces = new List<string>();

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

			using (conexion)
			{
				string buscar = "SELECT TOP 200 id, nombre FROM juegos ORDER BY id DESC";

				using (SqlCommand comando = new SqlCommand(buscar, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							int id = 0;
							string nombre = string.Empty;

							try
							{
								if (lector.IsDBNull(0) == false)
								{
									id = lector.GetInt32(0);
								}
							}
							catch { }

							try
							{
								if (lector.IsDBNull(1) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(1)) == false)
									{
										nombre = lector.GetString(1);
									}
								}
							}
							catch { }

							if (id > 0 && string.IsNullOrEmpty(nombre) == false)
							{
								enlaces.Add("https://pepeizqdeals.com/game/" + id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(nombre) + "/");
							}
						}
					}
				}
			}

			return enlaces;
		}

		public static List<string> BundlesAzar(SqlConnection conexion = null)
		{
			List<string> enlaces = new List<string>();

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

			using (conexion)
			{
				string buscar = "SELECT TOP 100 id, nombre FROM bundles ORDER BY NEWID()";

				using (SqlCommand comando = new SqlCommand(buscar, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							int id = 0;
							string nombre = string.Empty;

							try
							{
								if (lector.IsDBNull(0) == false)
								{
									id = lector.GetInt32(0);
								}
							}
							catch { }

							try
							{
								if (lector.IsDBNull(1) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(1)) == false)
									{
										nombre = lector.GetString(1);
									}
								}
							}
							catch { }

							if (id > 0 && string.IsNullOrEmpty(nombre) == false)
							{
								enlaces.Add("https://pepeizqdeals.com/bundle/" + id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(nombre) + "/");
							}
						}
					}
				}
			}

			return enlaces;
		}

		public static List<string> BundlesUltimos(SqlConnection conexion = null)
		{
			List<string> enlaces = new List<string>();

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

			using (conexion)
			{
				string buscar = "SELECT TOP 200 id, nombre FROM bundles ORDER BY id DESC";

				using (SqlCommand comando = new SqlCommand(buscar, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							int id = 0;
							string nombre = string.Empty;

							try
							{
								if (lector.IsDBNull(0) == false)
								{
									id = lector.GetInt32(0);
								}
							}
							catch { }

							try
							{
								if (lector.IsDBNull(1) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(1)) == false)
									{
										nombre = lector.GetString(1);
									}
								}
							}
							catch { }

							if (id > 0 && string.IsNullOrEmpty(nombre) == false)
							{
								enlaces.Add("https://pepeizqdeals.com/bundle/" + id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(nombre) + "/");
							}
						}
					}
				}
			}

			return enlaces;
		}

		public static List<string> NoticiasUltimasIngles(SqlConnection conexion = null)
		{
			List<string> enlaces = new List<string>();

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

			using (conexion)
			{
				string buscar = "SELECT TOP 100 id, tituloEn FROM noticias ORDER BY id DESC";

				using (SqlCommand comando = new SqlCommand(buscar, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							int id = 0;
							string nombre = string.Empty;

							try
							{
								if (lector.IsDBNull(0) == false)
								{
									id = lector.GetInt32(0);
								}
							}
							catch { }

							try
							{
								if (lector.IsDBNull(1) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(1)) == false)
									{
										nombre = lector.GetString(1);
									}
								}
							}
							catch { }

							if (id > 0 && string.IsNullOrEmpty(nombre) == false)
							{
								enlaces.Add("https://pepeizqdeals.com/news/" + id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(nombre) + "/");
							}
						}
					}
				}
			}

			return enlaces;
		}

		public static List<string> NoticiasUltimasEspañol(SqlConnection conexion = null)
		{
			List<string> enlaces = new List<string>();

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

			using (conexion)
			{
				string buscar = "SELECT TOP 100 id, tituloEs FROM noticias ORDER BY id DESC";

				using (SqlCommand comando = new SqlCommand(buscar, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							int id = 0;
							string nombre = string.Empty;

							try
							{
								if (lector.IsDBNull(0) == false)
								{
									id = lector.GetInt32(0);
								}
							}
							catch { }

							try
							{
								if (lector.IsDBNull(1) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(1)) == false)
									{
										nombre = lector.GetString(1);
									}
								}
							}
							catch { }

							if (id > 0 && string.IsNullOrEmpty(nombre) == false)
							{
								enlaces.Add("https://pepeizqdeals.com/news/" + id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(nombre) + "/");
							}
						}
					}
				}
			}

			return enlaces;
		}

		public static List<string> Curators(SqlConnection conexion = null)
		{
			List<string> enlaces = new List<string>();

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

			using (conexion)
			{
				string buscar = "SELECT slug FROM curators";

				using (SqlCommand comando = new SqlCommand(buscar, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							string slug = string.Empty;

							try
							{
								if (lector.IsDBNull(0) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(0)) == false)
									{
										slug = lector.GetString(0);
									}
								}
							}
							catch { }

							if (string.IsNullOrEmpty(slug) == false)
							{
								enlaces.Add("https://pepeizqdeals.com/curator/" + slug + "/");
							}
						}
					}
				}
			}

			return enlaces;
		}
	}
}
