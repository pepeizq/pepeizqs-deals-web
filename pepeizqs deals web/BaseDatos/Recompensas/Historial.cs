using Microsoft.Data.SqlClient;
using Sorteos2;

namespace BaseDatos.Recompensas
{
    public static class Historial
    {
        public static void Insertar(string usuarioId, int coins, string razon, DateTime fecha)
        {
            SqlConnection conexion = Herramientas.BaseDatos.Conectar();

            using (conexion)
            {
                string sqlInsertar = "INSERT INTO recompensasHistorial " +
                    "(usuarioId, coins, razon, fecha) VALUES " +
                    "(@usuarioId, @coins, @razon, @fecha) ";

                using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
                {
                    comando.Parameters.AddWithValue("@usuarioId", usuarioId);
                    comando.Parameters.AddWithValue("@coins", coins.ToString());
                    comando.Parameters.AddWithValue("@razon", razon);
                    comando.Parameters.AddWithValue("@fecha", fecha.ToString());

                    comando.ExecuteNonQuery();
                    try
                    {

                    }
                    catch
                    {

                    }
                }
            }
        }
    }
}
