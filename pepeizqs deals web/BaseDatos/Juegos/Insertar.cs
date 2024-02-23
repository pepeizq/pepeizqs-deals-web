#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace BaseDatos.Juegos
{
	public static class Insertar
	{
		public static void Ejecutar(Juego juego, SqlConnection conexion, string tabla = "juegos")
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

			string añadirIdMaestra1 = null;
			string añadirIdMaestra2 = null;

			if (tabla == "seccionMinimos")
			{
				añadirIdMaestra1 = ", idMaestra";
				añadirIdMaestra2 = ", @idMaestra";
			}

			string sqlAñadir = "INSERT INTO " + tabla + " " +
					"(idSteam, idGog, nombre, tipo, fechaSteamAPIComprobacion, imagenes, precioMinimosHistoricos, precioActualesTiendas, analisis, caracteristicas, media, nombreCodigo" + añadirMaestro1 + añadirF2P1 + añadirMayorEdad1 + añadirIdMaestra1 + ") VALUES " +
					"(@idSteam, @idGog, @nombre, @tipo, @fechaSteamAPIComprobacion, @imagenes, @precioMinimosHistoricos, @precioActualesTiendas, @analisis, @caracteristicas, @media, @nombreCodigo" + añadirMaestro2 + añadirF2P2 + añadirMayorEdad2 + añadirIdMaestra2 + ") ";

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
