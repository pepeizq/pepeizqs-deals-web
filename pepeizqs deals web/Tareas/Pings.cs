#nullable disable

using BaseDatos.Tiendas;
using Herramientas;
using Microsoft.Data.SqlClient;

namespace Tareas
{
    public class Pings : BackgroundService
    {
        private readonly ILogger<Pings> _logger;
        private readonly IServiceScopeFactory _factoria;
        private readonly IDecompiladores _decompilador;

        public Pings(ILogger<Pings> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
        {
            _logger = logger;
            _factoria = factory;
            _decompilador = decompilador;
        }

        protected override async Task ExecuteAsync(CancellationToken tokenParar)
        {
            using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(60));

            while (await timer.WaitForNextTickAsync(tokenParar))
            {
                string html = await Decompiladores.Estandar("https://tareas.pepeizqapps.com/");
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }
    }
}
