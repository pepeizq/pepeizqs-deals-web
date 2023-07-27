#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace BaseDatos.Juegos
{
	public static class Actualizar
	{
		public static void Ejecutar(Juego juego)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

			using (SqlConnection conexion = new SqlConnection(conexionTexto))
			{
				conexion.Open();

				string sqlEditar = "UPDATE juegos " +
					"SET idSteam=@idSteam, idGog=@idGog, nombre=@nombre, tipo=@tipo, fechaSteamAPIComprobacion=@fechaSteamAPIComprobacion, " +
						"imagenes=@imagenes, precioMinimosHistoricos=@precioMinimosHistoricos, precioActualesTiendas=@precioActualesTiendas, " +
						"analisis=@analisis, caracteristicas=@caracteristicas, media=@media ";

				if (juego.IdSteam > 0)
				{
					sqlEditar = sqlEditar + "WHERE idSteam=@idSteam";
				}
				else
				{
					if (juego.IdGog > 0)
					{
						sqlEditar = sqlEditar + "WHERE idGog=@idGog";
					}
					else
					{
						sqlEditar = sqlEditar + "WHERE id=@id";
					}
				}

				using (SqlCommand comando = new SqlCommand(sqlEditar, conexion))
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
