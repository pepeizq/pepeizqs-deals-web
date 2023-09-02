#nullable disable

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace pepeizqs_deals_web.Pages
{
    public class BundleModel : PageModel
    {
		public string idioma = string.Empty;

		public Bundles2.Bundle bundle = new Bundles2.Bundle();

		public void OnGet()
        {
			try
			{
				idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();
			}
			catch { }

			string id = Request.Query["id"];

			if (id != null)
			{
				bundle = BaseDatos.Bundles.Buscar.UnBundle(int.Parse(id));
			}
		}

		public string MostrarFechaTermina(DateTime fecha)
		{
			if (fecha.Year > 2022)
			{
				TimeSpan diferenciaTiempo = fecha.Subtract(DateTime.Now);

				if (diferenciaTiempo.Days > 1)
				{
					return string.Format(Herramientas.Idiomas.CogerCadena(idioma, "Bundles.String4"), diferenciaTiempo.Days);
				}
				else if (diferenciaTiempo.Days == 1)
				{
					return string.Format(Herramientas.Idiomas.CogerCadena(idioma, "Bundles.String5"), diferenciaTiempo.Days);
				}
				else if (diferenciaTiempo.Days == 0 && diferenciaTiempo.Minutes > 0)
				{
					return string.Format(Herramientas.Idiomas.CogerCadena(idioma, "Bundles.String6"), diferenciaTiempo.Days);
				}
				else 
				{
					return string.Format(Herramientas.Idiomas.CogerCadena(idioma, "Bundles.String7"), diferenciaTiempo.Days);
				}
			}

			return null;
		}

		public string MostrarPrecio(Bundles2.BundleTier tier)
		{
			string mensaje = "Tier " + tier.Posicion.ToString() + " • ";

			string precio = tier.Precio;
			precio = precio.Replace(".", ",");
			precio = precio + "€";

			return mensaje + precio;
		}

		public List<Bundles2.BundleTier> OrdenarTiers(List<Bundles2.BundleTier> tiers)
		{
			tiers.Sort(delegate (Bundles2.BundleTier t1, Bundles2.BundleTier t2)
			{
				return t1.Posicion.CompareTo(t2.Posicion);
			});

			return tiers;
		}

		public string MostrarMensajePick(List<Bundles2.BundleTier> tiers)
		{
			string mensaje = string.Empty;

			foreach (var tier in tiers)
			{
				string precio = tier.Precio;
				precio = precio.Replace(".", ",");
				precio = precio + "€";

				mensaje = mensaje + "<div>" + tier.CantidadJuegos.ToString() + "  " + Herramientas.Idiomas.CogerCadena(idioma, "Bundles.String8") + " • " + precio + "</div>";
			}

			return mensaje;
		}
	}
}
