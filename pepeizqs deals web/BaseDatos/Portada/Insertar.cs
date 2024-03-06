#nullable disable

using Microsoft.Data.SqlClient;
using Noticias;

namespace BaseDatos.Portada
{
	public static class Insertar
	{
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
