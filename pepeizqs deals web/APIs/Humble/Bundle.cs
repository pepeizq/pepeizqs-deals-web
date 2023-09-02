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
				EnlaceBase = "humblebundle.com",
				Pick = false,
				ImagenesExtra = new List<string> { "/imagenes/bundles/humble_logo1.webp", "/imagenes/bundles/humble_logo2.webp" }
			};

			DateTime fechaBundle = DateTime.Now;
			fechaBundle = fechaBundle.AddDays(14);
			fechaBundle = new DateTime(fechaBundle.Year, fechaBundle.Month, fechaBundle.Day, 20, 0, 0);

			bundle.FechaTermina = fechaBundle;

			return bundle;
		}

		public static string Referido(string enlace)
		{
			return enlace + "?partner=pepeizq";
		}

		public static async Task<Bundles2.Bundle> ExtraerDatos(Bundles2.Bundle bundle)
		{
			string html = await Decompiladores.Estandar(bundle.Enlace);

			if (html != null)
			{
				if (html.Contains("<script type=" + Strings.ChrW(34) + "application/ld+json" + Strings.ChrW(34) + ">") == true)
				{
					int int1 = html.IndexOf("<script type=" + Strings.ChrW(34) + "application/ld+json" + Strings.ChrW(34) + ">");
					string temp1 = html.Remove(0, int1 + 10);

					int int2 = temp1.IndexOf("</script>");
					string temp2 = temp1.Remove(int2, temp1.Length - int2);

					#region Nombre

					int int3 = temp2.IndexOf(Strings.ChrW(34) + "name" + Strings.ChrW(34));
					string temp3 = temp2.Remove(0, int3 + 7);

					int int4 = temp3.IndexOf(Strings.ChrW(34));
					string temp4 = temp3.Remove(0, int4 + 1);

					int int5 = temp4.IndexOf(Strings.ChrW(34));
					string temp5 = temp4.Remove(int5, temp4.Length - int5);

					bundle.NombreBundle = WebUtility.HtmlDecode(temp5.Trim());

					#endregion

					#region Imagen

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

					tiers.Add(tier2);

					#endregion

					bundle.Tiers = tiers;
				}
			}

			return bundle;
		}
	}
}
