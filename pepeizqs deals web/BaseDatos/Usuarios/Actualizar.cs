#nullable disable

using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace BaseDatos.Usuarios
{
    public static class Actualizar
    {
        public static void Claves(SqlConnection conexion, string usuarioId, Clave nuevaClave)
        {
            List<Clave> claves = new List<Clave>();

            string busqueda = "SELECT * FROM AspNetUsers WHERE Id=@Id";

            using (SqlCommand comando = new SqlCommand(busqueda, conexion))
            {
                comando.Parameters.AddWithValue("@Id", usuarioId);

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        if (lector.IsDBNull(24) == false)
                        {
                            if (string.IsNullOrEmpty(lector.GetString(24)) == false)
                            {
                                claves = JsonConvert.DeserializeObject<List<Clave>>(lector.GetString(24));
                            }
                        }
                    }
                }
            }

            claves.Add(nuevaClave);

            string sqlActualizar = "UPDATE AspNetUsers " +
                    "SET Keys=@Keys WHERE Id=@Id";

            using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
            {
                comando.Parameters.AddWithValue("@Id", usuarioId);
                comando.Parameters.AddWithValue("@Keys", JsonConvert.SerializeObject(claves));

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

    public class Clave
    {
        public string Nombre;
        public string JuegoId;
        public string Codigo;
    }
}
