#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Pendientes
{
	public static class Buscar
	{
		public static string Nombre(string nombre, SqlConnection conexion)
		{
            string busqueda = "SELECT * FROM juegos WHERE nombre=@nombre";

            using (SqlCommand comando = new SqlCommand(busqueda, conexion))
            {
                comando.Parameters.AddWithValue("@nombre", nombre);

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        return lector.GetInt32(0).ToString();
                    }
                }
            }

            return "0";
		}

        public static int TiendasCantidad(SqlConnection conexion = null)
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

			int cantidad = 0;

			using (conexion)
			{
                foreach (var tienda in Tiendas2.TiendasCargar.GenerarListado())
                {
                    if (tienda.Id != "steam")
                    {
                        string busqueda = "SELECT * FROM tienda" + tienda.Id + " WHERE (idJuegos='0' AND descartado='no')";

                        using (SqlCommand comando = new SqlCommand(busqueda, conexion))
                        {
                            SqlDataReader lector = comando.ExecuteReader();

                            using (lector)
                            {
                                while (lector.Read() == true)
                                {
									cantidad += 1;
                                }
                            }
                        }
                    }
                }
            }

            return cantidad;
        }

		public static int SuscripcionCantidad(SqlConnection conexion = null)
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

			int cantidad = 0;

			using (conexion)
			{
				foreach (var suscripcion in Suscripciones2.SuscripcionesCargar.GenerarListado())
				{
					if (suscripcion.AdminPendientes == true)
					{
						string busqueda = "SELECT * FROM temporal" + suscripcion.Id.ToString();

						using (SqlCommand comando = new SqlCommand(busqueda, conexion))
						{
							SqlDataReader lector = comando.ExecuteReader();

							using (lector)
							{
								while (lector.Read() == true)
								{
									cantidad += 1;
								}
							}
						}
					}
				}
			}

			return cantidad;
		}

		public static int StreamingCantidad(SqlConnection conexion = null)
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

			int cantidad = 0;

			using (conexion)
			{
                foreach (var streaming in Streaming2.StreamingCargar.GenerarListado())
				{
                    string busqueda = "SELECT * FROM streaming" + streaming.Id + " WHERE (idJuego IS NULL OR idJuego = '0') AND (descartado IS NULL OR descartado = 0)";

                    using (SqlCommand comando = new SqlCommand(busqueda, conexion))
                    {
                        SqlDataReader lector = comando.ExecuteReader();

                        using (lector)
                        {
                            while (lector.Read() == true)
                            {
                                cantidad += 1;
                            }
                        }
                    }
                } 
            }

			return cantidad;
		}

        public static int PlataformaCantidad(SqlConnection conexion = null)
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

			int cantidad = 0;

            foreach (var plataforma in Plataformas2.PlataformasCargar.GenerarListado())
            {
                string busqueda = "SELECT * FROM temporal" + plataforma.Id + "juegos";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					SqlDataReader lector = comando.ExecuteReader();

					using (lector)
					{
						while (lector.Read() == true)
						{
							cantidad += 1;
						}
					}
				}
			}

			return cantidad;
		}

		public static List<Pendiente> Tienda(string tiendaId, SqlConnection conexion)
        {
			List<Pendiente> listaPendientes = new List<Pendiente>();

			string busqueda = "SELECT * FROM tienda" + tiendaId + " WHERE (idJuegos='0' AND descartado='no')";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				SqlDataReader lector = comando.ExecuteReader();

				using (lector)
				{
					while (lector.Read())
					{
						Pendiente pendiente = new Pendiente
						{
							Enlace = lector.GetString(0),
							Nombre = lector.GetString(1),
							Imagen = lector.GetString(2)
						};

						listaPendientes.Add(pendiente);
					}
				}
			}

			return listaPendientes;
		}

        public static List<Pendiente> Suscripcion(Suscripciones2.SuscripcionTipo id, SqlConnection conexion = null)
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

            List<Pendiente> listaPendientes = new List<Pendiente>();

            using (conexion)
            {
                string busqueda = "SELECT * FROM temporal" + id;

                using (SqlCommand comando = new SqlCommand(busqueda, conexion))
                {
                    SqlDataReader lector = comando.ExecuteReader();

                    using (lector)
                    {
                        while (lector.Read() == true)
                        {
                            Pendiente pendiente = new Pendiente
                            {
                                Enlace = lector.GetString(0),
                                Nombre = lector.GetString(1),
                                Imagen = lector.GetString(2)
                            };

                            listaPendientes.Add(pendiente);
                        }
                    }
                }
            }

            return listaPendientes;
        }

        public static List<Pendiente> Streaming(Streaming2.StreamingTipo id, SqlConnection conexion = null)
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

            List<Pendiente> listaPendientes = new List<Pendiente>();

            using (conexion)
            {
                string busqueda = "SELECT * FROM streaming" + id + " WHERE (idJuego IS NULL OR idJuego = '0') AND (descartado IS NULL OR descartado = 0)";

                using (SqlCommand comando = new SqlCommand(busqueda, conexion))
                {
                    SqlDataReader lector = comando.ExecuteReader();

                    using (lector)
                    {
                        while (lector.Read() == true)
                        {
                            Pendiente pendiente = new Pendiente
                            {
                                Enlace = lector.GetString(0),
                                Nombre = lector.GetString(1)
                            };

                            listaPendientes.Add(pendiente);
                        }
                    }
                }
            }

            return listaPendientes;
        }

        public static List<Pendiente> Plataforma(Plataformas2.PlataformaTipo id, SqlConnection conexion = null)
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

			List<Pendiente> listaPendientes = new List<Pendiente>();

			string busqueda = "SELECT * FROM temporal" + id + "juegos";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read() == true)
					{
						Pendiente pendiente = new Pendiente
						{
							Enlace = lector.GetString(0),
							Nombre = lector.GetString(0)
						};

						listaPendientes.Add(pendiente);
					}
				}
			}

			return listaPendientes;
		}

        public static Pendiente PrimerJuegoTienda(string tiendaId, SqlConnection conexion)
		{
			string busqueda = "SELECT * FROM tienda" + tiendaId + " WHERE (idJuegos='0' AND descartado='no')";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				SqlDataReader lector = comando.ExecuteReader();

				using (lector)
				{
					while (lector.Read())
					{
						Pendiente pendiente = new Pendiente
						{
							Enlace = lector.GetString(0),
							Nombre = lector.GetString(1),
							Imagen = lector.GetString(2)
						};

						return pendiente;
					}
				}
			}

			return null;
		}

        public static Pendiente PrimerJuegoSuscripcion(string suscripcionId, SqlConnection conexion)
        {
            string busqueda = "SELECT * FROM temporal" + suscripcionId ;

            using (SqlCommand comando = new SqlCommand(busqueda, conexion))
            {
                SqlDataReader lector = comando.ExecuteReader();

                using (lector)
                {
                    while (lector.Read())
                    {
                        Pendiente pendiente = new Pendiente
                        {
                            Enlace = lector.GetString(0),
                            Nombre = lector.GetString(1),
                            Imagen = lector.GetString(2)
                        };

                        return pendiente;
                    }
                }
            }

            return null;
        }

        public static Pendiente PrimerJuegoStreaming(string streamingId, SqlConnection conexion)
        {
            string busqueda = "SELECT * FROM streaming" + streamingId + " WHERE idJuego IS NULL AND descartado IS NULL";

            using (SqlCommand comando = new SqlCommand(busqueda, conexion))
            {
                SqlDataReader lector = comando.ExecuteReader();

                using (lector)
                {
                    while (lector.Read())
                    {
                        Pendiente pendiente = new Pendiente
                        {
                            Enlace = lector.GetString(0),
                            Nombre = lector.GetString(1),
                            Imagen = "vacio"
                        };

                        return pendiente;
                    }
                }
            }

            return null;
        }

		public static Pendiente PrimerJuegoPlataforma(string plataformaId, SqlConnection conexion)
		{
			string busqueda = "SELECT * FROM temporal" + plataformaId + "juegos";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						if (lector.IsDBNull(1) == false)
						{
							Pendiente pendiente = new Pendiente
							{
								Enlace = lector.GetString(0),
								Nombre = lector.GetString(1),
								Imagen = "vacio"
							};

							return pendiente;
						}
						else
						{
							Pendiente pendiente = new Pendiente
							{
								Enlace = lector.GetString(0),
								Nombre = lector.GetString(0),
								Imagen = "vacio"
							};

							return pendiente;
						}
					}
				}
			}

			return null;
		}
	}

	public class Pendiente
	{
		public string Enlace;
		public string Nombre;
		public string Imagen;
	}

	public class PendientesTienda
	{
		public List<Pendiente> Pendientes;
		public Tiendas2.Tienda Tienda;
	}

	public class PendientesSuscripcion
	{
		public List<Pendiente> Pendientes;
		public Suscripciones2.Suscripcion Suscripcion;
	}

	public class PendientesStreaming
    {
        public List<Pendiente> Pendientes;
        public Streaming2.Streaming Streaming;
    }

	public class PendientesPlataforma
	{
		public List<Pendiente> Pendientes;
		public Plataformas2.Plataforma Plataforma;
	}
}
