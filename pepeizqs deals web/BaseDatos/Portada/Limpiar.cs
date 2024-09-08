#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Portada
{
	public static class Limpiar
	{
		public static void Ejecutar(string tabla, SqlConnection conexion = null)
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

            string limpiar = "TRUNCATE TABLE " + tabla;

			using (SqlCommand comando = new SqlCommand(limpiar, conexion))
			{
				comando.ExecuteNonQuery();
			}
		}
	}
}
