//https://docs.google.com/document/d/1LCOqVOzMI67E-bxA8rDTaHLhqZ4xyAP80xkgzFICR8w/edit?tab=t.0

#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace APIs.Playsum
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "playsum",
				Nombre = "Playsum",
				ImagenLogo = "/imagenes/tiendas/playsum_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/playsum_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/playsum_icono.ico",
				Color = "#a91aff",
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static string Referido(string enlace)
		{
			return enlace + "?plysm_ref_id=YiEguGaNJjlnglvi5JTNrVZi1z5OUoli";
		}

		public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador)
		{
			BaseDatos.Admin.Actualizar.Tiendas(Generar().Id, DateTime.Now, 0, conexion);

			string html = await Decompiladores.Estandar("https://api.playsum.live/v1/shop/products/rss");

			if (string.IsNullOrEmpty(html) == false)
			{
				BaseDatos.Errores.Insertar.Mensaje("test", html);
			}
		}
	}
}
