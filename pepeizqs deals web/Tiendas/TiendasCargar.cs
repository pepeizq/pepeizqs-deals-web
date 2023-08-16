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
				APIs.GOG.Tienda.Generar()
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
		}

		public static async void TareasGestionador(int orden, TimeSpan tiempoEntreTareas)
		{
            try
            {
                if (orden == 0)
                {
                    DateTime ultimaComprobacion = Admin.TareaLeerTienda(APIs.Steam.Tienda.Generar().Id);

                    if ((DateTime.Now - ultimaComprobacion) > tiempoEntreTareas)
                    {
                        await APIs.Steam.Tienda.BuscarOfertas(true);
                    }
                }
                else if (orden == 1)
                {
                    DateTime ultimaComprobacion = Admin.TareaLeerTienda(APIs.GamersGate.Tienda.Generar().Id);

                    if ((DateTime.Now - ultimaComprobacion) > tiempoEntreTareas)
                    {
                        await APIs.GamersGate.Tienda.BuscarOfertas();
                    }
                }
                else if (orden == 2)
                {
                    DateTime ultimaComprobacion = Admin.TareaLeerTienda(APIs.Gamesplanet.Tienda.GenerarUk().Id);

                    if ((DateTime.Now - ultimaComprobacion) > tiempoEntreTareas)
                    {
                        await APIs.Gamesplanet.Tienda.BuscarOfertasUk();
                    }
                }
                else if (orden == 3)
                {
                    DateTime ultimaComprobacion = Admin.TareaLeerTienda(APIs.Gamesplanet.Tienda.GenerarFr().Id);

                    if ((DateTime.Now - ultimaComprobacion) > tiempoEntreTareas)
                    {
                        await APIs.Gamesplanet.Tienda.BuscarOfertasFr();
                    }
                }
                else if (orden == 4)
                {
                    DateTime ultimaComprobacion = Admin.TareaLeerTienda(APIs.Gamesplanet.Tienda.GenerarDe().Id);

                    if ((DateTime.Now - ultimaComprobacion) > tiempoEntreTareas)
                    {
                        await APIs.Gamesplanet.Tienda.BuscarOfertasDe();
                    }
                }
                else if (orden == 5)
                {
                    DateTime ultimaComprobacion = Admin.TareaLeerTienda(APIs.Gamesplanet.Tienda.GenerarUs().Id);

                    if ((DateTime.Now - ultimaComprobacion) > tiempoEntreTareas)
                    {
                        await APIs.Gamesplanet.Tienda.BuscarOfertasUs();
                    }
                }
                else if (orden == 6)
                {
                    DateTime ultimaComprobacion = Admin.TareaLeerTienda(APIs.Fanatical.Tienda.Generar().Id);

                    if ((DateTime.Now - ultimaComprobacion) > tiempoEntreTareas)
                    {
                        await APIs.Fanatical.Tienda.BuscarOfertas();
                    }
                }
                else if (orden == 7)
                {
                    DateTime ultimaComprobacion = Admin.TareaLeerTienda(APIs.GreenManGaming.Tienda.Generar().Id);

                    if ((DateTime.Now - ultimaComprobacion) > tiempoEntreTareas)
                    {
                        await APIs.GreenManGaming.Tienda.BuscarOfertas();
                    }
                }
                else if (orden == 8)
                {
                    DateTime ultimaComprobacion = Admin.TareaLeerTienda(APIs.GOG.Tienda.Generar().Id);

                    if ((DateTime.Now - ultimaComprobacion) > tiempoEntreTareas)
                    {
                        await APIs.GOG.Tienda.BuscarOfertas();
                    }
                }
                else if (orden == 9)
                {
                    Divisas.CogerDatos();
                }
            }
            catch
            {

            }

        }
    }
}
