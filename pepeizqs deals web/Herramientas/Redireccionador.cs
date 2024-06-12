#nullable disable

using Bundles2;
using Juegos;
using Microsoft.AspNetCore.Mvc;
using Noticias;

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

		[ResponseCache(Duration = 6000)]
		[HttpGet("api/last-subscriptions/")]
		public IActionResult ApiSuscripcionesUltimos()
		{
			List<JuegoSuscripcion> suscripciones = global::BaseDatos.Suscripciones.Buscar.Ultimos("5");

			if (suscripciones != null)
			{
				return Ok(suscripciones);
			}

			return Redirect("~/");
		}

		[ResponseCache(Duration = 6000)]
		[HttpGet("api/last-subscriptions/{Cantidad}/")]
		public IActionResult ApiSuscripcionesUltimos(int Cantidad)
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

			List<JuegoSuscripcion> suscripciones = global::BaseDatos.Suscripciones.Buscar.Ultimos(cantidadFinal.ToString());

			if (suscripciones != null)
			{
				return Ok(suscripciones);
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
    }
}
