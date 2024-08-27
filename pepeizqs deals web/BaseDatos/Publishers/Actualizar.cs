#nullable disable

using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace BaseDatos.Publishers
{
	public static class Actualizar
	{
		public static void Acepciones(Publisher publisher, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE publishers " +
					"SET acepciones=@acepciones WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", publisher.Id);
				comando.Parameters.AddWithValue("@acepciones", JsonConvert.SerializeObject(publisher.Acepciones));

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
