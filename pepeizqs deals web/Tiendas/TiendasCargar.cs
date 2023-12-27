#nullable disable

using BaseDatos.Tiendas;
using Herramientas;

namespace Tiendas2
{
	public static class TiendasCargar
	{
		public static List<Tienda> GenerarListado()
		{
			List<Tienda> tiendas = new List<Tienda>
			{
				APIs.Steam.Tienda.Generar(),
				APIs.GamersGate.Tienda.Generar(),
				APIs.Humble.Tienda.Generar(),
				APIs.Humble.Tienda.GenerarChoice(),
				APIs.Gamesplanet.Tienda.GenerarUk(),
				APIs.Gamesplanet.Tienda.GenerarFr(),
				APIs.Gamesplanet.Tienda.GenerarDe(),
				APIs.Gamesplanet.Tienda.GenerarUs(),
				APIs.Fanatical.Tienda.Generar(),
				APIs.GreenManGaming.Tienda.Generar(),
				APIs.GOG.Tienda.Generar(),
                APIs.IndieGala.Tienda.Generar(),
                APIs.WinGameStore.Tienda.Generar(),
				APIs.EA.Tienda.Generar(),
				APIs.DLGamer.Tienda.Generar(),
				APIs.Battlenet.Tienda.Generar()
			};

			return tiendas;
		}

        public static async void AdminTiendas(string id)
        {
			if (id == APIs.Steam.Tienda.Generar().Id)
			{
				await APIs.Steam.Tienda.BuscarOfertas(true);
			}
			else if (id == APIs.GamersGate.Tienda.Generar().Id)
			{
				await APIs.GamersGate.Tienda.BuscarOfertas();
			}
			else if (id == APIs.Gamesplanet.Tienda.GenerarUk().Id)
			{
				await APIs.Gamesplanet.Tienda.BuscarOfertasUk();
			}
			else if (id == APIs.Gamesplanet.Tienda.GenerarFr().Id)
			{
				await APIs.Gamesplanet.Tienda.BuscarOfertasFr();
			}
			else if (id == APIs.Gamesplanet.Tienda.GenerarDe().Id)
			{
				await APIs.Gamesplanet.Tienda.BuscarOfertasDe();
			}
			else if (id == APIs.Gamesplanet.Tienda.GenerarUs().Id)
			{
				await APIs.Gamesplanet.Tienda.BuscarOfertasUs();
			}
			else if (id == APIs.Fanatical.Tienda.Generar().Id)
			{
				await APIs.Fanatical.Tienda.BuscarOfertas();
			}
			else if (id == APIs.GreenManGaming.Tienda.Generar().Id)
			{
				await APIs.GreenManGaming.Tienda.BuscarOfertas();
			}
			else if (id == APIs.GOG.Tienda.Generar().Id)
			{
				await APIs.GOG.Tienda.BuscarOfertas();
			}
			else if (id == APIs.IndieGala.Tienda.Generar().Id)
			{
				await APIs.IndieGala.Tienda.BuscarOfertas();
			}
			else if (id == APIs.WinGameStore.Tienda.Generar().Id)
			{
				await APIs.WinGameStore.Tienda.BuscarOfertas();
			}
            else if (id == APIs.EA.Tienda.Generar().Id)
            {
                await APIs.EA.Tienda.BuscarOfertas();
            }
			else if (id == APIs.DLGamer.Tienda.Generar().Id)
			{
				await APIs.DLGamer.Tienda.BuscarOfertas();
			}
            else if (id == APIs.Battlenet.Tienda.Generar().Id)
            {
                await APIs.Battlenet.Tienda.BuscarOfertas();
            }
        }

		public static async void TareasGestionador(TimeSpan tiempoEntreTareas)
		{
			List<string> ids = new List<string>();

			foreach (var tienda in GenerarListado())
			{
				ids.Add(tienda.Id);
			}

			int orden = Admin.TareaLeerOrden();
			int ordenTiendaAnterior = orden - 1;

			if (ordenTiendaAnterior < 0)
			{
				ordenTiendaAnterior = ids.Count - 1;
			}

			DateTime tiendaAnterior = Admin.TareaLeerTienda(ids[ordenTiendaAnterior]);
			DateTime ultimaComprobacion = Admin.TareaLeerTienda(ids[orden]);

			if ((DateTime.Now - ultimaComprobacion) > tiempoEntreTareas && (DateTime.Now - tiendaAnterior) > (tiempoEntreTareas * 2))
			{
				if (orden == 0)
				{
					Admin.TareaCambiarOrden(orden += 1);
					await APIs.Steam.Tienda.BuscarOfertas(true);
				}
				else if (orden == 1)
				{
					Admin.TareaCambiarOrden(orden += 1);
					await APIs.GamersGate.Tienda.BuscarOfertas();
				}
				else if (orden == 2)
				{
					Admin.TareaCambiarOrden(orden += 1);
					await APIs.Gamesplanet.Tienda.BuscarOfertasUk();
				}
				else if (orden == 3)
				{
					Admin.TareaCambiarOrden(orden += 1);
					await APIs.Gamesplanet.Tienda.BuscarOfertasFr();
				}
				else if (orden == 4)
				{
					Admin.TareaCambiarOrden(orden += 1);
					await APIs.Gamesplanet.Tienda.BuscarOfertasDe();
				}
				else if (orden == 5)
				{
					Admin.TareaCambiarOrden(orden += 1);
					await APIs.Gamesplanet.Tienda.BuscarOfertasUs();
				}
				else if (orden == 6)
				{
					Admin.TareaCambiarOrden(orden += 1);
					await APIs.Fanatical.Tienda.BuscarOfertas();
				}
				else if (orden == 7)
				{
					Admin.TareaCambiarOrden(orden += 1);
					await APIs.GreenManGaming.Tienda.BuscarOfertas();
				}
				else if (orden == 8)
				{
					Admin.TareaCambiarOrden(orden += 1);
					await APIs.GOG.Tienda.BuscarOfertas();
				}
				else if (orden == 9)
				{
					Admin.TareaCambiarOrden(orden += 1);
					await APIs.IndieGala.Tienda.BuscarOfertas();
				}
				else if (orden == 10)
				{
					Admin.TareaCambiarOrden(orden += 1);
					await APIs.WinGameStore.Tienda.BuscarOfertas();
				}
				else if (orden == 11)
				{
					Admin.TareaCambiarOrden(orden += 1);
					await APIs.EA.Tienda.BuscarOfertas();
				}
				else if (orden == 12)
				{
					Admin.TareaCambiarOrden(orden += 1);
					await APIs.DLGamer.Tienda.BuscarOfertas();
				}
				else if (orden == 13)
				{
					Admin.TareaCambiarOrden(orden += 1);
					await APIs.Battlenet.Tienda.BuscarOfertas();
				}
			}

			if (orden == 14)
			{
				Admin.TareaCambiarOrden(orden += 1);																	 
				Divisas.CogerDatos();
			}
			//else if (orden == 15)
			//{
			//	Admin.TareaCambiarOrden(orden += 1);
			//	await APIs.Steam.Tienda.BuscarOfertas(false);
			//}

			if (orden > 14)
			{
				Admin.TareaCambiarOrden(0);
			}
		}
    }
}
