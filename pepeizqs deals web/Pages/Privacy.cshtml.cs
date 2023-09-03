#nullable disable

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages
{
	public class PrivacyModel : PageModel
	{
		public string idioma = string.Empty;

		private readonly ILogger<PrivacyModel> _logger;

		public PrivacyModel(ILogger<PrivacyModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{
			try
			{
				idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();
			}
			catch { }
		}
	}
}