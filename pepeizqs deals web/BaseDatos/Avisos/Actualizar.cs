#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Avisos
{
    public class Actualizar
    {
        public static void Ejecutar(string mensaje, string idioma, SqlConnection conexion)
        {
            string sqlActualizar = "UPDATE avisos " +
                "SET mensaje=@mensaje, fecha=@fecha WHERE id=@id";

            using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
            {
                comando.Parameters.AddWithValue("@id", idioma);
                comando.Parameters.AddWithValue("@mensaje", mensaje);
                comando.Parameters.AddWithValue("@fecha", DateTime.Now.ToString());

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
