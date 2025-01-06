﻿#nullable disable

using BaseDatos.Juegos;
using Herramientas;
using Juegos;
using Microsoft.Data.SqlClient;

namespace Tareas
{
    public class MinimoListado
    {
        public Juego Juego { get; set; }
        public JuegoPrecio Historico { get; set; }
    }

    public class Minimos : BackgroundService
    {
        private readonly ILogger<Minimos> _logger;
        private readonly IServiceScopeFactory _factoria;
        private readonly IDecompiladores _decompilador;

        public Minimos(ILogger<Minimos> logger, IServiceScopeFactory factory, IDecompiladores decompilador)
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
                WebApplicationBuilder builder = WebApplication.CreateBuilder();
				string piscinaTiendas = builder.Configuration.GetValue<string>("PoolTiendas:Contenido");
				string piscinaUsada = Environment.GetEnvironmentVariable("APP_POOL_ID", EnvironmentVariableTarget.Process);

				if (piscinaTiendas == piscinaUsada)
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
                            TimeSpan tiempoSiguiente = TimeSpan.FromMinutes(5);

                            if (BaseDatos.Admin.Buscar.TareaPosibleUsar("minimos", tiempoSiguiente, conexion) == true)
                            {
								BaseDatos.Admin.Actualizar.TareaUso("minimos", DateTime.Now, conexion);

                                List<Juego> juegos = BaseDatos.Portada.Buscar.Minimos(conexion);

                                if (juegos != null)
                                {
                                    if (juegos.Count > 0)
                                    {
                                        List<MinimoListado> juegosConMinimos = new List<MinimoListado>();

                                        foreach (var juego in juegos)
                                        {
                                            if (juego.PrecioMinimosHistoricos != null)
                                            {
                                                if (juego.PrecioMinimosHistoricos.Count > 0)
                                                {
                                                    List<JuegoPrecio> historicosFinales = new List<JuegoPrecio>();

                                                    foreach (var historico in juego.PrecioMinimosHistoricos)
                                                    {
                                                        if (Herramientas.OfertaActiva.Verificar(historico) == true)
                                                        {
                                                            bool añadir = true;

                                                            if (historico.DRM == JuegoDRM.NoEspecificado)
                                                            {
                                                                añadir = false;
                                                            }

                                                            if (añadir == true)
                                                            {
                                                                historicosFinales.Add(historico);
                                                            }
                                                        }
                                                    }

                                                    if (historicosFinales.Count > 0)
                                                    {
                                                        foreach (var historicoFinal in historicosFinales)
                                                        {
                                                            Juego nuevoJuego = new Juego();
                                                            nuevoJuego = juego;
                                                            nuevoJuego.PrecioMinimosHistoricos = null;
                                                            nuevoJuego.PrecioActualesTiendas = null;
                                                            nuevoJuego.IdMaestra = juego.Id;

                                                            bool añadir = true;

                                                            if (nuevoJuego.Gratis != null)
                                                            {
                                                                if (nuevoJuego.Gratis.Count > 0)
                                                                {
                                                                    foreach (var gratis in nuevoJuego.Gratis)
                                                                    {
                                                                        if (gratis.DRM == historicoFinal.DRM)
                                                                        {
                                                                            añadir = false;
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            if (añadir == true)
                                                            {
                                                                MinimoListado minimoListado = new MinimoListado();
                                                                minimoListado.Juego = nuevoJuego;
                                                                minimoListado.Historico = historicoFinal;

                                                                juegosConMinimos.Add(minimoListado);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (juegosConMinimos != null)
                                        {
                                            if (juegosConMinimos.Count > 0)
                                            {
                                                BaseDatos.Portada.Limpiar.Ejecutar("seccionMinimos", conexion);

                                                juegosConMinimos = juegosConMinimos.OrderByDescending(x => int.Parse(x.Juego.Analisis.Cantidad.Replace(",", null))).ThenBy(x => x.Juego.Nombre).ToList();

                                                foreach (var minimo in juegosConMinimos)
                                                {
                                                    try
                                                    {
                                                        Juego juegoMinimoFinal = minimo.Juego;
                                                        juegoMinimoFinal.PrecioMinimosHistoricos = [minimo.Historico];

                                                        BaseDatos.Juegos.Insertar.Ejecutar(juegoMinimoFinal, conexion, "seccionMinimos", true);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        BaseDatos.Errores.Insertar.Mensaje("Seccion Minimos " + minimo.Juego.Nombre, ex);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            BaseDatos.Errores.Insertar.Mensaje("Tarea - Minimos", ex, conexion);
                        }
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
