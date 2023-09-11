#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Divisas
{
	public static class Insertar
	{
		public static void Ejecutar(Divisa divisa)
		{
            SqlConnection conexion = Herramientas.BaseDatos.Conectar();

            using (conexion)
			{
				string sqlAñadir = "INSERT INTO divisas " +
					"(id, cantidad, fecha) VALUES " +
					"(@id, @cantidad, @fecha) ";

				using (SqlCommand comando = new SqlCommand(sqlAñadir, conexion))
				{
					comando.Parameters.AddWithValue("@id", divisa.Id);
					comando.Parameters.AddWithValue("@cantidad", divisa.Cantidad);
					comando.Parameters.AddWithValue("@fecha", divisa.FechaActualizacion);

					try
					{
						comando.ExecuteNonQuery();
					}
					catch
					{

					}
				}
			}

			conexion.Dispose();
		}
	}
}
