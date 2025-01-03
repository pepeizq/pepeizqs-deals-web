#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Curators
{
	public static class Actualizar
	{
		public static void ImagenFondo(string imagenFondo, int id, SqlConnection conexion = null)
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

			string sqlActualizar = "UPDATE curators " +
						"SET imagenFondo=@imagenFondo WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);
				comando.Parameters.AddWithValue("@imagenFondo", imagenFondo);

				SqlDataReader lector = comando.ExecuteReader();

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
