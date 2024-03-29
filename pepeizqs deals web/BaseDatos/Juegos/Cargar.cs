﻿using Juegos;
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
				try
				{
					juego.Tipo = Enum.Parse<JuegoTipo>(lector.GetString(2));
				}
				catch { }				
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

			if (lector.IsDBNull(11) == false)
			{
				if (string.IsNullOrEmpty(lector.GetString(11)) == false)
				{
					try
					{
						juego.FechaSteamAPIComprobacion = DateTime.Parse(lector.GetString(11));
					}
					catch { }
				}
			}				

			if (lector.IsDBNull(12) == false)
			{
				if (string.IsNullOrEmpty(lector.GetString(12)) == false)
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
				if (string.IsNullOrEmpty(lector.GetString(13)) == false)
				{
					try
					{
						juego.Bundles = JsonConvert.DeserializeObject<List<JuegoBundle>>(lector.GetString(13));
					}
					catch { }
				}
			}

			if (lector.IsDBNull(14) == false)
			{
				if (string.IsNullOrEmpty(lector.GetString(14)) == false)
				{
					try
					{
						juego.Gratis = JsonConvert.DeserializeObject<List<JuegoGratis>>(lector.GetString(14));
					}
					catch { }
				}
			}

			if (lector.IsDBNull(15) == false)
			{
				if (string.IsNullOrEmpty(lector.GetString(15)) == false)
				{
					try
					{
						juego.NombreCodigo = lector.GetString(15);
					}
					catch { }
				}
			}

			try
			{
				if (lector.IsDBNull(16) == false)
				{
					try
					{
						juego.IdMaestra = lector.GetInt32(16);
					}
					catch { }
				}
			}
			catch { }

			if (lector.IsDBNull(17) == false)
			{
				if (string.IsNullOrEmpty(lector.GetString(17)) == false)
				{
					try
					{
						juego.UsuariosInteresados = JsonConvert.DeserializeObject<List<JuegoUsuariosInteresados>>(lector.GetString(17));
					}
					catch { }
				}
			}

			try
			{
				if (lector.IsDBNull(18) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(18)) == false)
					{
						juego.SlugGOG = lector.GetString(18);
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(19) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(19)) == false)
					{
						juego.Maestro = lector.GetString(19);
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(20) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(20)) == false)
					{
						juego.FreeToPlay = lector.GetString(20);
					}
				}
			}
			catch { }

			try
			{
				if (lector.IsDBNull(21) == false)
				{
					if (string.IsNullOrEmpty(lector.GetString(21)) == false)
					{
						juego.MayorEdad = lector.GetString(21);
					}
				}
			}
			catch { }

			return juego;
		}
	}
}
