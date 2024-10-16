#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using Suscripciones2;

namespace BaseDatos.Suscripciones
{
    public static class Buscar
    {
		public static JuegoSuscripcion Cargar(SqlDataReader lector)
		{
			JuegoSuscripcion suscripcion = new JuegoSuscripcion
			{
				Tipo = SuscripcionesCargar.DevolverSuscripcion(lector.GetInt32(0)).Id,
				JuegoId = lector.GetInt32(1),
				Nombre = lector.GetString(2),
				Imagen = lector.GetString(3),
				DRM = JuegoDRM2.DevolverDRM(lector.GetInt32(4)),
				Enlace = lector.GetString(5),
				FechaEmpieza = Convert.ToDateTime(lector.GetString(6)),
				FechaTermina = Convert.ToDateTime(lector.GetString(7))
			};

			if (lector.IsDBNull(8) == false)
			{
				suscripcion.ImagenNoticia = lector.GetString(8);
			}

            if (lector.IsDBNull(9) == false)
            {
                suscripcion.Id = lector.GetInt32(9);
            }

            return suscripcion;
		}

		public static List<JuegoSuscripcion> Actuales(SqlConnection conexion = null)
        {
            List<JuegoSuscripcion> suscripciones = new List<JuegoSuscripcion>();

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
                string busqueda = "SELECT * FROM suscripciones WHERE GETDATE() BETWEEN fechaEmpieza AND fechaTermina";

                using (SqlCommand comando = new SqlCommand(busqueda, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
							suscripciones.Add(Cargar(lector));
						}
                    }
                }
            } 

			if (suscripciones.Count > 0)
			{
				suscripciones.Reverse();
			}

            return suscripciones;
        }

		public static List<JuegoSuscripcion> Año(string año, SqlConnection conexion = null)
		{
			List<JuegoSuscripcion> suscripciones = new List<JuegoSuscripcion>();

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
				string busqueda = "SELECT * FROM suscripciones WHERE YEAR(fechaEmpieza) = " + año + " AND GETDATE() > fechaTermina";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							suscripciones.Add(Cargar(lector));
						}
					}
				}
			}

			if (suscripciones.Count > 0)
			{
				suscripciones.Reverse();
			}

			return suscripciones;
		}

		public static List<JuegoSuscripcion> UnTipo(string suscripcionTexto, Herramientas.Tiempo tiempo)
		{
			List<JuegoSuscripcion> suscripciones = new List<JuegoSuscripcion>();

			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string busqueda = "SELECT * FROM suscripciones WHERE suscripcion=@suscripcion";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@suscripcion", SuscripcionesCargar.DevolverSuscripcion(suscripcionTexto).Id);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							JuegoSuscripcion suscripcion = Cargar(lector);

							if (tiempo == Herramientas.Tiempo.Atemporal)
							{
								suscripciones.Add(suscripcion);
							}
							else if (tiempo == Herramientas.Tiempo.Actual)
							{ 
								if (DateTime.Now >= suscripcion.FechaEmpieza && DateTime.Now <= suscripcion.FechaTermina)
								{
									suscripciones.Add(suscripcion);
								}
							}
							else if (tiempo == Herramientas.Tiempo.Pasado)
							{
								if (DateTime.Now > suscripcion.FechaTermina)
								{
									suscripciones.Add(suscripcion);
								}
							}
						}
					}
				}
			}

			if (suscripciones.Count > 0) 
			{
				suscripciones = suscripciones.OrderBy(x => x.Nombre).ToList();
			}

			return suscripciones;
		}

		public static JuegoSuscripcion UnJuego(int id, SqlConnection conexion = null)
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
				string busqueda = "SELECT * FROM suscripciones WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@id", id);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							return Cargar(lector);
						}
					}
				}
			}

			return null;
		}

		public static JuegoSuscripcion UnJuego(int juegoId)
		{
			List<JuegoSuscripcion> resultados = new List<JuegoSuscripcion>();
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string busqueda = "SELECT * FROM suscripciones WHERE juegoId=@juegoId";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@juegoId", juegoId);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							resultados.Add(Cargar(lector));
						}
					}
				}
			}

			if (resultados.Count > 0)
			{
				return resultados[resultados.Count - 1];
			}

			return null;
		}

		public static List<JuegoSuscripcion> Ultimos(string cantidad)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				return Ultimos(conexion, cantidad);
			}
		}

		public static List<JuegoSuscripcion> Ultimos(SqlConnection conexion, string cantidad)
		{
			List<JuegoSuscripcion> juegos = new List<JuegoSuscripcion>();

			string busqueda = "SELECT TOP " + cantidad + " * FROM suscripciones ORDER BY id DESC";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						juegos.Add(Cargar(lector));
					}
				}
			}

			return juegos;
		}
	}
}
