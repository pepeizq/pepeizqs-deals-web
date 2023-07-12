using Juegos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace pepeizqs_deals_web.Pages.Juegos
{
    public class EditarModel : PageModel
    {
		public Juego juego = new Juego();

		public string errorMensaje = string.Empty;
		public string exitoMensaje = string.Empty;

		public void OnGet()
        {
            string id = Request.Query["Id"];

            try
            {
                WebApplicationBuilder builder = WebApplication.CreateBuilder();
                string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

                using (SqlConnection conexion = new SqlConnection(conexionTexto))
				{
					conexion.Open();
					String seleccionarJuego = "SELECT * FROM juegos WHERE id=@id";

					using (SqlCommand comando = new SqlCommand(seleccionarJuego, conexion))
					{
						comando.Parameters.AddWithValue("@id", id);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							if (lector.Read())
							{
								juego.Id = lector.GetInt32(0);
								juego.Nombre = lector.GetString(1);
								//juego.Imagen = lector.GetString(2);
								//juego.Drm = lector.GetString(3);
								//juego.Enlace = lector.GetString(4);
							}
						}
					}
				}
			}
            catch (Exception ex) 
            { 
                errorMensaje = ex.Message;
            }
        }

        public void OnPost() 
        {
			juego.Id = int.Parse(Request.Form["id"]);
			juego.Nombre = Request.Form["nombre"];
			//juego.Imagen = Request.Form["imagen"];
			//juego.Drm = Request.Form["drm"];
			//juego.Enlace = Request.Form["enlace"];

			//if (juego.Id == 0 || juego.Nombre == string.Empty || juego.Imagen == string.Empty || juego.Drm == string.Empty || juego.Enlace == string.Empty)
			//{
			//	errorMensaje = "error";
			//	return;
			//}

			try
			{
				WebApplicationBuilder builder = WebApplication.CreateBuilder();
				string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

				using (SqlConnection conexion = new SqlConnection(conexionTexto))
				{
					conexion.Open();

					string sqlActualizar = "UPDATE juegos " +
						"SET nombre=@nombre, imagen=@imagen, drm=@drm, enlace=@enlace " +
						"WHERE id=@id";

					using (SqlCommand comando = new SqlCommand(sqlActualizar, conexion))
					{
						comando.Parameters.AddWithValue("@id", juego.Id);
						comando.Parameters.AddWithValue("@nombre", juego.Nombre);
						//comando.Parameters.AddWithValue("@imagen", juego.Imagen);
						//comando.Parameters.AddWithValue("@drm", juego.Drm);
						//comando.Parameters.AddWithValue("@enlace", juego.Enlace);

						comando.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				errorMensaje = ex.Message;
				return;
			}

			juego.Id = 0;
			juego.Nombre = string.Empty;
			//juego.Imagen = string.Empty;
			//juego.Drm = string.Empty;
			//juego.Enlace = string.Empty;

			exitoMensaje = "exito";

			Response.Redirect("/Juegos/Index");
		}
    }
}
