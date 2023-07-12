#nullable disable

using Juegos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace pepeizqs_deals_web.Pages.Juegos
{
    public class AñadirModel : PageModel
    {
		public string idPrecarga = string.Empty;

		public Juego juegoAñadir = JuegoCrear.Generar();

        public string errorMensaje = string.Empty;
        public string exitoMensaje = string.Empty;

        public void OnGet()
        {
			string id = Request.Query["id"];

            if (id != null)
            {
				if (id.Length > 0)
				{
					juegoAñadir = Steam.Juego.CargarDatos(id).Result;
				}
            }

			
		}

        public IActionResult OnPost() 
        {
            if (Request.Form["precarga"] != string.Empty) 
            {
				idPrecarga = Steam.Juego.LimpiarID(Request.Form["precarga"]);

				return RedirectToPage("./Añadir", new { id = idPrecarga });
			}
            else 
            {
				//juegoAñadir.Id = int.Parse(Request.Form["id"]);
				//juegoAñadir.Nombre = Request.Form["nombre"];
				//juego.Imagen = Request.Form["imagen"];
				//juego.Drm = Request.Form["drm"];
				//juego.Enlace = Request.Form["enlace"];

				//if (juego.Id == 0 || juego.Nombre == string.Empty || juego.Imagen == string.Empty || juego.Drm == string.Empty || juego.Enlace == string.Empty)
				//{
				//    errorMensaje = "error " + juego.Id.ToString();
				//    return;
				//}

				//try
				//{
				//	WebApplicationBuilder builder = WebApplication.CreateBuilder();
				//	string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

				//	using (SqlConnection conexion = new SqlConnection(conexionTexto))
				//	{
				//		conexion.Open();

				//		string sqlAñadir = "INSERT INTO juegos " +
				//			"(id, nombre, imagen, drm, enlace) VALUES " +
				//			"(@id, @nombre, @imagen, @drm, @enlace) ";

				//		using (SqlCommand comando = new SqlCommand(sqlAñadir, conexion))
				//		{
				//			comando.Parameters.AddWithValue("@id", juegoAñadir.Id);
				//			comando.Parameters.AddWithValue("@nombre", juegoAñadir.Nombre);
				//			//comando.Parameters.AddWithValue("@imagen", juego.Imagen);
				//			//comando.Parameters.AddWithValue("@drm", juego.Drm);
				//			//comando.Parameters.AddWithValue("@enlace", juego.Enlace);

				//			comando.ExecuteNonQuery();
				//		}
				//	}
				//}
				//catch (Exception ex)
				//{
				//	errorMensaje = ex.Message;
				//	//return;
				//}

				//juegoAñadir.Id = 0;
				//juegoAñadir.Nombre = string.Empty;
				////juego.Imagen = string.Empty;
				////juego.Drm = string.Empty;
				////juego.Enlace = string.Empty;

				//exitoMensaje = "exito";

				return null;
			}



			

			
		}
    }
}
