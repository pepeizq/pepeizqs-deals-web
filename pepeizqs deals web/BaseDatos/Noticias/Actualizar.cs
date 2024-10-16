using Microsoft.Data.SqlClient;

namespace BaseDatos.Noticias
{
	public static class Actualizar
	{
		public static void TituloEn(string id, string titulo, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE noticias " +
					"SET tituloEn=@tituloEn " +
					"WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);
				comando.Parameters.AddWithValue("@tituloEn", titulo);

				comando.ExecuteNonQuery();
				try
				{

				}
				catch
				{

				}
			}
		}

		public static void TituloEs(string id, string titulo, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE noticias " +
					"SET tituloEs=@tituloEs " +
					"WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);
				comando.Parameters.AddWithValue("@tituloEs", titulo);

				comando.ExecuteNonQuery();
				try
				{

				}
				catch
				{

				}
			}
		}

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

		public static void FechaTermina(string id, string nuevaFecha, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE noticias " +
					"SET fechaTermina=@fechaTermina " +
					"WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);
				comando.Parameters.AddWithValue("@fechaTermina", nuevaFecha);

				comando.ExecuteNonQuery();
				try
				{

				}
				catch
				{

				}
			}
		}

		public static void Tipo(string id, string nuevoTipo, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE noticias " +
					"SET noticiaTipo=@noticiaTipo " +
					"WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);
				comando.Parameters.AddWithValue("@noticiaTipo", nuevoTipo);

				comando.ExecuteNonQuery();
				try
				{

				}
				catch
				{

				}
			}
		}

		public static void ContenidoEn(string id, string nuevoContenido, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE noticias " +
					"SET contenidoEn=@contenidoEn " +
					"WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);
				comando.Parameters.AddWithValue("@contenidoEn", nuevoContenido);

				comando.ExecuteNonQuery();
				try
				{

				}
				catch
				{

				}
			}
		}

		public static void ContenidoEs(string id, string nuevoContenido, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE noticias " +
					"SET contenidoEs=@contenidoEs " +
					"WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);
				comando.Parameters.AddWithValue("@contenidoEs", nuevoContenido);

				comando.ExecuteNonQuery();
				try
				{

				}
				catch
				{

				}
			}
		}

		public static void BundleId(string id, string nuevoValor, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE noticias " +
					"SET bundleId=@bundleId " +
					"WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);
				comando.Parameters.AddWithValue("@bundleId", nuevoValor);

				comando.ExecuteNonQuery();
				try
				{

				}
				catch
				{

				}
			}
		}

		public static void GratisIds(string id, string nuevoValor, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE noticias " +
					"SET gratisIds=@gratisIds " +
					"WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);
				comando.Parameters.AddWithValue("@gratisIds", nuevoValor);

				comando.ExecuteNonQuery();
				try
				{

				}
				catch
				{

				}
			}
		}

		public static void SuscripcionesIds(string id, string nuevoValor, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE noticias " +
					"SET suscripcionesIds=@suscripcionesIds " +
					"WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);
				comando.Parameters.AddWithValue("@suscripcionesIds", nuevoValor);

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
