﻿#nullable disable

using Bundles2;
using Juegos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Noticias;
using System.Text;

namespace Herramientas
{
	public class Redireccionador : Controller
	{
		[ResponseCache(Duration = 6000)]
		[HttpGet("api/game/{id}")]
		public IActionResult ApiJuego(int Id)
		{
			Juego juego = global::BaseDatos.Juegos.Buscar.UnJuego(Id.ToString());

			if (juego != null)
			{
				return Ok(juego);
			}

			return Redirect("~/");
        }

		[ResponseCache(Duration = 6000)]
		[HttpGet("api/steam/{id}")]
		public IActionResult ApiJuegoSteam(int Id)
		{
			Juego juego = global::BaseDatos.Juegos.Buscar.UnJuego(null, Id.ToString());

			if (juego != null)
			{
				return Ok(juego);
			}

			return Redirect("~/");
		}

		[ResponseCache(Duration = 6000)]
		[HttpGet("api/gog/{id}")]
		public IActionResult ApiJuegoGog(string Id)
		{
			Juego juego = global::BaseDatos.Juegos.Buscar.UnJuego(null, null, Id);

			if (juego != null)
			{
				return Ok(juego);
			}

			return Redirect("~/");
		}

        [ResponseCache(Duration = 6000)]
        [HttpGet("api/bundle/{id}/")]
        public IActionResult ApiBundle(int Id)
        {
            Bundle bundle = global::BaseDatos.Bundles.Buscar.UnBundle(Id);

            if (bundle != null)
            {
                return Ok(bundle);
            }

            return Redirect("~/");
        }

        [ResponseCache(Duration = 6000)]
		[HttpGet("api/bundle/{id}/{juegos}")]
		public IActionResult ApiBundle(int Id, string Juegos)
		{
			Bundle bundle = global::BaseDatos.Bundles.Buscar.UnBundle(Id);

			if (bundle != null)
			{
				if (string.IsNullOrEmpty(Juegos) == false)
				{
					if (Juegos.ToLower() == "games")
					{
						foreach (var juego in bundle.Juegos) 
						{ 
							juego.Juego = global::BaseDatos.Juegos.Buscar.UnJuego(juego.JuegoId);
						}
					}
				}

				return Ok(bundle);
			}

			return Redirect("~/");
		}

		[ResponseCache(Duration = 6000)]
		[HttpGet("api/last-bundles/")]
		public IActionResult ApiBundlesUltimos()
		{
			List<Bundle> bundles = global::BaseDatos.Bundles.Buscar.Ultimos("5");

			if (bundles != null)
			{
				return Ok(bundles);
			}

			return Redirect("~/");
		}

		[ResponseCache(Duration = 6000)]
        [HttpGet("api/last-bundles/{Cantidad}/")]
        public IActionResult ApiBundlesUltimos(int Cantidad)
        {
			int cantidadFinal = 5;

			if (Cantidad > 0)
			{
				cantidadFinal = Cantidad;
			}

			if (cantidadFinal > 25)
			{
				cantidadFinal = 25;
			}

            List<Bundle> bundles = global::BaseDatos.Bundles.Buscar.Ultimos(cantidadFinal.ToString());

            if (bundles != null)
            {
                return Ok(bundles);
            }

            return Redirect("~/");
        }

        [ResponseCache(Duration = 6000)]
		[HttpGet("api/news/{id}")]
		public IActionResult ApiNoticia(int Id)
		{
			Noticia noticia = global::BaseDatos.Noticias.Buscar.UnaNoticia(Id);

			if (noticia != null)
			{
				return Ok(noticia);
			}

			return Redirect("~/");
		}

		[ResponseCache(Duration = 6000)]
		[HttpGet("api/last-news/")]
		public IActionResult ApiNoticiasUltimas()
		{
			List<Noticia> noticias = global::BaseDatos.Noticias.Buscar.Ultimas("5");

			if (noticias != null)
			{
				return Ok(noticias);
			}

			return Redirect("~/");
		}

		[ResponseCache(Duration = 6000)]
		[HttpGet("api/last-news/{Cantidad}/")]
		public IActionResult ApiNoticiasUltimas(int Cantidad)
		{
			int cantidadFinal = 5;

			if (Cantidad > 0)
			{
				cantidadFinal = Cantidad;
			}

			if (cantidadFinal > 25)
			{
				cantidadFinal = 25;
			}

			List<Noticia> noticias = global::BaseDatos.Noticias.Buscar.Ultimas(cantidadFinal.ToString());

			if (noticias != null)
			{
				return Ok(noticias);
			}

			return Redirect("~/");
		}

        [ResponseCache(Duration = 6000)]
        [HttpGet("api/last-free/")]
        public IActionResult ApiGratisUltimos()
        {
            List<JuegoGratis> gratis = global::BaseDatos.Gratis.Buscar.Ultimos("5");

            if (gratis != null)
            {
                return Ok(gratis);
            }

            return Redirect("~/");
        }

        [ResponseCache(Duration = 6000)]
        [HttpGet("api/last-free/{Cantidad}/")]
        public IActionResult ApiGratisUltimos(int Cantidad)
        {
            int cantidadFinal = 5;

            if (Cantidad > 0)
            {
                cantidadFinal = Cantidad;
            }

            if (cantidadFinal > 25)
            {
                cantidadFinal = 25;
            }

            List<JuegoGratis> gratis = global::BaseDatos.Gratis.Buscar.Ultimos(cantidadFinal.ToString());

            if (gratis != null)
            {
                return Ok(gratis);
            }

            return Redirect("~/");
        }

        [ResponseCache(Duration = 2000)]
		[HttpGet("link/{id}")]
		public IActionResult CogerAcortador(int Id)
		{
			Enlace enlace = global::BaseDatos.Enlaces.Buscar.Id(Id.ToString());

			if (enlace != null) 
			{
				return Redirect(enlace.Base);
			}
			else
			{
				return Redirect("~/");
			}			
		}

		[HttpGet("news-rss")]
        public IActionResult CogerNoticiasRSS()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                if (global::BaseDatos.Usuarios.Buscar.RolDios(User.Identity.Name) == true)
                {
					List<Noticia> noticias = global::BaseDatos.Noticias.Buscar.Ultimas("10");

					if (noticias.Count > 0)
					{
                        return Ok(noticias);
                    }
                }
            }

            return Redirect("~/");
        }

        [HttpGet("sitemap.xml")]
        public IActionResult Sitemap()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"\r\n        xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">\r\n");

            string textoIndex = "<url>" + Environment.NewLine +
                    "<loc>https://pepeizqdeals.com/</loc>" + Environment.NewLine +
                    "<changefreq>hourly</changefreq>" + Environment.NewLine +
                    "<priority>0.9</priority> " + Environment.NewLine +
                    "</url>";

            sb.Append(textoIndex);

            string textoBundles = "<url>" + Environment.NewLine +
                    "<loc>https://pepeizqdeals.com/bundles/</loc>" + Environment.NewLine +
                    "<changefreq>daily</changefreq>" + Environment.NewLine +
                    "<priority>0.7</priority> " + Environment.NewLine +
                    "</url>";

            sb.Append(textoBundles);

            string textoGratis = "<url>" + Environment.NewLine +
                    "<loc>https://pepeizqdeals.com/free/</loc>" + Environment.NewLine +
                    "<changefreq>daily</changefreq>" + Environment.NewLine +
                    "<priority>0.7</priority> " + Environment.NewLine +
                    "</url>";

            sb.Append(textoGratis);

            string textoSuscripciones = "<url>" + Environment.NewLine +
                    "<loc>https://pepeizqdeals.com/subscriptions/</loc>" + Environment.NewLine +
                    "<changefreq>daily</changefreq>" + Environment.NewLine +
                    "<priority>0.7</priority> " + Environment.NewLine +
                    "</url>";

            sb.Append(textoSuscripciones);

            string textoMinimos = "<url>" + Environment.NewLine +
                    "<loc>https://pepeizqdeals.com/historical-lows/</loc>" + Environment.NewLine +
                    "<changefreq>hourly</changefreq>" + Environment.NewLine +
                    "<priority>0.9</priority> " + Environment.NewLine +
                    "</url>";

            sb.Append(textoMinimos);

            string textoNoticias = "<url>" + Environment.NewLine +
                    "<loc>https://pepeizqdeals.com/last-news/</loc>" + Environment.NewLine +
                    "<changefreq>hourly</changefreq>" + Environment.NewLine +
                    "<priority>0.9</priority> " + Environment.NewLine +
                    "</url>";

            sb.Append(textoNoticias);

            string textoAñadidos = "<url>" + Environment.NewLine +
                    "<loc>https://pepeizqdeals.com/last-added/</loc>" + Environment.NewLine +
                    "<changefreq>hourly</changefreq>" + Environment.NewLine +
                    "<priority>0.9</priority> " + Environment.NewLine +
                    "</url>";

            sb.Append(textoAñadidos);

            List<Noticia> noticias = global::BaseDatos.Noticias.Buscar.Ultimas("20");

            if (noticias.Count > 0)
            {
                foreach (Noticia noticia in noticias)
                {
                    DateTime fechaTemporal = noticia.FechaEmpieza;
                    fechaTemporal = fechaTemporal.AddDays(7);

                    if (fechaTemporal > DateTime.Now)
                    {
                        string texto = "<url>" + Environment.NewLine +
                        "<loc>https://pepeizqdeals.com/news/" + noticia.Id.ToString() + "/" + Herramientas.EnlaceAdaptador.Nombre(noticia.TituloEn) + "/</loc>" + Environment.NewLine +
                        "<news:news>" + Environment.NewLine +
                        "<news:publication>" + Environment.NewLine +
                        "<news:name>pepeizq's deals</news:name>" + Environment.NewLine +
                        "<news:language>en</news:language>" + Environment.NewLine +
                        "</news:publication>" + Environment.NewLine +
                        "<news:publication_date>" + noticia.FechaEmpieza.ToString("yyyy-MM-dd") + "</news:publication_date>" + Environment.NewLine +
                        "<news:title>" + noticia.TituloEn + "</news:title>" + Environment.NewLine +
                        "</news:news>" + Environment.NewLine +
                        "</url>";

                        sb.Append(texto);
                    }
                }
            }

            sb.Append("</urlset>");

            return new ContentResult
            {
                ContentType = "application/xml",
                Content = sb.ToString(),
                StatusCode = 200
            };
        }

        [HttpGet("sitemap-games.xml")]
        public IActionResult SitemapJuegos()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"\r\n        xmlns:news=\"http://www.google.com/schemas/sitemap-news/0.9\">\r\n");

            string textoIndex = "<url>" + Environment.NewLine +
                    "<loc>https://pepeizqdeals.com/</loc>" + Environment.NewLine +
                    "<changefreq>hourly</changefreq>" + Environment.NewLine +
                    "<priority>0.9</priority> " + Environment.NewLine +
                    "</url>";

            sb.Append(textoIndex);

            List<Juego> juegosConMinimos = new List<Juego>();

            SqlConnection conexion = BaseDatos.Conectar();

            using (conexion)
            {
                juegosConMinimos = global::BaseDatos.Juegos.Buscar.Todos(conexion, "seccionMinimos");
            }

            if (juegosConMinimos.Count > 0)
            {
                int i = 0;
                while (i < 100)
                {
                    string textoJuegos = "<url>" + Environment.NewLine +
                         "<loc>https://pepeizqdeals.com/game/" + juegosConMinimos[i].IdMaestra + "/" + EnlaceAdaptador.Nombre(juegosConMinimos[i].Nombre) + "/</loc>" + Environment.NewLine +
                         "<changefreq>hourly</changefreq>" + Environment.NewLine +
                         "<priority>0.9</priority> " + Environment.NewLine +
                         "</url>";

                    sb.Append(textoJuegos);

                    i += 1;
                }
            }

            sb.Append("</urlset>");

            return new ContentResult
            {
                ContentType = "application/xml",
                Content = sb.ToString(),
                StatusCode = 200
            };
        }
    }
}
