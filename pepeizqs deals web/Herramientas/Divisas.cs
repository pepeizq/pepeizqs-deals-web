#nullable disable

using BaseDatos.Divisas;
using Microsoft.Data.SqlClient;
using System.Xml;

namespace Herramientas
{
	public enum JuegoMoneda
	{
		Euro,
		Dolar,
		Libra
	}

	public static class Divisas
	{
		public static async Task ActualizarDatos(SqlConnection conexion)
		{
			XmlDocument documento = new XmlDocument();

			try 
			{
				documento.Load("http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml");

				foreach (XmlNode nodo in documento.DocumentElement.ChildNodes[2].ChildNodes[0].ChildNodes)
				{
					if (nodo.Attributes["rate"].Value != null)
					{
						if (nodo.Attributes["currency"].Value == "USD")
						{
							Divisa dolar = new Divisa
							{
								Id = "USD",
								Cantidad = Convert.ToDecimal(nodo.Attributes["rate"].Value),
								FechaActualizacion = DateTime.Now
							};

							if (Buscar.Ejecutar(dolar.Id) == null)
							{
								Insertar.Ejecutar(dolar, conexion);
							}
							else
							{
								Actualizar.Ejecutar(dolar, conexion);
							}
						}

						if (nodo.Attributes["currency"].Value == "GBP")
						{
							Divisa libra = new Divisa
							{
								Id = "GBP",
								Cantidad = Convert.ToDecimal(nodo.Attributes["rate"].Value),
								FechaActualizacion = DateTime.Now
							};

							if (Buscar.Ejecutar(libra.Id) == null)
							{
								Insertar.Ejecutar(libra, conexion);
							}
							else
							{
								Actualizar.Ejecutar(libra, conexion);
							}
						}
					}
				}
			}
			catch { }
		}

		public static string MensajeDolar()
		{
			string mensaje = string.Empty;

			Divisa dolar = Buscar.Ejecutar("USD");

			if (dolar != null)
			{
				mensaje = "Dolar: " + dolar.Cantidad.ToString() + " " + Calculadora.HaceTiempo(dolar.FechaActualizacion, "es-ES");
			}

			return mensaje;
		}

		public static string MensajeLibra()
		{
			string mensaje = string.Empty;

			Divisa libra = Buscar.Ejecutar("GBP");

			if (libra != null)
			{
				mensaje = "Libra: " + libra.Cantidad.ToString() + " " + Calculadora.HaceTiempo(libra.FechaActualizacion, "es-ES");
			}

			return mensaje;
		}

		public static decimal Cambio(decimal cantidad, JuegoMoneda moneda)
		{
			string buscar = string.Empty;

            if (moneda == JuegoMoneda.Dolar)
            {
				buscar = "USD";
            }
			else if (moneda == JuegoMoneda.Libra)
			{
				buscar = "GBP";
			}

			if (buscar != string.Empty)
			{
				Divisa divisa = Buscar.Ejecutar(buscar);

				decimal temp = cantidad / divisa.Cantidad;

				temp = Math.Round(temp, 2);

				return temp;
			}

			return 0;
		}

		public static string DevolverSimbolo(decimal cantidad, JuegoMoneda moneda)
		{
			string precioTexto = string.Empty;

			if (moneda == JuegoMoneda.Dolar)
			{
				precioTexto = "$" + cantidad.ToString();
			}
			else if (moneda == JuegoMoneda.Libra)
			{
				precioTexto = "£" + cantidad.ToString();
			}

			if (precioTexto.Contains(".") == true)
			{
				int int1 = precioTexto.IndexOf(".");

				if (int1 == precioTexto.Length - 2)
				{
					precioTexto = precioTexto + "0";
				}
			}

			return precioTexto;
		}
	}

	public class Divisa
	{
		public string Id;
		public decimal Cantidad;
		public DateTime FechaActualizacion;
	}
}
