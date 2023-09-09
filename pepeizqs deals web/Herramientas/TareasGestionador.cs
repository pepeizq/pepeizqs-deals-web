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
			Tiendas2.TiendasCargar.TareasGestionador(TimeSpan.FromMinutes(30));
		}
	}
}
