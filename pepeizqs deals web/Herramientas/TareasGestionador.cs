#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using Noticias;

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
						global::BaseDatos.Portada.Insertar.Juego(juego, "portadaJuegosDestacados", conexion);
					}
				}

				if (juegosMinimosMostrar.Count > 0)
				{
					global::BaseDatos.Portada.Limpiar.Ejecutar("portadaJuegosMinimos", conexion);

					foreach (var juego in juegosMinimosMostrar)
					{
						global::BaseDatos.Portada.Insertar.Juego(juego, "portadaJuegosMinimos", conexion);
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
						global::BaseDatos.Portada.Insertar.Juego(juego, "portadaJuegosAnadidos", conexion);
					}
				}

				//----------------------------------------------------------

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
							if (i < 10)
							{
								if (noticia.Tipo == NoticiaTipo.Eventos && noticiaEvento.Count == 0)
								{
									DateTime fechaEncabezado = noticia.FechaEmpieza;
									fechaEncabezado = fechaEncabezado.AddDays(3);

									if (DateTime.Now < fechaEncabezado)
									{
										noticiaEvento.Add(noticia);
									}
									else
									{
										noticiasMostrar.Add(noticia);
									}
								}
								else
								{
									noticiasMostrar.Add(noticia);
								}
							}

							i += 1;
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
		}

		public void TiendasTarea()
		{
			Tiendas2.TiendasCargar.TareasGestionador(TimeSpan.FromMinutes(30));
		}
	}
}
