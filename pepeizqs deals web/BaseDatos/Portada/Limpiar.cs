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

            string limpiar = @"DELETE FROM seccionMinimos WHERE NOT EXISTS (SELECT * FROM juegos
                               WHERE ultimaModificacion >= DATEADD(day, -1, GETDATE()) AND JSON_PATH_EXISTS(analisis, '$.Cantidad') > 0 AND CONVERT(bigint, REPLACE(JSON_VALUE(analisis, '$.Cantidad'),',','')) > 99 AND 
                               ((mayorEdad IS NOT NULL AND mayorEdad = 'false') OR (mayorEdad IS NULL)) AND (freeToPlay = 'false' OR freeToPlay IS NULL))";

			using (SqlCommand comando = new SqlCommand(limpiar, conexion))
			{
				comando.ExecuteNonQuery();
			}
		}
	}
}
