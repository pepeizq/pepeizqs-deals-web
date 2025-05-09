#nullable disable

using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BaseDatos.Sitemaps
{
	public static class Buscar
	{
		public static int Cantidad(string tabla, SqlConnection conexion = null)
		{
			int cantidad = 0;

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
				string buscar = "SELECT COUNT(*) FROM @tabla";

				buscar = buscar.Replace("@tabla", tabla);

				using (SqlCommand comando = new SqlCommand(buscar, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							try
							{
								if (lector.IsDBNull(0) == false)
								{
									cantidad = lector.GetInt32(0);
								}
							}
							catch { }
						}
					}
				}
			}

			return cantidad;
		}

		public static List<string> Juegos(int id1, int id2, SqlConnection conexion = null)
		{
			List<string> lineas = new List<string>();

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
				string buscar = "SELECT id, nombre, ultimaModificacion FROM juegos WHERE id > @id1 AND id < @id2";

				buscar = buscar.Replace("@id1", id1.ToString());
				buscar = buscar.Replace("@id2", id2.ToString());

				using (SqlCommand comando = new SqlCommand(buscar, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							int id = 0;
							string nombre = string.Empty;
							DateTime fecha = new DateTime();

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

							try
							{
								if (lector.IsDBNull(2) == false)
								{
									fecha = lector.GetDateTime(2);
								}
							}
							catch { }

							if (id > 0 && string.IsNullOrEmpty(nombre) == false)
							{
								string texto = "<url>" + Environment.NewLine +
									 "<loc>https://pepeizqdeals.com/game/" + id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(nombre) + "/</loc>" + Environment.NewLine;

								if (fecha.Year > 1)
								{
									texto = texto + "<lastmod>" + fecha.ToString("yyyy-MM-dd") + "</lastmod>" + Environment.NewLine;
								}

								texto = texto + "</url>";

								lineas.Add(texto);
							}
						}
					}
				}
			}

			return lineas;
		}

		public static List<string> Bundles(int id1, int id2, SqlConnection conexion = null)
		{
			List<string> lineas = new List<string>();

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
				string buscar = "SELECT id, nombre, fechaEmpieza FROM bundles WHERE id > @id1 AND id < @id2";

				buscar = buscar.Replace("@id1", id1.ToString());
				buscar = buscar.Replace("@id2", id2.ToString());

				using (SqlCommand comando = new SqlCommand(buscar, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							int id = 0;
							string nombre = string.Empty;
							DateTime fecha = new DateTime();

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

							try
							{
								if (lector.IsDBNull(2) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(2)) == false)
									{
										fecha = DateTime.Parse(lector.GetString(2));
									}
								}
							}
							catch { }

							if (id > 0 && string.IsNullOrEmpty(nombre) == false)
							{
								string texto = "<url>" + Environment.NewLine +
									 "<loc>https://pepeizqdeals.com/bundle/" + id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(nombre) + "/</loc>" + Environment.NewLine;

								if (fecha.Year > 1)
								{
									texto = texto + "<lastmod>" + fecha.ToString("yyyy-MM-dd") + "</lastmod>" + Environment.NewLine;
								}

								texto = texto + "</url>";

								lineas.Add(texto);
							}
						}
					}
				}
			}

			return lineas;
		}

		public static List<string> NoticiasIngles(int id1, int id2, SqlConnection conexion = null)
		{
			List<string> lineas = new List<string>();

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
				string buscar = "SELECT id, tituloEn, fechaEmpieza FROM noticias WHERE id > @id1 AND id < @id2";

				buscar = buscar.Replace("@id1", id1.ToString());
				buscar = buscar.Replace("@id2", id2.ToString());

				using (SqlCommand comando = new SqlCommand(buscar, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							int id = 0;
							string nombre = string.Empty;
							DateTime fecha = new DateTime();

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

							try
							{
								if (lector.IsDBNull(2) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(2)) == false)
									{
										fecha = DateTime.Parse(lector.GetString(2));
									}
								}
							}
							catch { }

							if (id > 0 && string.IsNullOrEmpty(nombre) == false)
							{
								string texto = "<url>" + Environment.NewLine +
									 "<loc>https://pepeizqdeals.com/news/" + id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(nombre) + "/</loc>" + Environment.NewLine;

								if (fecha.Year > 1)
								{
									texto = texto + "<lastmod>" + fecha.ToString("yyyy-MM-dd") + "</lastmod>" + Environment.NewLine;
								}

								texto = texto + "</url>";

								lineas.Add(texto);
							}
						}
					}
				}
			}

			return lineas;
		}

		public static List<string> NoticiasEspañol(int id1, int id2, SqlConnection conexion = null)
		{
			List<string> lineas = new List<string>();

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
				string buscar = "SELECT id, tituloEs, fechaEmpieza FROM noticias WHERE id > @id1 AND id < @id2";

				buscar = buscar.Replace("@id1", id1.ToString());
				buscar = buscar.Replace("@id2", id2.ToString());

				using (SqlCommand comando = new SqlCommand(buscar, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							int id = 0;
							string nombre = string.Empty;
							DateTime fecha = new DateTime();

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

							try
							{
								if (lector.IsDBNull(2) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(2)) == false)
									{
										fecha = DateTime.Parse(lector.GetString(2));
									}
								}
							}
							catch { }

							if (id > 0 && string.IsNullOrEmpty(nombre) == false)
							{
								string texto = "<url>" + Environment.NewLine +
									 "<loc>https://pepeizqdeals.com/news/" + id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(nombre) + "/?language=es</loc>" + Environment.NewLine;

								if (fecha.Year > 1)
								{
									texto = texto + "<lastmod>" + fecha.ToString("yyyy-MM-dd") + "</lastmod>" + Environment.NewLine;
								}

								texto = texto + "</url>";

								lineas.Add(texto);
							}
						}
					}
				}
			}

			return lineas;
		}

		public static List<string> Curators(SqlConnection conexion = null)
		{
			List<string> lineas = new List<string>();

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
				string buscar = "SELECT TOP 5000 slug, fecha FROM curators";

				using (SqlCommand comando = new SqlCommand(buscar, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							string slug = string.Empty;
							DateTime fecha = new DateTime();

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

							try
							{
								if (lector.IsDBNull(1) == false)
								{
									fecha = lector.GetDateTime(1);
								}
							}
							catch { }

							if (string.IsNullOrEmpty(slug) == false)
							{
								string texto = "<url>" + Environment.NewLine +
									 "<loc>https://pepeizqdeals.com/curator/" + slug + "/</loc>" + Environment.NewLine;

								if (fecha.Year > 1)
								{
									texto = texto + "<lastmod>" + fecha.ToString("yyyy-MM-dd") + "</lastmod>" + Environment.NewLine;
								}

								texto = texto + "</url>";

								lineas.Add(texto);
							}
						}
					}
				}
			}

			return lineas;
		}
	}
}
