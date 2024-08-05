#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Recompensas
{
    public class RecompensaHistorial
    {
        public string UsuarioId { get; set; }
        public int Coins { get; set; }
        public string Razon { get; set; }
        public DateTime Fecha { get; set; }
    }

    public static class Historial
    {
        public static RecompensaHistorial Cargar(SqlDataReader lector)
        {
            RecompensaHistorial entrada = new RecompensaHistorial
            {
                UsuarioId = lector.GetString(1),
                Coins = lector.GetInt32(2),
                Razon = lector.GetString(3),
                Fecha = Convert.ToDateTime(lector.GetString(4))
            };

            return entrada;
        }

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
                  
                    try
                    {
                        comando.ExecuteNonQuery();
                    }
                    catch
                    {

                    }
                }
            }
        }

        public static List<RecompensaHistorial> LeerUsuario(string usuarioId)
        {
            List<RecompensaHistorial> entradas = new List<RecompensaHistorial>();

            SqlConnection conexion = Herramientas.BaseDatos.Conectar();

            using (conexion)
            {
                string busqueda = "SELECT TOP 30 * FROM recompensasHistorial WHERE usuarioId=@usuarioId ORDER BY fecha DESC";

                using (SqlCommand comando = new SqlCommand(busqueda, conexion))
                {
                    comando.Parameters.AddWithValue("@usuarioId", usuarioId);

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            entradas.Add(Cargar(lector));
                        }
                    }
                }
            }

            return entradas;
        }
    }
}
