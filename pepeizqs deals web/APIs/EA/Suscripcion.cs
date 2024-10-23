#nullable disable

using Herramientas;
using Juegos;
using System.Text.Json;

namespace APIs.EA
{
	public static class Suscripcion
	{
		public static Suscripciones2.Suscripcion Generar()
		{
			Suscripciones2.Suscripcion eaPlay = new Suscripciones2.Suscripcion
			{
				Id = Suscripciones2.SuscripcionTipo.EAPlay,
				Nombre = "EA Play",
				ImagenLogo = "/imagenes/suscripciones/humblechoice.webp",
				ImagenIcono = "/imagenes/tiendas/ea_icono.webp",
				Enlace = "https://www.ea.com/ea-play",
				DRMDefecto = JuegoDRM.EA,
				AdminInteracturar = false
			};

			return eaPlay;
		}

		public static async Task Buscar()
		{
			string html = await Decompiladores.Estandar("https://api3.origin.com/supercat/GB/en_GB/supercat-PCWIN_MAC-GB-en_GB.json.gz");

			if (string.IsNullOrEmpty(html) == false)
			{
				EABD basedatos = JsonSerializer.Deserialize<EABD>(html);

				if (basedatos != null)
				{
					if (basedatos.Juegos != null)
					{
						if (basedatos.Juegos.Count > 0)
						{
							foreach (var juego in basedatos.Juegos)
							{
								if (juego.Suscripcion != null)
								{
									if (string.IsNullOrEmpty(juego.Suscripcion.Enlace) == false)
									{

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
