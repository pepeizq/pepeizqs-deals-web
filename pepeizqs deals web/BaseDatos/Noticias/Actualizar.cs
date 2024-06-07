using Microsoft.Data.SqlClient;

namespace BaseDatos.Noticias
{
	public static class Actualizar
	{
		public static void Imagen(string id, string imagen, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE noticias " +
					"SET imagen=@imagen " +
					"WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);
				comando.Parameters.AddWithValue("@imagen", imagen);

				comando.ExecuteNonQuery();
				try
				{

				}
				catch
				{

				}
			}
		}

        public static void Enlace(string id, string enlace, SqlConnection conexion)
        {
            string sqlActualizar = "UPDATE noticias " +
                    "SET enlace=@enlace " +
                    "WHERE id=@id";

            using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
            {
                comando.Parameters.AddWithValue("@id", id);
                comando.Parameters.AddWithValue("@enlace", enlace);

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
