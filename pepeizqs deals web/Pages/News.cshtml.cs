#nullable disable

using Bundles2;
using Gratis2;
using Juegos;
using Microsoft.AspNetCore.Mvc;
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

		[BindProperty(SupportsGet = true)]
		public int id { get; set; }

		[BindProperty(SupportsGet = true)]
		public string nombre { get; set; }

		public void OnGet()
        {
			try
			{
				idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();
			}
			catch { }		

            if (id > 0)
            {
				noticia = BaseDatos.Noticias.Buscar.UnaNoticia(id);
			}
		}
	}
}
