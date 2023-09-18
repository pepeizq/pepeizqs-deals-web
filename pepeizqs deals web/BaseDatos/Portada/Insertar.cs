using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Noticias;

namespace BaseDatos.Portada
{
	public static class Insertar
	{
		public static void Juego(global::Juegos.Juego juego, string tabla, SqlConnection conexion)
		{
			string sqlAñadir = "INSERT INTO " + tabla + " " +
							"(idSteam, idGog, nombre, tipo, fechaSteamAPIComprobacion, imagenes, precioMinimosHistoricos, precioActualesTiendas, analisis, caracteristicas, media, nombreCodigo, idMaestra) VALUES " +
							"(@idSteam, @idGog, @nombre, @tipo, @fechaSteamAPIComprobacion, @imagenes, @precioMinimosHistoricos, @precioActualesTiendas, @analisis, @caracteristicas, @media, @nombreCodigo, @idMaestra) ";

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
				comando.Parameters.AddWithValue("@idMaestra", juego.Id);
			
				try
				{
					comando.ExecuteNonQuery();
				}
				catch
				{

				}
			}
		}

		public static void Noticia(Noticia noticia, string tabla, SqlConnection conexion)
		{
			string sqlInsertar = "INSERT INTO " + tabla + " " +
						"(noticiaTipo, enlace, juegos, fechaEmpieza, fechaTermina, bundleTipo, tituloEn, tituloEs, contenidoEn, contenidoEs, idMaestra) VALUES " +
						"(@noticiaTipo, @enlace, @juegos, @fechaEmpieza, @fechaTermina, @bundleTipo, @tituloEn, @tituloEs, @contenidoEn, @contenidoEs, @idMaestra) ";

			using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
			{
				comando.Parameters.AddWithValue("@noticiaTipo", noticia.Tipo);
				comando.Parameters.AddWithValue("@enlace", noticia.Enlace);
				comando.Parameters.AddWithValue("@juegos", noticia.Juegos);
				comando.Parameters.AddWithValue("@fechaEmpieza", noticia.FechaEmpieza.ToString());
				comando.Parameters.AddWithValue("@fechaTermina", noticia.FechaTermina.ToString());
				comando.Parameters.AddWithValue("@bundleTipo", noticia.BundleTipo);
				comando.Parameters.AddWithValue("@tituloEn", noticia.TituloEn);
				comando.Parameters.AddWithValue("@tituloEs", noticia.TituloEs);
				comando.Parameters.AddWithValue("@contenidoEn", noticia.ContenidoEn);
				comando.Parameters.AddWithValue("@contenidoEs", noticia.ContenidoEs);
				comando.Parameters.AddWithValue("@idMaestra", noticia.Id);

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
