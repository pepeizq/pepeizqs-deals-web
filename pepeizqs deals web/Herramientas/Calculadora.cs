﻿using Juegos;

namespace Herramientas
{
	public enum Tiempo
	{
		Atemporal,
		Actual,
		Pasado
	}

	public static class Calculadora
	{
		public static int SacarDescuento(decimal precioBase, decimal precioRebajado)
		{
			int descuento = 0;

			if (precioBase > 0 && precioRebajado > 0) 
			{
				if (precioBase != precioRebajado)
				{
					decimal temp = (precioRebajado / precioBase) * 100;

					descuento = Decimal.ToInt32(100 - temp);
				}
			}

			return descuento;
		}

		public static string RedondearAnalisis(string idioma, string cantidad, bool incluirTexto = true)
		{
			int numCantidad = int.Parse(cantidad.Replace(",", null));
			string cantidadFinal = string.Empty;

			if (numCantidad > 0)
			{
				int divisor = 0;

				if (numCantidad > 0 && numCantidad < 100)
				{
					divisor = 10;
				}
				else if (numCantidad > 99 && numCantidad < 1000)
				{
					divisor = 100;
				}
				else if (numCantidad > 999 && numCantidad < 10000)
				{
					divisor = 1000;
				}
				else if (numCantidad > 9999 && numCantidad < 100000)
				{
					divisor = 10000;
				}
				else if (numCantidad > 99999 && numCantidad < 1000000)
				{
					divisor = 100000;
				}
				else if (numCantidad > 999999 && numCantidad < 10000000)
				{
					divisor = 1000000;
				}
				else if (numCantidad > 9999999)
				{
					divisor = 10000000;
				}

				numCantidad = (int)(Math.Round(double.Parse(cantidad.Replace(",", null)) / divisor, 0) * divisor);

				if (incluirTexto == true)
				{
					cantidadFinal = numCantidad.ToString("N0") + "+ " + Idiomas.BuscarTexto(idioma, "String9", "Game");
				}
				else
				{
					cantidadFinal = numCantidad.ToString("N0") + "+";
				}
			}

			return cantidadFinal;
		}

		public static string DiferenciaTiempo(this DateTime? fecha2, string? idioma = null, int modo = 0)
		{
            //modos
            //0 -> hace X tiempo
            //1 -> termina en X tiempo
            //2 -> terminan en X tiempo

			if (string.IsNullOrEmpty(idioma) == true)
			{
				idioma = "en";
			}

			DateTime fecha = new DateTime();

			if (fecha2 == null)
			{
				fecha = DateTime.Now;
			}
			else
			{
				fecha = (DateTime)fecha2;
			}

            string mensaje = string.Empty;
			TimeSpan diferenciaTiempo = new TimeSpan();
			
			if (modo == 0)
			{
				diferenciaTiempo = DateTime.Now.Subtract(fecha);
            }
			else
			{
				diferenciaTiempo = fecha.Subtract(DateTime.Now);
			}

			if (diferenciaTiempo <= TimeSpan.FromSeconds(60))
			{
				if (diferenciaTiempo.Seconds == 1)
				{
                    mensaje = string.Format(Idiomas.BuscarTexto(idioma, modo.ToString() + ".String0", "Calculator"), diferenciaTiempo.Seconds);
                }
				else
				{
                    mensaje = string.Format(Idiomas.BuscarTexto(idioma, modo.ToString() + ".String1", "Calculator"), diferenciaTiempo.Seconds);
                }				
			}
			else if (diferenciaTiempo <= TimeSpan.FromMinutes(60))
			{
				if (diferenciaTiempo.Minutes == 1)
				{
					mensaje = Idiomas.BuscarTexto(idioma, modo.ToString() + ".String3", "Calculator");
				}
				else if (diferenciaTiempo.Minutes > 1)
				{
					mensaje = string.Format(Idiomas.BuscarTexto(idioma, modo.ToString() + ".String2", "Calculator"), diferenciaTiempo.Minutes);
				}
			}
			else if (diferenciaTiempo <= TimeSpan.FromHours(24))
			{
				if (diferenciaTiempo.Hours == 1)
				{
					mensaje = Idiomas.BuscarTexto(idioma, modo.ToString() + ".String5", "Calculator");
				}
				else if (diferenciaTiempo.Hours > 1)
				{
					mensaje = string.Format(Idiomas.BuscarTexto(idioma, modo.ToString() + ".String4", "Calculator"), diferenciaTiempo.Hours);
				}
			}
			else if (diferenciaTiempo < TimeSpan.FromDays(31))
			{
				if (diferenciaTiempo.Days == 1)
				{
					if (diferenciaTiempo.Hours == 0)
					{
                        mensaje = Idiomas.BuscarTexto(idioma, modo.ToString() + ".String17", "Calculator");
                    }
					else if (diferenciaTiempo.Hours == 1)
					{
						mensaje = Idiomas.BuscarTexto(idioma, modo.ToString() + ".String16", "Calculator");
                    }
					else if (diferenciaTiempo.Hours > 1)
					{
                        mensaje = string.Format(Idiomas.BuscarTexto(idioma, modo.ToString() + ".String7", "Calculator"), diferenciaTiempo.Hours);
                    }					
				}
				else if (diferenciaTiempo.Days > 1)
				{
					mensaje = string.Format(Idiomas.BuscarTexto(idioma, modo.ToString() + ".String6", "Calculator"), diferenciaTiempo.Days);
				}
			}
			else if (diferenciaTiempo <= TimeSpan.FromDays(365))
			{
				if (diferenciaTiempo.Days > 30 && diferenciaTiempo.Days < 60)
				{
					int dias = diferenciaTiempo.Days - 30;

					if (dias > 1)
					{
                        mensaje = string.Format(Idiomas.BuscarTexto(idioma, modo.ToString() + ".String9", "Calculator"), dias);
                    }
                    else if (dias == 1)
					{
                        mensaje = Idiomas.BuscarTexto(idioma,  modo.ToString() + ".String14", "Calculator");
                    }
					else
					{
                        mensaje = Idiomas.BuscarTexto(idioma, modo.ToString() + ".String15", "Calculator");
                    }
                }
				else if (diferenciaTiempo.Days >= 60)
				{
					mensaje = string.Format(Idiomas.BuscarTexto(idioma, modo.ToString() + ".String8", "Calculator"), diferenciaTiempo.Days / 30);
				}
			}
			else
			{
				if (diferenciaTiempo.Days > 365 && diferenciaTiempo.Days < 730)
				{
					int meses = (diferenciaTiempo.Days - 365) / 30;

					if (meses > 1) 
					{
                        mensaje = string.Format(Idiomas.BuscarTexto(idioma, modo.ToString() + ".String11", "Calculator"), meses);
                    }
                    else if (meses == 1)
                    {
						mensaje = Idiomas.BuscarTexto(idioma, modo.ToString() + ".String12", "Calculator");
                    }
					else
					{
						mensaje = Idiomas.BuscarTexto(idioma, modo.ToString() + ".String13", "Calculator");
                    }
                }
				else if (diferenciaTiempo.Days >= 730)
				{
					mensaje = string.Format(Idiomas.BuscarTexto(idioma, modo.ToString() + ".String10", "Calculator"), diferenciaTiempo.Days / 365);
				}
			}

			return mensaje;
		}

        public static string DiferenciaDuranteDias(DateTime fechaEmpieza, DateTime fechaTermina, string idioma)
		{
            string mensaje = string.Empty;
            TimeSpan diferenciaTiempo = new TimeSpan();
            diferenciaTiempo = fechaTermina.Subtract(fechaEmpieza);

			if (diferenciaTiempo.Days == 1)
			{
                mensaje = Idiomas.BuscarTexto(idioma, "3.String1", "Calculator");
            }
			else if (diferenciaTiempo.Days > 1)
			{
                mensaje = string.Format(Idiomas.BuscarTexto(idioma, "3.String2", "Calculator"), diferenciaTiempo.Days);
            }
			else
			{
				mensaje = Idiomas.BuscarTexto(idioma, "3.String1", "Calculator");
			}

			return mensaje;
        }
	}
}
