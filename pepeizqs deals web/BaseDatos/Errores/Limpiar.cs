#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Errores
{
    public static class Limpiar
    {
        public static void Ejecutar(SqlConnection conexion)
        {
            string limpiar = "TRUNCATE TABLE errores";

            using (SqlCommand comando = new SqlCommand(limpiar, conexion))
            {
                comando.ExecuteNonQuery();
            }
        }
    }
}
