#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Pendientes
{
    public static class LeerJuego
    {
        public static Pendiente Ejecutar(string tienda)
        {
            SqlConnection conexion = Herramientas.BaseDatos.Conectar();

            using (conexion)
            {
                string busqueda = "SELECT * FROM tienda" + tienda + " WHERE (idJuegos='0' AND descartado='no')";

                using (SqlCommand comando = new SqlCommand(busqueda, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
							Pendiente pendiente = new Pendiente
							{
								enlace = lector.GetString(0),
								nombre = lector.GetString(1),
								imagen = lector.GetString(2)
							};

							return pendiente;
						}
                    }
                }
            }

            conexion.Dispose();

            return null;
        }
    }

    public class Pendiente
    {
        public string nombre;
        public string enlace;
        public string imagen;
    }
}
