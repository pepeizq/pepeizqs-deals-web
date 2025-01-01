#nullable disable

using Microsoft.Data.SqlClient;
using Tiendas2;

namespace BaseDatos.Admin
{
	public static class Buscar
	{
		public static string Dato(string id, SqlConnection conexion = null)
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

			string seleccionarDato = "SELECT * FROM adminDatos WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(seleccionarDato, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					if (lector.Read() == true)
					{
						return lector.GetString(1);
					}
				}
			}

			return null;
		}

		public static bool TiendasPosibleUsar(TimeSpan tiempo, string tiendaId, SqlConnection conexion = null)
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

			string seleccionarTarea = "SELECT * FROM adminTiendas WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(seleccionarTarea, conexion))
			{
				comando.Parameters.AddWithValue("@id", tiendaId);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					if (lector.Read() == true)
					{
						DateTime ultimaComprobacion = DateTime.Parse(lector.GetString(1));

						if ((DateTime.Now - ultimaComprobacion) > tiempo)
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		public static AdminTarea TiendasEnUso(TimeSpan tiempo, SqlConnection conexion = null)
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

			List<AdminTarea> tiendas = new List<AdminTarea>();

			string seleccionarTarea = "SELECT * FROM adminTiendas";

			using (SqlCommand comando = new SqlCommand(seleccionarTarea, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						AdminTarea tienda = new AdminTarea
						{
							Id = lector.GetString(0),
							Fecha = DateTime.Parse(lector.GetString(1))
						};

						bool añadir = true;

						foreach (var tienda2 in TiendasCargar.GenerarListado())
						{
							if (tienda2.Id == tienda.Id)
							{
								if (tienda2.AdminInteractuar == false)
								{
									añadir = false;
								}

								break;
							}
						}

						if (añadir == true)
						{
							tiendas.Add(tienda);
						}
					}
				}
			}

			if (tiendas.Count > 0)
			{
				tiendas = tiendas.OrderBy(x => x.Fecha).ToList();

				foreach (var tienda in tiendas)
				{
					DateTime ultimaComprobacion = tienda.Fecha;

					if ((DateTime.Now - ultimaComprobacion) < tiempo)
					{
						return tienda;
					}
				}
			}

			return null;
		}

		public static int TiendasValorAdicional(string id, string valor, SqlConnection conexion = null)
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

			string sqlBusqueda = "SELECT * FROM adminTiendas WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlBusqueda, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					if (lector.Read() == true)
					{
						if (lector.IsDBNull(lector.GetOrdinal(valor)) == false)
						{
							return lector.GetInt32(lector.GetOrdinal(valor));
						}
					}
				}
			}

			return 0;
		}

		public static AdminTarea Tarea(string id, SqlConnection conexion = null)
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

			string seleccionarTarea = "SELECT * FROM adminTiendas WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(seleccionarTarea, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						if (lector.IsDBNull(0) == false)
						{
							if (string.IsNullOrEmpty(lector.GetString(0)) == false)
							{
								AdminTarea tarea = new AdminTarea
								{
									Id = lector.GetString(0),
									Fecha = DateTime.Parse(lector.GetString(1)),
									Cantidad = lector.GetInt32(2),
									Valor1 = lector.GetInt32(3),
									Valor2 = lector.GetInt32(4)
								};

								return tarea;
							}
						}
					}
				}
			}

			return null;
		}

		public static bool TareaPosibleUsar(string id, TimeSpan tiempo, SqlConnection conexion = null)
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

			string seleccionarTarea = "SELECT * FROM adminTareas WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(seleccionarTarea, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					if (lector.Read() == true)
					{
						DateTime ultimaComprobacion = DateTime.Parse(lector.GetString(1));

						if ((DateTime.Now - ultimaComprobacion) < tiempo)
						{
							return false;
						}
					}
				}
			}

			return true;
		}
	}

	public class AdminTarea
	{
		public string Id;
		public DateTime Fecha;
		public int Cantidad;
		public int Valor1;
		public int Valor2;
	}
}
