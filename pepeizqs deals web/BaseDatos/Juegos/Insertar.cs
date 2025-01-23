#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace BaseDatos.Juegos
{
	public static class Insertar
	{
		public static void Ejecutar(Juego juego, SqlConnection conexion = null, string tabla = "juegos", bool noExiste = false)
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

            string añadirBundles1 = null;
			string añadirBundles2 = null;

			if (juego.Bundles != null)
			{
				añadirBundles1 = ", bundles";
				añadirBundles2 = ", @bundles";
			}

			string añadirGratis1 = null;
			string añadirGratis2 = null;

			if (juego.Gratis != null)
			{
				añadirGratis1 = ", gratis";
				añadirGratis2 = ", @gratis";
			}

			string añadirSuscripciones1 = null;
			string añadirSuscripciones2 = null;

			if (juego.Suscripciones != null)
			{
				añadirSuscripciones1 = ", suscripciones";
				añadirSuscripciones2 = ", @suscripciones";
			}

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

			string añadirEtiquetas1 = null;
			string añadirEtiquetas2 = null;

			if (juego.Etiquetas != null)
			{
				if (juego.Etiquetas.Count > 0)
				{
					añadirEtiquetas1 = ", etiquetas";
					añadirEtiquetas2 = ", @etiquetas";
				}
			}

			string añadirDeck1 = null;
			string añadirDeck2 = null;

			if (juego.Deck != JuegoDeck.Desconocido)
			{
				añadirDeck1 = ", deck";
				añadirDeck2 = ", @deck";
			}

			string añadirIdMaestra1 = null;
			string añadirIdMaestra2 = null;

			if (tabla == "seccionMinimos")
			{
				añadirIdMaestra1 = ", idMaestra";
				añadirIdMaestra2 = ", @idMaestra";
			}

			string sqlAñadir = "INSERT INTO " + tabla + " " +
					"(idSteam, idGog, nombre, tipo, fechaSteamAPIComprobacion, imagenes, precioMinimosHistoricos, precioActualesTiendas, analisis, caracteristicas, media, nombreCodigo, categorias, generos" + añadirBundles1 + añadirGratis1 + añadirSuscripciones1 + añadirMaestro1 + añadirF2P1 + añadirMayorEdad1 + añadirEtiquetas1 + añadirDeck1 + añadirIdMaestra1 + ") VALUES " +
					"(@idSteam, @idGog, @nombre, @tipo, @fechaSteamAPIComprobacion, @imagenes, @precioMinimosHistoricos, @precioActualesTiendas, @analisis, @caracteristicas, @media, @nombreCodigo, @categorias, @generos" + añadirBundles2 + añadirGratis2 + añadirSuscripciones2 + añadirMaestro2 + añadirF2P2 + añadirMayorEdad2 + añadirEtiquetas2 + añadirDeck2 + añadirIdMaestra2 + ") ";

			if (noExiste == true)
			{
				sqlAñadir = "IF EXISTS (SELECT 1 FROM " + tabla + " WHERE JSON_VALUE(precioMinimosHistoricos, '$[0].Enlace') != '" + juego.PrecioMinimosHistoricos[0].Enlace + "' AND idMaestra=" + juego.IdMaestra + " AND JSON_VALUE(precioMinimosHistoricos, '$[0].DRM') = " + (int)juego.PrecioMinimosHistoricos[0].DRM + ") BEGIN DELETE FROM seccionMinimos WHERE (idMaestra=" + juego.IdMaestra + " AND JSON_VALUE(precioMinimosHistoricos, '$[0].DRM') = " + (int)juego.PrecioMinimosHistoricos[0].DRM + ") END IF NOT EXISTS (SELECT 1 FROM " + tabla + " WHERE JSON_VALUE(precioMinimosHistoricos, '$[0].Enlace') = '" + juego.PrecioMinimosHistoricos[0].Enlace + "' AND idMaestra=" + juego.IdMaestra + ") BEGIN " + sqlAñadir + " END"; 
			}

			using (SqlCommand comando = new SqlCommand(sqlAñadir, conexion))
			{
				comando.Parameters.AddWithValue("@idSteam", juego.IdSteam);
				comando.Parameters.AddWithValue("@idGog", juego.IdGog);
				comando.Parameters.AddWithValue("@nombre", juego.Nombre);
				comando.Parameters.AddWithValue("@tipo", juego.Tipo);
				comando.Parameters.AddWithValue("@fechaSteamAPIComprobacion", juego.FechaSteamAPIComprobacion.ToString());
				comando.Parameters.AddWithValue("@imagenes", JsonSerializer.Serialize(juego.Imagenes));
				comando.Parameters.AddWithValue("@precioMinimosHistoricos", JsonSerializer.Serialize(juego.PrecioMinimosHistoricos));
				comando.Parameters.AddWithValue("@precioActualesTiendas", JsonSerializer.Serialize(juego.PrecioActualesTiendas));
				comando.Parameters.AddWithValue("@analisis", JsonSerializer.Serialize(juego.Analisis));
				comando.Parameters.AddWithValue("@caracteristicas", JsonSerializer.Serialize(juego.Caracteristicas));
				comando.Parameters.AddWithValue("@media", JsonSerializer.Serialize(juego.Media));
				comando.Parameters.AddWithValue("@nombreCodigo", Herramientas.Buscador.LimpiarNombre(juego.Nombre));
				comando.Parameters.AddWithValue("@categorias", JsonSerializer.Serialize(juego.Categorias));
				comando.Parameters.AddWithValue("@generos", JsonSerializer.Serialize(juego.Generos));

				if (juego.Bundles != null)
				{
					comando.Parameters.AddWithValue("@bundles", JsonSerializer.Serialize(juego.Bundles));
				}

				if (juego.Gratis != null)
				{
					comando.Parameters.AddWithValue("@gratis", JsonSerializer.Serialize(juego.Gratis));
				}

				if (juego.Suscripciones != null)
				{
					comando.Parameters.AddWithValue("@suscripciones", JsonSerializer.Serialize(juego.Suscripciones));
				}

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

				if (juego.Etiquetas != null)
				{
					if (juego.Etiquetas.Count > 0)
					{
						comando.Parameters.AddWithValue("@etiquetas", JsonSerializer.Serialize(juego.Etiquetas));
					}
				}

				if (juego.Deck != JuegoDeck.Desconocido)
				{
					comando.Parameters.AddWithValue("@deck", juego.Deck);
				}

				if (tabla == "seccionMinimos")
				{
					comando.Parameters.AddWithValue("@idMaestra", juego.IdMaestra);
				}

				try
				{
					comando.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					Errores.Insertar.Mensaje("Añadir juego " + juego.Nombre, ex);
				}				
			}
		}

		public static async Task<string> GogReferencia(string idJuego, SqlConnection conexion = null)
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

			bool sePuedeInsertar = true;
			string buscar = "SELECT * FROM gogReferencias WHERE idJuego=@idJuego";

			using (SqlCommand comando = new SqlCommand(buscar, conexion))
			{
				comando.Parameters.AddWithValue("@idJuego", idJuego);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					if (lector.Read() == true)
					{
						if (lector.IsDBNull(1) == false)
						{
							sePuedeInsertar = false;
							return lector.GetInt32(1).ToString();
						}
					}
				}
			}

			if (sePuedeInsertar == true)
			{
				string idReferencia = await APIs.GOG.Juego.BuscarReferencia(idJuego);

				string insertar = "INSERT INTO gogReferencias (idJuego, idReferencia) VALUES (@idJuego, @idReferencia)";

				using (SqlCommand comando = new SqlCommand(insertar, conexion))
				{
					comando.Parameters.AddWithValue("@idJuego", idJuego);
					comando.Parameters.AddWithValue("@idReferencia", idReferencia);

					try
					{
						comando.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						Errores.Insertar.Mensaje("Añadir GOG Referencia " + idJuego, ex);
					}
				}

				return idReferencia;
			}
			
			return idJuego;
		}
	}
}
