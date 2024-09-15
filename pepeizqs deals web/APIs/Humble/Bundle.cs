#nullable disable

using Herramientas;
using Microsoft.VisualBasic;
using System.Net;

namespace APIs.Humble
{
	public static class Bundle
	{
		public static Bundles2.Bundle Generar()
		{
			Bundles2.Bundle bundle = new Bundles2.Bundle()
			{
				Tipo = Bundles2.BundleTipo.HumbleBundle,
				NombreTienda = "Humble Bundle",
				ImagenTienda = "/imagenes/bundles/humblebundle_300x80.webp",
				ImagenIcono = "/imagenes/tiendas/humblestore_icono.ico",
				EnlaceBase = "humblebundle.com",
				Pick = false,
				ImagenesExtra = new List<string> { "/imagenes/bundles/humble_logo1.webp", "/imagenes/bundles/humble_logo2.webp" }
			};

			DateTime fechaEmpieza = DateTime.Now;
			fechaEmpieza = new DateTime(fechaEmpieza.Year, fechaEmpieza.Month, fechaEmpieza.Day, 20, 0, 0);

			bundle.FechaEmpieza = fechaEmpieza;

			DateTime fechaTermina = DateTime.Now;
			fechaTermina = fechaTermina.AddDays(21);
			fechaTermina = new DateTime(fechaTermina.Year, fechaTermina.Month, fechaTermina.Day, 20, 0, 0);

			bundle.FechaTermina = fechaTermina;

			return bundle;
		}

		public static string Referido(string enlace)
		{
            enlace = enlace + "?partner=pepeizq";

            enlace = enlace.Replace(":", "%3A");
            enlace = enlace.Replace("/", "%2F");
            enlace = enlace.Replace("/", "%2F");
            enlace = enlace.Replace("?", "%3F");
            enlace = enlace.Replace("=", "%3D");

            return "https://humblebundleinc.sjv.io/c/1382810/2059850/25796?u=" + enlace;
        }

		public static async Task<Bundles2.Bundle> ExtraerDatos(Bundles2.Bundle bundle)
		{
            if (bundle.Enlace != "https://www.humblebundle.com/")
            {
				string html = await Decompiladores.Estandar(bundle.Enlace);

				if (string.IsNullOrEmpty(html) == false)
				{
					if (html.Contains("<script type=" + Strings.ChrW(34) + "application/ld+json" + Strings.ChrW(34) + ">") == true)
					{
						int int1 = html.IndexOf("<script type=" + Strings.ChrW(34) + "application/ld+json" + Strings.ChrW(34) + ">");
						string temp1 = html.Remove(0, int1 + 10);

						int int2 = temp1.IndexOf("</script>");
						string temp2 = temp1.Remove(int2, temp1.Length - int2);

						int int3 = temp2.IndexOf(Strings.ChrW(34) + "name" + Strings.ChrW(34));

						if (int3 > -1)
						{
							//Nuevos

							#region Nombre

							string temp3 = temp2.Remove(0, int3 + 7);

							int int4 = temp3.IndexOf(Strings.ChrW(34));
							string temp4 = temp3.Remove(0, int4 + 1);

							int int5 = temp4.IndexOf(Strings.ChrW(34));
							string temp5 = temp4.Remove(int5, temp4.Length - int5);

							bundle.NombreBundle = WebUtility.HtmlDecode(temp5.Trim());

							#endregion

							#region ImagenLogo

							int int6 = temp2.IndexOf(Strings.ChrW(34) + "image" + Strings.ChrW(34));
							string temp6 = temp2.Remove(0, int6 + 8);

							int int7 = temp6.IndexOf(Strings.ChrW(34));
							string temp7 = temp6.Remove(0, int7 + 1);

							int int8 = temp7.IndexOf(Strings.ChrW(34));
							string temp8 = temp7.Remove(int8, temp7.Length - int8);

							bundle.ImagenBundle = WebUtility.HtmlDecode(temp8.Trim());

							#endregion

							List<Bundles2.BundleTier> tiers = new List<Bundles2.BundleTier>();

							#region Tier1

							int int9 = temp2.IndexOf(Strings.ChrW(34) + "highPrice" + Strings.ChrW(34));
							string temp9 = temp2.Remove(0, int9 + 11);

							int int10 = temp9.IndexOf(Strings.ChrW(34));
							string temp10 = temp9.Remove(0, int10 + 1);

							int int11 = temp10.IndexOf(Strings.ChrW(34));
							string temp11 = temp10.Remove(int11, temp10.Length - int11);

							Bundles2.BundleTier tier1 = new Bundles2.BundleTier
							{
								Posicion = 1,
								Precio = temp11.Trim()
							};

							tiers.Add(tier1);

							#endregion

							#region TierUltimo

							int int12 = temp2.IndexOf(Strings.ChrW(34) + "offerCount" + Strings.ChrW(34));
							string temp12 = temp2.Remove(0, int12 + 13);

							int int13 = temp12.IndexOf(Strings.ChrW(34));
							string temp13 = temp12.Remove(0, int13 + 1);

							int int14 = temp13.IndexOf(Strings.ChrW(34));
							string temp14 = temp13.Remove(int14, temp13.Length - int14);

							int int15 = temp2.IndexOf(Strings.ChrW(34) + "lowPrice" + Strings.ChrW(34));
							string temp15 = temp2.Remove(0, int15 + 11);

							int int16 = temp15.IndexOf(Strings.ChrW(34));
							string temp16 = temp15.Remove(0, int16 + 1);

							int int17 = temp16.IndexOf(Strings.ChrW(34));
							string temp17 = temp16.Remove(int17, temp16.Length - int17);

							Bundles2.BundleTier tier2 = new Bundles2.BundleTier
							{
								Posicion = int.Parse(temp14.Trim()),
								Precio = temp17.Trim()
							};

							bool añadir = true;

							if (tiers.Count > 0)
							{
								foreach (var tier in tiers)
								{
									if (tier.Precio == tier2.Precio)
									{
										añadir = false;
									}
								}
							}

							if (añadir == true)
							{
								tiers.Add(tier2);
							}

							#endregion

							bundle.Tiers = tiers;

							#region ImagenNoticia

							int int18 = html.IndexOf("itemprop=" + Strings.ChrW(34) + "image" + Strings.ChrW(34));

							if (int18 != -1)
							{
								string temp18 = html.Remove(int18, html.Length - int18);

								int int19 = temp18.LastIndexOf("<meta content=");
								string temp19 = temp18.Remove(0, int19 + 2);

								int int20 = temp19.IndexOf(Strings.ChrW(34));
								string temp20 = temp19.Remove(0, int20 + 1);

								temp20 = temp20.Replace(Strings.ChrW(34).ToString(), null);
								temp20 = temp20.Trim();

								bundle.ImagenNoticia = temp20;
							}

							#endregion
						}
						else
						{
							//Antiguos

							#region Nombre

							int int4 = html.IndexOf("<title>");
							string temp4 = html.Remove(0, int4 + 7);

							int int5 = temp4.IndexOf("</title>");
							string temp5 = temp4.Remove(int5, temp4.Length - int5);

							bundle.NombreBundle = WebUtility.HtmlDecode(temp5.Trim());

							#endregion

							#region ImagenLogo

							int int6 = html.IndexOf(Strings.ChrW(34) + "overpage-logo" + Strings.ChrW(34));
							string temp6 = html.Remove(0, int6 + 8);

							int int7 = temp6.IndexOf("src=");
							string temp7 = temp6.Remove(0, int7 + 5);

							int int8 = temp7.IndexOf(Strings.ChrW(34));
							string temp8 = temp7.Remove(int8, temp7.Length - int8);

							bundle.ImagenBundle = WebUtility.HtmlDecode(temp8.Trim());

							#endregion

							#region ImagenNoticia

							int int18 = html.IndexOf("itemprop=" + Strings.ChrW(34) + "image" + Strings.ChrW(34));

							if (int18 != -1)
							{
								string temp18 = html.Remove(int18, html.Length - int18);

								int int19 = temp18.LastIndexOf("<meta content=");
								string temp19 = temp18.Remove(0, int19 + 2);

								int int20 = temp19.IndexOf(Strings.ChrW(34));
								string temp20 = temp19.Remove(0, int20 + 1);

								temp20 = temp20.Replace(Strings.ChrW(34).ToString(), null);
								temp20 = temp20.Trim();

								bundle.ImagenNoticia = temp20;
							}

							#endregion
						}
					}
				}
			}

			return bundle;
		}
	}
}
