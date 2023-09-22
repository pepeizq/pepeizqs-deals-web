#nullable disable

using Microsoft.Data.SqlClient;

namespace BaseDatos.Noticias
{
	public static class Insertar
	{
		public static void Ejecutar(global::Noticias.Noticia noticia)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				if (noticia.Tipo == global::Noticias.NoticiaTipo.Bundles)
				{
					string sqlInsertar = "INSERT INTO noticias " +
						"(noticiaTipo, enlace, juegos, fechaEmpieza, fechaTermina, bundleTipo, tituloEn, tituloEs, contenidoEn, contenidoEs) VALUES " +
						"(@noticiaTipo, @enlace, @juegos, @fechaEmpieza, @fechaTermina, @bundleTipo, @tituloEn, @tituloEs, @contenidoEn, @contenidoEs) ";

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

						try
						{
							comando.ExecuteNonQuery();
						}
						catch
						{

						}
					}
				}
				else if (noticia.Tipo == global::Noticias.NoticiaTipo.Gratis)
				{
					string sqlInsertar = "INSERT INTO noticias " +
						"(noticiaTipo, enlace, juegos, fechaEmpieza, fechaTermina, gratisTipo, tituloEn, tituloEs, contenidoEn, contenidoEs) VALUES " +
						"(@noticiaTipo, @enlace, @juegos, @fechaEmpieza, @fechaTermina, @gratisTipo, @tituloEn, @tituloEs, @contenidoEn, @contenidoEs) ";

					using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
					{
						comando.Parameters.AddWithValue("@noticiaTipo", noticia.Tipo);
						comando.Parameters.AddWithValue("@enlace", noticia.Enlace);
						comando.Parameters.AddWithValue("@juegos", noticia.Juegos);
						comando.Parameters.AddWithValue("@fechaEmpieza", noticia.FechaEmpieza.ToString());
						comando.Parameters.AddWithValue("@fechaTermina", noticia.FechaTermina.ToString());
						comando.Parameters.AddWithValue("@gratisTipo", noticia.GratisTipo);
						comando.Parameters.AddWithValue("@tituloEn", noticia.TituloEn);
						comando.Parameters.AddWithValue("@tituloEs", noticia.TituloEs);
						comando.Parameters.AddWithValue("@contenidoEn", noticia.ContenidoEn);
						comando.Parameters.AddWithValue("@contenidoEs", noticia.ContenidoEs);

						try
						{
							comando.ExecuteNonQuery();
						}
						catch
						{

						}
					}
				}
				else if (noticia.Tipo == global::Noticias.NoticiaTipo.Suscripciones)
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

					string sqlInsertar = "INSERT INTO noticias " +
						"(noticiaTipo" + añadirEnlace1 + ", juegos, fechaEmpieza, fechaTermina, suscripcionTipo, tituloEn, tituloEs, contenidoEn, contenidoEs) VALUES " +
						"(@noticiaTipo" + añadirEnlace2 + ", @juegos, @fechaEmpieza, @fechaTermina, @suscripcionTipo, @tituloEn, @tituloEs, @contenidoEn, @contenidoEs) ";

					using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
					{
						comando.Parameters.AddWithValue("@noticiaTipo", noticia.Tipo);

						if (noticia.Enlace != null)
						{
							comando.Parameters.AddWithValue("@enlace", noticia.Enlace);
						}
						
						comando.Parameters.AddWithValue("@juegos", noticia.Juegos);
						comando.Parameters.AddWithValue("@fechaEmpieza", noticia.FechaEmpieza.ToString());
						comando.Parameters.AddWithValue("@fechaTermina", noticia.FechaTermina.ToString());
						comando.Parameters.AddWithValue("@suscripcionTipo", noticia.SuscripcionTipo);
						comando.Parameters.AddWithValue("@tituloEn", noticia.TituloEn);
						comando.Parameters.AddWithValue("@tituloEs", noticia.TituloEs);
						comando.Parameters.AddWithValue("@contenidoEn", noticia.ContenidoEn);
						comando.Parameters.AddWithValue("@contenidoEs", noticia.ContenidoEs);
						comando.ExecuteNonQuery();
						try
						{
							
						}
						catch
						{

						}
					}
				}
				else
				{
					string añadirImagen1 = string.Empty;
					string añadirImagen2 = string.Empty;

					if (noticia.Imagen != null)
					{
						añadirImagen1 = ", imagen";
						añadirImagen2 = ", @imagen";
					}
					else
					{
						añadirImagen1 = null;
						añadirImagen2 = null;
					}

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

					string añadirJuegos1 = string.Empty;
					string añadirJuegos2 = string.Empty;

					if (noticia.Juegos != null)
					{
						añadirJuegos1 = ", juegos";
						añadirJuegos2 = ", @juegos";
					}
					else
					{
						añadirJuegos1 = null;
						añadirJuegos2 = null;
					}

					string sqlInsertar = "INSERT INTO noticias " +
						"(noticiaTipo" + añadirImagen1 + añadirEnlace1 + añadirJuegos1 + ", fechaEmpieza, fechaTermina, tituloEn, tituloEs, contenidoEn, contenidoEs) VALUES " +
						"(@noticiaTipo" + añadirImagen2 + añadirEnlace2 + añadirJuegos2 + ", @fechaEmpieza, @fechaTermina, @tituloEn, @tituloEs, @contenidoEn, @contenidoEs) ";

					using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
					{
						comando.Parameters.AddWithValue("@noticiaTipo", noticia.Tipo);

						if (noticia.Imagen != null)
						{
							comando.Parameters.AddWithValue("@imagen", noticia.Imagen);
						}
						
						if (noticia.Enlace != null)
						{
							comando.Parameters.AddWithValue("@enlace", noticia.Enlace);
						}

						if (noticia.Juegos != null)
						{
							comando.Parameters.AddWithValue("@juegos", noticia.Juegos);
						}

						comando.Parameters.AddWithValue("@fechaEmpieza", noticia.FechaEmpieza.ToString());
						comando.Parameters.AddWithValue("@fechaTermina", noticia.FechaTermina.ToString());
						comando.Parameters.AddWithValue("@tituloEn", noticia.TituloEn);
						comando.Parameters.AddWithValue("@tituloEs", noticia.TituloEs);
						comando.Parameters.AddWithValue("@contenidoEn", noticia.ContenidoEn);
						comando.Parameters.AddWithValue("@contenidoEs", noticia.ContenidoEs);
						
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

			conexion.Dispose();
		}
	}
}
