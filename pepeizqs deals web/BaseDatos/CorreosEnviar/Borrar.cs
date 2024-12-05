#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.CorreosEnviar
{
	public static class Borrar
	{
		public static void Ejecutar(int id, SqlConnection conexion)
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

			string borrar = "DELETE FROM correosEnviar WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(borrar, conexion))
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
