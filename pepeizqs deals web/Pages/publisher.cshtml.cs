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
			idioma = Request.Query["language"];

			if (string.IsNullOrEmpty(idioma) == true)
			{
				try
				{
					idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();
				}
				catch { }
			}

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
						bool a�adir = true;

						if (string.IsNullOrEmpty(publisher.Nombre) == true)
						{
							a�adir = false;
						}

						if (string.IsNullOrEmpty(publisher.Imagen) == false)
						{
							if (publisher.Imagen == "https://avatars.akamai.steamstatic.com/_full.jpg")
							{
								a�adir = false;
							}
						}

						if (a�adir == true)
						{
                            BaseDatos.Publishers.Insertar.Ejecutar(publisher);
                        }
					}
				}
			}
		}
	}
}
