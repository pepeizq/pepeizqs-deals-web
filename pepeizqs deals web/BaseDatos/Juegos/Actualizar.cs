﻿#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace BaseDatos.Juegos
{
	public static class Actualizar
	{
		public static void Ejecutar(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET idSteam=@idSteam, idGog=@idGog, nombre=@nombre, tipo=@tipo, fechaSteamAPIComprobacion=@fechaSteamAPIComprobacion, " +
						"imagenes=@imagenes, precioMinimosHistoricos=@precioMinimosHistoricos, precioActualesTiendas=@precioActualesTiendas, " +
						"analisis=@analisis, caracteristicas=@caracteristicas, media=@media ";

			if (juego.IdSteam > 0)
			{
				sqlActualizar = sqlActualizar + "WHERE idSteam=@idSteam";
			}
			else
			{
				if (juego.IdGog > 0)
				{
					sqlActualizar = sqlActualizar + "WHERE idGog=@idGog";
				}
				else
				{
					sqlActualizar = sqlActualizar + "WHERE id=@id";
				}
			}

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@idSteam", juego.IdSteam);
				comando.Parameters.AddWithValue("@idGog", juego.IdGog);
				comando.Parameters.AddWithValue("@nombre", juego.Nombre);
				comando.Parameters.AddWithValue("@tipo", juego.Tipo);
				comando.Parameters.AddWithValue("@fechaSteamAPIComprobacion", juego.FechaSteamAPIComprobacion);
				comando.Parameters.AddWithValue("@imagenes", JsonConvert.SerializeObject(juego.Imagenes));
				comando.Parameters.AddWithValue("@precioMinimosHistoricos", JsonConvert.SerializeObject(juego.PrecioMinimosHistoricos));
				comando.Parameters.AddWithValue("@precioActualesTiendas", JsonConvert.SerializeObject(juego.PrecioActualesTiendas));
				comando.Parameters.AddWithValue("@analisis", JsonConvert.SerializeObject(juego.Analisis));
				comando.Parameters.AddWithValue("@caracteristicas", JsonConvert.SerializeObject(juego.Caracteristicas));
				comando.Parameters.AddWithValue("@media", JsonConvert.SerializeObject(juego.Media));

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