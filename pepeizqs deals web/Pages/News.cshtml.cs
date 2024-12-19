#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Noticias;

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
			idioma = Request.Query["language"];

			if (string.IsNullOrEmpty(idioma) == true)
			{
				try
				{
					idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();
				}
				catch { }
			}

			if (id > 0)
            {
				noticia = BaseDatos.Noticias.Buscar.UnaNoticia(id);
			}
		}
	}
}
