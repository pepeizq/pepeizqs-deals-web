#nullable disable

using Microsoft.Data.SqlClient;
using Sorteos2;

namespace BaseDatos.Sorteos
{
	public static class Insertar
	{
		public static void Ejecutar(Sorteo sorteo)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string sqlInsertar = "INSERT INTO sorteos " +
					"(juegoId, grupoId, clave, participantes, fechaTermina) VALUES " +
					"(@juegoId, @grupoId, @clave, @participantes, @fechaTermina) ";

				using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
				{
					comando.Parameters.AddWithValue("@juegoId", sorteo.JuegoId.ToString());
					comando.Parameters.AddWithValue("@grupoId", sorteo.GrupoId);
					comando.Parameters.AddWithValue("@clave", sorteo.Clave);
					comando.Parameters.AddWithValue("@participantes", sorteo.Participantes.ToString());
					comando.Parameters.AddWithValue("@fechaTermina", sorteo.FechaTermina.ToString());

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
}
