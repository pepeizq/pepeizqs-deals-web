﻿#nullable disable

using BaseDatos.Enlaces;
using Bundles2;
using Gratis2;
using Suscripciones2;

namespace Herramientas
{
	public static class EnlaceAcortador
	{
		private static string dominio = "https://pepeizqdeals.com";

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
				else if (tienda == APIs.IndieGala.Tienda.Generar().Id)
				{
					enlace = APIs.IndieGala.Tienda.Referido(enlace);
				}
				else if (tienda == APIs.WinGameStore.Tienda.Generar().Id)
				{
					enlace = APIs.WinGameStore.Tienda.Referido(enlace);
				}
				else if (tienda == APIs.DLGamer.Tienda.Generar().Id)
				{
					enlace = APIs.DLGamer.Tienda.Referido(enlace);
				}
                else if (tienda == APIs.JoyBuggy.Tienda.Generar().Id)
                {
                    enlace = APIs.JoyBuggy.Tienda.Referido(enlace);
                }
                else if (tienda == APIs._2Game.Tienda.Generar().Id)
                {
                    enlace = APIs._2Game.Tienda.Referido(enlace);
                }
            }

			//----------------------------------------

			Enlace enlaceFinal = Buscar.Base(enlace);

			if (enlaceFinal == null)
			{
				enlaceFinal = Insertar.Ejecutar(enlace);
			}

			return dominio + "/link/" + enlaceFinal.Id + "/";
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

			if (enlaceFinal == null && string.IsNullOrEmpty(enlace) == false)
			{
				enlaceFinal = Insertar.Ejecutar(enlace);
			}

			if (enlaceFinal != null)
			{
				return dominio + "/link/" + enlaceFinal.Id + "/";
			}
			else
			{
				return null;
			}
		}

		public static string Generar(string enlace, GratisTipo tipo)
		{
            if (tipo == APIs.Steam.Gratis.Generar().Tipo)
            {
                enlace = APIs.Steam.Gratis.Referido(enlace);
            }
			else if (tipo == APIs.Fanatical.Gratis.Generar().Tipo)
            {
                enlace = APIs.Fanatical.Gratis.Referido(enlace);
            }
            else if (tipo == APIs.Humble.Gratis.Generar().Tipo)
            {
                enlace = APIs.Humble.Gratis.Referido(enlace);
            }
            else if (tipo == APIs.IndieGala.Gratis.Generar().Tipo)
            {
                enlace = APIs.IndieGala.Gratis.Referido(enlace);
            }

            //----------------------------------------

            Enlace enlaceFinal = Buscar.Base(enlace);

			if (enlaceFinal == null)
			{
				enlaceFinal = Insertar.Ejecutar(enlace);
			}

			return dominio + "/link/" + enlaceFinal.Id + "/";
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

			return dominio + "/link/" + enlaceFinal.Id + "/";
		}
	}

	public class Enlace
	{
		public string Id;
		public string Base;
	}
}
