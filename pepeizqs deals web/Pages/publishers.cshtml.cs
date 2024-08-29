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
			try
			{
				idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();
			}
			catch { }

			publishers = BaseDatos.Publishers.Buscar.Todos();
		}
    }
}
