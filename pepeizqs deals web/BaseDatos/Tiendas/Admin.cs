#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;
using Tiendas2;

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
									mensaje = Calculadora.DiferenciaTiempo(DateTime.Parse(lector.GetString(1)), "es-ES");
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

			return mensaje;
		}

		public static AdminTarea TiendaSiguiente(SqlConnection conexion)
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
							Fecha = DateTime.Parse(lector.GetString(1)),
							Mensaje = lector.GetString(2),
							MinimoHoras = true
						};

						bool añadir = true;

						foreach (var tienda2 in TiendasCargar.GenerarListado())
						{
							if (tienda2.Id == tienda.Id)
							{
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

			tiendas = tiendas.OrderBy(x => x.Fecha).ToList();

			DateTime ahora = DateTime.Now;

			if (ahora.Hour == 19 || ahora.Hour == 20)
			{
				foreach (var tienda in tiendas)
				{
					if (tienda.Id == APIs.Steam.Tienda.Generar().Id)
					{
                        if (ahora - tienda.Fecha > TimeSpan.FromMinutes(20))
                        {
							tienda.MinimoHoras = false;

							return tienda;
						}
					}
					else if (tienda.Id == APIs.Humble.Tienda.Generar().Id)
					{
						if (ahora - tienda.Fecha > TimeSpan.FromMinutes(20))
						{
                            tienda.MinimoHoras = false;

                            return tienda;
						}
					}
				}
			}
			else if (ahora.Hour == 17)
			{
                foreach (var tienda in tiendas)
                {
                    if (tienda.Id == APIs.Fanatical.Tienda.Generar().Id)
                    {
                        if (ahora - tienda.Fecha > TimeSpan.FromHours(1))
                        {
                            tienda.MinimoHoras = false;

                            return tienda;
                        }
                    }
                }
            }
            else if (ahora.Hour == 15)
            {
                foreach (var tienda in tiendas)
                {
                    if (tienda.Id == APIs.GOG.Tienda.Generar().Id)
                    {
                        if (ahora - tienda.Fecha > TimeSpan.FromHours(1))
                        {
                            tienda.MinimoHoras = false;

                            return tienda;
                        }
                    }
                }
            }

            return tiendas[0];
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
			bool usar = false;

			string seleccionarTarea = "SELECT * FROM adminTiendas WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(seleccionarTarea, conexion))
			{
				comando.Parameters.AddWithValue("@id", tiendaId);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					if (lector.Read() == true)
					{
						DateTime ultimaComprobacion = DateTime.Parse(lector.GetString(1));

						if ((DateTime.Now - ultimaComprobacion) < tiempo)
						{
							usar = true;
						}
					}
				}
			}
			
			return usar;
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

	public class AdminTarea
	{
		public string Id;
		public DateTime Fecha;
		public string Mensaje;
		public bool MinimoHoras;
	}
}
