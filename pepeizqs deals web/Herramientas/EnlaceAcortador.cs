#nullable disable

using BaseDatos.Enlaces;
using Bundles2;
using Gratis2;
using Suscripciones2;

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
				else if (tienda == APIs.Fanatical.Tienda.Generar().Id)
				{
					enlace = APIs.Fanatical.Tienda.Referido(enlace);
				}
				else if (tienda == APIs.GreenManGaming.Tienda.Generar().Id)
				{
					enlace = APIs.GreenManGaming.Tienda.Referido(enlace);
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

		public static string Generar(string enlace, BundleTipo tipo)
		{
			if (tipo == BundleTipo.HumbleBundle)
			{
				enlace = APIs.Humble.Bundle.Referido(enlace);
			}
			else if (tipo == BundleTipo.Fanatical)
			{
				enlace = APIs.Fanatical.Bundle.Referido(enlace);
			}

			//----------------------------------------

			Enlace enlaceFinal = Buscar.Base(enlace);

			if (enlaceFinal == null)
			{
				enlaceFinal = Insertar.Ejecutar(enlace);
			}

			return "/link/" + enlaceFinal.Id;
		}

		public static string Generar(string enlace, GratisTipo tipo)
		{
			//if (tipo == SuscripcionTipo.HumbleChoice)
			//{
			//	enlace = APIs.Humble.Suscripcion.Referido(enlace);
			//}

			//----------------------------------------

			Enlace enlaceFinal = Buscar.Base(enlace);

			if (enlaceFinal == null)
			{
				enlaceFinal = Insertar.Ejecutar(enlace);
			}

			return "/link/" + enlaceFinal.Id;
		}

		public static string Generar(string enlace, SuscripcionTipo tipo)
		{
			if (tipo == SuscripcionTipo.HumbleChoice)
			{
				enlace = APIs.Humble.Suscripcion.Referido(enlace);
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
