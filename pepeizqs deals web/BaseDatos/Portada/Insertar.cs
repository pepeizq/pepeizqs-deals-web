#nullable disable

using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Noticias;

namespace BaseDatos.Portada
{
	public static class Insertar
	{
		public static void Juego(global::Juegos.Juego juego, string tabla, SqlConnection conexion)
		{
			string añadirMaestro1 = null;
			string añadirMaestro2 = null;

			if (string.IsNullOrEmpty(juego.Maestro) == false)
			{
				if (juego.Maestro.Length > 1)
				{
					añadirMaestro1 = ", maestro";
					añadirMaestro2 = ", @maestro";
				}
			}

			string añadirF2P1 = null;
			string añadirF2P2 = null;

			if (string.IsNullOrEmpty(juego.FreeToPlay) == false)
			{
				añadirF2P1 = ", freeToPlay";
				añadirF2P2 = ", @freeToPlay";
			}

			string añadirMayorEdad1 = null;
			string añadirMayorEdad2 = null;

			if (string.IsNullOrEmpty(juego.MayorEdad) == false)
			{
				añadirMayorEdad1 = ", mayorEdad";
				añadirMayorEdad2 = ", @mayorEdad";
			}

			string sqlAñadir = "INSERT INTO " + tabla + " " +
							"(idSteam, idGog, nombre, tipo, fechaSteamAPIComprobacion, imagenes, precioMinimosHistoricos, precioActualesTiendas, analisis, caracteristicas, media, nombreCodigo, idMaestra" + añadirMaestro1 + añadirF2P1 + añadirMayorEdad1 + ") VALUES " +
							"(@idSteam, @idGog, @nombre, @tipo, @fechaSteamAPIComprobacion, @imagenes, @precioMinimosHistoricos, @precioActualesTiendas, @analisis, @caracteristicas, @media, @nombreCodigo, @idMaestra" + añadirMaestro2 + añadirF2P2 + añadirMayorEdad2 + ") ";

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
				comando.Parameters.AddWithValue("@idMaestra", juego.IdMaestra);
				
				if (string.IsNullOrEmpty(juego.Maestro) == false)
				{
					if (juego.Maestro.Length > 1)
					{
						comando.Parameters.AddWithValue("@maestro", juego.Maestro);
					}
				}

				if (string.IsNullOrEmpty(juego.FreeToPlay) == false)
				{
					comando.Parameters.AddWithValue("@freeToPlay", juego.FreeToPlay);
				}

				if (string.IsNullOrEmpty(juego.MayorEdad) == false)
				{
					comando.Parameters.AddWithValue("@mayorEdad", juego.MayorEdad);
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

		public static void Noticia(Noticia noticia, string tabla, SqlConnection conexion)
		{
			string añadirEnlace1 = string.Empty;
			string añadirEnlace2 = string.Empty;

			if (noticia.Enlace != null)
			{
				añadirEnlace1 = ", enlace";
				añadirEnlace2 = ", @enlace";
			}
			else
			{
				añadirEnlace1 = null;
				añadirEnlace2 = null;
			}

			string añadirBundle1 = string.Empty;
			string añadirBundle2 = string.Empty;

			if (noticia.Tipo == NoticiaTipo.Bundles)
			{
				añadirBundle1 = ", bundleTipo";
				añadirBundle2 = ", @bundleTipo";
			}

			string añadirGratis1 = string.Empty;
			string añadirGratis2 = string.Empty;

			if (noticia.Tipo == NoticiaTipo.Gratis)
			{
				añadirGratis1 = ", gratisTipo";
				añadirGratis2 = ", @gratisTipo";
			}

			string añadirSuscripcion1 = string.Empty;
			string añadirSuscripcion2 = string.Empty;

			if (noticia.Tipo == NoticiaTipo.Suscripciones)
			{
				añadirSuscripcion1 = ", suscripcionTipo";
				añadirSuscripcion2 = ", @suscripcionTipo";
			}

			string sqlInsertar = "INSERT INTO " + tabla + " " +
						"(noticiaTipo" + añadirEnlace1 + añadirBundle1 + añadirGratis1 + añadirSuscripcion1 + ", imagen, juegos, fechaEmpieza, fechaTermina, tituloEn, tituloEs, contenidoEn, contenidoEs, idMaestra) VALUES " +
						"(@noticiaTipo" + añadirEnlace2 + añadirBundle2 + añadirGratis2 + añadirSuscripcion2 + ", @imagen, @juegos, @fechaEmpieza, @fechaTermina, @tituloEn, @tituloEs, @contenidoEn, @contenidoEs, @idMaestra) ";

			using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
			{
				comando.Parameters.AddWithValue("@noticiaTipo", noticia.Tipo);

				if (noticia.Enlace != null)
				{
					comando.Parameters.AddWithValue("@enlace", noticia.Enlace);
				}

				comando.Parameters.AddWithValue("@imagen", noticia.Imagen);
				comando.Parameters.AddWithValue("@juegos", noticia.Juegos);
				comando.Parameters.AddWithValue("@fechaEmpieza", noticia.FechaEmpieza.ToString());
				comando.Parameters.AddWithValue("@fechaTermina", noticia.FechaTermina.ToString());
				comando.Parameters.AddWithValue("@tituloEn", noticia.TituloEn);
				comando.Parameters.AddWithValue("@tituloEs", noticia.TituloEs);
				comando.Parameters.AddWithValue("@contenidoEn", noticia.ContenidoEn);
				comando.Parameters.AddWithValue("@contenidoEs", noticia.ContenidoEs);
				comando.Parameters.AddWithValue("@idMaestra", noticia.Id);

				if (noticia.Tipo == NoticiaTipo.Bundles)
				{
					comando.Parameters.AddWithValue("@bundleTipo", noticia.BundleTipo);
				}

				if (noticia.Tipo == NoticiaTipo.Gratis)
				{
					comando.Parameters.AddWithValue("@gratisTipo", noticia.GratisTipo);
				}

				if (noticia.Tipo == NoticiaTipo.Suscripciones)
				{
					comando.Parameters.AddWithValue("@suscripcionTipo", noticia.SuscripcionTipo);
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
