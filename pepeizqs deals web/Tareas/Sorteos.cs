#nullable disable

using BaseDatos.Tiendas;
using Herramientas;
using Microsoft.Data.SqlClient;
using Sorteos2;

namespace Tareas
{
	public class Sorteos
	{
		public static async Task Ejecutar(SqlConnection conexion)
		{
			await Task.Delay(1000);

			TimeSpan tiempoSiguiente = TimeSpan.FromMinutes(10);

			if (Admin.ComprobarTareaUso(conexion, "sorteos", tiempoSiguiente) == true)
			{
				Admin.ActualizarTareaUso(conexion, "sorteos", DateTime.Now);

				List<Sorteo> listaSorteos = BaseDatos.Sorteos.Buscar.Todos(conexion);

				if (listaSorteos != null)
				{
					if (listaSorteos.Count > 0)
					{
						foreach (var sorteo in listaSorteos)
						{
							if (DateTime.Now > sorteo.FechaTermina && string.IsNullOrEmpty(sorteo.GanadorId) == true)
							{
								if (sorteo.Participantes != null)
								{
									if (sorteo.Participantes.Count > 0)
									{
										Random rnd = new Random();
										int ganador = rnd.Next(0, sorteo.Participantes.Count);
										string usuarioId = sorteo.Participantes[ganador];

										string correo = BaseDatos.Usuarios.Buscar.UnUsuarioCorreo(conexion, usuarioId);

										if (string.IsNullOrEmpty(correo) == false)
										{
											Juegos.Juego juego = BaseDatos.Juegos.Buscar.UnJuego(sorteo.JuegoId.ToString());

											BaseDatos.Usuarios.Clave nuevaClave = new BaseDatos.Usuarios.Clave
											{
												Nombre = juego.Nombre,
												JuegoId = juego.Id.ToString(),
												Codigo = sorteo.Clave
											};

											BaseDatos.Usuarios.Actualizar.Claves(conexion, usuarioId, nuevaClave);
											BaseDatos.Sorteos.Actualizar.Ganador(sorteo, conexion, usuarioId);
											Correos.EnviarGanadorSorteo(juego, sorteo, correo);
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
