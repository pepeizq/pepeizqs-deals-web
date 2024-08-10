#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Errores
{
    public static class Insertar
	{
        public static void Ejecutar(string seccion, Exception ex)
		{
            SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
                Ejecutar(seccion, ex, conexion);
            }
        }

        public static void Ejecutar(string seccion, Exception ex, SqlConnection conexion)
		{
            string sqlInsertar = "INSERT INTO errores " +
                                "(seccion, mensaje, stacktrace, fecha) VALUES " +
                                "(@seccion, @mensaje, @stacktrace, @fecha) ";

            using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
            {
                comando.Parameters.AddWithValue("@seccion", seccion);
                comando.Parameters.AddWithValue("@mensaje", ex.Message);
                comando.Parameters.AddWithValue("@stacktrace", ex.StackTrace);
                comando.Parameters.AddWithValue("@fecha", DateTime.Now.ToString());
  
                try
                {
                    comando.ExecuteNonQuery();
                }
                catch
                {

                }
            }

            Environment.Exit(1);
        }

        public static void Mensaje(string seccion, string mensaje)
        {
            SqlConnection conexion = Herramientas.BaseDatos.Conectar();

            using (conexion)
            {
                Mensaje(seccion, mensaje, conexion);
            }
        }

        public static void Mensaje(string seccion, string mensaje, SqlConnection conexion)
        {
            string sqlInsertar = "INSERT INTO errores " +
                               "(seccion, mensaje, stacktrace, fecha) VALUES " +
                               "(@seccion, @mensaje, @stacktrace, @fecha) ";

            using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
            {
                comando.Parameters.AddWithValue("@seccion", seccion);
                comando.Parameters.AddWithValue("@mensaje", "nada");
                comando.Parameters.AddWithValue("@stacktrace", mensaje);
                comando.Parameters.AddWithValue("@fecha", DateTime.Now.ToString());
             
                try
                {
                    comando.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Ejecutar("Errores", ex);
                }
            }
        }
    }
}
