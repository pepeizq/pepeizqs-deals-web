#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Divisas
{
	public static class Actualizar
	{
		public static void Ejecutar(Divisa divisa, SqlConnection conexion)
		{
            string sqlActualizar = "UPDATE divisas " +
                    "SET id=@id, cantidad=@cantidad, fecha=@fecha WHERE id=@id";

            using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
            {
                comando.Parameters.AddWithValue("@id", divisa.Id);
                comando.Parameters.AddWithValue("@cantidad", divisa.Cantidad);
                comando.Parameters.AddWithValue("@fecha", divisa.FechaActualizacion);

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
