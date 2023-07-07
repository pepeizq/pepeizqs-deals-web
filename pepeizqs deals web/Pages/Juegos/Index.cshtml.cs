using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace pepeizqs_deals_web.Pages.Juegos
{
    public class IndexModel : PageModel
    {
        public List<Juego> listaJuegos = new List<Juego>();

        public void OnGet()
        {
            try
            {
                WebApplicationBuilder builder = WebApplication.CreateBuilder();
                string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");
            
                using (SqlConnection conexion = new SqlConnection(conexionTexto))
                {
                    conexion.Open();
                    String seleccionarTodo = "SELECT * FROM juegos";

                    using (SqlCommand comando = new SqlCommand(seleccionarTodo, conexion))
                    {
                        using (SqlDataReader lector = comando.ExecuteReader())
                        {
                            while (lector.Read()) 
                            {
                                Juego juego = new Juego
                                {
                                    Id = lector.GetInt32(0),
                                    Nombre = lector.GetString(1),
                                    Imagen = lector.GetString(2),
                                    Drm = lector.GetString(3),
                                    Enlace = lector.GetString(4)
                                };

                                listaJuegos.Add(juego);
                            }
                        }
                    }
                }
            }
			catch (Exception ex)
			{
                Console.WriteLine(ex.Message);
            }
		}

        public class Juego
        {
			[Key]
			[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
			public int Id { get; set; }
            public string Nombre { get; set; }
            public string Imagen { get; set; }
            public string Drm { get; set; }
            public string Enlace { get; set; }
        }
    }
}
