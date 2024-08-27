#nullable disable

using BaseDatos.Publishers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages
{
    public class publisherModel : PageModel
    {
		public string idioma = string.Empty;

		[BindProperty(SupportsGet = true)]
		public string id { get; set; }

		public Publisher publisher = new Publisher();

        public async Task OnGetAsync()
		{
			try
			{
				idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();
			}
			catch { }

			if (string.IsNullOrEmpty(id) == false)
			{
				id = id.Replace(" ", null);
				id = id.ToLower();

				publisher = BaseDatos.Publishers.Buscar.Id(id);

				if (publisher == null)
				{
					publisher = await APIs.Steam.Curator.SacarPublisher(id);

					if (publisher != null)
					{
						BaseDatos.Publishers.Insertar.Ejecutar(publisher);
					}
				}
			}
		}
	}
}
