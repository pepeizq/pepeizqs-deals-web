using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace BaseDatos.Usuarios
{
    public static class Actualizar
    {
        public static void Claves(SqlConnection conexion, string usuarioId)
        {
            string sqlBuscar = string.Empty;


            string sqlActualizar = "UPDATE AspNetUsers " +
                    "SET Keys=@Keys WHERE Id=@Id";

            using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
            {
                comando.Parameters.AddWithValue("@Id", usuarioId);
                comando.Parameters.AddWithValue("@Keys", JsonConvert.SerializeObject(juego.UsuariosInteresados));

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
