#nullable disable

using Juegos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Noticias;

namespace Herramientas
{
	public class Redireccionador : Controller
	{
		[ResponseCache(Duration = 2000)]
		[HttpGet("game/{id}")]
		public IActionResult CogerJuegoId(int Id)
		{
			return Redirect("~/game?id=" + Id.ToString());
		}

		[ResponseCache(Duration = 2000)]
		[HttpGet("steam/{id}")]
		public IActionResult CogerJuegoIdSteam(int Id)
		{
			return Redirect("~/game?idSteam=" + Id.ToString());
		}

		[ResponseCache(Duration = 2000)]
		[HttpGet("api/{id}")]
		public IActionResult CogerApiId(int Id)
		{
            if (User.Identity.IsAuthenticated == true)
			{
				if (global::BaseDatos.Usuarios.Buscar.RolDios(User.Identity.Name) == true)
				{
                    Juego juego = global::BaseDatos.Juegos.Buscar.UnJuego(Id.ToString());

                    if (juego != null)
                    {
                        return Content(JsonConvert.SerializeObject(juego));
                    }
                }
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

		[ResponseCache(Duration = 2000)]
		[HttpGet("news/{id}")]
		public IActionResult CogerNoticiaId(int Id)
		{
			return Redirect("~/news?id=" + Id.ToString());
		}

		[ResponseCache(Duration = 2000)]
		[HttpGet("bundle/{id}")]
		public IActionResult CogerBundleId(int Id)
		{
			return Redirect("~/bundle?id=" + Id.ToString());
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
