#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Errores
{
    public static class Buscar
    {
        public static List<Error> Todos()
        {
            SqlConnection conexion = Herramientas.BaseDatos.Conectar();

            using (conexion)
            {
                return Todos(conexion);
            }
        }

        public static List<Error> Todos(SqlConnection conexion)
        {
            List<Error> listaErrores = new List<Error>();

            string busqueda = "SELECT * FROM errores";

            using (SqlCommand comando = new SqlCommand(busqueda, conexion))
            {
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        Error error = new Error
                        {
                            Mensaje = lector.GetString(1),
                            Fecha = DateTime.Parse(lector.GetString(3)),
                            Seccion = lector.GetString(4)
                        };

                        if (lector.IsDBNull(2) == false)
                        {
                            if (string.IsNullOrEmpty(lector.GetString(2)) == false) 
                            { 
                                error.Stacktrace = lector.GetString(2);
                            }
                        }

						if (lector.IsDBNull(5) == false)
						{
							if (string.IsNullOrEmpty(lector.GetString(5)) == false)
							{
								error.Enlace = lector.GetString(5);
							}
						}

						listaErrores.Add(error);
                    }
                }
            }

            if (listaErrores.Count > 0) 
            {
                listaErrores = listaErrores.OrderBy(x => x.Fecha).Reverse().ToList();
            }

            return listaErrores;
        }
    }

    public class Error
    {
        public string Seccion;
        public string Mensaje;
        public string Stacktrace;
        public DateTime Fecha;
        public string Enlace;
    }
}
