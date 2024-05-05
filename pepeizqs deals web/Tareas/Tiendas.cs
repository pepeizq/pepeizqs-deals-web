#nullable disable

using BaseDatos.Tiendas;
using Herramientas;
using Microsoft.Data.SqlClient;

namespace Tareas
{
	public class Tiendas
	{
		public static async Task Ejecutar(SqlConnection conexion, IDecompiladores decompilador)
		{
			TimeSpan siguienteComprobacion = TimeSpan.FromMinutes(120);
			List<string> ids = new List<string>();

			foreach (var tienda in Tiendas2.TiendasCargar.GenerarListado())
			{
				if (tienda.AdminInteractuar == true)
				{
					ids.Add(tienda.Id);
				}
			}

			if (Admin.ComprobarTiendasUso(conexion, TimeSpan.FromSeconds(30)) == false)
			{
				AdminTarea tiendaComprobar = Admin.TiendaSiguiente(conexion);

				if (DateTime.Now - tiendaComprobar.fecha > siguienteComprobacion)
				{
					try
					{
						await Tiendas2.TiendasCargar.TareasGestionador(conexion, tiendaComprobar.id, decompilador);
					}
					catch (Exception ex)
					{
						BaseDatos.Errores.Insertar.Ejecutar(tiendaComprobar.id, ex, conexion);
					}
				}
			}
		}
	}
}
