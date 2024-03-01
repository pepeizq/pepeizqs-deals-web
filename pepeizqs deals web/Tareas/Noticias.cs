#nullable disable

using Microsoft.Data.SqlClient;
using Noticias;

namespace Tareas
{
	public class Noticias
	{
		public async Task Ejecutar(SqlConnection conexion)
		{
			await Task.Delay(1000);

			List<Noticia> noticiasMostrar = new List<Noticia>();
			List<Noticia> noticiaEvento = new List<Noticia>();

			List<Noticia> noticias = BaseDatos.Noticias.Buscar.Todas().OrderBy(x => x.FechaEmpieza).Reverse().ToList();

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
				BaseDatos.Portada.Limpiar.Ejecutar("portadaNoticias", conexion);

				foreach (var noticia in noticiasMostrar)
				{
					BaseDatos.Portada.Insertar.Noticia(noticia, "portadaNoticias", conexion);
				}
			}

			if (noticiaEvento.Count > 0)
			{
				BaseDatos.Portada.Limpiar.Ejecutar("portadaNoticiasEvento", conexion);

				foreach (var noticia in noticiaEvento)
				{
					BaseDatos.Portada.Insertar.Noticia(noticia, "portadaNoticiasEvento", conexion);
				}
			}
		}
	}
}
