#nullable disable

using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace BaseDatos.Tiendas
{
	public static class Comprobar
	{
		public static async void Steam(JuegoPrecio oferta, JuegoAnalisis analisis, ViewDataDictionary objeto, SqlConnection conexion)
		{
			Juego juego = JuegoCrear.Generar();

			bool insertar = false;
			bool actualizar = false;

			string idSteam = string.Empty;

			if (oferta.Enlace.Contains("https://store.steampowered.com/app/") == true)
			{
				idSteam = APIs.Steam.Juego.LimpiarID(oferta.Enlace);
			}

			if (string.IsNullOrEmpty(idSteam) == false)
			{
				int numeroId = 0;

				try
				{
					numeroId = int.Parse(idSteam);
				}
				catch
				{

				}

				if (numeroId > 0)
				{
					string buscarJuego = "SELECT * FROM juegos WHERE idSteam=@idSteam";

					using (SqlCommand comando = new SqlCommand(buscarJuego, conexion))
					{
						comando.Parameters.AddWithValue("@idSteam", idSteam);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							if (lector.Read() == false)
							{
								try
								{
									juego = await APIs.Steam.Juego.CargarDatos(idSteam);
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

									insertar = true;
								}
								else
								{
									insertar = false;
								}
							}
							else
							{
								juego = Juegos.Buscar.Cargar(juego, lector);

								actualizar = true;
							}
						}
					}

					if (juego.Id == 38208)
					{
						BaseDatos.Errores.Insertar.Mensaje(juego.Nombre, JsonConvert.SerializeObject(juego.Analisis));
					}

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

					if (insertar == true && actualizar == false)
					{
						bool insertar2 = true;
						string buscarJuego2 = "SELECT * FROM juegos WHERE idSteam=@idSteam";

						if (conexion.State == System.Data.ConnectionState.Closed)
						{
							conexion = Herramientas.BaseDatos.Conectar();
						}

						using (SqlCommand comando = new SqlCommand(buscarJuego2, conexion))
						{
							comando.Parameters.AddWithValue("@idSteam", juego.IdSteam);

							using (SqlDataReader lector = comando.ExecuteReader())
							{
								if (lector.Read() == true)
								{
									insertar2 = false;
								}
							}
						}

						if (insertar2 == true)
						{
							Juegos.Insertar.Ejecutar(juego, conexion);
						}					
					}

					if (actualizar == true && insertar == false)
					{
						bool actualizarAPI = false;

						DateTime fechaComprobacion = Convert.ToDateTime(juego.FechaSteamAPIComprobacion);
						fechaComprobacion = fechaComprobacion.AddDays(35);

						if (fechaComprobacion < DateTime.Now)
						{
							juego = await ActualizarDatosAPI(juego);
						}

						Juegos.Precios.Actualizar(juego, oferta, conexion, actualizarAPI);
					}
				}
			}
		}

		private static async Task<Juego> ActualizarDatosAPI(Juego juego)
		{
			if (juego.IdSteam > 0)
			{
                Juego nuevoJuego = null;
				
				try
				{
                    nuevoJuego = await APIs.Steam.Juego.CargarDatos(juego.IdSteam.ToString());
                }
				catch { }
				
				if (nuevoJuego != null)
				{
                    juego.Nombre = nuevoJuego.Nombre;
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
					juego.Generos = nuevoJuego.Generos;
                }
			}

			return juego;
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

				SELECT id, precioMinimosHistoricos, precioActualesTiendas, usuariosInteresados, idSteam FROM juegos WHERE id IN (SELECT numero FROM @tabla);
				END;
				END;";

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
								ofertasHistoricas = JsonConvert.DeserializeObject<List<JuegoPrecio>>(lector.GetString(1));
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
								ofertasActuales = JsonConvert.DeserializeObject<List<JuegoPrecio>>(lector.GetString(2));
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
								usuariosInteresados = JsonConvert.DeserializeObject<List<JuegoUsuariosInteresados>>(lector.GetString(3));
							}
						}

						int idSteam = 0;
						if (lector.IsDBNull(4) == false)
						{
							idSteam = lector.GetInt32(4);
						}

						if (id > 0)
						{
							Juegos.Precios.Actualizar(id, idSteam, ofertasActuales, ofertasHistoricas, oferta, conexion, slugGOG, idGog, slugEpic, usuariosInteresados);
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
						Errores.Insertar.Ejecutar("Insertar Tienda", ex);
					}
				}
			}
		}
	}
}
