﻿#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

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

			string añadirIdMaestra1 = null;
			string añadirIdMaestra2 = null;

			if (tabla == "seccionMinimos")
			{
				añadirIdMaestra1 = ", idMaestra";
				añadirIdMaestra2 = ", @idMaestra";
			}

			if (string.IsNullOrEmpty(juego.MayorEdad) == false)
			{
				añadirMayorEdad1 = ", mayorEdad";
				añadirMayorEdad2 = ", @mayorEdad";
			}

			string sqlAñadir = "INSERT INTO " + tabla + " " +
					"(idSteam, idGog, nombre, tipo, fechaSteamAPIComprobacion, imagenes, precioMinimosHistoricos, precioActualesTiendas, analisis, caracteristicas, media, nombreCodigo, categorias, generos" + añadirBundles1 + añadirGratis1 + añadirSuscripciones1 + añadirMaestro1 + añadirF2P1 + añadirMayorEdad1 + añadirIdMaestra1 + ") VALUES " +
					"(@idSteam, @idGog, @nombre, @tipo, @fechaSteamAPIComprobacion, @imagenes, @precioMinimosHistoricos, @precioActualesTiendas, @analisis, @caracteristicas, @media, @nombreCodigo, @categorias, @generos" + añadirBundles2 + añadirGratis2 + añadirSuscripciones2 + añadirMaestro2 + añadirF2P2 + añadirMayorEdad2 + añadirIdMaestra2 + ") ";

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
				comando.Parameters.AddWithValue("@imagenes", JsonConvert.SerializeObject(juego.Imagenes));
				comando.Parameters.AddWithValue("@precioMinimosHistoricos", JsonConvert.SerializeObject(juego.PrecioMinimosHistoricos));
				comando.Parameters.AddWithValue("@precioActualesTiendas", JsonConvert.SerializeObject(juego.PrecioActualesTiendas));
				comando.Parameters.AddWithValue("@analisis", JsonConvert.SerializeObject(juego.Analisis));
				comando.Parameters.AddWithValue("@caracteristicas", JsonConvert.SerializeObject(juego.Caracteristicas));
				comando.Parameters.AddWithValue("@media", JsonConvert.SerializeObject(juego.Media));
				comando.Parameters.AddWithValue("@nombreCodigo", Herramientas.Buscador.LimpiarNombre(juego.Nombre));
				comando.Parameters.AddWithValue("@categorias", JsonConvert.SerializeObject(juego.Categorias));
				comando.Parameters.AddWithValue("@generos", JsonConvert.SerializeObject(juego.Generos));

				if (juego.Bundles != null)
				{
					comando.Parameters.AddWithValue("@bundles", JsonConvert.SerializeObject(juego.Bundles));
				}

				if (juego.Gratis != null)
				{
					comando.Parameters.AddWithValue("@gratis", JsonConvert.SerializeObject(juego.Gratis));
				}

				if (juego.Suscripciones != null)
				{
					comando.Parameters.AddWithValue("@suscripciones", JsonConvert.SerializeObject(juego.Suscripciones));
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
	}
}
