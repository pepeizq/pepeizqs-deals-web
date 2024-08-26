﻿#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BaseDatos.Juegos
{
	public static class Buscar
	{
		public static Juego Cargar(Juego juego, SqlDataReader lector)
		{
            if (lector.IsDBNull(0) == false)
			{
                juego.Id = lector.GetInt32(0);
            }

            try
            {
                if (lector.IsDBNull(1) == false)
                {
                    if (string.IsNullOrEmpty(lector.GetString(1)) == false)
                    {
                        juego.Nombre = lector.GetString(1);
                    }
                }
            }
            catch { }

            try
            {
                if (lector.IsDBNull(2) == false)
                {
                    if (string.IsNullOrEmpty(lector.GetString(2)) == false)
                    {
                        juego.Tipo = Enum.Parse<JuegoTipo>(lector.GetString(2));
                    }
                }
            }
            catch { }

            try
            {
                if (lector.IsDBNull(3) == false)
                {
                    if (string.IsNullOrEmpty(lector.GetString(3)) == false)
                    {
                        juego.Imagenes = JsonConvert.DeserializeObject<JuegoImagenes>(lector.GetString(3));
                    }
                }
            }
            catch { }

            try
            {
                if (lector.IsDBNull(4) == false)
                {
                    if (string.IsNullOrEmpty(lector.GetString(4)) == false)
                    {
                        juego.PrecioMinimosHistoricos = JsonConvert.DeserializeObject<List<JuegoPrecio>>(lector.GetString(4));
                    }
                }
            }
            catch { }

            try
            {
                if (lector.IsDBNull(5) == false)
                {
                    if (string.IsNullOrEmpty(lector.GetString(5)) == false)
                    {
                        juego.PrecioActualesTiendas = JsonConvert.DeserializeObject<List<JuegoPrecio>>(lector.GetString(5));
                    }
                }
            }
            catch { }

            try
            {
                if (lector.IsDBNull(6) == false)
                {
                    if (string.IsNullOrEmpty(lector.GetString(6)) == false)
                    {
                        juego.Analisis = JsonConvert.DeserializeObject<JuegoAnalisis>(lector.GetString(6));
                    }
                }
            }
            catch { }

            try
            {
                if (lector.IsDBNull(7) == false)
                {
                    if (string.IsNullOrEmpty(lector.GetString(7)) == false)
                    {
                        juego.Caracteristicas = JsonConvert.DeserializeObject<JuegoCaracteristicas>(lector.GetString(7));
                    }
                }
            }
            catch { }

            try
            {
                if (lector.IsDBNull(8) == false)
                {
                    if (string.IsNullOrEmpty(lector.GetString(8)) == false)
                    {
                        juego.Media = JsonConvert.DeserializeObject<JuegoMedia>(lector.GetString(8));
                    }
                }
            }
            catch { }

            if (lector.IsDBNull(9) == false)
			{
                juego.IdSteam = lector.GetInt32(9);
            }

            if (lector.IsDBNull(10) == false)
			{
                juego.IdGog = lector.GetInt32(10);
            }

            try
            {
                if (lector.IsDBNull(11) == false)
                {
                    if (string.IsNullOrEmpty(lector.GetString(11)) == false)
                    {
                        juego.FechaSteamAPIComprobacion = DateTime.Parse(lector.GetString(11));
                    }
                }
            }
            catch { }

            try
            {
                if (lector.IsDBNull(12) == false)
                {
                    if (string.IsNullOrEmpty(lector.GetString(12)) == false)
                    {
                        juego.Suscripciones = JsonConvert.DeserializeObject<List<JuegoSuscripcion>>(lector.GetString(12));
                    }
                }
            }
            catch { }

            try
            {
                if (lector.IsDBNull(13) == false)
                {
                    if (string.IsNullOrEmpty(lector.GetString(13)) == false)
                    {
                        juego.Bundles = JsonConvert.DeserializeObject<List<JuegoBundle>>(lector.GetString(13));
                    }
                }
            }
            catch { }

            try
            {
                if (lector.IsDBNull(14) == false)
                {
                    if (string.IsNullOrEmpty(lector.GetString(14)) == false)
                    {
                        juego.Gratis = JsonConvert.DeserializeObject<List<JuegoGratis>>(lector.GetString(14));
                    }
                }
            }
            catch { }

            try
            {
                if (lector.IsDBNull(15) == false)
                {
                    if (string.IsNullOrEmpty(lector.GetString(15)) == false)
                    {
                        juego.NombreCodigo = lector.GetString(15);
                    }
                }
            }
            catch { }

            try
            {
                if (lector.IsDBNull(16) == false)
                {
                    juego.IdMaestra = lector.GetInt32(16);
                }
            }
            catch { }

            try
            {
                if (lector.IsDBNull(17) == false)
                {
                    if (string.IsNullOrEmpty(lector.GetString(17)) == false)
                    {
                        juego.UsuariosInteresados = JsonConvert.DeserializeObject<List<JuegoUsuariosInteresados>>(lector.GetString(17));
                    }
                }
            }
            catch { }

            try
            {
                if (lector.IsDBNull(18) == false)
                {
                    if (string.IsNullOrEmpty(lector.GetString(18)) == false)
                    {
                        juego.SlugGOG = lector.GetString(18);
                    }
                }
            }
            catch { }

            try
            {
                if (lector.IsDBNull(19) == false)
                {
                    if (string.IsNullOrEmpty(lector.GetString(19)) == false)
                    {
                        juego.Maestro = lector.GetString(19);
                    }
                }
            }
            catch { }

            try
            {
                if (lector.IsDBNull(20) == false)
                {
                    if (string.IsNullOrEmpty(lector.GetString(20)) == false)
                    {
                        juego.FreeToPlay = lector.GetString(20);
                    }
                }
            }
            catch { }

            try
            {
                if (lector.IsDBNull(21) == false)
                {
                    if (string.IsNullOrEmpty(lector.GetString(21)) == false)
                    {
                        juego.MayorEdad = lector.GetString(21);
                    }
                }
            }
            catch { }

            try
            {
                if (lector.IsDBNull(22) == false)
                {
                    juego.UltimaModificacion = lector.GetDateTime(22);
                }
            }
            catch { }

            try
            {
                if (lector.IsDBNull(23) == false)
                {
                    if (string.IsNullOrEmpty(lector.GetString(23)) == false)
                    {
                        juego.SlugEpic = lector.GetString(23);
                    }
                }
            }
            catch { }

			try
			{
				if (lector.IsDBNull(24) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(24)) == false)
					{
						juego.Categorias = JsonConvert.DeserializeObject<List<string>>(lector.GetString(24));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(25) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(25)) == false)
					{
						juego.Generos = JsonConvert.DeserializeObject<List<string>>(lector.GetString(25));
					}
				}
			}
			catch { }

			return juego;
		}

		public static Juego UnJuego(int id)
		{
			return UnJuego(id.ToString());
		}

        public static Juego UnJuego(string id = null, string idSteam = null, string idGog = null)
		{
			string sqlBuscar = string.Empty;
			string idParametro = string.Empty;
			string idBuscar = string.Empty;

			if (id != null)
			{
				sqlBuscar = "SELECT * FROM juegos WHERE id=@id";
				idParametro = "@id";
				idBuscar = id;
			}
			else
			{
				if (idSteam != null)
				{
					sqlBuscar = "SELECT * FROM juegos WHERE idSteam=@idSteam";
					idParametro = "@idSteam";
					idBuscar = idSteam;
				}
				else
				{
					if (idGog != null)
					{
						sqlBuscar = "SELECT * FROM juegos WHERE slugGog=@slugGog";
						idParametro = "@slugGog";
						idBuscar = idGog;
					}
				}
			}

			if (sqlBuscar != string.Empty) 
			{
				SqlConnection conexion = Herramientas.BaseDatos.Conectar();

				using (conexion)
				{
					string buscar = sqlBuscar;

					using (SqlCommand comando = new SqlCommand(buscar, conexion))
					{
						comando.Parameters.AddWithValue(idParametro, idBuscar);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							if (lector.Read())
							{
								Juego juego = JuegoCrear.Generar();
								juego = Cargar(juego, lector);

								return juego;
							}
						}
					}
				}
			}	

			return null;
		}

		public static List<Juego> MultiplesJuegosSteam(List<string> ids)
		{
			List<Juego> juegos = new List<Juego>();
			string sqlBuscar = string.Empty;

			if (ids != null)
			{
				if (ids.Count > 0)
				{
					int i = 0;
					while (i < ids.Count)
					{
						if (i == 0)
						{
							sqlBuscar = "SELECT * FROM juegos WHERE idSteam=" + ids[i];
						}
						else
						{
							sqlBuscar = sqlBuscar + " OR idSteam=" + ids[i];
						}

						i += 1;
					}
				}
			}

			if (string.IsNullOrEmpty(sqlBuscar) == false)
			{
				sqlBuscar = "SET QUERY_GOVERNOR_COST_LIMIT 15000" + Environment.NewLine + sqlBuscar;

				SqlConnection conexion = Herramientas.BaseDatos.Conectar();

				using (conexion)
				{
					using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
					{
						comando.CommandTimeout = 0;

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							while (lector.Read())
							{
								Juego juego = new Juego();
								juego = Cargar(juego, lector);
								juegos.Add(juego);
							}
						}
					}
				}					
			}

			return juegos;
		}

		public static List<Juego> Nombre(string nombre, int cantidad = 30)
        {
            List<Juego> juegos = new List<Juego>();

			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				juegos = Nombre(nombre, conexion, cantidad);
            }		

			if (juegos.Count > 0)
			{
				return juegos.OrderBy(x => x.Nombre)
							 .ThenBy(x => x.Id)
							 .ToList();
            }

            return null;
        }

        public static List<Juego> Nombre(string nombre, SqlConnection conexion, int cantidad = 30)
		{
			if (conexion != null)
			{
				if (conexion.State != System.Data.ConnectionState.Open)
				{
					conexion = Herramientas.BaseDatos.Conectar();
				}
			}

			List<Juego> juegos = new List<Juego>();

			string busqueda = string.Empty;

			if (nombre.Contains(" ") == true)
			{
				if (nombre.Contains("  ") == true)
				{
					nombre = nombre.Replace("  ", " ");
				}

				string[] palabras = nombre.Split(" ");

				int i = 0;
				foreach (var palabra in palabras) 
				{
					string palabraLimpia = Herramientas.Buscador.LimpiarNombre(palabra, true);
					
					if (palabraLimpia.Length > 0)
					{
                        if (i == 0)
                        {
                            busqueda = "SELECT TOP " + cantidad + " * FROM juegos WHERE CHARINDEX('" + palabraLimpia + "', nombreCodigo) > 0 ";
                        }
                        else
                        {
                            bool buscar = true;

                            if (palabra.ToLower() == "and")
                            {
                                buscar = false;
                            }
                            else if (palabra.ToLower() == "dlc")
                            {
                                buscar = false;
                            }
                            if (palabra.ToLower() == "expansion")
                            {
                                buscar = false;
                            }

                            if (buscar == true)
                            {
                                busqueda = busqueda + " AND CHARINDEX('" + palabraLimpia + "', nombreCodigo) > 0 ";
                            }
                        }

                        i += 1;
                    }                   
				}
			}
			else
			{
				busqueda = "SELECT TOP " + cantidad + " * FROM juegos WHERE nombreCodigo LIKE '%" + Herramientas.Buscador.LimpiarNombre(nombre) + "%'";
			}

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						Juego juego = new Juego();
						juego = Cargar(juego, lector);
						juegos.Add(juego);
					}
				}
			}

            if (juegos.Count > 0)
            {
                return juegos.OrderBy(x => x.Nombre)
                             .ThenBy(x => x.Id)
                             .ToList();
            }

            return juegos;
        }

        public static List<Juego> Todos(SqlConnection conexion, string tabla = null, int dias = 0)
		{
			string tabla2 = string.Empty;

			if (tabla == null)
			{
				tabla2 = "juegos";
			}
			else
			{
				tabla2 = tabla;
			}

			List<Juego> juegos = new List<Juego>();

			string busqueda = "SELECT * FROM " + tabla2;

            if (dias > 0)
            {
                busqueda = busqueda + Environment.NewLine + "WHERE ultimaModificacion >= DATEADD(day, -" + dias.ToString() + ", GETDATE())";
            }

			string limite = "SET QUERY_GOVERNOR_COST_LIMIT 15000;";
			using (SqlCommand setQueryGovernorCommand = new SqlCommand(limite, conexion))
			{
				setQueryGovernorCommand.ExecuteNonQuery();
			}

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						Juego juego = new Juego();
						juego = Cargar(juego, lector);

						juegos.Add(juego);
					}
				}
			}

			return juegos;
		}

        public static List<Juego> Ultimos(SqlConnection conexion, string tabla, int cantidad)
        {
            List<Juego> juegos = new List<Juego>();

			string busqueda = "SELECT TOP (" + cantidad + ") * FROM " + tabla + " ORDER BY id DESC";

			using (SqlCommand comando = new SqlCommand(busqueda, conexion))
            {
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        Juego juego = new Juego();
                        juego = Cargar(juego, lector);

                        juegos.Add(juego);
                    }
                }
            }

            return juegos;
        }

		public static List<Juego> DLCs(string idMaestro = null, SqlConnection conexion = null, bool limpiarConexion = true)
		{
			List<Juego> dlcs = new List<Juego>();

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

			using (conexion)
			{
				string busqueda = null;

                if (string.IsNullOrEmpty(idMaestro) == false)
				{
					busqueda = "SELECT * FROM juegos WHERE maestro='" + idMaestro + "'";
				}
				else
				{
					busqueda = "SELECT * FROM juegos WHERE (maestro IS NULL AND tipo='1') or (maestro='no' AND tipo='1')";
				}

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							Juego dlc = new Juego();
							dlc = Cargar(dlc, lector);

							dlcs.Add(dlc);
						}
					}
				}
			}
			
			return dlcs.OrderBy(x => x.Nombre).ToList();
		}

		public static Juego Aleatorio()
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string buscar = "SELECT TOP 1 * FROM seccionMinimos ORDER BY NEWID()";

				using (SqlCommand comando = new SqlCommand(buscar, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read())
						{
							Juego juego = JuegoCrear.Generar();
							juego = Cargar(juego, lector);

							return juego;
						}
					}
				}
			}

			return null;
		}
	}
}
