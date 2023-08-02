#nullable disable

using BaseDatos.Enlaces;

namespace Herramientas
{
	public static class EnlaceAcortador
	{
		public static string Generar(string enlace, string tienda = null)
		{
			if (tienda != null)
			{
				if (tienda == APIs.Steam.Tienda.Generar().Id)
				{
					enlace = APIs.Steam.Tienda.Referido(enlace);
				}
				else if (tienda == APIs.GamersGate.Tienda.Generar().Id)
				{
					enlace = APIs.GamersGate.Tienda.Referido(enlace);
				}
				else if (tienda == APIs.Humble.Tienda.Generar().Id || tienda == APIs.Humble.Tienda.GenerarChoice().Id)
                {
                    enlace = APIs.Humble.Tienda.Referido(enlace);
                }
				else if (tienda == APIs.Gamesplanet.Tienda.GenerarUk().Id ||
					tienda == APIs.Gamesplanet.Tienda.GenerarFr().Id || 
					tienda == APIs.Gamesplanet.Tienda.GenerarDe().Id || 
					tienda == APIs.Gamesplanet.Tienda.GenerarUs().Id)
				{
					enlace = APIs.Gamesplanet.Tienda.Referido(enlace);
				}
            }

			//----------------------------------------

			Enlace enlaceFinal = Buscar.Base(enlace);

			if (enlaceFinal == null)
			{
				enlaceFinal = Insertar.Ejecutar(enlace);
			}

			return "/link/" + enlaceFinal.Id;
		}
	}

	public class Enlace
	{
		public string Id;
		public string Base;
	}
}
