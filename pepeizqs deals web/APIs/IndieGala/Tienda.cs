#nullable disable

using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace APIs.IndieGala
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "indiegala",
				Nombre = "IndieGala",
				ImagenLogo = "/imagenes/tiendas/indiegala_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/indiegala_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/indiegala_icono.ico",
				Color = "#ffccd4",
				AdminEnseñar = true,
				AdminInteractuar = true
			};

			return tienda;
		}

		public static string Referido(string enlace)
		{
			return enlace + "?ref=pepeizq";
		}

		public static async Task BuscarOfertas(ViewDataDictionary objeto = null)
		{
			//https://www.indiegala.com/games/ajax/on-sale/percentage-off/15
		}
	}
}
