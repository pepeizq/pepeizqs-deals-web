#nullable disable

using Juegos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace pepeizqs_deals_web.Pages
{
    public class GameModel : PageModel
    {
		public Juego juego = JuegoCrear.Generar();

		public void OnGet()
        {
			string id = Request.Query["id"];
			string idSteam = Request.Query["idSteam"];
			string idGog = Request.Query["idGog"];

			string sqlBuscar = string.Empty;
			string idParametro = string.Empty;
			string idBuscar = string.Empty;

			if (id != null)
			{
				sqlBuscar = "SELECT * FROM juegos WHERE id=@id";
				idParametro = "@id";
				idBuscar = id;
			}
			else
			{
				if (idSteam != null)
				{
					sqlBuscar = "SELECT * FROM juegos WHERE idSteam=@idSteam";
					idParametro = "@idSteam";
					idBuscar = idSteam;
				}
				else
				{
					if (idGog != null)
					{
						sqlBuscar = "SELECT * FROM juegos WHERE idGog=@idGog";
						idParametro = "@idGog";
						idBuscar = idGog;
					}
				}
			}

			try
			{
				WebApplicationBuilder builder = WebApplication.CreateBuilder();
				string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

				using (SqlConnection conexion = new SqlConnection(conexionTexto))
				{
					conexion.Open();
					String seleccionarJuego = sqlBuscar;

					using (SqlCommand comando = new SqlCommand(seleccionarJuego, conexion))
					{
						comando.Parameters.AddWithValue(idParametro, idBuscar);

						using (SqlDataReader lector = comando.ExecuteReader())
						{
							if (lector.Read())
							{
								juego = JuegoBaseDatos.CargarJuego(juego, lector);
							}
						}
					}
				}
			}
			catch 
			{
				
			}
		}
    }
}
