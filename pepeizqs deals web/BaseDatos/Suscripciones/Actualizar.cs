#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Suscripciones
{
	public static class Actualizar
	{
		public static void FechaTermina(JuegoSuscripcion suscripcion, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE suscripciones " +
					"SET fechaTermina=@fechaTermina WHERE enlace=@enlace";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@enlace", suscripcion.Enlace);
				comando.Parameters.AddWithValue("@fechaTermina", suscripcion.FechaTermina.ToString());

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
