﻿using BaseDatos.Tiendas;
using Herramientas;
using Microsoft.Data.SqlClient;

namespace Tareas
{
    public class Solicitudes : BackgroundService
    {
        private readonly ILogger<Solicitudes> _logger;
        private readonly IServiceScopeFactory _factoria;
        private readonly IDecompiladores _decompilador;

        public Solicitudes(ILogger<Solicitudes> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
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
                SqlConnection conexion = new SqlConnection();

                try
                {
                    conexion = Herramientas.BaseDatos.Conectar();
                }
                catch { }

                if (conexion.State == System.Data.ConnectionState.Open)
                {
                    try
                    {
                        TimeSpan tiempoSiguiente = TimeSpan.FromMinutes(30);

                        if (Admin.ComprobarTareaUso(conexion, "solicitudes", tiempoSiguiente) == true)
                        {
                            Admin.ActualizarTareaUso(conexion, "solicitudes", DateTime.Now);

                            List<BaseDatos.Usuarios.SolicitudGrupo> solicitudes = BaseDatos.Usuarios.Solicitud.DevolverTodo(conexion);

                            if (solicitudes.Count > 0)
                            {
                                Admin.ActualizarDato(conexion, "solicitudes", solicitudes.Count.ToString());
                            }
                            else
                            {
                                Admin.ActualizarDato(conexion, "solicitudes", "0");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        BaseDatos.Errores.Insertar.Ejecutar("Tarea - Solicitudes", ex, conexion);
                    }
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }
    }
}
