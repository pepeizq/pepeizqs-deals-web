#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace BaseDatos.Juegos
{
	public static class Insertar
	{
		public static void Ejecutar(Juego juego, SqlConnection conexion)
		{
			string añadirMaestro1 = null;
			string añadirMaestro2 = null;

			if (string.IsNullOrEmpty(juego.Maestro) == false)
			{
				añadirMaestro1 = ", maestro";
				añadirMaestro2 = ", @maestro";
			}

			string sqlAñadir = "INSERT INTO juegos " +
					"(idSteam, idGog, nombre, tipo, fechaSteamAPIComprobacion, imagenes, precioMinimosHistoricos, precioActualesTiendas, analisis, caracteristicas, media, nombreCodigo" + añadirMaestro1 + ") VALUES " +
					"(@idSteam, @idGog, @nombre, @tipo, @fechaSteamAPIComprobacion, @imagenes, @precioMinimosHistoricos, @precioActualesTiendas, @analisis, @caracteristicas, @media, @nombreCodigo" + añadirMaestro2 + ") ";

			using (SqlCommand comando = new SqlCommand(sqlAñadir, conexion))
			{
				comando.Parameters.AddWithValue("@idSteam", juego.IdSteam);
				comando.Parameters.AddWithValue("@idGog", juego.IdGog);
				comando.Parameters.AddWithValue("@nombre", juego.Nombre);
				comando.Parameters.AddWithValue("@tipo", juego.Tipo);
				comando.Parameters.AddWithValue("@fechaSteamAPIComprobacion", juego.FechaSteamAPIComprobacion.ToString());
				comando.Parameters.AddWithValue("@imagenes", JsonConvert.SerializeObject(juego.Imagenes));
				comando.Parameters.AddWithValue("@precioMinimosHistoricos", JsonConvert.SerializeObject(juego.PrecioMinimosHistoricos));
				comando.Parameters.AddWithValue("@precioActualesTiendas", JsonConvert.SerializeObject(juego.PrecioActualesTiendas));
				comando.Parameters.AddWithValue("@analisis", JsonConvert.SerializeObject(juego.Analisis));
				comando.Parameters.AddWithValue("@caracteristicas", JsonConvert.SerializeObject(juego.Caracteristicas));
				comando.Parameters.AddWithValue("@media", JsonConvert.SerializeObject(juego.Media));
				comando.Parameters.AddWithValue("@nombreCodigo", Herramientas.Buscador.LimpiarNombre(juego.Nombre));

				if (string.IsNullOrEmpty(juego.Maestro) == false)
				{
					comando.Parameters.AddWithValue("@maestro", juego.Maestro);
				}					

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
