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
				Tienda = "Fanatical",
				EnlaceBase = "fanatical.com",
				Pick = false
			};

			DateTime fechaBundle = DateTime.Now;
			fechaBundle = fechaBundle.AddDays(14);
			fechaBundle = new DateTime(fechaBundle.Year, fechaBundle.Month, fechaBundle.Day, 17, 0, 0);

			bundle.FechaTermina = fechaBundle;

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
									bundle.Nombre = WebUtility.HtmlDecode(juego.Nombre);

									string imagen = juego.Imagen;
									imagen = imagen.Replace("/400x225/", "/1280x720/");
									bundle.Imagen = imagen;

									DateTime fechaTermina = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
									fechaTermina = fechaTermina.AddSeconds(Convert.ToDouble(juego.FechaTermina));
									fechaTermina = fechaTermina.ToLocalTime();

									bundle.FechaTermina = Convert.ToDateTime(fechaTermina);

									if (juego.Bundle.Tier1 != null)
									{
										if (juego.Bundle.Tier1.Juegos != null)
										{
											if (juego.Bundle.Tier1.Juegos.Count > 0)
											{
												if (bundle.Tiers == null)
												{
													bundle.Tiers = new List<Bundles2.BundleTier>();
												}

												Bundles2.BundleTier tier1 = new Bundles2.BundleTier
												{
													Posicion = 1,
													Precio = juego.PrecioRebajado.EUR
												};

												bundle.Tiers.Add(tier1);

												foreach (var juegob in juego.Bundle.Tier1.Juegos)
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
				}
			}

            return bundle;
		}
	}
}
