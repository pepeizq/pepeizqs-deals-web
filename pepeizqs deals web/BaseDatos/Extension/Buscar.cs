#nullable disable

using Juegos;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace BaseDatos.Extension
{
	public class Extension
	{
		public int Id { get; set; }
		public string Nombre { get; set; }
		public List<JuegoPrecio> MinimosHistoricos { get; set; }
		public List<JuegoPrecio> PreciosActuales { get; set; }
		public List<JuegoBundle> Bundles { get; set; }
		public List<JuegoGratis> Gratis { get; set; }
		public List<JuegoSuscripcion> Suscripciones { get; set; }
		public int IdSteam { get; set; }
		public int IdGOG { get; set; }
		public string SlugGOG { get; set; }
		public string SlugEpic { get; set; }
		public double Dolar { get; set; }
		public double Libra { get; set; }
	}

	public static class Buscar
	{
		public static Extension Steam2(string id, SqlConnection conexion = null)
		{
			string buscar = "SELECT id, nombre, precioMinimosHistoricos, precioActualesTiendas, bundles, gratis, suscripciones, idSteam, idGOG, slugGOG, slugEpic FROM juegos WHERE idSteam=@idSteam";

			return GenerarDatos(buscar, "@idSteam", id, conexion);
		}

		public static Extension Gog2(string slug, SqlConnection conexion = null)
		{
			string buscar = "SELECT id, nombre, precioMinimosHistoricos, precioActualesTiendas, bundles, gratis, suscripciones, idSteam, idGOG, slugGOG, slugEpic FROM juegos WHERE slugGOG=@slugGOG";

			return GenerarDatos(buscar, "@slugGOG", slug, conexion);
		}

		public static Extension EpicGames2(string slug, SqlConnection conexion = null)
		{
			string buscar = "SELECT id, nombre, precioMinimosHistoricos, precioActualesTiendas, bundles, gratis, suscripciones, idSteam, idGOG, slugGOG, slugEpic FROM juegos WHERE slugEpic=@slugEpic";

			return GenerarDatos(buscar, "@slugEpic", slug, conexion);
		}

		private static Extension GenerarDatos(string buscar, string parametro, string valor, SqlConnection conexion = null)
		{
			if (conexion == null)
			{
				conexion = Herramientas.BaseDatos.Conectar();
			}
			else
			{
				if (conexion.State != System.Data.ConnectionState.Open)
				{
					conexion = Herramientas.BaseDatos.Conectar();
				}
			}

			Extension extension = new Extension();
			bool buscarDivisas = false;

			using (SqlCommand comando = new SqlCommand(buscar, conexion))
			{
				comando.Parameters.AddWithValue(parametro, valor);

				using (SqlDataReader lector = comando.ExecuteReader())
				{
					if (lector.Read() == true)
					{
						if (lector.IsDBNull(0) == false)
						{
							extension.Id = lector.GetInt32(0);
						}

						if (lector.IsDBNull(1) == false)
						{
							if (string.IsNullOrEmpty(lector.GetString(1)) == false)
							{
								extension.Nombre = lector.GetString(1);
							}
						}

						if (lector.IsDBNull(2) == false)
						{
							if (string.IsNullOrEmpty(lector.GetString(2)) == false)
							{
								extension.MinimosHistoricos = JsonSerializer.Deserialize<List<JuegoPrecio>>(lector.GetString(2));

								if (extension.MinimosHistoricos != null)
								{
									if (extension.MinimosHistoricos.Count > 0)
									{
										foreach (var precio in extension.MinimosHistoricos)
										{
											if (precio.Moneda != Herramientas.JuegoMoneda.Euro)
											{
												buscarDivisas = true;
												break;
											}
										}
									}
								}
							}
						}

						if (lector.IsDBNull(3) == false)
						{
							if (string.IsNullOrEmpty(lector.GetString(3)) == false)
							{
								extension.PreciosActuales = JsonSerializer.Deserialize<List<JuegoPrecio>>(lector.GetString(3));

								if (extension.PreciosActuales != null)
								{
									if (extension.PreciosActuales.Count > 0)
									{
										foreach (var precio in extension.PreciosActuales)
										{
											if (precio.Moneda != Herramientas.JuegoMoneda.Euro)
											{
												buscarDivisas = true;
												break;
											}
										}
									}
								}
							}
						}

						if (lector.IsDBNull(4) == false)
						{
							if (string.IsNullOrEmpty(lector.GetString(4)) == false)
							{
								extension.Bundles = JsonSerializer.Deserialize<List<JuegoBundle>>(lector.GetString(4));
							}
						}

						if (lector.IsDBNull(5) == false)
						{
							if (string.IsNullOrEmpty(lector.GetString(5)) == false)
							{
								extension.Gratis = JsonSerializer.Deserialize<List<JuegoGratis>>(lector.GetString(5));
							}
						}

						if (lector.IsDBNull(6) == false)
						{
							if (string.IsNullOrEmpty(lector.GetString(6)) == false)
							{
								extension.Suscripciones = JsonSerializer.Deserialize<List<JuegoSuscripcion>>(lector.GetString(6));
							}
						}

						if (lector.IsDBNull(7) == false)
						{
							extension.IdSteam = lector.GetInt32(7);
						}

						if (lector.IsDBNull(8) == false)
						{
							extension.IdGOG = lector.GetInt32(8);
						}

						if (lector.IsDBNull(9) == false)
						{
							if (string.IsNullOrEmpty(lector.GetString(9)) == false)
							{
								extension.SlugGOG = lector.GetString(9);
							}
						}

						if (lector.IsDBNull(10) == false)
						{
							if (string.IsNullOrEmpty(lector.GetString(10)) == false)
							{
								extension.SlugEpic = lector.GetString(10);
							}
						}

						if (buscarDivisas == false)
						{
							return extension;
						}
					}
				}
			}

			string buscar2 = "SELECT id, cantidad FROM divisas WHERE id='USD' OR id='GBP'";

			using (SqlCommand comando = new SqlCommand(buscar2, conexion))
			{
				using (SqlDataReader lector = comando.ExecuteReader())
				{
					while (lector.Read() == true)
					{
						if (lector.IsDBNull(0) == false)
						{
							if (lector.GetString(0) == "USD")
							{
								extension.Dolar = double.Parse(lector.GetString(1));
							}

							if (lector.GetString(0) == "GBP")
							{
								extension.Libra = double.Parse(lector.GetString(1));
							}
						}
					}

					return extension;
				}
			}
		}
	}
}
