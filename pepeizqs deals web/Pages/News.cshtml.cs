#nullable disable

using Bundles2;
using Gratis2;
using Juegos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
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
		public string enlace = null;
		public string mensajeEnlace = null;
		public string contenido = null;
		public string video = null;
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

            if (id != null)
            {
				noticia = BaseDatos.Noticias.Buscar.UnaNoticia(int.Parse(id));

				if (noticia != null)
				{
                    if (noticia.Tipo == NoticiaTipo.Bundles)
                    {
						if (noticia.Enlace != null)
						{
							enlace = noticia.Enlace;
							mensajeEnlace = Herramientas.Idiomas.CogerCadena(idioma, "News.String7");
						}

                        imagenLogo = BundlesCargar.DevolverBundle(noticia.BundleTipo).ImagenTienda;
                    }
                    else if (noticia.Tipo == NoticiaTipo.Gratis)
					{
						if (noticia.Enlace != null)
						{
							enlace = noticia.Enlace;
							mensajeEnlace = Herramientas.Idiomas.CogerCadena(idioma, "News.String8");
						}

						imagenLogo = GratisCargar.DevolverGratis(noticia.GratisTipo).Imagen;
					}
					else if (noticia.Tipo == NoticiaTipo.Suscripciones)
					{
						if (noticia.Enlace != null)
						{
							enlace = noticia.Enlace;
							mensajeEnlace = Herramientas.Idiomas.CogerCadena(idioma, "News.String9");
						}

						imagenLogo = SuscripcionesCargar.DevolverSuscripcion(noticia.SuscripcionTipo).Imagen;
					}

					if (noticia.Juegos != null)
					{
						List<string> juegos = Herramientas.Listados.Generar(noticia.Juegos);

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

					contenido = Herramientas.Idiomas.MirarTexto(idioma, noticia.ContenidoEn, noticia.ContenidoEs);

					if (contenido != null)
					{
						contenido = contenido.Replace("<img ", "<img style=" + Strings.ChrW(34) + "max-width: 100%; margin-top: 20px; margin-bottom: 20px;" + Strings.ChrW(34)) + " ";
					}

					if (noticia.FechaTermina.Year > 2022)
					{
						fechaEmpieza = Herramientas.Idiomas.CogerCadena(idioma, "News.String6") + " " + Herramientas.Calculadora.HaceTiempo(noticia.FechaEmpieza, idioma);
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
						List<string> listaJuegos = Herramientas.Listados.Generar(noticia.Juegos);

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

					if (juegos.Count == 1)
					{
						if (juegos[0].Media.Video != null)
						{
							video = "<video controls autoplay src=" + Strings.ChrW(34) + juegos[0].Media.Video + Strings.ChrW(34) + "/>";
						}
					}
				}
			}
		}
	}
}
