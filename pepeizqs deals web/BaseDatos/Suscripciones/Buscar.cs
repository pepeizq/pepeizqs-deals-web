#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using Suscripciones2;

namespace BaseDatos.Suscripciones
{
    public static class Buscar
    {
        public static List<JuegoSuscripcion> Todos()
        {
            List<JuegoSuscripcion> suscripciones = new List<JuegoSuscripcion>();

            SqlConnection conexion = Herramientas.BaseDatos.Conectar();

            using (conexion)
            {
                string busqueda = "SELECT * FROM suscripciones";

                using (SqlCommand comando = new SqlCommand(busqueda, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
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

							suscripciones.Add(suscripcion);
                        }
                    }
                }
            }

            conexion.Dispose();       

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

			conexion.Dispose();

			if (suscripciones.Count > 0) 
			{
				suscripciones = suscripciones.OrderBy(x => x.Nombre).ToList();
			}

			return suscripciones;
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

							resultados.Add(suscripcion);
						}
					}
				}
			}

			if (resultados.Count > 0)
			{
				return resultados[resultados.Count - 1];
			}

			conexion.Dispose();

			return null;
		}
	}
}
