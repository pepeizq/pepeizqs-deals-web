#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

            string añadirSlugEpic = null;

            if (string.IsNullOrEmpty(juego.SlugEpic) == false)
            {
                añadirSlugEpic = ", slugEpic=@slugEpic";
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

			string añadirUltimaModificacion = null;

            if (juego.UltimaModificacion != null)
            {
                añadirUltimaModificacion = ", ultimaModificacion=@ultimaModificacion";
            }

			string añadirEtiquetas = null;

			if (juego.Etiquetas != null)
			{
				if (juego.Etiquetas.Count > 0)
				{
					añadirEtiquetas = ", etiquetas=@etiquetas";
				}
			}

			string sqlActualizar = "UPDATE juegos " +
					"SET idSteam=@idSteam, idGog=@idGog, analisis=@analisis, " +
						"precioMinimosHistoricos=@precioMinimosHistoricos, precioActualesTiendas=@precioActualesTiendas, " +
                        "nombreCodigo=@nombreCodigo" + añadirUltimaModificacion + añadirEtiquetas + añadirSlugGog + añadirSlugEpic;

			if (actualizarAPI == true)
			{
				sqlActualizar = sqlActualizar + ", nombre=@nombre, tipo=@tipo, fechaSteamAPIComprobacion=@fechaSteamAPIComprobacion, imagenes=@imagenes, caracteristicas=@caracteristicas, media=@media, analisis=@analisis, maestro=@maestro, freeToPlay=@freeToPlay, categorias=@categorias, generos=@generos";
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
					comando.Parameters.AddWithValue("@analisis", JsonConvert.SerializeObject(juego.Analisis));
					comando.Parameters.AddWithValue("@nombreCodigo", Herramientas.Buscador.LimpiarNombre(juego.Nombre));

                    if (juego.UltimaModificacion != null)
                    {
                        comando.Parameters.AddWithValue("@ultimaModificacion", juego.UltimaModificacion);
                    }

					if (juego.Etiquetas != null)
					{
						if (juego.Etiquetas.Count > 0)
						{
							comando.Parameters.AddWithValue("@etiquetas", JsonConvert.SerializeObject(juego.Etiquetas));
						}
					}

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
						comando.Parameters.AddWithValue("@categorias", JsonConvert.SerializeObject(juego.Categorias));
						comando.Parameters.AddWithValue("@generos", JsonConvert.SerializeObject(juego.Generos));						
					}

					if (string.IsNullOrEmpty(juego.SlugGOG) == false)
					{
						comando.Parameters.AddWithValue("@slugGOG", juego.SlugGOG);
					}

                    if (string.IsNullOrEmpty(juego.SlugEpic) == false)
                    {
                        comando.Parameters.AddWithValue("@slugEpic", juego.SlugEpic);
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

		public static void Comprobacion(int id, List<JuegoPrecio> ofertasActuales, List<JuegoPrecio> ofertasHistoricas, SqlConnection conexion = null, string slugGOG = null, string idGOG = null, string slugEpic = null, DateTime? ultimaModificacion = null)
		{
			if (conexion == null)
			{
				conexion = Herramientas.BaseDatos.Conectar();
			}
			else
			{
				if (conexion.State != System.Data.ConnectionState.Open)
				{
					conexion = Herramientas.BaseDatos.Conectar();
				}
			}

			string añadirSlugGog = null;

			if (string.IsNullOrEmpty(slugGOG) == false)
			{
				añadirSlugGog = ", idGog=@idGog, slugGOG=@slugGOG";
			}

			string añadirSlugEpic = null;

			if (string.IsNullOrEmpty(slugEpic) == false)
			{
				añadirSlugEpic = ", slugEpic=@slugEpic";
			}

			string añadirUltimaModificacion = null;

			if (ultimaModificacion != null)
			{
				añadirUltimaModificacion = ", ultimaModificacion=@ultimaModificacion";
			}

			string sqlActualizar = "UPDATE juegos " +
					"SET precioMinimosHistoricos=@precioMinimosHistoricos, precioActualesTiendas=@precioActualesTiendas" +
					añadirUltimaModificacion + añadirSlugGog + añadirSlugEpic + " WHERE id=@id";

			using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
			{
				comando.Parameters.AddWithValue("@id", id);
				comando.Parameters.AddWithValue("@precioMinimosHistoricos", JsonConvert.SerializeObject(ofertasHistoricas));
				comando.Parameters.AddWithValue("@precioActualesTiendas", JsonConvert.SerializeObject(ofertasActuales));

				if (ultimaModificacion != null)
				{
					comando.Parameters.AddWithValue("@ultimaModificacion", ultimaModificacion);
				}

				if (string.IsNullOrEmpty(slugGOG) == false)
				{
					comando.Parameters.AddWithValue("@idGog", idGOG);
					comando.Parameters.AddWithValue("@slugGOG", slugGOG);
				}

				if (string.IsNullOrEmpty(slugEpic) == false)
				{
					comando.Parameters.AddWithValue("@slugEpic", slugEpic);
				}

				try
				{
					comando.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					Errores.Insertar.Ejecutar("Actualizar Datos " + BaseDatos.Juegos.Buscar.UnJuego(id).Nombre, ex);
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

		public static void Media(Juego nuevoJuego, Juego juego, SqlConnection conexion = null)
		{
			if (juego.Imagenes.Library_600x900.Contains("i.imgur.com") == false)
			{
				juego.Imagenes.Library_600x900 = nuevoJuego.Imagenes.Library_600x900;
			}

			if (juego.Imagenes.Library_1920x620.Contains("i.imgur.com") == false)
			{
				juego.Imagenes.Library_1920x620 = nuevoJuego.Imagenes.Library_1920x620;
			}

			if (juego.Imagenes.Logo.Contains("i.imgur.com") == false)
			{
				juego.Imagenes.Logo = nuevoJuego.Imagenes.Logo;
			}

			juego.Imagenes.Capsule_231x87 = nuevoJuego.Imagenes.Capsule_231x87;
			juego.Imagenes.Header_460x215 = nuevoJuego.Imagenes.Header_460x215;

			juego.Nombre = nuevoJuego.Nombre;
			juego.Caracteristicas.Descripcion = nuevoJuego.Caracteristicas.Descripcion;
			juego.Caracteristicas.Windows = nuevoJuego.Caracteristicas.Windows;
			juego.Caracteristicas.Mac = nuevoJuego.Caracteristicas.Mac;
			juego.Caracteristicas.Linux = nuevoJuego.Caracteristicas.Linux;

			List<string> desarrolladores = new List<string>();

			if (nuevoJuego.Caracteristicas.Desarrolladores != null)
			{
				if (nuevoJuego.Caracteristicas.Desarrolladores.Count > 0)
				{
					foreach (var desarrollador in nuevoJuego.Caracteristicas.Desarrolladores)
					{
						desarrolladores.Add(desarrollador.Trim());
					}
				}
			}

			juego.Caracteristicas.Desarrolladores = desarrolladores;

			List<string> publishers = new List<string>();

			if (nuevoJuego.Caracteristicas.Publishers != null)
			{
				if (nuevoJuego.Caracteristicas.Publishers.Count > 0)
				{
					foreach (var publisher in nuevoJuego.Caracteristicas.Publishers)
					{
						publishers.Add(publisher.Trim());
					}
				}
			}

			juego.Caracteristicas.Publishers = publishers;

			juego.Media = nuevoJuego.Media;
			juego.Categorias = nuevoJuego.Categorias;
			juego.Generos = nuevoJuego.Generos;

			if (conexion == null)
			{
				conexion = Herramientas.BaseDatos.Conectar();
			}

			using (conexion)
			{
				string sqlActualizar = "UPDATE juegos " +
									"SET nombre=@nombre, imagenes=@imagenes, caracteristicas=@caracteristicas, media=@media, nombreCodigo=@nombreCodigo, categorias=@categorias, generos=@generos, fechaSteamAPIComprobacion=@fechaSteamAPIComprobacion WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
				{
					comando.Parameters.AddWithValue("@id", juego.Id);
					comando.Parameters.AddWithValue("@nombre", juego.Nombre);
					comando.Parameters.AddWithValue("@imagenes", JsonConvert.SerializeObject(juego.Imagenes));
					comando.Parameters.AddWithValue("@caracteristicas", JsonConvert.SerializeObject(juego.Caracteristicas));
					comando.Parameters.AddWithValue("@media", JsonConvert.SerializeObject(juego.Media));
					comando.Parameters.AddWithValue("@nombreCodigo", Herramientas.Buscador.LimpiarNombre(juego.Nombre));
					comando.Parameters.AddWithValue("@categorias", JsonConvert.SerializeObject(juego.Categorias));
					comando.Parameters.AddWithValue("@generos", JsonConvert.SerializeObject(juego.Generos));
					comando.Parameters.AddWithValue("@fechaSteamAPIComprobacion", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffff"));

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
