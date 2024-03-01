#nullable disable

using Microsoft.Data.SqlClient;
using System.Net;

namespace BaseDatos.Noticias
{
	public static class Insertar
	{
		public static void Ejecutar(global::Noticias.Noticia noticia)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
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

				string añadirBundle1 = string.Empty;
				string añadirBundle2 = string.Empty;

				if (noticia.Tipo == global::Noticias.NoticiaTipo.Bundles)
				{
					añadirBundle1 = ", bundleTipo";
					añadirBundle2 = ", @bundleTipo";
				}

				string añadirGratis1 = string.Empty;
				string añadirGratis2 = string.Empty;

				if (noticia.Tipo == global::Noticias.NoticiaTipo.Gratis)
				{
					añadirGratis1 = ", gratisTipo";
					añadirGratis2 = ", @gratisTipo";
				}

				string añadirSuscripcion1 = string.Empty;
				string añadirSuscripcion2 = string.Empty;

				if (noticia.Tipo == global::Noticias.NoticiaTipo.Suscripciones)
				{
					añadirSuscripcion1 = ", suscripcionTipo";
					añadirSuscripcion2 = ", @suscripcionTipo";
				}

				string sqlInsertar = "INSERT INTO noticias " +
					"(noticiaTipo" + añadirImagen1 + añadirEnlace1 + añadirJuegos1 + añadirBundle1 + añadirGratis1 + añadirSuscripcion1 + ", fechaEmpieza, fechaTermina, tituloEn, tituloEs, contenidoEn, contenidoEs) VALUES " +
					"(@noticiaTipo" + añadirImagen2 + añadirEnlace2 + añadirJuegos2 + añadirBundle2 + añadirGratis2 + añadirSuscripcion2 + ", @fechaEmpieza, @fechaTermina, @tituloEn, @tituloEs, @contenidoEn, @contenidoEs) ";

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

					if (noticia.Tipo == global::Noticias.NoticiaTipo.Bundles)
					{
						comando.Parameters.AddWithValue("@bundleTipo", noticia.BundleTipo);
					}

					if (noticia.Tipo == global::Noticias.NoticiaTipo.Gratis)
					{
						comando.Parameters.AddWithValue("@gratisTipo", noticia.GratisTipo);
					}

					if (noticia.Tipo == global::Noticias.NoticiaTipo.Suscripciones)
					{
						comando.Parameters.AddWithValue("@suscripcionTipo", noticia.SuscripcionTipo);
					}

					comando.Parameters.AddWithValue("@fechaEmpieza", noticia.FechaEmpieza.ToString());
					comando.Parameters.AddWithValue("@fechaTermina", noticia.FechaTermina.ToString());
					comando.Parameters.AddWithValue("@tituloEn", WebUtility.HtmlDecode(noticia.TituloEn));
					comando.Parameters.AddWithValue("@tituloEs", WebUtility.HtmlDecode(noticia.TituloEs));
					comando.Parameters.AddWithValue("@contenidoEn", noticia.ContenidoEn);
					comando.Parameters.AddWithValue("@contenidoEs", noticia.ContenidoEs);
					
					try
					{
                        comando.ExecuteNonQuery();
                    }
					catch (Exception ex)
                    {
                        Errores.Insertar.Ejecutar("Portada Noticias", ex, conexion);
                    }
				}
			}
		}
	}
}
