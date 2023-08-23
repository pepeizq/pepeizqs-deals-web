using Microsoft.Data.SqlClient;
using Suscripciones2;

namespace BaseDatos.Suscripciones
{
    public static class Buscar
    {
        public static List<global::Juegos.JuegoSuscripcion> Todos()
        {
            List<global::Juegos.JuegoSuscripcion> suscripciones = new List<global::Juegos.JuegoSuscripcion>();

            SqlConnection conexion = Herramientas.BaseDatos.Conectar();

            using (conexion)
            {
                conexion.Open();

                string busqueda = "SELECT * FROM suscripciones";

                using (SqlCommand comando = new SqlCommand(busqueda, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
							global::Juegos.JuegoSuscripcion suscripcion = new global::Juegos.JuegoSuscripcion
							{
								Suscripcion = SuscripcionesCargar.DevolverSuscripcion(lector.GetInt32(0)).Id,
								JuegoId = lector.GetInt32(1),
								Nombre = lector.GetString(2),
								Imagen = lector.GetString(3),
								DRM = global::Juegos.JuegoDRM2.DevolverDRM(lector.GetInt32(4)),
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
    }
}
