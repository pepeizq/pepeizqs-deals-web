#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Foro
{
	public static class Actualizar
	{
		public static void Post(int postId, string postContenido, SqlConnection conexion = null)
		{
			if (conexion == null)
			{
				conexion = Herramientas.BaseDatos.Conectar();
			}
			else
			{
				if (conexion.State != System.Data.ConnectionState.Open)
				{
					conexion = Herramientas.BaseDatos.Conectar();
				}
			}

			string actualizar = "UPDATE foroPost SET contenido=@contenido, fechaEdicion=@fechaEdicion WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(actualizar, conexion))
			{
				comando.Parameters.AddWithValue("@contenido", postContenido);
				comando.Parameters.AddWithValue("@id", postId);
				comando.Parameters.AddWithValue("@fechaEdicion", DateTime.Now);

				comando.ExecuteNonQuery();
			}
		}

		public static void Fijo(int postId, SqlConnection conexion = null)
		{
			if (conexion == null)
			{
				conexion = Herramientas.BaseDatos.Conectar();
			}
			else
			{
				if (conexion.State != System.Data.ConnectionState.Open)
				{
					conexion = Herramientas.BaseDatos.Conectar();
				}
			}

			string actualizar = @"UPDATE foroPost 
SET fijo = CASE WHEN fijo IS NOT NULL THEN ~fijo ELSE 'True' END
WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(actualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", postId);

				comando.ExecuteNonQuery();
			}
		}
	}
}
