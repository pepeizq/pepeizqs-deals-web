﻿#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace BaseDatos.Juegos
{
	public static class Actualizar
	{
		public static void Ejecutar(Juego juego, SqlConnection conexion, bool actualizarAPI = false)
		{
            if (conexion.State != System.Data.ConnectionState.Open)
            {
                conexion = Herramientas.BaseDatos.Conectar();
            }

            string añadirSlugGog = null;

			if (string.IsNullOrEmpty(juego.SlugGOG) == false)
			{
				añadirSlugGog = ", slugGOG=@slugGOG";
			}

			if (actualizarAPI == true)
			{
				if (string.IsNullOrEmpty(juego.Maestro) == true)
				{
					juego.Maestro = "no";
				}

				if (string.IsNullOrEmpty(juego.FreeToPlay) == true)
				{
					juego.FreeToPlay = "false";
				}
			}

			string sqlActualizar = "UPDATE juegos " +
					"SET idSteam=@idSteam, idGog=@idGog, " +
						"precioMinimosHistoricos=@precioMinimosHistoricos, precioActualesTiendas=@precioActualesTiendas, " +
						"nombreCodigo=@nombreCodigo" + añadirSlugGog;

			if (actualizarAPI == true)
			{
				sqlActualizar = sqlActualizar + ", nombre=@nombre, tipo=@tipo, fechaSteamAPIComprobacion=@fechaSteamAPIComprobacion, imagenes=@imagenes, caracteristicas=@caracteristicas, media=@media, analisis=@analisis, maestro=@maestro, freeToPlay=@freeToPlay";
			}

			if (juego.IdSteam > 0)
			{
				sqlActualizar = sqlActualizar + " WHERE idSteam=@idSteam";
			}
			else
			{
				if (juego.IdGog > 0)
				{
					sqlActualizar = sqlActualizar + " WHERE idGog=@idGog";
				}
				else
				{
					sqlActualizar = sqlActualizar + " WHERE id=@id";
				}
			}

			if (sqlActualizar.Contains("WHERE id") == true)
			{
				using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
				{
					comando.Parameters.AddWithValue("@id", juego.Id);
					comando.Parameters.AddWithValue("@idSteam", juego.IdSteam);
					comando.Parameters.AddWithValue("@idGog", juego.IdGog);
					comando.Parameters.AddWithValue("@precioMinimosHistoricos", JsonConvert.SerializeObject(juego.PrecioMinimosHistoricos));
					comando.Parameters.AddWithValue("@precioActualesTiendas", JsonConvert.SerializeObject(juego.PrecioActualesTiendas));
					comando.Parameters.AddWithValue("@nombreCodigo", Herramientas.Buscador.LimpiarNombre(juego.Nombre));

					if (actualizarAPI == true)
					{
						comando.Parameters.AddWithValue("@nombre", juego.Nombre);
						comando.Parameters.AddWithValue("@tipo", juego.Tipo);
						comando.Parameters.AddWithValue("@imagenes", JsonConvert.SerializeObject(juego.Imagenes));
						comando.Parameters.AddWithValue("@fechaSteamAPIComprobacion", juego.FechaSteamAPIComprobacion.ToString("yyyy-MM-ddTHH:mm:ss.fffffff"));
						comando.Parameters.AddWithValue("@analisis", JsonConvert.SerializeObject(juego.Analisis));
						comando.Parameters.AddWithValue("@caracteristicas", JsonConvert.SerializeObject(juego.Caracteristicas));
						comando.Parameters.AddWithValue("@media", JsonConvert.SerializeObject(juego.Media));
						comando.Parameters.AddWithValue("@maestro", juego.Maestro);
						comando.Parameters.AddWithValue("@freeToPlay", juego.FreeToPlay);
					}

					if (string.IsNullOrEmpty(juego.SlugGOG) == false)
					{
						comando.Parameters.AddWithValue("@slugGOG", juego.SlugGOG);
					}
					
					try
					{
						comando.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						Errores.Insertar.Ejecutar("Actualizar Datos " + juego.Nombre, ex);
					}
				}
			}		
		}

		public static void UsuariosInteresados(int idJuego, SqlConnection conexion, List<JuegoUsuariosInteresados> usuariosInteresados)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET usuariosInteresados=@usuariosInteresados WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", idJuego);
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

		public static void Bundles(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET bundles=@bundles WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);

				if (juego.Bundles != null)
				{
					comando.Parameters.AddWithValue("@bundles", JsonConvert.SerializeObject(juego.Bundles));
				}
				else
				{
					comando.Parameters.AddWithValue("@bundles", "null");
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

		public static void Gratis(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET gratis=@gratis WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);

				if (juego.Gratis != null)
				{
					comando.Parameters.AddWithValue("@gratis", JsonConvert.SerializeObject(juego.Gratis));
				}
				else
				{
					comando.Parameters.AddWithValue("@gratis", "null");
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

		public static void DlcMaestro(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET maestro=@maestro WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@maestro", juego.Maestro);

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void FreeToPlay(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET freeToPlay=@freeToPlay WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@freeToPlay", juego.FreeToPlay);

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void MayorEdad(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET mayorEdad=@mayorEdad WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@mayorEdad", juego.MayorEdad);

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void Nombre(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET nombre=@nombre WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@nombre", juego.Nombre);

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void Media(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET nombre=@nombre, imagenes=@imagenes, caracteristicas=@caracteristicas, media=@media, nombreCodigo=@nombreCodigo WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@nombre", juego.Nombre);
				comando.Parameters.AddWithValue("@imagenes", JsonConvert.SerializeObject(juego.Imagenes));
				comando.Parameters.AddWithValue("@caracteristicas", JsonConvert.SerializeObject(juego.Caracteristicas));
				comando.Parameters.AddWithValue("@media", JsonConvert.SerializeObject(juego.Media));
				comando.Parameters.AddWithValue("@nombreCodigo", Herramientas.Buscador.LimpiarNombre(juego.Nombre));

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void Tipo(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET tipo=@tipo WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@tipo", juego.Tipo);

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void IdSteam(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET idSteam=@idSteam WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@idSteam", juego.IdSteam);

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void Deseados(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET usuariosInteresados=@usuariosInteresados WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@usuariosInteresados", JsonConvert.SerializeObject(juego.UsuariosInteresados));

				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void SlugGOG(Juego juego, SqlConnection conexion)
		{
			string sqlActualizar = "UPDATE juegos " +
					"SET slugGOG=@slugGOG WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", juego.Id);
				comando.Parameters.AddWithValue("@slugGOG", juego.SlugGOG);

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
