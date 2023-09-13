﻿#nullable disable

using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace BaseDatos.Bundles
{
	public static class Buscar
	{
		public static List<Bundles2.Bundle> Todos(Herramientas.Tiempo tiempo)
		{
			List<Bundles2.Bundle> bundles = new List<Bundles2.Bundle>();

			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string busqueda = "SELECT * FROM bundles";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							Bundles2.Bundle bundle = new Bundles2.Bundle
							{
								Id = lector.GetInt32(0),
								Tipo = Bundles2.BundlesCargar.DevolverBundle(int.Parse(lector.GetString(1))).Tipo,
								NombreBundle = lector.GetString(2),
								NombreTienda = lector.GetString(3),
								ImagenBundle = lector.GetString(4),
								Enlace = lector.GetString(5),
								FechaEmpieza = Convert.ToDateTime(lector.GetString(6)),
								FechaTermina = Convert.ToDateTime(lector.GetString(7)),
								Juegos = JsonConvert.DeserializeObject<List<Bundles2.BundleJuego>>(lector.GetString(8)),
								Tiers = JsonConvert.DeserializeObject<List<Bundles2.BundleTier>>(lector.GetString(9)),
								Pick = Convert.ToBoolean(lector.GetString(10))
							};

							if (tiempo == Herramientas.Tiempo.Atemporal)
							{
								bundles.Add(bundle);
							}
							else if (tiempo == Herramientas.Tiempo.Actual)
							{
								if (DateTime.Now >= bundle.FechaEmpieza && DateTime.Now <= bundle.FechaTermina)
								{
									bundles.Add(bundle);
								}
							}
							else if (tiempo == Herramientas.Tiempo.Pasado)
							{
								if (DateTime.Now > bundle.FechaTermina)
								{
									bundles.Add(bundle);
								}
							}
						}
					}
				}
			}

			conexion.Dispose();

			return bundles;
		}

		public static List<Bundles2.Bundle> UnTipo(string tipo, Herramientas.Tiempo tiempo)
		{
			List<Bundles2.Bundle> bundles = new List<Bundles2.Bundle>();

			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string busqueda = "SELECT * FROM bundles";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							Bundles2.Bundle bundle = new Bundles2.Bundle
							{
								Id = lector.GetInt32(0),
								Tipo = Bundles2.BundlesCargar.DevolverBundle(int.Parse(lector.GetString(1))).Tipo,
								NombreBundle = lector.GetString(2),
								NombreTienda = lector.GetString(3),
								ImagenBundle = lector.GetString(4),
								Enlace = lector.GetString(5),
								FechaEmpieza = Convert.ToDateTime(lector.GetString(6)),
								FechaTermina = Convert.ToDateTime(lector.GetString(7)),
								Juegos = JsonConvert.DeserializeObject<List<Bundles2.BundleJuego>>(lector.GetString(8)),
								Tiers = JsonConvert.DeserializeObject<List<Bundles2.BundleTier>>(lector.GetString(9)),
								Pick = Convert.ToBoolean(lector.GetString(10))
							};

							if (tipo != "0")
							{
								if (bundle.Tipo == Bundles2.BundlesCargar.DevolverBundle(tipo).Tipo)
								{
									if (tiempo == Herramientas.Tiempo.Atemporal)
									{
										bundles.Add(bundle);
									}
									else if (tiempo == Herramientas.Tiempo.Actual)
									{
										if (DateTime.Now >= bundle.FechaEmpieza && DateTime.Now <= bundle.FechaTermina)
										{
											bundles.Add(bundle);
										}
									}
									else if (tiempo == Herramientas.Tiempo.Pasado)
									{
										if (DateTime.Now > bundle.FechaTermina)
										{
											bundles.Add(bundle);
										}
									}
								}
							}												
						}
					}
				}
			}

			conexion.Dispose();

			return bundles;
		}

		public static Bundles2.Bundle UnBundle(int bundleId)
		{
			SqlConnection conexion = Herramientas.BaseDatos.Conectar();

			using (conexion)
			{
				string busqueda = "SELECT * FROM bundles WHERE id=@id";

				using (SqlCommand comando = new SqlCommand(busqueda, conexion))
				{
					comando.Parameters.AddWithValue("@id", bundleId);

					using (SqlDataReader lector = comando.ExecuteReader())
					{
						while (lector.Read())
						{
							Bundles2.Bundle bundle = new Bundles2.Bundle
							{
								Id = lector.GetInt32(0),
								Tipo = Bundles2.BundlesCargar.DevolverBundle(int.Parse(lector.GetString(1))).Tipo,
								NombreBundle = lector.GetString(2),
								NombreTienda = lector.GetString(3),
								ImagenBundle = lector.GetString(4),
								Enlace = lector.GetString(5),
								FechaEmpieza = Convert.ToDateTime(lector.GetString(6)),
								FechaTermina = Convert.ToDateTime(lector.GetString(7)),
								Juegos = JsonConvert.DeserializeObject<List<Bundles2.BundleJuego>>(lector.GetString(8)),
								Tiers = JsonConvert.DeserializeObject<List<Bundles2.BundleTier>>(lector.GetString(9)),
								Pick = Convert.ToBoolean(lector.GetString(10))
							};

							return bundle;
						}
					}
				}
			}

			conexion.Dispose();

			return null;
		}
	}
}