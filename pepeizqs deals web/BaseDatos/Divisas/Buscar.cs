#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace BaseDatos.Divisas
{
	public static class Buscar
	{
		public static Divisa Ejecutar(string id)
		{
			if (id != null) 
			{
                SqlConnection conexion = Herramientas.BaseDatos.Conectar();

                using (conexion)
				{
					string sqlBuscar = "SELECT * FROM divisas WHERE id=@id";

					using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
					{
						comando.Parameters.AddWithValue("@id", id);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							if (lector.Read())
							{
								Divisa divisa = new Divisa
								{
									Id = lector.GetString(0),
									Cantidad = Convert.ToDecimal(lector.GetString(1)),
									FechaActualizacion = DateTime.Parse(lector.GetString(2))
								};

								return divisa;
							}
						}
					}
				}

				conexion.Dispose();
			}

			return null;
		}
	}
}
