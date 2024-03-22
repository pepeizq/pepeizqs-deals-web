using Microsoft.Data.SqlClient;

namespace BaseDatos.Juegos
{
	public static class Borrar
	{
		public static void Ejecutar(string id, SqlConnection conexion)
		{
            string borrarJuego = "DELETE FROM juegos WHERE id=@id";

            using (SqlCommand comando = new SqlCommand(borrarJuego, conexion))
            {
                comando.Parameters.AddWithValue("@id", id);

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
