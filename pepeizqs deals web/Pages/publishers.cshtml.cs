#nullable disable

using BaseDatos.Publishers;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages
{
    public class publishersModel : PageModel
    {
		public string idioma = string.Empty;

		public List<Publisher> publishers = new List<Publisher>();

		public async Task OnGetAsync()
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

			await Task.Delay(1);
			publishers = BaseDatos.Publishers.Buscar.Todos();

			if (publishers != null)
			{
				if (publishers.Count > 0)
				{
					publishers = publishers.OrderBy(x => x.Nombre).ToList();
				}
			}
		}
    }
}
