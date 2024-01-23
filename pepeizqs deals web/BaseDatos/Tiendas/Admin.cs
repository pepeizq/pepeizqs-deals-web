#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Tiendas
{
	public static class Admin
	{
		public static void Actualizar(string tienda, DateTime fecha, string mensaje, SqlConnection conexion, string valorAdicional = "0", string valorAdicional2 = "0")
		{
			bool insertar = false;
			bool actualizar = false;

			string seleccionarJuego = "SELECT * FROM adminTiendas WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(seleccionarJuego, conexion))
			{
				comando.Parameters.AddWithValue("@id", tienda);

				SqlDataReader lector = comando.ExecuteReader();

				using (lector)
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
					"(id, fecha, mensaje, valorAdicional) VALUES " +
					"(@id, @fecha, @mensaje, @valorAdicional)";

				SqlCommand comando = new SqlCommand(sqlAñadir, conexion);

				using (comando)
				{
					comando.Parameters.AddWithValue("@id", tienda);
					comando.Parameters.AddWithValue("@fecha", fecha.ToString());
					comando.Parameters.AddWithValue("@mensaje", mensaje);
					comando.Parameters.AddWithValue("@valorAdicional", valorAdicional);

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
						"SET fecha=@fecha, mensaje=@mensaje, valorAdicional=@valorAdicional, valorAdicional2=@valorAdicional2 WHERE id=@id";

				SqlCommand comando = new SqlCommand(sqlActualizar, conexion);

				using (comando)
				{
					comando.Parameters.AddWithValue("@id", tienda);
					comando.Parameters.AddWithValue("@fecha", fecha.ToString());
					comando.Parameters.AddWithValue("@mensaje", mensaje);
					comando.Parameters.AddWithValue("@valorAdicional", valorAdicional);
					comando.Parameters.AddWithValue("@valorAdicional2", valorAdicional2);

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

		public static string ComprobacionMensaje(string tienda)
		{
			string mensaje = string.Empty;

            SqlConnection conexion = Herramientas.BaseDatos.Conectar();

            using (conexion)
			{
				string seleccionarJuego = "SELECT * FROM adminTiendas WHERE id=@id";
				SqlCommand comando = new SqlCommand(seleccionarJuego, conexion);

                using (comando)
				{
					comando.Parameters.AddWithValue("@id", tienda);

					SqlDataReader lector = comando.ExecuteReader();

                    using (lector)
					{
						if (lector.Read() == true)
						{	
							try
							{
								if (lector.GetString(1) != null)
								{
									mensaje = Calculadora.HaceTiempo(DateTime.Parse(lector.GetString(1)), "es-ES");
								}

                                if (lector.GetString(2) != null)
                                {
                                    mensaje = mensaje + " • " + lector.GetString(2);
                                }
                            }
							catch
							{

							}									
						}
					}
				}
			}

			conexion.Dispose();

			return mensaje;
		}

		public static AdminTiendas TiendaComprobar()
		{
			List<AdminTiendas> tiendas = new List<AdminTiendas>();

			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

            using (conexion)
            {
                string seleccionarTarea = "SELECT * FROM adminTiendas";

                using (SqlCommand comando = new SqlCommand(seleccionarTarea, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
						while (lector.Read())
						{
							AdminTiendas tienda = new AdminTiendas
							{
								tienda = lector.GetString(0),
								fecha = DateTime.Parse(lector.GetString(1))
							};

							tiendas.Add(tienda);
						}
                    }
                }
            }

			conexion.Dispose();

			tiendas.OrderBy(x => x.fecha).ToList();

			return tiendas[0];
        }

		public static string TareaLeerUltimaComprobacion()
		{
			string comprobacion = null;

			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string seleccionarTarea = "SELECT * FROM cronGestionador WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(seleccionarTarea, conexion))
				{
					comando.Parameters.AddWithValue("@id", "1");

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							comprobacion = lector.GetString(2);
						}
					}
				}
			}

			conexion.Dispose();

			return comprobacion;
		}

		public static DateTime TareaLeerTienda(string tienda)
		{
            DateTime fecha = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            SqlConnection conexion = Herramientas.BaseDatos.Conectar();

            using (conexion)
            {
                string seleccionarTienda = "SELECT * FROM adminTiendas WHERE id=@id";

                using (SqlCommand comando = new SqlCommand(seleccionarTienda, conexion))
                {
                    comando.Parameters.AddWithValue("@id", tienda);

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read() == true)
                        {
                            fecha = Convert.ToDateTime(lector.GetString(1));
                        }
                    }
                }
            }

            conexion.Dispose();

			return fecha;
        }

		public static void TareaCambiarOrden(int orden)
		{
            SqlConnection conexion = Herramientas.BaseDatos.Conectar();

            using (conexion)
            {
                string sqlActualizar = "UPDATE cronGestionador " +
                            "SET id=@id, posicion=@posicion WHERE id=@id";

                using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
                {
                    comando.Parameters.AddWithValue("@id", "0");
                    comando.Parameters.AddWithValue("@posicion", orden);

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

			conexion.Dispose();
        }

		public static void TareaCambiarUltimaComprobacion(string comprobacion)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string sqlActualizar = "UPDATE cronGestionador " +
							"SET id=@id, valor1=@valor1 WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
				{
					comando.Parameters.AddWithValue("@id", "1");
					comando.Parameters.AddWithValue("@valor1", comprobacion);

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

			conexion.Dispose();
		}

		public static string CargarValorAdicional(string tienda, SqlConnection conexion)
		{
			string valor = string.Empty;

			string seleccionarJuego = "SELECT * FROM adminTiendas WHERE id=@id";
			SqlCommand comando = new SqlCommand(seleccionarJuego, conexion);

			using (comando)
			{
				comando.Parameters.AddWithValue("@id", tienda);

				SqlDataReader lector = comando.ExecuteReader();

				using (lector)
				{
					if (lector.Read() == true)
					{
						try
						{
							valor = lector.GetString(3);
						}
						catch
						{

						}
					}
				}
			}

			return valor;
		}

		public static string CargarValorAdicional2(string tienda, SqlConnection conexion)
		{
			string valor = string.Empty;

			string seleccionarJuego = "SELECT * FROM adminTiendas WHERE id=@id";
			SqlCommand comando = new SqlCommand(seleccionarJuego, conexion);

			using (comando)
			{
				comando.Parameters.AddWithValue("@id", tienda);

				SqlDataReader lector = comando.ExecuteReader();

				using (lector)
				{
					if (lector.Read() == true)
					{
						try
						{
							valor = lector.GetString(4);
						}
						catch
						{

						}
					}
				}
			}

			return valor;
		}
	}

	public class AdminTiendas
	{
		public string tienda;
		public DateTime fecha;
	}
}
