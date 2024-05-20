﻿#nullable disable

using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace BaseDatos.Bundles
{
	public static class Actualizar
	{
		public static void Ejecutar(Bundles2.Bundle bundle, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE bundles " +
					"SET bundleTipo=@bundleTipo, nombre=@nombre, tienda=@tienda, imagen=@imagen, enlace=@enlace, fechaEmpieza=@fechaEmpieza, fechaTermina=@fechaTermina, juegos=@juegos, tiers=@tiers, pick=@pick, imagenNoticia=@imagenNoticia " + 
					"WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", bundle.Id);
				comando.Parameters.AddWithValue("@bundleTipo", bundle.Tipo);
				comando.Parameters.AddWithValue("@nombre", bundle.NombreBundle);
				comando.Parameters.AddWithValue("@tienda", bundle.NombreTienda);
				comando.Parameters.AddWithValue("@imagen", bundle.ImagenBundle);
				comando.Parameters.AddWithValue("@enlace", bundle.Enlace);
				comando.Parameters.AddWithValue("@fechaEmpieza", bundle.FechaEmpieza.ToString());
				comando.Parameters.AddWithValue("@fechaTermina", bundle.FechaTermina.ToString());
				comando.Parameters.AddWithValue("@juegos", JsonConvert.SerializeObject(bundle.Juegos));
				comando.Parameters.AddWithValue("@tiers", JsonConvert.SerializeObject(bundle.Tiers));
				comando.Parameters.AddWithValue("@pick", bundle.Pick.ToString());
				comando.Parameters.AddWithValue("@imagenNoticia", bundle.ImagenNoticia);

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
