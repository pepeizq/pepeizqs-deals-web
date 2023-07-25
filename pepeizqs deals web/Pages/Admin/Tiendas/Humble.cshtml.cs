#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages.Admin.Tiendas
{
    public class HumbleModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Texto { get; set; }
        }

        public void OnGet()
        {

        }

        public void OnPost() 
        { 
            if (Input.Texto != null) 
            {
                APIs.Humble.Tienda.BuscarOfertas(ViewData, Input.Texto);
            }
        }
    }
}
