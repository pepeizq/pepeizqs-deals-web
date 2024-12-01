#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;
using System;
using Tiendas2;

namespace BaseDatos.Tiendas
{
	public static class Admin
	{
		public static void Actualizar(string tienda, DateTime fecha, string mensaje, SqlConnection conexion, int valorAdicional = 0, int valorAdicional2 = 0)
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
                            if (lector.IsDBNull(1) == false)
                            {
                                if (string.IsNullOrEmpty(lector.GetString(1)) == false)
                                {
									try
									{
                                        mensaje = Calculadora.DiferenciaTiempo(DateTime.Parse(lector.GetString(1)), "es-ES");
                                    }
									catch { }
                                    
                                }
                            }

							if (lector.IsDBNull(2) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(2)) == false)
								{
                                    mensaje = mensaje + " • " + lector.GetString(2);
                                }
							}								
						}
					}
				}
			}

			return mensaje;
		}

		public static AdminTarea LeerTarea(string id, SqlConnection conexion)
		{
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
                                    Mensaje = lector.GetString(2)
                                };

                                return tarea;
                            }
						}
					}
				}
			}

			return null;
        }

        public static AdminTarea ComprobarTiendasUso(SqlConnection conexion, TimeSpan tiempo)
		{
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


        public static bool ComprobarTiendaUso(SqlConnection conexion, TimeSpan tiempo, string tiendaId)
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


		public static bool ComprobarTareaUso(SqlConnection conexion, string id, TimeSpan tiempo)
		{
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

        public static void ActualizarTareaUso(SqlConnection conexion, string id, DateTime fecha)
		{
            string sqlActualizar = "UPDATE adminTareas " +
                        "SET fecha=@fecha WHERE id=@id";

            SqlCommand comando = new SqlCommand(sqlActualizar, conexion);

            using (comando)
            {
                comando.Parameters.AddWithValue("@id", id);
                comando.Parameters.AddWithValue("@fecha", fecha.ToString());

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

		public static string LeerDato(SqlConnection conexion = null, string id = null)
		{
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

		public static void ActualizarDato(SqlConnection conexion, string id, string contenido)
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

            using (conexion)
			{
                string sqlActualizar = "UPDATE adminDatos " +
					"SET contenido=@contenido WHERE id=@id";

                SqlCommand comando = new SqlCommand(sqlActualizar, conexion);

                using (comando)
                {
                    comando.Parameters.AddWithValue("@id", id);
                    comando.Parameters.AddWithValue("@contenido", contenido);

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

		public static int CargarValorAdicional(string id, string valor, SqlConnection conexion = null)
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
							BaseDatos.Errores.Insertar.Mensaje(id, lector.GetInt32(3).ToString());
							return lector.GetInt32(lector.GetOrdinal(valor));
						}
					}
				}
			}

			return 0;
		}
	}

	public class AdminTarea
	{
		public string Id;
		public DateTime Fecha;
		public string Mensaje;
		public bool MinimoHoras;
	}
}
