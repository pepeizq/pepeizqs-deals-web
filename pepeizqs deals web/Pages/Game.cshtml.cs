#nullable disable

using Bundles2;
using Gratis2;
using Herramientas;
using Juegos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using Suscripciones2;

namespace pepeizqs_deals_web.Pages
{
    public class GameModel : PageModel
    {
		public string idioma = string.Empty;

		public Juego juego = JuegoCrear.Generar();

		public void OnGet()
        {
			try
			{
				idioma = Request.Headers["Accept-Language"].ToString().Split(";").FirstOrDefault()?.Split(",").FirstOrDefault();
			}
			catch { }

			string id = Request.Query["id"];
			string idSteam = Request.Query["idSteam"];
			string idGog = Request.Query["idGog"];

			juego = BaseDatos.Juegos.Buscar.UnJuego(id, idSteam, idGog);
		}

		public bool VerificarMostrarDRM(string idioma, JuegoDRM drm, Juego juego)
		{
			bool mostrar = false;

			if (JuegoFicha.CogerMinimoDRM(idioma, drm, juego.PrecioMinimosHistoricos, juego.PrecioActualesTiendas, false) != null)
			{
				mostrar = true;
			}
			else if (PrepararBundles(idioma, juego.Bundles, drm) != null)
			{
				mostrar = true;
			}
			else if (PrepararGratis(idioma, juego.Gratis, drm) != null)
			{
				mostrar = true;
			}
			else if (PrepararSuscripcion(idioma, juego.Suscripciones, drm) != null)
			{
				mostrar = true;
			}

			return mostrar;
		}

		public string PrepararBundles(string idioma, List<JuegoBundle> listaBundles, JuegoDRM drm)
		{
			if (listaBundles != null)
			{
				if (listaBundles.Count > 0)
				{
					string mensaje = null;
					List<JuegoBundle> bundlesDisponibles = new List<JuegoBundle>();

					foreach (var bundle in listaBundles)
					{
						if (drm == bundle.DRM)
						{
							if (DateTime.Now >= bundle.FechaEmpieza && DateTime.Now <= bundle.FechaTermina)
							{
								bundlesDisponibles.Add(bundle);								
							}
						}
					}

					if (bundlesDisponibles.Count > 0)
					{
						if (bundlesDisponibles.Count == 1)
						{
							mensaje = Idiomas.CogerCadena(idioma, "Game.String19") + " <a href=" + Strings.ChrW(34) + EnlaceAcortador.Generar(bundlesDisponibles[0].Enlace, bundlesDisponibles[0].Tipo) + Strings.ChrW(34) + " target=" + Strings.ChrW(34) + "_blank" + Strings.ChrW(34) + ">" + BaseDatos.Bundles.Buscar.UnBundle(bundlesDisponibles[0].BundleId).NombreBundle + " (" + BundlesCargar.DevolverBundle(bundlesDisponibles[0].Tipo).NombreTienda + ")</a>";
						}
						else if (bundlesDisponibles.Count > 1)
						{
							mensaje = Idiomas.CogerCadena(idioma, "Game.String20") + " <ul>";

							foreach (var bundle in bundlesDisponibles) 
							{
								mensaje = mensaje + "<li><a href=" + Strings.ChrW(34) + EnlaceAcortador.Generar(bundle.Enlace, bundle.Tipo) + Strings.ChrW(34) + " target=" + Strings.ChrW(34) + "_blank" + Strings.ChrW(34) + ">" + BaseDatos.Bundles.Buscar.UnBundle(bundle.BundleId).NombreBundle + " (" + BundlesCargar.DevolverBundle(bundle.Tipo).NombreTienda + ")</a></li>";
							}

							mensaje = mensaje + "</ul>";
						}
					}

					if (mensaje == null)
					{
						List<JuegoBundle> bundlesAntiguos = new List<JuegoBundle>();

						foreach (var bundle in listaBundles)
						{
							if (drm == bundle.DRM)
							{
								if (DateTime.Now > bundle.FechaTermina)
								{
									bundlesAntiguos.Add(bundle);
								}
							}
						}

						if (bundlesAntiguos.Count > 0) 
						{
							mensaje = Idiomas.CogerCadena(idioma, "Game.String21") + " ";

							for (int i = 0; i < bundlesAntiguos.Count; i += 1)
							{
								if (i > 0)
								{
									mensaje = mensaje + ", ";
								}

								mensaje = mensaje + "<a href=" + Strings.ChrW(34) + "/bundle/" + bundlesAntiguos[i].BundleId + Strings.ChrW(34) + " target=" + Strings.ChrW(34) + "_blank" + Strings.ChrW(34) + ">" + BaseDatos.Bundles.Buscar.UnBundle(bundlesAntiguos[i].BundleId).NombreBundle + " (" + BundlesCargar.DevolverBundle(bundlesAntiguos[i].Tipo).NombreTienda + ")</a></li>";
							}
						}
					}

					if (mensaje != null)
					{
						mensaje = "<div class=" + Strings.ChrW(34) + "juego-minimo" + Strings.ChrW(34) + ">" + mensaje + "</div>";
					}

					return mensaje;
				}
			}

			return null;
		}

		public string PrepararGratis(string idioma, List<JuegoGratis> listaGratis, JuegoDRM drm)
		{
			if (listaGratis != null)
			{
				if (listaGratis.Count > 0)
				{
					int veces = 0;
					string mensaje = null;

					foreach (var gratis in listaGratis)
					{
						if (drm == gratis.DRM)
						{
							if (DateTime.Now >= gratis.FechaEmpieza && DateTime.Now <= gratis.FechaTermina)
							{
								mensaje = "<a href=" + Strings.ChrW(34) + EnlaceAcortador.Generar(gratis.Enlace, gratis.Tipo) + Strings.ChrW(34) + " target=" + Strings.ChrW(34) + "_blank" + Strings.ChrW(34) + ">" + Idiomas.CogerCadena(idioma, "Game.String16") + " " + GratisCargar.DevolverGratis(gratis.Tipo.ToString()).Nombre + "</a>";
							}
							else
							{
								veces += 1;

								if (veces == 1)
								{
									mensaje = Idiomas.CogerCadena(idioma, "Game.String17") + " " + GratisCargar.DevolverGratis(gratis.Tipo.ToString()).Nombre + " " + Calculadora.HaceTiempo(gratis.FechaTermina, idioma);
								}
								else if (veces > 1)
								{
									mensaje = string.Format(Idiomas.CogerCadena(idioma, "Game.String18"), veces);
								}
							}
						}
					}

					if (mensaje != null)
					{
						mensaje = "<div class=" + Strings.ChrW(34) + "juego-minimo" + Strings.ChrW(34) + ">" + mensaje + "</div>";
					}

					return mensaje;
				}
			}

			return null;
		}

		public string PrepararSuscripcion(string idioma, List<JuegoSuscripcion> listaSuscripciones, JuegoDRM drm)
		{
			if (listaSuscripciones != null)
			{
				if (listaSuscripciones.Count > 0)
				{
					string mensaje = null;

					foreach (var suscripcion in listaSuscripciones)
					{
						if (drm == suscripcion.DRM)
						{
							if (DateTime.Now >= suscripcion.FechaEmpieza && DateTime.Now <= suscripcion.FechaTermina)
							{
								mensaje = "<a href=" + Strings.ChrW(34) + EnlaceAcortador.Generar(suscripcion.Enlace, suscripcion.Tipo) + Strings.ChrW(34) + " target=" + Strings.ChrW(34) + "_blank" + Strings.ChrW(34) + ">" + Idiomas.CogerCadena(idioma, "Game.String14") + " " + SuscripcionesCargar.DevolverSuscripcion(suscripcion.Tipo.ToString()).Nombre + "</a>";
							}
							else
							{
								mensaje = Idiomas.CogerCadena(idioma, "Game.String15") + " " + SuscripcionesCargar.DevolverSuscripcion(suscripcion.Tipo).Nombre + " " + Calculadora.HaceTiempo(suscripcion.FechaTermina, idioma);
							}
						}
					}

					if (mensaje != null)
					{
						mensaje = "<div class=" + Strings.ChrW(34) + "juego-minimo" + Strings.ChrW(34) + ">" + mensaje + "</div>";
					}

					return mensaje;
				}
			}

			return null;
		}
	}
}
