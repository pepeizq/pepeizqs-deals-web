#nullable disable

using Herramientas;
using Microsoft.Data.SqlClient;

namespace Tareas
{
	public class Divisas
	{
		public static async Task Ejecutar(SqlConnection conexion)
		{
			TimeSpan tiempo = TimeSpan.FromDays(1);

			Divisa dolar = BaseDatos.Divisas.Buscar.Ejecutar(conexion, "USD");

			DateTime ultimaComprobacion = dolar.FechaActualizacion;

			if (DateTime.Now - ultimaComprobacion > tiempo)
			{
				await Herramientas.Divisas.ActualizarDatos(conexion);
			}
		}
	}
}
