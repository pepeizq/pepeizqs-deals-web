#nullable disable

using Juegos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace pepeizqs_deals_web.Pages.Juegos
{
    public class IndexModel : PageModel
    {
		public string busquedaJuegos = string.Empty;

		public List<JuegoAdminBusqueda> listaJuegos = new List<JuegoAdminBusqueda>();

		public void OnGet()
        {
			WebApplicationBuilder builder = WebApplication.CreateBuilder();
			string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

			using (SqlConnection conexion = new SqlConnection(conexionTexto))
			{
				conexion.Open();
				string busqueda = "SELECT * FROM adminBusqueda";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							JuegoAdminBusqueda juego = new JuegoAdminBusqueda
							{
								Id = lector.GetInt32(0),
								Nombre = lector.GetString(1)
							};

							listaJuegos.Add(juego);
						}
					}						
				}
			}
		}

		public IActionResult OnPost()
		{
			if (busquedaJuegos != string.Empty)
			{
				WebApplicationBuilder builder = WebApplication.CreateBuilder();
				string conexionTexto = builder.Configuration.GetConnectionString("pepeizqs_deals_webContextConnection");

				using (SqlConnection conexion = new SqlConnection(conexionTexto))
				{
					conexion.Open();
					string busqueda = "SELECT * FROM juegos WHERE nombre LIKE '%" + busquedaJuegos.ToLower() + "%'";

					using (SqlCommand comando = new SqlCommand(busqueda, conexion))
					{
						using (SqlDataReader lector = comando.ExecuteReader())
						{
							while (lector.Read())
							{
								JuegoAdminBusqueda juego = new JuegoAdminBusqueda
								{
									Id = lector.GetInt32(0),
									Nombre = lector.GetString(1)
								};

								listaJuegos.Add(juego);
							}
						}
					}

					string limpiar = "TRUNCATE TABLE adminBusqueda";

					using (SqlCommand comando = new SqlCommand(limpiar, conexion))
					{
						comando.ExecuteNonQuery();
					}

					foreach (JuegoAdminBusqueda juego in listaJuegos)
					{
						string añadir = "INSERT INTO adminBusqueda " +
							"(id, nombre) VALUES " +
							"(@id, @nombre) ";

						using (SqlCommand comando = new SqlCommand(añadir, conexion))
						{
							comando.Parameters.AddWithValue("@id", juego.Id);
							comando.Parameters.AddWithValue("@nombre", juego.Nombre);

							comando.ExecuteNonQuery();
						}
					}
				}
			}

			return RedirectToPage();
		}
	}
}
