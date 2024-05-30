﻿#nullable disable

using Bundles2;
using Juegos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Noticias;

namespace Herramientas
{
	public class Redireccionador : Controller
	{
		[ResponseCache(Duration = 5000)]
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

		[ResponseCache(Duration = 5000)]
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

		[ResponseCache(Duration = 5000)]
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

		[ResponseCache(Duration = 5000)]
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

        [ResponseCache(Duration = 5000)]
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

        [ResponseCache(Duration = 5000)]
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
					List<Noticia> noticias = global::BaseDatos.Noticias.Buscar.Todas();
					List<Noticia> noticiasMostrar = new List<Noticia>();

					if (noticias != null)
					{
						if (noticias.Count > 0)
						{
							foreach (var noticia in noticias)
							{
								if (DateTime.Now >= noticia.FechaEmpieza && DateTime.Now <= noticia.FechaTermina)
								{
									noticiasMostrar.Add(noticia);
								}
							}
						}
                    }

					if (noticiasMostrar.Count > 0)
					{
                        return Content(JsonConvert.SerializeObject(noticiasMostrar));
                    }
                }
            }

            return Redirect("~/");
        }
    }
}
