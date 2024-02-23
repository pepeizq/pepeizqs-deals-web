﻿using Microsoft.Data.SqlClient;

namespace BaseDatos.Errores
{
	public static class Insertar
	{
		public static void Ejecutar(string seccion, Exception ex)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string sqlInsertar = "INSERT INTO errores " +
                    "(seccion, mensaje, stacktrace, fecha) VALUES " +
                    "(@seccion, @mensaje, @stacktrace, @fecha) ";

				using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
				{
                    comando.Parameters.AddWithValue("@seccion", seccion);
                    comando.Parameters.AddWithValue("@mensaje", ex.Message);
                    comando.Parameters.AddWithValue("@stacktrace", ex.StackTrace);
                    comando.Parameters.AddWithValue("@fecha", DateTime.Now.ToString());

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
