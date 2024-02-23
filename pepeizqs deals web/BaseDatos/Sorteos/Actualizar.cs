using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Sorteos2;

namespace BaseDatos.Sorteos
{
    public static class Actualizar
    {
        public static void Participante(Sorteo sorteo, SqlConnection conexion)
        {
            string sqlActualizar = "UPDATE sorteos " +
                    "SET participantes=@participantes WHERE id=@id";

            using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
            {
                comando.Parameters.AddWithValue("@id", sorteo.Id);
                comando.Parameters.AddWithValue("@participantes", sorteo.Participantes.ToString());

                try
                {
                    comando.ExecuteNonQuery();
                }
                catch
                {

                }
            }
        }

        public static void Ganador(Sorteo sorteo, SqlConnection conexion)
        {
            string sqlActualizar = "UPDATE sorteos " +
                    "SET ganadorId=@ganadorId WHERE id=@id";

            using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
            {
                comando.Parameters.AddWithValue("@id", sorteo.Id);
                comando.Parameters.AddWithValue("@ganadorId", sorteo.GanadorId);

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
}
