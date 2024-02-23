#nullable disable

using BaseDatos.Juegos;
using BaseDatos.Tiendas;
using Juegos;
using Microsoft.Data.SqlClient;
using Noticias;

namespace Herramientas
{
	public class Tareas
	{
		public static async Task Portada(SqlConnection conexion)
		{
			await Task.Delay(1000);

			List<Juego> juegosDestacadosMostrar = new List<Juego>();
			List<Juego> juegosMinimosMostrar = new List<Juego>();

			List<Juego> juegos = new List<Juego>();
            List<Juego> juegosConMinimos = new List<Juego>();

            juegos = Buscar.Todos(conexion, "seccionMinimos");

			if (juegos != null)
			{
                if (juegos.Count > 0)
                {
                    foreach (var juego in juegos)
                    {
                        if (juego != null)
                        {
                            if (juego.PrecioActualesTiendas != null)
                            {
								bool borrar = true;
								
                                foreach (var oferta in juego.PrecioActualesTiendas)
								{
                                    bool fechaEncaja = JuegoFicha.CalcularAntiguedad(oferta);

                                    if (fechaEncaja == true)
									{
										borrar = false;
									}
								}

								if (borrar == true)
								{
									string sqlBorrar = "DELETE FROM seccionMinimos WHERE idMaestra='" + juego.IdMaestra + "'";

                                    using (SqlCommand comando = new SqlCommand(sqlBorrar, conexion))
									{
                                        comando.ExecuteNonQuery();
                                    }
                                }
								else
								{
									juegosConMinimos.Add(juego);
								}
                            }
                        }
                    }
                }

				if (juegosConMinimos != null)
				{
					if (juegosConMinimos.Count > 0)
					{
                        juegosConMinimos.Sort(delegate (Juego j1, Juego j2)
                        {
                            JuegoPrecio j1Oferta = null;

                            if (j1.PrecioActualesTiendas.Count > 0)
                            {
                                j1Oferta = j1.PrecioActualesTiendas[0];
                            }

                            JuegoPrecio j2Oferta = null;

                            if (j2.PrecioActualesTiendas.Count > 0)
                            {
                                j2Oferta = j2.PrecioActualesTiendas[0];
                            }

                            if (j1Oferta != null && j2Oferta != null)
                            {
                                return j2Oferta.FechaUltimoMinimo.CompareTo(j1Oferta.FechaUltimoMinimo);
                            }
                            else
                            {
                                if (j1Oferta == null && j2Oferta != null)
                                {
                                    return 1;
                                }
                                else if (j1Oferta != null && j2Oferta == null)
                                {
                                    return -1;
                                }
                            }

                            return 0;
                        });

                        #region Destacados

                        juegosDestacadosMostrar.Clear();

						int i = 0;

						foreach (var minimo in juegosConMinimos)
						{
							bool añadir = true;

							if (minimo.Analisis == null)
							{
								añadir = false;
							}
							else if (minimo.Tipo != JuegoTipo.Game)
							{
								añadir = false;
							}
							else if (minimo.Bundles != null)
							{
								añadir = false;
							}
							else if (minimo.Gratis != null)
							{
								añadir = false;
							}
							else if (minimo.Suscripciones != null)
							{
								añadir = false;
							}
							else if (string.IsNullOrEmpty(minimo.FreeToPlay) == false)
							{
								if (minimo.FreeToPlay.ToLower() == "true")
								{
									añadir = false;
								}
							}

							if (añadir == true && minimo != null)
							{
								if (minimo.Analisis != null)
								{
									if (string.IsNullOrEmpty(minimo.Analisis.Cantidad) == false)
									{
										string tempCantidad = minimo.Analisis.Cantidad;
										tempCantidad = tempCantidad.Replace(".", null);
										tempCantidad = tempCantidad.Replace(",", null);

										if (Convert.ToInt32(tempCantidad) >= 5000)
										{
											if (i < 60)
											{
												juegosDestacadosMostrar.Add(minimo);
												i += 1;
											}
										}
									}
								}
							}
						}

						if (juegosDestacadosMostrar.Count > 0)
						{
							global::BaseDatos.Portada.Limpiar.Ejecutar("portadaJuegosDestacados", conexion);

							foreach (var juego in juegosDestacadosMostrar)
							{
								global::BaseDatos.Portada.Insertar.Juego(juego, "portadaJuegosDestacados", conexion);
							}
						}

						#endregion

						#region Minimos

						juegosMinimosMostrar.Clear();

						int j = 0;
						while (juegosMinimosMostrar.Count < 100)
						{
							bool añadir = true;

                            if (juegosDestacadosMostrar.Count > 0)
                            {
                                foreach (var destacado in juegosDestacadosMostrar)
                                {
                                    if (destacado.Id == juegosConMinimos[j].Id)
                                    {
                                        añadir = false;
                                    }
                                }
                            }

                            if (juegosConMinimos[j] != null)
							{
								if (juegosConMinimos[j].Analisis == null)
								{
									añadir = false;
								}
								else
								{
									if (string.IsNullOrEmpty(juegosConMinimos[j].Analisis.Cantidad) == false)
									{
										string tempCantidad = juegosConMinimos[j].Analisis.Cantidad;
										tempCantidad = tempCantidad.Replace(".", null);
										tempCantidad = tempCantidad.Replace(",", null);

										if (Convert.ToInt32(tempCantidad) < 500)
										{
											añadir = false;
										}
									}
									else
									{
										añadir = false;
									}
								}

								if (juegosConMinimos[j].Gratis != null)
								{
									if (juegosConMinimos[j].Gratis.Count > 0)
									{
										foreach (var gratis in juegosConMinimos[j].Gratis)
										{
											if (DateTime.Now >= gratis.FechaEmpieza && DateTime.Now <= gratis.FechaTermina)
											{
												añadir = false;
											}
										}
									}
								}

								if (string.IsNullOrEmpty(juegosConMinimos[j].FreeToPlay) == false)
								{
									if (juegosConMinimos[j].FreeToPlay.ToLower() == "true")
									{
										añadir = false;
									}
								}

								if (añadir == true)
								{
									juegosMinimosMostrar.Add(juegosConMinimos[j]);
								}
							}

							j += 1;
						}

						if (juegosMinimosMostrar.Count > 0)
						{
							global::BaseDatos.Portada.Limpiar.Ejecutar("portadaJuegosMinimos", conexion);

							foreach (var juego in juegosMinimosMostrar)
							{
								global::BaseDatos.Portada.Insertar.Juego(juego, "portadaJuegosMinimos", conexion);
							}
						}

						if (juegosConMinimos.Count > 0)
						{
							global::BaseDatos.Portada.Limpiar.Ejecutar("seccionMinimos", conexion);

							foreach (var juego in juegosConMinimos)
							{
								global::BaseDatos.Portada.Insertar.Juego(juego, "seccionMinimos", conexion);
							}
						}

						#endregion
					}
				}
			}
		}

        public static async Task Noticias(SqlConnection conexion)
		{
            await Task.Delay(1000);

            List<Noticia> noticiasMostrar = new List<Noticia>();
            List<Noticia> noticiaEvento = new List<Noticia>();

            List<Noticia> noticias = global::BaseDatos.Noticias.Buscar.Todas().OrderBy(x => x.FechaEmpieza).Reverse().ToList();

            if (noticias.Count > 0)
            {
                int i = 0;
                foreach (var noticia in noticias)
                {
                    if (DateTime.Now >= noticia.FechaEmpieza && DateTime.Now <= noticia.FechaTermina)
                    {
                        if (noticia.Tipo == NoticiaTipo.Eventos && noticiaEvento.Count == 0)
                        {
                            DateTime fechaEncabezado = noticia.FechaEmpieza;
                            fechaEncabezado = fechaEncabezado.AddDays(3);

                            if (DateTime.Now < fechaEncabezado)
                            {
                                noticiaEvento.Add(noticia);
                            }
                        }

                        if (i < 6)
                        {
                            noticiasMostrar.Add(noticia);
                            i += 1;
                        }
                    }
                }
            }

            if (noticiasMostrar.Count > 0)
            {
                global::BaseDatos.Portada.Limpiar.Ejecutar("portadaNoticias", conexion);

                foreach (var noticia in noticiasMostrar)
                {
                    global::BaseDatos.Portada.Insertar.Noticia(noticia, "portadaNoticias", conexion);
                }
            }

            if (noticiaEvento.Count > 0)
            {
                global::BaseDatos.Portada.Limpiar.Ejecutar("portadaNoticiasEvento", conexion);

                foreach (var noticia in noticiaEvento)
                {
                    global::BaseDatos.Portada.Insertar.Noticia(noticia, "portadaNoticiasEvento", conexion);
                }
            }
        }

        public async static Task Tiendas(SqlConnection conexion, IDecompiladores decompilador)
		{
			TimeSpan tiempoSiguiente = TimeSpan.FromMinutes(120);
			List<string> ids = new List<string>();

			foreach (var tienda in Tiendas2.TiendasCargar.GenerarListado())
			{
				if (tienda.AdminInteractuar == true)
				{
					ids.Add(tienda.Id);
				}				
			}

			if (Admin.TiendasComprobarUso(conexion, TimeSpan.FromSeconds(30)) == false)
			{
				AdminTiendas tiendaComprobar = Admin.TiendaSiguiente(conexion);

				DateTime ultimaComprobacion = tiendaComprobar.fecha;

				if ((DateTime.Now - ultimaComprobacion) > tiempoSiguiente)
				{
					try
					{
						await Tiendas2.TiendasCargar.TareasGestionador(conexion, tiendaComprobar.tienda, decompilador);
					}
					catch (Exception ex) 
					{
						global::BaseDatos.Errores.Insertar.Ejecutar(tiendaComprobar.tienda, ex);
					}	
				}
			}
		}

		public async static Task Divisas(SqlConnection conexion)
		{
			TimeSpan tiempo = TimeSpan.FromDays(1);

			Divisa dolar = global::BaseDatos.Divisas.Buscar.Ejecutar(conexion, "USD");

			DateTime ultimaComprobacion = dolar.FechaActualizacion;

			if ((DateTime.Now - ultimaComprobacion) > tiempo)
			{
				await Herramientas.Divisas.ActualizarDatos(conexion);
			}			
		}

        public async static Task Sorteos(SqlConnection conexion)
		{
            List<Sorteos2.Sorteo> listaSorteos = global::BaseDatos.Sorteos.Buscar.Todos();

			if (listaSorteos != null)
			{
				if (listaSorteos.Count > 0)
				{
					foreach (var sorteo in listaSorteos)
					{
						if (DateTime.Now > sorteo.FechaTermina && string.IsNullOrEmpty(sorteo.GanadorId) == true)
						{
							if (sorteo.Participantes != null)
							{
								if (sorteo.Participantes.Count > 0)
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
