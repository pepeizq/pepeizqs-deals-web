#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace Herramientas
{
	public interface ITareasGestionador
	{
		public void PortadaTarea();
		public void TiendasTarea();
	}

	public class TareasGestionador : ITareasGestionador
	{
		public void PortadaTarea()
		{
			List<Juego> juegosDestacadosMostrar = new List<Juego>();
			List<Juego> juegosMinimosMostrar = new List<Juego>();
			List<Juego> juegosAñadidosMostrar = new List<Juego>();

			SqlConnection conexion = BaseDatos.Conectar();

			using (conexion)
			{
				List<Juego> juegos = new List<Juego>();

				juegos = global::BaseDatos.Juegos.Buscar.Todos(conexion);

				//----------------------------------------------------------

				List<Juego> juegosConMinimos = global::BaseDatos.Juegos.Precios.DevolverMinimos(juegos);

				if (juegosConMinimos != null)
				{
					if (juegosConMinimos.Count > 0)
					{
						juegosDestacadosMostrar.Clear();
						juegosMinimosMostrar.Clear();

						int i = 0;

						foreach (var minimo in juegosConMinimos)
						{
							if (i < 6)
							{
								if (minimo.Analisis != null)
								{
									if (minimo.Analisis.Cantidad.Length >= 6)
									{
										juegosDestacadosMostrar.Add(minimo);
										i += 1;
									}
								}
							}
							else
							{
								break;
							}
						}

						for (int j = 0; juegosMinimosMostrar.Count < 10; j += 1)
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

							if (juegosConMinimos[j].Analisis == null)
							{
								añadir = false;
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

							if (añadir == true)
							{
								juegosMinimosMostrar.Add(juegosConMinimos[j]);
							}
						}
					}
				}

				if (juegosDestacadosMostrar.Count > 0)
				{
					global::BaseDatos.Portada.Limpiar.Ejecutar("portadaJuegosDestacados", conexion);
					

					foreach (var juego in juegosDestacadosMostrar)
					{
						string sqlAñadir = "INSERT INTO portadaJuegosDestacados " +
							"(idSteam, idGog, nombre, tipo, fechaSteamAPIComprobacion, imagenes, precioMinimosHistoricos, precioActualesTiendas, analisis, caracteristicas, media, nombreCodigo) VALUES " +
							"(@idSteam, @idGog, @nombre, @tipo, @fechaSteamAPIComprobacion, @imagenes, @precioMinimosHistoricos, @precioActualesTiendas, @analisis, @caracteristicas, @media, @nombreCodigo) ";

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

				if (juegosMinimosMostrar.Count > 0)
				{
					global::BaseDatos.Portada.Limpiar.Ejecutar("portadaJuegosMinimos", conexion);


					foreach (var juego in juegosMinimosMostrar)
					{
						string sqlAñadir = "INSERT INTO portadaJuegosMinimos " +
							"(idSteam, idGog, nombre, tipo, fechaSteamAPIComprobacion, imagenes, precioMinimosHistoricos, precioActualesTiendas, analisis, caracteristicas, media, nombreCodigo) VALUES " +
							"(@idSteam, @idGog, @nombre, @tipo, @fechaSteamAPIComprobacion, @imagenes, @precioMinimosHistoricos, @precioActualesTiendas, @analisis, @caracteristicas, @media, @nombreCodigo) ";

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

				//----------------------------------------------------------

				if (juegos.Count > 0)
				{
					juegos = juegos.OrderBy(x => x.Id).Reverse().ToList();

					int i = 0;
					foreach (Juego juego in juegos)
					{
						if (i < 10)
						{
							juegosAñadidosMostrar.Add(juego);
						}

						i += 1;
					}
				}

				if (juegosAñadidosMostrar.Count > 0)
				{
					global::BaseDatos.Portada.Limpiar.Ejecutar("portadaJuegosAnadidos", conexion);

					foreach (var juego in juegosAñadidosMostrar)
					{
						string sqlAñadir = "INSERT INTO portadaJuegosAnadidos " +
							"(idSteam, idGog, nombre, tipo, fechaSteamAPIComprobacion, imagenes, precioMinimosHistoricos, precioActualesTiendas, analisis, caracteristicas, media, nombreCodigo) VALUES " +
							"(@idSteam, @idGog, @nombre, @tipo, @fechaSteamAPIComprobacion, @imagenes, @precioMinimosHistoricos, @precioActualesTiendas, @analisis, @caracteristicas, @media, @nombreCodigo) ";

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

		public void TiendasTarea()
		{
			Tiendas2.TiendasCargar.TareasGestionador(TimeSpan.FromMinutes(30));
		}
	}
}
