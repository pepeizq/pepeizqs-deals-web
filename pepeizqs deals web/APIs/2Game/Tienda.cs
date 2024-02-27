#nullable disable

using Herramientas;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;

namespace APIs._2Game
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "2game",
				Nombre = "2Game",
				ImagenLogo = "/imagenes/tiendas/2game_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/2game_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/2game_icono.webp",
				Color = "#beb2f1",
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador, ViewDataDictionary objeto = null)
		{
			BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, "0 ofertas detectadas", conexion);

			string html = await decompilador.Estandar("https://2game.com/feeds/GoogleShopping_EU.xml");

			if (string.IsNullOrEmpty(html) == false)
			{

			}
		}
	}
}
