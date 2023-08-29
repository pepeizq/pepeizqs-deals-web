using Juegos;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace BaseDatos.Juegos
{
	public static class Cargar
	{
		public static Juego Ejecutar(Juego juego, SqlDataReader lector)
		{
			juego.Id = lector.GetInt32(0);
			juego.Nombre = lector.GetString(1);

			if (lector.GetString(2) != null)
			{
				juego.Tipo = Enum.Parse<JuegoTipo>(lector.GetString(2));
			}

			if (lector.GetString(3) != null)
			{
				try
				{
					juego.Imagenes = JsonConvert.DeserializeObject<JuegoImagenes>(lector.GetString(3));
				}
				catch { }
			}

			if (lector.GetString(4) != null)
			{
				try
				{
					juego.PrecioMinimosHistoricos = JsonConvert.DeserializeObject<List<JuegoPrecio>>(lector.GetString(4));
				}
				catch { }
			}

			if (lector.GetString(5) != null)
			{
				try
				{
					juego.PrecioActualesTiendas = JsonConvert.DeserializeObject<List<JuegoPrecio>>(lector.GetString(5));
				}
				catch { }
			}

			if (lector.GetString(6) != null)
			{
				try
				{
					juego.Analisis = JsonConvert.DeserializeObject<JuegoAnalisis>(lector.GetString(6));
				}
				catch { }
			}

			if (lector.GetString(7) != null)
			{
				try
				{
					juego.Caracteristicas = JsonConvert.DeserializeObject<JuegoCaracteristicas>(lector.GetString(7));
				}
				catch { }
			}

			if (lector.GetString(8) != null)
			{
				try
				{
					juego.Media = JsonConvert.DeserializeObject<JuegoMedia>(lector.GetString(8));
				}
				catch { }
			}

			juego.IdSteam = lector.GetInt32(9);
			juego.IdGog = lector.GetInt32(10);

			if (lector.GetString(11) != null)
			{
				try
				{
					juego.FechaSteamAPIComprobacion = DateTime.Parse(lector.GetString(11));
				}
				catch { }
			}

			if (lector.IsDBNull(12) == false)
			{
				if (lector.GetString(12) != null)
				{
					try
					{
						juego.Suscripciones = JsonConvert.DeserializeObject<List<JuegoSuscripcion>>(lector.GetString(12));
					}
					catch { }
				}
			}

			if (lector.IsDBNull(13) == false)
			{
				if (lector.GetString(13) != null)
				{
					try
					{
						//juego.Suscripciones = JsonConvert.DeserializeObject<List<JuegoSuscripcion>>(lector.GetString(12));
					}
					catch { }
				}
			}

			if (lector.IsDBNull(14) == false)
			{
				if (lector.GetString(14) != null)
				{
					try
					{
						juego.Gratis = JsonConvert.DeserializeObject<List<JuegoGratis>>(lector.GetString(14));
					}
					catch { }
				}
			}

			return juego;
		}
	}
}
