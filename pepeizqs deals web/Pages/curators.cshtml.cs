#nullable disable

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages
{
    public class curatorsModel : PageModel
    {
		public List<BaseDatos.Curators.Curator> curators = new List<BaseDatos.Curators.Curator>();

		public string idioma = string.Empty;

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

			curators = BaseDatos.Curators.Buscar.Todos();

			if (curators.Count > 0)
			{
				curators = curators.OrderBy(x => x.Nombre).ToList();
			}
		}
	}
}
