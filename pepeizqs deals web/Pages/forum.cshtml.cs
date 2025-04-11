#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages
{
	public class forumModel : PageModel
	{
		public string idioma = string.Empty;

		[BindProperty(SupportsGet = true)]
		public int categoriaId { get; set; }

		[BindProperty(SupportsGet = true)]
		public int postId { get; set; }

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
		}
	}
}
