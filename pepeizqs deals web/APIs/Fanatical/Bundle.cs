#nullable disable

using Herramientas;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Net;

namespace APIs.Fanatical
{
	public static class Bundle
	{
		public static Bundles2.Bundle Generar()
		{
			Bundles2.Bundle bundle = new Bundles2.Bundle()
			{
				Tipo = Bundles2.BundleTipo.Fanatical,
				NombreTienda = "Fanatical",
				ImagenTienda = "/imagenes/bundles/fanatical_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/fanatical_icono.ico",
				EnlaceBase = "fanatical.com",
				Pick = false
			};

            DateTime fechaEmpieza = DateTime.Now;
			int hora = 17;

			if (fechaEmpieza.Hour == 16)
			{
				hora = 16;
			}

            fechaEmpieza = new DateTime(fechaEmpieza.Year, fechaEmpieza.Month, fechaEmpieza.Day, hora, 0, 0);

            bundle.FechaEmpieza = fechaEmpieza;

            DateTime fechaTermina = DateTime.Now;
			fechaTermina = fechaTermina.AddDays(21);
			fechaTermina = new DateTime(fechaTermina.Year, fechaTermina.Month, fechaTermina.Day, 17, 0, 0);

			bundle.FechaTermina = fechaTermina;

			return bundle;
		}

		public static string Referido(string enlace)
		{
			return enlace + "?ref=pepeizq";
		}

		public static async Task<Bundles2.Bundle> ExtraerDatos(Bundles2.Bundle bundle)
		{
			if (bundle.Enlace.Contains("/pick-and-mix/") == true)
			{
				bundle.Pick = true;

				string html = await Decompiladores.Estandar(bundle.Enlace);

				if (html != null)
				{
					if (html.Contains("<title>") == true)
					{
						int int1 = html.IndexOf("<title>");
						string temp1 = html.Remove(0, int1 + 7);

						int int2 = temp1.IndexOf("</title>");
						string temp2 = temp1.Remove(int2, temp1.Length - int2);

						if (temp2.Contains("|") == true)
						{
							int int3 = temp2.IndexOf("|");
							temp2 = temp2.Remove(int3, temp2.Length - int3);
						}

						bundle.NombreBundle = temp2.Trim();
					}

					if (html.Contains("<img srcset=") == true)
					{
						int int1 = html.LastIndexOf("<img srcset=");
						string temp1 = html.Remove(0, int1 + 7);

						int int2 = temp1.IndexOf("https://fanatical");
						string temp2 = temp1.Remove(0, int2);

						int int3 = temp2.IndexOf("?");
						string temp3 = temp2.Remove(int3, temp2.Length - int3);

						bundle.ImagenBundle = temp3.Trim();
						bundle.ImagenNoticia = temp3.Trim();
					}
				}
			}
            else
            {
				string html = await Decompiladores.Estandar("https://feed.fanatical.com/feed");

				if (html != null)
				{
					html = html.Replace("{" + Strings.ChrW(34) + "title" + Strings.ChrW(34), ",{" + Strings.ChrW(34) + "title" + Strings.ChrW(34));

					html = html.Remove(0, 1);
					html = "[" + html + "]";

					List<FanaticalJuego> juegos = JsonConvert.DeserializeObject<List<FanaticalJuego>>(html);

					if (juegos != null)
					{
						if (juegos.Count > 0)
						{
							foreach (var juego in juegos)
							{
								if (juego.Enlace == bundle.Enlace)
								{
									bundle.NombreBundle = WebUtility.HtmlDecode(juego.Nombre);

									string imagen = juego.Imagen;
									imagen = imagen.Replace("/400x225/", "/1280x720/");
									bundle.ImagenBundle = imagen;
									bundle.ImagenNoticia = imagen;

									DateTime fechaTermina = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
									fechaTermina = fechaTermina.AddSeconds(Convert.ToDouble(juego.FechaTermina));
									fechaTermina = fechaTermina.ToLocalTime();

									bundle.FechaTermina = Convert.ToDateTime(fechaTermina);

									if (bundle.Tiers == null)
									{
										bundle.Tiers = new List<Bundles2.BundleTier>();

										Bundles2.BundleTier tier1 = new Bundles2.BundleTier
										{
											Posicion = 1,
											Precio = juego.PrecioRebajado.EUR
										};

										bundle.Tiers.Add(tier1);
									}

									ComprobarTierBundle(bundle, juego.Bundle.Tier1);
									ComprobarTierBundle(bundle, juego.Bundle.Tier2);
									ComprobarTierBundle(bundle, juego.Bundle.Tier3);
								}
							}
						}
					}
				}
			}

            return bundle;
		}
	
		private static void ComprobarTierBundle(Bundles2.Bundle bundle, FanaticalJuegoBundleTier tier)
		{
			if (tier != null)
			{
				if (tier.Juegos != null)
				{
					if (tier.Juegos.Count > 0)
					{
						foreach (var juegob in tier.Juegos)
						{
							Juegos.Juego juegoc = BaseDatos.Juegos.Buscar.UnJuego(null, juegob.SteamId);

							if (juegoc != null)
							{
								if (bundle.Juegos == null)
								{
									bundle.Juegos = new List<Bundles2.BundleJuego>();
								}

								Bundles2.BundleJuego juegod = new Bundles2.BundleJuego();

								juegod.JuegoId = juegoc.Id.ToString();
								juegod.Nombre = juegoc.Nombre;
								juegod.Imagen = juegoc.Imagenes.Capsule_231x87;
								juegod.DRM = Juegos.JuegoDRM.Steam;
								juegod.Tier = bundle.Tiers[0];

								bool añadir = true;

								if (bundle.Juegos.Count > 0)
								{
									foreach (var juegoe in bundle.Juegos)
									{
										if (juegoe.JuegoId == juegod.JuegoId)
										{
											añadir = false;
										}
									}
								}

								if (añadir == true)
								{
									bundle.Juegos.Add(juegod);
								}
							}
						}
					}
				}
			}
		}
	}
}
