#nullable disable

using BaseDatos.Divisas;
using System.Globalization;
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
		public static void Ejecutar()
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
								Insertar.Ejecutar(dolar);
							}
							else
							{
								Actualizar.Ejecutar(dolar);
							}							
						}
						else if (nodo.Attributes["currency"].Value == "GBP")
						{
							Divisa libra = new Divisa
							{
								Id = "GBP",
								Cantidad = Convert.ToDecimal(nodo.Attributes["rate"].Value),
								FechaActualizacion = DateTime.Now
							};

							if (Buscar.Ejecutar(libra.Id) == null)
							{
								Insertar.Ejecutar(libra);
							}
							else
							{
								Actualizar.Ejecutar(libra);
							}
						}
					}
				}
			}
			catch 
			{ 
			
			}
		}

		public static string Mensaje()
		{
			string mensaje = string.Empty;

			Divisa dolar = Buscar.Ejecutar("USD");

			if (dolar != null)
			{
				mensaje = mensaje + "Dolar: " + Calculadora.HaceTiempo(dolar.FechaActualizacion);
			}

			Divisa libra = Buscar.Ejecutar("GBP");

			if (libra != null)
			{
				if (mensaje.Length > 0)
				{
					mensaje = mensaje + " • ";
				}

				mensaje = mensaje + "Libra: " + Calculadora.HaceTiempo(libra.FechaActualizacion);
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
	}

	public class Divisa
	{
		public string Id;
		public decimal Cantidad;
		public DateTime FechaActualizacion;
	}
}
