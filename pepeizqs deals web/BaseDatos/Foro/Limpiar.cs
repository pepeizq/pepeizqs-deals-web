#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Foro
{
	public static class Limpiar
	{
		public static void Post(int id, SqlConnection conexion = null)
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

			string busqueda = "DELETE FROM foroPost WHERE id=@id OR respuestaId=@id";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);

				comando.ExecuteNonQuery();
			}
		}
	}
}
