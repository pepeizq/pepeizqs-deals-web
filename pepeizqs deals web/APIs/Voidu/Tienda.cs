#nullable disable

using Herramientas;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;

namespace APIs.Voidu
{
	public static class Tienda
	{
		public static Tiendas2.Tienda Generar()
		{
			Tiendas2.Tienda tienda = new Tiendas2.Tienda
			{
				Id = "voidu",
				Nombre = "Voidu",
				ImagenLogo = "/imagenes/tiendas/voidu_logo.webp",
				Imagen300x80 = "/imagenes/tiendas/voidu_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/voidu_icono.ico",
				Color = "#f58331",
				AdminEnseñar = true,
				AdminInteractuar = false
			};

			return tienda;
		}

		public static async Task BuscarOfertas(SqlConnection conexion, IDecompiladores decompilador, ViewDataDictionary objeto = null)
		{
			BaseDatos.Tiendas.Admin.Actualizar(Tienda.Generar().Id, DateTime.Now, "0 ofertas detectadas", conexion);

			string html = await decompilador.Estandar("https://files.channable.com/FDPJ7_Cg8Pqi90kXICkb3g==.xml");

			if (string.IsNullOrEmpty(html) == false) 
			{ 
			
			}
		}
	}
}
