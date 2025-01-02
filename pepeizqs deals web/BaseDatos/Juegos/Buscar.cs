#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System.Text.Json;

namespace BaseDatos.Juegos
{
	public static class Buscar
	{
		public static Juego Cargar(Juego juego, SqlDataReader lector)
		{
            try
            {
				if (lector.IsDBNull(0) == false)
				{
					juego.Id = lector.GetInt32(0);
				}
			}
            catch { }

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
                        juego.Imagenes = JsonSerializer.Deserialize<JuegoImagenes>(lector.GetString(3));
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
                        juego.PrecioMinimosHistoricos = JsonSerializer.Deserialize<List<JuegoPrecio>>(lector.GetString(4));
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
                        juego.PrecioActualesTiendas = JsonSerializer.Deserialize<List<JuegoPrecio>>(lector.GetString(5));
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
                        juego.Analisis = JsonSerializer.Deserialize<JuegoAnalisis>(lector.GetString(6));
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
                        juego.Caracteristicas = JsonSerializer.Deserialize<JuegoCaracteristicas>(lector.GetString(7));
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
                        juego.Media = JsonSerializer.Deserialize<JuegoMedia>(lector.GetString(8));
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
                        juego.Suscripciones = JsonSerializer.Deserialize<List<JuegoSuscripcion>>(lector.GetString(12));
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
                        juego.Bundles = JsonSerializer.Deserialize<List<JuegoBundle>>(lector.GetString(13));
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
                        juego.Gratis = JsonSerializer.Deserialize<List<JuegoGratis>>(lector.GetString(14));
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
                        juego.UsuariosInteresados = JsonSerializer.Deserialize<List<JuegoUsuariosInteresados>>(lector.GetString(17));
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
						juego.Categorias = JsonSerializer.Deserialize<List<string>>(lector.GetString(24));
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
						juego.Generos = JsonSerializer.Deserialize<List<string>>(lector.GetString(25));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(26) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(26)) == false)
					{
						juego.Etiquetas = JsonSerializer.Deserialize<List<string>>(lector.GetString(26));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(27) == false)
				{
					juego.Deck = Enum.Parse<JuegoDeck>(lector.GetInt32(27).ToString());
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(28) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(28)) == false)
					{
						juego.DeckTokens = JsonSerializer.Deserialize<List<JuegoDeckToken>>(lector.GetString(28));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(29) == false)
				{
					juego.DeckComprobacion = lector.GetDateTime(29);
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(30) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(30)) == false)
					{
						juego.Historicos = JsonSerializer.Deserialize<List<JuegoHistorico>>(lector.GetString(30));
					}
				}
			}
			catch { }

            try
            {
                if (lector.IsDBNull(31) == false)
                {
                    if (string.IsNullOrEmpty(lector.GetString(31)) == false)
                    {
                        juego.GalaxyGOG = JsonSerializer.Deserialize<JuegoGalaxyGOG>(lector.GetString(31));
                    }
                }
            }
            catch { }

			try
			{
				if (lector.IsDBNull(32) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(32)) == false)
					{
						juego.CantidadJugadores = JsonSerializer.Deserialize<JuegoCantidadJugadoresSteam>(lector.GetString(32));
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(33) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(33)) == false)
					{
						juego.CuratorsSteam = JsonSerializer.Deserialize<List<string>>(lector.GetString(33));
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

		public static List<Juego> MultiplesJuegosSteam(List<string> ids, SqlConnection conexion = null)
		{
			List<Juego> juegos = new List<Juego>();
			string sqlBuscar = string.Empty;

			if (ids != null)
			{
				if (ids.Count > 0)
				{
					sqlBuscar = "SELECT * FROM juegos WHERE idSteam IN (";

					int i = 0;
					while (i < ids.Count)
					{
						if (i == 0)
						{
							sqlBuscar = sqlBuscar + "'" + ids[i] + "'";
						}
						else
						{
							sqlBuscar = sqlBuscar + ", '" + ids[i] + "'";
						}

						i += 1;
					}

					sqlBuscar = sqlBuscar + ")";
				}
			}

			if (string.IsNullOrEmpty(sqlBuscar) == false)
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

				using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
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
			}

			return juegos;
		}

		public static List<Juego> MultiplesJuegosGOG(List<string> ids, SqlConnection conexion = null)
		{
			List<Juego> juegos = new List<Juego>();
			string sqlBuscar = string.Empty;

			if (ids != null)
			{
				if (ids.Count > 0)
				{
					sqlBuscar = "SELECT * FROM juegos WHERE idGOG IN (";

					int i = 0;
					while (i < ids.Count)
					{
						if (i == 0)
						{
							sqlBuscar = sqlBuscar + "'" + ids[i] + "'";
						}
						else
						{
							sqlBuscar = sqlBuscar + ", '" + ids[i] + "'";
						}

						i += 1;
					}

					sqlBuscar = sqlBuscar + ")";
				}
			}

			if (string.IsNullOrEmpty(sqlBuscar) == false)
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

				using (SqlCommand comando = new SqlCommand(sqlBuscar, conexion))
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
			}

			return juegos;
		}

		public static List<Juego> Nombre(string nombre, int cantidad = 30, SqlConnection conexion = null)
        {
            List<Juego> juegos = new List<Juego>();

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

        public static List<Juego> Nombre(string nombre, SqlConnection conexion, int cantidad = 30, bool todo = true, int tipo = -1)
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

            List<Juego> juegos = new List<Juego>();

			string busqueda = string.Empty;
			string busquedaTodo = "*";

			if (todo == false)
			{
				busquedaTodo = "id, nombre, imagenes, precioMinimosHistoricos, precioActualesTiendas, bundles, gratis, suscripciones, tipo, analisis";
			}

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
                    if (string.IsNullOrEmpty(palabra) == false)
                    {
						string palabraLimpia = Herramientas.Buscador.LimpiarNombre(palabra, true);

						if (palabraLimpia.Length > 0)
						{
							if (i == 0)
							{
								busqueda = "SELECT TOP " + cantidad + " " + busquedaTodo + " FROM juegos WHERE CHARINDEX('" + palabraLimpia + "', nombreCodigo) > 0 ";
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
			}
			else
			{
				busqueda = "SELECT TOP " + cantidad + " " + busquedaTodo + " FROM juegos WHERE nombreCodigo LIKE '%" + Herramientas.Buscador.LimpiarNombre(nombre) + "%'";
			}

			if (tipo > -1)
			{
				busqueda = busqueda + " AND tipo = " + tipo.ToString();
			}
    
            using (SqlCommand comando = new SqlCommand(busqueda, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read())
					{
						if (todo == true)
						{
							Juego juego = new Juego();
							juego = Cargar(juego, lector);
							juegos.Add(juego);
						}
						else
						{
							Juego juego = new Juego();

							if (lector.IsDBNull(0) == false)
							{
								juego.Id = lector.GetInt32(0);
								juego.IdMaestra = lector.GetInt32(0);
							}

							if (lector.IsDBNull(1) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(1)) == false)
								{
									juego.Nombre = lector.GetString(1);
									juego.NombreCodigo = Herramientas.Buscador.LimpiarNombre(juego.Nombre);
								}
							}

							if (lector.IsDBNull(2) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(2)) == false)
								{
									juego.Imagenes = JsonSerializer.Deserialize<JuegoImagenes>(lector.GetString(2));
								}
							}

							if (lector.IsDBNull(3) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(3)) == false)
								{
									juego.PrecioMinimosHistoricos = JsonSerializer.Deserialize<List<JuegoPrecio>>(lector.GetString(3));
								}
							}

							if (lector.IsDBNull(4) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(4)) == false)
								{
									juego.PrecioActualesTiendas = JsonSerializer.Deserialize<List<JuegoPrecio>>(lector.GetString(4));
								}
							}

							if (lector.IsDBNull(5) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(5)) == false)
								{
									juego.Bundles = JsonSerializer.Deserialize<List<JuegoBundle>>(lector.GetString(5));
								}
							}

							if (lector.IsDBNull(6) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(6)) == false)
								{
									juego.Gratis = JsonSerializer.Deserialize<List<JuegoGratis>>(lector.GetString(6));
								}
							}

							if (lector.IsDBNull(7) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(7)) == false)
								{
									juego.Suscripciones = JsonSerializer.Deserialize<List<JuegoSuscripcion>>(lector.GetString(7));
								}
							}

							if (lector.IsDBNull(8) == false)
							{
								juego.Tipo = Enum.Parse<JuegoTipo>(lector.GetString(8));
							}

							if (lector.IsDBNull(9) == false)
							{
								if (string.IsNullOrEmpty(lector.GetString(9)) == false)
								{
									juego.Analisis = JsonSerializer.Deserialize<JuegoAnalisis>(lector.GetString(9));
								}
							}

							juegos.Add(juego);
						}
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

        public static List<Juego> Todos(SqlConnection conexion = null, string tabla = null, int dias = 0, bool analisis = false)
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

            List<Juego> juegos = new List<Juego>();

            using (conexion)
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

                string busqueda = "SELECT * FROM " + tabla2;
                string donde = string.Empty;

                if (dias > 0)
                {
                    if (string.IsNullOrEmpty(donde) == false)
                    {
                        donde = donde + " AND";
                    }

                    donde = donde + " ultimaModificacion >= DATEADD(day, -" + dias.ToString() + ", GETDATE())";
                }

                if (analisis == true)
                {
                    if (string.IsNullOrEmpty(donde) == false)
                    {
                        donde = donde + " AND";
                    }

                    donde = donde + " JSON_PATH_EXISTS(analisis, '$.Cantidad') > 0 AND CONVERT(bigint, REPLACE(JSON_VALUE(analisis, '$.Cantidad'),',','')) > 99";
                }

                if (string.IsNullOrEmpty(donde) == false)
                {
                    busqueda = busqueda + " WHERE" + donde;
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

		public static List<Juego> DLCs(string idMaestro = null, SqlConnection conexion = null)
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

        public static List<Juego> Filtro(List<string> ids, int cantidad, SqlConnection conexion = null)
        {
            List<Juego> resultados = new List<Juego>();

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
				List<string> etiquetas = new List<string>();
				List<string> categorias = new List<string>();
				List<string> generos = new List<string>();
				List<string> decks = new List<string>();
				List<string> sistemas = new List<string>();
				List<string> tipos = new List<string>();

				if (ids != null)
				{
                    if (ids.Count > 0)
                    {
                        foreach (var id in ids)
                        {
                            if (id.Contains("t") == true)
                            {
                                etiquetas.Add(id.Replace("t", null));
                            }

                            if (id.Contains("c") == true)
                            {
                                categorias.Add(id.Replace("c", null));
                            }

                            if (id.Contains("g") == true)
                            {
                                generos.Add(id.Replace("g", null));
                            }

                            if (id.Contains("d") == true)
                            {
                                decks.Add(id.Replace("d", null));
                            }

							if (id.Contains("s") == true)
							{
								sistemas.Add(id.Replace("s", null));
							}

							if (id.Contains("i") == true)
							{
								tipos.Add(id.Replace("i", null));
							}
						}
                    }
                }				
				
				string etiquetasTexto = string.Empty;
				
				if (etiquetas.Count > 0)
				{
                    int i = 0;

                    foreach (var etiqueta in etiquetas)
                    {
                        if (i == 0)
                        {
                            etiquetasTexto = "etiquetas LIKE '%" + Strings.ChrW(34) + etiqueta + Strings.ChrW(34) + "%'";
                        }
                        else
                        {
                            etiquetasTexto = etiquetasTexto + " OR etiquetas LIKE '%" + Strings.ChrW(34) + etiqueta + Strings.ChrW(34) + "%'";
                        }

                        i += 1;
                    }

                    if (string.IsNullOrEmpty(etiquetasTexto) == false)
                    {
                        etiquetasTexto = " AND ISJSON(etiquetas) > 0 AND (" + etiquetasTexto + ")";
                    }
                }

				string categoriasTexto = string.Empty;
				
				if (categorias.Count > 0)
				{
					int i = 0;

                    foreach (var categoria in categorias)
                    {
                        if (i == 0)
                        {
                            categoriasTexto = "categorias LIKE '%" + Strings.ChrW(34) + categoria + Strings.ChrW(34) + "%'";
                        }
                        else
                        {
                            categoriasTexto = categoriasTexto + " OR categorias LIKE '%" + Strings.ChrW(34) + categoria + Strings.ChrW(34) + "%'";
                        }

                        i += 1;
                    }

                    if (string.IsNullOrEmpty(categoriasTexto) == false)
                    {
                        categoriasTexto = "  AND ISJSON(categorias) > 0 AND (" + categoriasTexto + ")";
                    }
                }

				string generosTexto = string.Empty;              			
				
				if (generos.Count > 0)
				{
                    int i = 0;

                    foreach (var genero in generos)
                    {
                        if (i == 0)
                        {
                            generosTexto = "generos LIKE '%" + Strings.ChrW(34) + genero + Strings.ChrW(34) + "%'";
                        }
                        else
                        {
                            generosTexto = generosTexto + " OR generos LIKE '%" + Strings.ChrW(34) + genero + Strings.ChrW(34) + "%'";
                        }

                        i += 1;
                    }

                    if (string.IsNullOrEmpty(generosTexto) == false)
                    {
                        generosTexto = " AND ISJSON(generos) > 0 AND (" + generosTexto + ")";
                    }
                }

				string deckTexto = string.Empty;

				if (decks.Count > 0)
				{
                    int i = 0;

                    foreach (var deck in decks)
                    {
                        if (i == 0)
                        {
                            deckTexto = "deck = " + deck;
                        }
                        else
                        {
                            deckTexto = deckTexto + " OR deck = " + deck;
                        }

                        i += 1;
                    }

                    if (string.IsNullOrEmpty(deckTexto) == false)
                    {
                        deckTexto = " AND (" + deckTexto + ")";
                    }
                }

				string sistemasTexto = string.Empty;

				if (sistemas.Count > 0)
				{
					foreach (var sistema in sistemas)
					{
						if (string.IsNullOrEmpty(sistemasTexto) == false)
						{
							sistemasTexto = sistemasTexto + " OR ";
						}

						if (sistema == "1")
						{
							sistemasTexto = sistemasTexto + "caracteristicas LIKE '%" + Strings.ChrW(34) + "Windows" + Strings.ChrW(34) + ":true%'";
						}

						if (sistema == "2")
						{
							sistemasTexto = sistemasTexto + "caracteristicas LIKE '%" + Strings.ChrW(34) + "Mac" + Strings.ChrW(34) + ":true%'";
						}

						if (sistema == "3")
						{
							sistemasTexto = sistemasTexto + "caracteristicas LIKE '%" + Strings.ChrW(34) + "Linux" + Strings.ChrW(34) + ":true%'";
						}
					}

					if (string.IsNullOrEmpty(sistemasTexto) == false)
					{
						sistemasTexto = " AND (" + sistemasTexto + ")";
					}
				}

				string tiposTexto = string.Empty;

				if (tipos.Count > 0)
				{
					int i = 0;

					foreach (var tipo in tipos)
					{
						if (i == 0)
						{
							tiposTexto = "tipo = " + tipo;
						}
						else
						{
							tiposTexto = tiposTexto + " OR tipo = " + tipo;
						}

						i += 1;
					}

					if (string.IsNullOrEmpty(tiposTexto) == false)
					{
						tiposTexto = " AND (" + tiposTexto + ")";
					}
				}

				string busqueda = "SELECT TOP " + cantidad.ToString() + " *, CONVERT(bigint, REPLACE(JSON_VALUE(analisis, '$.Cantidad'),',','')) AS Cantidad FROM juegos " + Environment.NewLine + 
                    "WHERE ISJSON(analisis) > 0 " + etiquetasTexto + " " + categoriasTexto + " " + generosTexto + " " + deckTexto + " " + sistemasTexto + " " + tiposTexto +
					" ORDER BY Cantidad DESC";

                using (SqlCommand comando = new SqlCommand(busqueda, conexion))
                {
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Juego juego = new Juego();
                            juego = Cargar(juego, lector);

                            resultados.Add(juego);
                        }
                    }
                }
            }

            return resultados;
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

		public static List<string> Sitemap()
		{
			List<string> enlaces = new List<string>();

			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string buscar = "SELECT id, nombre FROM seccionMinimos";

				using (SqlCommand comando = new SqlCommand(buscar, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							int id = 0;
							string nombre = string.Empty;

							try
							{
								if (lector.IsDBNull(0) == false)
								{
									id = lector.GetInt32(0);
								}
							}
							catch { }

							try
							{
								if (lector.IsDBNull(1) == false)
								{
									if (string.IsNullOrEmpty(lector.GetString(1)) == false)
									{
										nombre = lector.GetString(1);
									}
								}
							}
							catch { }

							if (id > 0 && string.IsNullOrEmpty(nombre) == false)
							{
								enlaces.Add("https://pepeizqdeals.com/game/" + id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(nombre) + "/");
							}
						}
					}
				}
			}

			return enlaces;
		}
	}
}
