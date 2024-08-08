#nullable disable

using Microsoft.Data.SqlClient;
using Noticias;
using System.Net;

namespace BaseDatos.Noticias
{
	public static class Insertar
	{
		public static int Ejecutar(Noticia noticia)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string añadirImagen1 = string.Empty;
				string añadirImagen2 = string.Empty;

				if (string.IsNullOrEmpty(noticia.Imagen) == false)
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

				if (string.IsNullOrEmpty(noticia.Enlace) == false)
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

				if (string.IsNullOrEmpty(noticia.Juegos) == false)
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

				string añadirBundleId1 = string.Empty;
				string añadirBundleId2 = string.Empty;

				if (noticia.BundleId > 0)
				{
					añadirBundleId1 = ", bundleId";
					añadirBundleId2 = ", @bundleId";
				}

				string sqlInsertar = "INSERT INTO noticias " +
					"(noticiaTipo" + añadirImagen1 + añadirEnlace1 + añadirJuegos1 + añadirBundle1 + añadirGratis1 + añadirSuscripcion1 + añadirBundleId1 + ", fechaEmpieza, fechaTermina, tituloEn, tituloEs, contenidoEn, contenidoEs) VALUES " +
					"(@noticiaTipo" + añadirImagen2 + añadirEnlace2 + añadirJuegos2 + añadirBundle2 + añadirGratis2 + añadirSuscripcion2 + añadirBundleId2 + ", @fechaEmpieza, @fechaTermina, @tituloEn, @tituloEs, @contenidoEn, @contenidoEs) ";

				using (SqlCommand comando = new SqlCommand(sqlInsertar, conexion))
				{
					comando.Parameters.AddWithValue("@noticiaTipo", noticia.Tipo);

					if (string.IsNullOrEmpty(noticia.Imagen) == false)
					{
						comando.Parameters.AddWithValue("@imagen", noticia.Imagen);
					}

					if (string.IsNullOrEmpty(noticia.Enlace) == false)
					{
						comando.Parameters.AddWithValue("@enlace", noticia.Enlace);
					}

					if (string.IsNullOrEmpty(noticia.Juegos) == false)
					{
						comando.Parameters.AddWithValue("@juegos", noticia.Juegos);
					}

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

					if (noticia.BundleId > 0)
					{
						comando.Parameters.AddWithValue("@bundleId", noticia.BundleId);
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

						return Buscar.Ultimo(conexion).Id;
                    }
					catch (Exception ex)
                    {
                        Errores.Insertar.Ejecutar("Portada Noticias", ex, conexion);
                    }
				}

				return 0;
			}
		}
	}
}
