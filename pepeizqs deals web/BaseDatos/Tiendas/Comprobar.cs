#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace BaseDatos.Tiendas
{
	public static class Comprobar
	{
		public static async void Steam(JuegoPrecio oferta, JuegoAnalisis analisis, SqlConnection conexion = null, int deck = 0)
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

			string idSteam2 = APIs.Steam.Juego.LimpiarID(oferta.Enlace);

			if (string.IsNullOrEmpty(idSteam2) == false)
			{
				idSteam2 = APIs.Steam.Tienda.IdsEspeciales(idSteam2);

				int idSteam = 0;

				try
				{
					idSteam = int.Parse(idSteam2);
				}
				catch
				{

				}

				if (idSteam > 0)
				{
					Juego juego = JuegoCrear.Generar();

					if (analisis != null && juego != null)
					{
						if (string.IsNullOrEmpty(analisis.Cantidad) == false && string.IsNullOrEmpty(analisis.Porcentaje) == false)
						{
							JuegoAnalisis nuevoAnalisis = new JuegoAnalisis();
							nuevoAnalisis.Cantidad = analisis.Cantidad;
							nuevoAnalisis.Porcentaje = analisis.Porcentaje;

							juego.Analisis = nuevoAnalisis;
						}
					}

					string buscarJuego = "SELECT id, precioMinimosHistoricos, precioActualesTiendas, usuariosInteresados, idSteam, historicos, fechaSteamAPIComprobacion FROM juegos WHERE idSteam=@idSteam";

					using (SqlCommand comando = new SqlCommand(buscarJuego, conexion))
					{
						comando.Parameters.AddWithValue("@idSteam", idSteam);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							if (lector.Read() == true)
							{
								juego.IdSteam = idSteam;

								if (string.IsNullOrEmpty(lector.GetString(6)) == false)
								{
									bool actualizarAPI = false;

									DateTime fechaComprobacion = DateTime.Parse(lector.GetString(6));

									if (DateTime.Now.Subtract(fechaComprobacion) > TimeSpan.FromDays(91))
									{
										actualizarAPI = true;
									}

									if (actualizarAPI == true)
									{
										ActualizarDatosSteamAPI(juego, oferta, analisis, deck, conexion);
									}
									else
									{
										int id = 0;
										if (lector.IsDBNull(0) == false)
										{
											id = lector.GetInt32(0);
										}

										List<JuegoPrecio> ofertasHistoricas = new List<JuegoPrecio>();
										if (lector.IsDBNull(1) == false)
										{
											if (string.IsNullOrEmpty(lector.GetString(1)) == false)
											{
												ofertasHistoricas = JsonSerializer.Deserialize<List<JuegoPrecio>>(lector.GetString(1));
											}
										}

										if (ofertasHistoricas == null)
										{
											ofertasHistoricas = new List<JuegoPrecio>();
										}

										if (ofertasHistoricas.Count == 0)
										{
											ofertasHistoricas.Add(oferta);
										}

										List<JuegoPrecio> ofertasActuales = new List<JuegoPrecio>();
										if (lector.IsDBNull(2) == false)
										{
											if (string.IsNullOrEmpty(lector.GetString(2)) == false)
											{
												ofertasActuales = JsonSerializer.Deserialize<List<JuegoPrecio>>(lector.GetString(2));
											}
										}

										if (ofertasActuales == null)
										{
											ofertasActuales = new List<JuegoPrecio>();
										}

										if (ofertasActuales.Count == 0)
										{
											ofertasActuales.Add(oferta);
										}

										List<JuegoUsuariosInteresados> usuariosInteresados = new List<JuegoUsuariosInteresados>();
										if (lector.IsDBNull(3) == false)
										{
											if (string.IsNullOrEmpty(lector.GetString(3)) == false)
											{
												usuariosInteresados = JsonSerializer.Deserialize<List<JuegoUsuariosInteresados>>(lector.GetString(3));
											}
										}

										if (lector.IsDBNull(4) == false)
										{
											idSteam = lector.GetInt32(4);
										}

										List<JuegoHistorico> historicos = new List<JuegoHistorico>();
										if (lector.IsDBNull(5) == false)
										{
											if (string.IsNullOrEmpty(lector.GetString(5)) == false)
											{
												historicos = JsonSerializer.Deserialize<List<JuegoHistorico>>(lector.GetString(5));
											}
										}

										if (id > 0)
										{
											Juegos.Precios.Actualizar(id, idSteam, ofertasActuales, ofertasHistoricas, historicos, oferta, conexion, null, null, null, usuariosInteresados, juego.Analisis);
										}
									}
								}
							}
							else
							{
								try
								{
									juego = await APIs.Steam.Juego.CargarDatosJuego(idSteam2);
								}
								catch
								{

								}

								if (juego != null)
								{
									if (juego.PrecioActualesTiendas == null)
									{
										juego.PrecioActualesTiendas = new List<JuegoPrecio>();
										juego.PrecioMinimosHistoricos = new List<JuegoPrecio>();
									}

									if (juego.PrecioActualesTiendas.Count == 0)
									{
										juego.PrecioActualesTiendas.Add(oferta);
										juego.PrecioMinimosHistoricos.Add(oferta);
									}

									Juegos.Insertar.Ejecutar(juego, conexion);
								}
							}
						}
					}
				}
			}
		}

		private static async void ActualizarDatosSteamAPI(Juego juego, JuegoPrecio oferta, JuegoAnalisis analisis, int deck = 0, SqlConnection conexion = null)
		{
			if (juego.IdSteam > 0)
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

				string buscarJuego = "SELECT * FROM juegos WHERE idSteam=@idSteam";

				using (SqlCommand comando = new SqlCommand(buscarJuego, conexion))
				{
					comando.Parameters.AddWithValue("@idSteam", juego.IdSteam);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							juego = Juegos.Buscar.Cargar(juego, lector);

							Juego nuevoJuego = null;

							try
							{
								nuevoJuego = await APIs.Steam.Juego.CargarDatosJuego(juego.IdSteam.ToString());
							}
							catch (Exception ex)
							{
								BaseDatos.Errores.Insertar.Mensaje("API Steam", ex);
							}

							if (nuevoJuego != null)
							{
								juego.Nombre = nuevoJuego.Nombre;
								juego.Caracteristicas = nuevoJuego.Caracteristicas;
								juego.Media = nuevoJuego.Media;

								if (juego.Tipo == JuegoTipo.DLC)
								{
									if (string.IsNullOrEmpty(juego.Maestro) == true)
									{
										juego.Maestro = nuevoJuego.Maestro;
									}
								}

								juego.FechaSteamAPIComprobacion = DateTime.Now;
								juego.Categorias = nuevoJuego.Categorias;

								if (nuevoJuego.Analisis != null)
								{
									if (string.IsNullOrEmpty(nuevoJuego.Analisis.Porcentaje) == false && string.IsNullOrEmpty(nuevoJuego.Analisis.Cantidad) == false)
									{
										JuegoAnalisis reseñas = new JuegoAnalisis
										{
											Cantidad = nuevoJuego.Analisis.Cantidad,
											Porcentaje = nuevoJuego.Analisis.Porcentaje
										};

										juego.Analisis = reseñas;
									}
								}
							}

							juego.Etiquetas = nuevoJuego.Etiquetas;

							if (deck > 0)
							{
								juego.Deck = Enum.Parse<JuegoDeck>(deck.ToString());
							}

							Juegos.Precios.Steam(juego, oferta, conexion, true);
						}
					}
				}
			}
		}

		public static void Resto(JuegoPrecio oferta, SqlConnection conexion, string idGog = null, string slugGOG = null, string slugEpic = null)
		{
			bool encontrado = false;

			string buscarJuegos = @"DECLARE @ids NVARCHAR(MAX); 

				SET @ids = (SELECT idJuegos FROM tienda@oferta.Tienda WHERE enlace='@oferta.Enlace' AND descartado='no'); 

				IF @ids IS NOT NULL BEGIN
				IF @ids != '0' BEGIN
				DECLARE @pos INT; 
				DECLARE @nextpos INT; 
				DECLARE @valuelen INT; 
				DECLARE @tabla TABLE (numero int NOT NULL); 

				SELECT @pos = 0, @nextpos = 1; 

				WHILE @nextpos > 0
				BEGIN
					SELECT @nextpos = charindex(',', @ids, @pos + 1)
					SELECT @valuelen = CASE WHEN @nextpos > 0
											THEN @nextpos
											ELSE len(@ids) + 1
										END - @pos - 1
					INSERT @tabla (numero)
						VALUES (convert(int, substring(@ids, @pos + 1, @valuelen)))
					SELECT @pos = @nextpos;
				END

				SELECT id, precioMinimosHistoricos, precioActualesTiendas, usuariosInteresados, idSteam, historicos, analisis FROM juegos WHERE id IN (SELECT numero FROM @tabla);
				END;
				END;";

			oferta.Tienda = oferta.Tienda.Replace("'", null);
			oferta.Enlace = oferta.Enlace.Replace("'", null);

			buscarJuegos = buscarJuegos.Replace("@oferta.Tienda", oferta.Tienda);
			buscarJuegos = buscarJuegos.Replace("@oferta.Enlace", oferta.Enlace);

            using (SqlCommand comandoBuscar = new SqlCommand(buscarJuegos, conexion))
			{
				using (SqlDataReader lector = comandoBuscar.ExecuteReader())
				{
					while (lector.Read() == true)
					{
						encontrado = true;

						int id = 0;
						if (lector.IsDBNull(0) == false)
						{
							id = lector.GetInt32(0);
						}

						List<JuegoPrecio> ofertasHistoricas = new List<JuegoPrecio>();
						if (lector.IsDBNull(1) == false)
						{
							if (string.IsNullOrEmpty(lector.GetString(1)) == false)
							{
								ofertasHistoricas = JsonSerializer.Deserialize<List<JuegoPrecio>>(lector.GetString(1));
							}
						}
						
						if (ofertasHistoricas == null)
						{
							ofertasHistoricas = new List<JuegoPrecio>();							
						}

						if (ofertasHistoricas.Count == 0)
						{
							ofertasHistoricas.Add(oferta);
						}

						List<JuegoPrecio> ofertasActuales = new List<JuegoPrecio>();
						if (lector.IsDBNull(2) == false)
						{
							if (string.IsNullOrEmpty(lector.GetString(2)) == false)
							{
								ofertasActuales = JsonSerializer.Deserialize<List<JuegoPrecio>>(lector.GetString(2));
							}
						}

                        if (ofertasActuales == null)
                        {
                            ofertasActuales = new List<JuegoPrecio>();
                        }

                        if (ofertasActuales.Count == 0)
						{
							ofertasActuales.Add(oferta);
						}

						List<JuegoUsuariosInteresados> usuariosInteresados = new List<JuegoUsuariosInteresados>();
						if (lector.IsDBNull(3) == false)
						{
							if (string.IsNullOrEmpty(lector.GetString(3)) == false)
							{
								usuariosInteresados = JsonSerializer.Deserialize<List<JuegoUsuariosInteresados>>(lector.GetString(3));
							}
						}

						int idSteam = 0;
						if (lector.IsDBNull(4) == false)
						{
							idSteam = lector.GetInt32(4);
						}

						List<JuegoHistorico> historicos = new List<JuegoHistorico>();
						if (lector.IsDBNull(5) == false)
						{
							if (string.IsNullOrEmpty(lector.GetString(5)) == false)
							{
								historicos = JsonSerializer.Deserialize<List<JuegoHistorico>>(lector.GetString(5));
							}
						}

						JuegoAnalisis analisis = null;
						if (lector.IsDBNull(6) == false)
						{
							if (string.IsNullOrEmpty(lector.GetString(6)) == false)
							{
								if (lector.GetString(6) != "null")
								{
									analisis = JsonSerializer.Deserialize<JuegoAnalisis>(lector.GetString(6));
								}
							}
						}

						if (id > 0)
						{
							Juegos.Precios.Actualizar(id, idSteam, ofertasActuales, ofertasHistoricas, historicos, oferta, conexion, slugGOG, idGog, slugEpic, usuariosInteresados, analisis);
						}
					}
				}
			}

			if (encontrado == false)
			{
				string buscarId = @"IF NOT EXISTS (SELECT * from tienda@oferta.tienda WHERE enlace = '@oferta.enlace') BEGIN

					DECLARE @nuevaId NVARCHAR(MAX); 

					SET @nuevaId = (SELECT id FROM juegos WHERE nombreCodigo='oferta.nombreCodigo'); 

					IF @nuevaId IS NULL
					BEGIN 
					SET @nuevaId = 0;
					END; 

					INSERT INTO tienda@oferta.tienda 
					(enlace, nombre, imagen, idJuegos, descartado) VALUES 
					('@oferta.enlace', '@oferta.nombre', '@oferta.imagen', @nuevaId, 'no'); 

					END;";

				buscarId = buscarId.Replace("@oferta.nombreCodigo", Herramientas.Buscador.LimpiarNombre(oferta.Nombre));
				buscarId = buscarId.Replace("@oferta.enlace", oferta.Enlace);
				buscarId = buscarId.Replace("@oferta.tienda", oferta.Tienda);
				buscarId = buscarId.Replace("@oferta.nombre", oferta.Nombre.Replace("'", "''"));
				buscarId = buscarId.Replace("@oferta.imagen", oferta.Imagen);

				using (SqlCommand comandoInsertar = new SqlCommand(buscarId, conexion))
				{
					try
					{
						comandoInsertar.ExecuteReader();
					}
					catch (Exception ex)
					{
						Errores.Insertar.Mensaje("Insertar Tienda: " + oferta?.Enlace, ex);
					}
				}
			}
		}
	}
}
