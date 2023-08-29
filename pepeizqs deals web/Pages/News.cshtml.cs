#nullable disable

using Juegos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Noticias;
using Suscripciones2;

namespace pepeizqs_deals_web.Pages
{
    public class NewsModel : PageModel
    {
		public string idioma = string.Empty;

		public Noticia noticia = new Noticia();

		public string imagenLogo = null;
        public string fondo = null;
		public string titulo = null;
		public string contenido = null;
		public string fechaEmpieza = null;
		public string fechaTermina = null;
		public List<Juego> juegos = new List<Juego>();

        public void OnGet()
        {
			try
			{
				idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();
			}
			catch { }		

			string id = Request.Query["id"];

			noticia = BaseDatos.Noticias.Buscar.UnaNoticia(int.Parse(id));

            if (noticia != null) 
            { 
				if (noticia.Tipo == NoticiaTipo.Suscripciones)
				{
					imagenLogo = SuscripcionesCargar.DevolverSuscripcion(noticia.SuscripcionTipo).Imagen;
				}

                if (noticia.Juegos != null)
				{
					List<string> juegos = GenerarLista(noticia.Juegos);

					if (juegos != null)
					{
						Juego juego = BaseDatos.Juegos.Buscar.UnJuego(juegos[0]);

						if (juego != null)
						{
							fondo = juego.Imagenes.Library_1920x620;
						}
					}
				}

				titulo = Herramientas.Idiomas.MirarTexto(idioma, noticia.TituloEn, noticia.TituloEs);

				if (titulo != null)
				{
					if (noticia.Tipo == NoticiaTipo.Suscripciones)
					{
						foreach (var suscripcion in SuscripcionesCargar.GenerarListado())
						{
							titulo = titulo.Replace(suscripcion.Nombre + " • ", null);
						}
					}
				}

				contenido = Herramientas.Idiomas.MirarTexto(idioma, noticia.ContenidoEn, noticia.ContenidoEs);

				if (noticia.FechaTermina.Year > 2022)
				{
					fechaEmpieza = noticia.FechaEmpieza.Day.ToString() + "/" + noticia.FechaEmpieza.Month.ToString() + "/" + noticia.FechaEmpieza.Year.ToString();
				}

				if (noticia.FechaTermina.Year > 2022)
				{
					TimeSpan diferenciaTiempo = noticia.FechaTermina.Subtract(DateTime.Now);

					if (diferenciaTiempo.Days > 1)
					{
						fechaTermina = string.Format(Herramientas.Idiomas.CogerCadena(idioma, "News.String1"), diferenciaTiempo.Days);
					}
					else if (diferenciaTiempo.Days == 1)
					{
						fechaTermina = string.Format(Herramientas.Idiomas.CogerCadena(idioma, "News.String2"), diferenciaTiempo.Days);
					}
					else if (diferenciaTiempo.Days == 0)
					{
						fechaTermina = string.Format(Herramientas.Idiomas.CogerCadena(idioma, "News.String3"), diferenciaTiempo.Days);
					}
					else if (diferenciaTiempo.Days < 0)
					{
						fechaTermina = string.Format(Herramientas.Idiomas.CogerCadena(idioma, "News.String4"), diferenciaTiempo.Days);
					}
				}

				if (noticia.Juegos != null)
				{
					List<string> listaJuegos = GenerarLista(noticia.Juegos);

					if (listaJuegos.Count > 0) 
					{ 
						foreach (var listaJuego in listaJuegos)
						{
							Juego juego = BaseDatos.Juegos.Buscar.UnJuego(listaJuego);

							if (juego != null) 
							{
								juegos.Add(juego);
							}
						}
					}
				}
            }          
		}

		private List<string> GenerarLista(string datos)
		{
			if (datos != null)
			{
				List<string> lista = new List<string>();
				string datos2 = datos;

				int i = 0;
				int j = 100000;

				while (i < j)
				{
					if (datos2.Contains(",") == true)
					{
						int int1 = datos2.IndexOf(",");

						string añadir = datos2.Remove(int1, datos2.Length - int1);

						if (añadir.Length > 0)
						{
							lista.Add(añadir);
						}

						datos2 = datos2.Remove(0, int1 + 1);
					}
					else
					{
						if (datos2.Length > 0)
						{
							lista.Add(datos2);
						}

						break;
					}

					i += 1;
				}

				if (lista.Count > 0)
				{
					return lista;
				}
			}

			return null;
		}
	}
}
