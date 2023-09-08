//https://github.com/damienbod/AspNetCoreHangfire

#nullable disable

namespace Herramientas
{
	public interface ITareasGestionador
	{
		public void HacerTarea();
	}

	public class TareasGestionador : ITareasGestionador
	{
		public void HacerTarea()
		{
			int orden = global::BaseDatos.Tiendas.Admin.TareaLeerOrden();

			Tiendas2.TiendasCargar.TareasGestionador(orden, TimeSpan.FromMinutes(20));

			orden += 1;

			if (orden + 1 > Tiendas2.TiendasCargar.GenerarListado().Count)
			{
				orden = 0;
			}

			global::BaseDatos.Tiendas.Admin.TareaCambiarOrden(orden);
		}
	}
}
