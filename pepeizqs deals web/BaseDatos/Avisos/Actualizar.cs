#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Avisos
{
    public class Actualizar
    {
        public static void Ejecutar(string mensaje, string idioma, string enlace, SqlConnection conexion)
        {
            string sqlActualizar = "UPDATE avisos " +
                "SET mensaje=@mensaje, fecha=@fecha, enlace=@enlace WHERE id=@id";

            using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
            {
                comando.Parameters.AddWithValue("@id", idioma);
                comando.Parameters.AddWithValue("@mensaje", mensaje);
                comando.Parameters.AddWithValue("@fecha", DateTime.Now.ToString());
                comando.Parameters.AddWithValue("@enlace", enlace);

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
