﻿#nullable disable

using Juegos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;

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
                }
			}

			return juego;
		}

		public static void Resto(JuegoPrecio oferta, ViewDataDictionary objeto, SqlConnection conexion, string idGog = null, string slugGOG = null)
		{
			//Buscar en tabla tienda
			List<int> listaIds = new List<int>();
			string buscarTienda = "SELECT * FROM tienda" + oferta.Tienda + " WHERE enlace=@enlace";

            using (SqlCommand comandoBuscar = new SqlCommand(buscarTienda, conexion))
			{
                comandoBuscar.Parameters.AddWithValue("@enlace", oferta.Enlace);

                using (SqlDataReader lector = comandoBuscar.ExecuteReader())
				{
					if (lector.Read() == true)
					{
						string tempIds = lector.GetString(3);

						if (string.IsNullOrEmpty(tempIds) == false)
						{
							int i = 0;
							while (i < 100)
							{
								if (tempIds.Contains(",") == true)
								{
									int int1 = tempIds.IndexOf(",");
									string temp1 = tempIds.Remove(int1, tempIds.Length - int1);

									listaIds.Add(int.Parse(temp1));

									tempIds = tempIds.Remove(0, int1 + 1);
								}
								else
								{
									listaIds.Add(int.Parse(tempIds));
									break;
								}

								i += 1;
							}
						}						
					}
				}
			}

			//Insertar en tabla tienda o actualizar juego
			if (listaIds.Count == 0)
			{
				int idBuscarJuego = 0;

				string buscarNombre = "SELECT * FROM juegos WHERE nombreCodigo=@nombreCodigo";

				using (SqlCommand comandoBuscar2 = new SqlCommand(buscarNombre, conexion))
				{
					comandoBuscar2.Parameters.AddWithValue("@nombreCodigo", Herramientas.Buscador.LimpiarNombre(oferta.Nombre));

					using (SqlDataReader lector = comandoBuscar2.ExecuteReader())
					{
						if (lector.Read() == true)
						{
							idBuscarJuego = lector.GetInt32(0);
						}
					}
				}

				string sqlAñadir = "INSERT INTO tienda" + oferta.Tienda + " " +
					"(enlace, nombre, imagen, idJuegos, descartado) VALUES " +
					"(@enlace, @nombre, @imagen, @idJuegos, @descartado)";

				using (SqlCommand comandoInsertar = new SqlCommand(sqlAñadir, conexion))
				{
					comandoInsertar.Parameters.AddWithValue("@enlace", oferta.Enlace);
					comandoInsertar.Parameters.AddWithValue("@nombre", oferta.Nombre);
					comandoInsertar.Parameters.AddWithValue("@imagen", oferta.Imagen);
					comandoInsertar.Parameters.AddWithValue("@idJuegos", idBuscarJuego);
					comandoInsertar.Parameters.AddWithValue("@descartado", "no");

					try
					{
						comandoInsertar.ExecuteNonQuery();
					}
					catch
					{

					}
				}
			}
			else if (listaIds.Count > 0)
			{
				foreach (int id in listaIds)
				{
					if (id > 0)
					{
						string buscarJuego = "SELECT * FROM juegos WHERE id=@id";

						using (SqlCommand comandoBuscar3 = new SqlCommand(buscarJuego, conexion))
						{
							comandoBuscar3.Parameters.AddWithValue("@id", id);

							using (SqlDataReader lector = comandoBuscar3.ExecuteReader())
							{
								if (lector.Read() == true)
								{
                                    Juego juego = JuegoCrear.Generar();

									juego = Juegos.Buscar.Cargar(juego, lector);

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

									if (juego.IdGog == 0)
									{
										if (idGog != null)
										{
											juego.IdGog = int.Parse(idGog);
											juego.SlugGOG = slugGOG;
										}
									}
									else
									{
										if (slugGOG != null)
										{
											juego.SlugGOG = slugGOG;
										}
									}

									Juegos.Precios.Actualizar(juego, oferta, conexion, false);
								}
							}
						}
					}
					else
					{
						string buscarNombre = "SELECT * FROM juegos WHERE nombreCodigo=@nombreCodigo";

						SqlCommand comandoBuscar2 = new SqlCommand(buscarNombre, conexion);

						using (comandoBuscar2)
						{
							comandoBuscar2.Parameters.AddWithValue("@nombreCodigo", Herramientas.Buscador.LimpiarNombre(oferta.Nombre));

							using (SqlDataReader lector = comandoBuscar2.ExecuteReader())
							{
								if (lector.Read() == true)
								{
									int idBuscarJuego2 = lector.GetInt32(0);

									string sqlActualizar = "UPDATE tienda" + oferta.Tienda + " " +
											"SET idJuegos=@idJuegos WHERE enlace=@enlace";

									using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
									{
										comando.Parameters.AddWithValue("@idJuegos", idBuscarJuego2);
										comando.Parameters.AddWithValue("@enlace", oferta.Enlace);

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
						}
					}
				}
			}
		}
	}
}
