﻿#nullable disable

using BaseDatos.Juegos;
using Microsoft.VisualBasic;

namespace Herramientas
{
	public static class Buscador
	{
		public static string LimpiarNombre(string nombre, bool quitarEspacio = false)
		{
			if (string.IsNullOrEmpty(nombre) == false)
			{
				if (quitarEspacio == true)
				{
					nombre = nombre.Replace(" ", null);
				}

				nombre = nombre.Replace("&#39;", "'");
				nombre = nombre.Replace("&amp;", "&");
				nombre = nombre.Replace("&quot;", Strings.ChrW(34).ToString());

				List<string> caracteres = new List<string>
				{
					"!", "#", "$", "%", "&", "'", "(", ")", "*", "+", ",", "-", ".", "/",
					":", ";", "<", "=", ">", "?", "@", "[", "\\", "]", "^", "_", "`", "{", "|", "}", "~", "€", "‚", "ƒ", "„",
					"…", "†", "‡", "ˆ", "‰", "Š", "‹", "Œ", "Ž", "‘", "’", "“", "”", "•", "˜", "™", "š", "›", "œ", "ž", "Ÿ", "¡",
					"¢", "£", "¤", "¥", "¦", "§", "¨", "©", "ª", "«", "¬", "®", "¯", "°", "±", "²", "³", "´", "µ", "¶", "·", "¸",
					"¹", "º", "»", "¼", "½", "¾", "¿", "Æ", "Ç", "Ð", "×", "Ø", "Þ", "ß", "æ", "ç", "÷", "ø", "þ",
					Strings.ChrW(34).ToString(), Strings.ChrW(127).ToString(), Strings.ChrW(129).ToString(), Strings.ChrW(141).ToString(),
					Strings.ChrW(143).ToString(), Strings.ChrW(144).ToString(), Strings.ChrW(150).ToString(), Strings.ChrW(151).ToString(),
					Strings.ChrW(157).ToString(), Strings.ChrW(160).ToString(), Strings.ChrW(173).ToString(), Strings.ChrW(8211).ToString()
				};

				foreach (string caracter in caracteres)
				{
					nombre = nombre.Replace(caracter, null);
				}

				nombre = nombre.Replace("À", "A");
				nombre = nombre.Replace("Á", "A");
				nombre = nombre.Replace("Â", "A");
				nombre = nombre.Replace("Ã", "A");
				nombre = nombre.Replace("Ä", "A");
				nombre = nombre.Replace("Å", "A");
				nombre = nombre.Replace("à", "a");
				nombre = nombre.Replace("á", "a");
				nombre = nombre.Replace("â", "a");
				nombre = nombre.Replace("ã", "a");
				nombre = nombre.Replace("ä", "a");
				nombre = nombre.Replace("å", "a");

				nombre = nombre.Replace("È", "E");
				nombre = nombre.Replace("É", "E");
				nombre = nombre.Replace("Ê", "E");
				nombre = nombre.Replace("Ë", "E");
				nombre = nombre.Replace("è", "e");
				nombre = nombre.Replace("é", "e");
				nombre = nombre.Replace("ê", "e");
				nombre = nombre.Replace("ë", "e");

				nombre = nombre.Replace("Ì", "I");
				nombre = nombre.Replace("Í", "I");
				nombre = nombre.Replace("Î", "I");
				nombre = nombre.Replace("Ï", "I");
				nombre = nombre.Replace("ì", "i");
				nombre = nombre.Replace("í", "i");
				nombre = nombre.Replace("î", "i");
				nombre = nombre.Replace("ï", "i");

				nombre = nombre.Replace("Ò", "O");
				nombre = nombre.Replace("Ó", "O");
				nombre = nombre.Replace("Ô", "O");
				nombre = nombre.Replace("Õ", "O");
				nombre = nombre.Replace("Ö", "O");
				nombre = nombre.Replace("ð", "o");
				nombre = nombre.Replace("ò", "o");
				nombre = nombre.Replace("ó", "o");
				nombre = nombre.Replace("ô", "o");
				nombre = nombre.Replace("õ", "o");
				nombre = nombre.Replace("ö", "o");

				nombre = nombre.Replace("Ù", "U");
				nombre = nombre.Replace("Ú", "U");
				nombre = nombre.Replace("Û", "U");
				nombre = nombre.Replace("Ü", "U");
				nombre = nombre.Replace("ù", "u");
				nombre = nombre.Replace("ú", "u");
				nombre = nombre.Replace("û", "u");
				nombre = nombre.Replace("ü", "u");

				nombre = nombre.Replace("Ñ", "N");
				nombre = nombre.Replace("ñ", "n");

				nombre = nombre.Replace("Ý", "Y");
				nombre = nombre.Replace("ý", "y");
				nombre = nombre.Replace("ÿ", "y");

				nombre = nombre.ToLower();
				nombre = nombre.Trim();

				return nombre;
			}

			return null;
		}

		public static bool ComprobarBundles(Juegos.Juego juego)
		{
			if (juego.Bundles != null)
			{
				if (juego.Bundles.Count > 0)
				{
					foreach (var bundle in juego.Bundles)
					{
						if (DateTime.Now >= bundle.FechaEmpieza && DateTime.Now <= bundle.FechaTermina)
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		public static bool ComprobarGratis(Juegos.Juego juego)
		{
			if (juego.Gratis != null)
			{
				if (juego.Gratis.Count > 0)
				{
					foreach (var gratis in juego.Gratis)
					{
						if (DateTime.Now >= gratis.FechaEmpieza && DateTime.Now <= gratis.FechaTermina)
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		public static bool ComprobarSuscripcion(Juegos.Juego juego)
		{
			if (juego.Suscripciones != null)
			{
				if (juego.Suscripciones.Count > 0)
				{
					foreach (var suscripcion in juego.Suscripciones)
					{
						if (DateTime.Now >= suscripcion.FechaEmpieza && DateTime.Now <= suscripcion.FechaTermina)
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		public static string GenerarMensaje(string idioma, Juegos.Juego juego, bool buscarBundles, bool buscarGratis, bool buscarSuscripciones)
		{
			string mensaje = string.Empty;

			if (string.IsNullOrEmpty(juego.FreeToPlay) == false)
			{
				if (juego.FreeToPlay.ToLower() == "true")
				{
					return Herramientas.Idiomas.BuscarTexto(idioma, "SearchMessage6", "Header");
				}
			}

			if (buscarBundles == true)
			{
				if (juego.Bundles != null)
				{
					if (juego.Bundles.Count > 0)
					{
						foreach (var bundle in juego.Bundles)
						{
							if (DateTime.Now >= bundle.FechaEmpieza && DateTime.Now <= bundle.FechaTermina)
							{
								Bundles2.Bundle bundle2 = global::BaseDatos.Bundles.Buscar.UnBundle(bundle.BundleId);

                                return string.Format(Herramientas.Idiomas.BuscarTexto(idioma, "SearchMessage4", "Header"), bundle2.NombreBundle, bundle2.NombreTienda);
							}
						}
					}
				}
			}

			if (buscarGratis == true)
			{
				if (juego.Gratis != null)
				{
					if (juego.Gratis.Count > 0)
					{
						foreach (var gratis in juego.Gratis)
						{
							if (DateTime.Now >= gratis.FechaEmpieza && DateTime.Now <= gratis.FechaTermina)
							{
								return string.Format(Herramientas.Idiomas.BuscarTexto(idioma, "SearchMessage5", "Header"), Gratis2.GratisCargar.DevolverGratis(gratis.Tipo).Nombre);
							}
						}
					}
				}
			}
			
			if (buscarSuscripciones == true)
			{
				if (juego.Suscripciones != null)
				{
					if (juego.Suscripciones.Count > 0)
					{
						double precio = 1000000;
						string nombre = string.Empty;

						foreach (var suscripcion in juego.Suscripciones)
						{
							if (DateTime.Now >= suscripcion.FechaEmpieza && DateTime.Now <= suscripcion.FechaTermina)
							{
								Suscripciones2.Suscripcion suscripcion2 = Suscripciones2.SuscripcionesCargar.DevolverSuscripcion(suscripcion.Tipo);

								if (suscripcion2.Precio < precio)
								{
									precio = suscripcion2.Precio;
									nombre = suscripcion2.Nombre;
								}
							}
						}

						if (precio < 1000000)
						{
							return string.Format(Herramientas.Idiomas.BuscarTexto(idioma, "SearchMessage3", "Header"), nombre);
						}
					}
				}
			}

			decimal minimoCantidad = 10000000;
			Juegos.JuegoPrecio minimoFinal = new Juegos.JuegoPrecio();

			if (juego.PrecioActualesTiendas != null)
			{
				foreach (var oferta in juego.PrecioActualesTiendas)
				{
					bool drmAdecuado = true;

					if (oferta.DRM == Juegos.JuegoDRM.NoEspecificado)
					{
						drmAdecuado = false;
					}
					else if (oferta.DRM == Juegos.JuegoDRM.Microsoft)
					{
						drmAdecuado = false;
					}

					if (drmAdecuado == true)
					{
						if (Herramientas.OfertaActiva.Verificar(oferta) == true)
						{
							decimal tempPrecio = oferta.Precio;

							if (oferta.Moneda != Herramientas.JuegoMoneda.Euro)
							{
								tempPrecio = Herramientas.Divisas.Cambio(tempPrecio, oferta.Moneda);
							}

							if (tempPrecio < minimoCantidad)
							{
								minimoCantidad = tempPrecio;
							}
						}
					}
				}
			}

			if (minimoCantidad > 0 && minimoCantidad < 10000000)
			{
				return string.Format(Herramientas.Idiomas.BuscarTexto(idioma, "SearchMessage1", "Header"), Herramientas.Precios.Euro(minimoCantidad));
			}
			else
			{
				return Herramientas.Idiomas.BuscarTexto(idioma, "SearchMessage2", "Header");
			}
		}
	}
}
