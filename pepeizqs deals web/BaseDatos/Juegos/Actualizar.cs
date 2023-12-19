#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace BaseDatos.Juegos
{
	public static class Actualizar
	{
		public static void Ejecutar(Juego juego, SqlConnection conexion)
		{
			string añadirSlugGog = null;

			if (string.IsNullOrEmpty(juego.SlugGOG) == false)
			{
				añadirSlugGog = ", slugGOG=@slugGOG";
			}

			string añadirMaestro = null;

			if (string.IsNullOrEmpty(juego.Maestro) == false)
			{
				añadirMaestro = ", maestro=@maestro";
			}

			string sqlActualizar = "UPDATE juegos " +
					"SET idSteam=@idSteam, idGog=@idGog, nombre=@nombre, tipo=@tipo, fechaSteamAPIComprobacion=@fechaSteamAPIComprobacion, " +
						"imagenes=@imagenes, precioMinimosHistoricos=@precioMinimosHistoricos, precioActualesTiendas=@precioActualesTiendas, " +
						"analisis=@analisis, caracteristicas=@caracteristicas, media=@media, nombreCodigo=@nombreCodigo" + añadirSlugGog + añadirMaestro + " ";

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
				comando.Parameters.AddWithValue("@id", juego.Id);
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
				comando.Parameters.AddWithValue("@nombreCodigo", Herramientas.Buscador.LimpiarNombre(juego.Nombre));

				if (string.IsNullOrEmpty(juego.SlugGOG) == false)
				{
					comando.Parameters.AddWithValue("@slugGOG", juego.SlugGOG);
				}
				else if (string.IsNullOrEmpty(juego.Maestro) == false)
				{
					comando.Parameters.AddWithValue("@maestro", juego.Maestro);
				}

				comando.ExecuteNonQuery();
				try
				{
					
				}
				catch
				{

				}
			}
		}

		public static void UsuariosInteresados(Juego juego, SqlConnection conexion, List<JuegoUsuariosInteresados> usuariosInteresados)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET usuariosInteresados=@usuariosInteresados WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@usuariosInteresados", JsonConvert.SerializeObject(usuariosInteresados));

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void Imagenes(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET imagenes=@imagenes WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@imagenes", JsonConvert.SerializeObject(juego.Imagenes));

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void PreciosActuales(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET precioActualesTiendas=@precioActualesTiendas WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@precioActualesTiendas", JsonConvert.SerializeObject(juego.PrecioActualesTiendas));

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void PreciosHistoricos(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET precioMinimosHistoricos=@precioMinimosHistoricos WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@precioMinimosHistoricos", JsonConvert.SerializeObject(juego.PrecioMinimosHistoricos));

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void Suscripciones(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET suscripciones=@suscripciones WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@suscripciones", JsonConvert.SerializeObject(juego.Suscripciones));

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
