#nullable disable

using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using System.Drawing;

namespace Herramientas.Ficheros
{
	public static class Imagenes
	{
		public static async Task<string> DescargarYGuardar(string enlace, string nombreFichero, int ancho, int alto)
		{
			bool generar = false;

			if (File.Exists("./wwwroot/imagenes/webps/" + nombreFichero + ".webp") == false)
			{
				generar = true;
			}
			else if (File.Exists("./wwwroot/imagenes/webps/" + nombreFichero + ".webp") == true)
			{
				var info = new FileInfo("./wwwroot/imagenes/webps/" + nombreFichero + ".webp");

				if (info.Length == 0)
				{
					generar = true;
				}
			}

			if (generar == true)
			{
				HttpClient cliente = new HttpClient();

				try
				{
					byte[] bytes = await cliente.GetByteArrayAsync(enlace);

					string ruta = Path.Combine("./wwwroot/imagenes/webps/", nombreFichero);

					using (FileStream ficheroWebpStream = new FileStream(ruta + ".webp", FileMode.Create))
					{
						using (ImageFactory imagenFabrica = new ImageFactory(preserveExifData: false))
						{
							FormFile ficheroWebp = new FormFile(new MemoryStream(bytes), 0, bytes.Length, null, nombreFichero + ".webp");

							if (ancho > 0 && alto > 0)
							{
								imagenFabrica.Load(ficheroWebp.OpenReadStream()).Resize(new ResizeLayer(new Size(ancho, alto))).Format(new WebPFormat()).Save(ficheroWebpStream);
							}
							else
							{
								imagenFabrica.Load(ficheroWebp.OpenReadStream()).Format(new WebPFormat()).Save(ficheroWebpStream);
							}

							return "imagenes/webps/" + nombreFichero + ".webp";
						}
					}
				}
				catch
				{
					return enlace;
				}
			}

			return "imagenes/webps/" + nombreFichero + ".webp";
		}

		public static string ServidorExterno(string enlace, int ancho = 0, int alto = 0)
		{
			if (string.IsNullOrEmpty(enlace) == false)
			{
				if (enlace.IndexOf("/") == 0)
				{
					enlace = "https://pepeizqdeals.com" + enlace;
				}

				bool añadirServidor = true;

				if (enlace.Contains("https://hb.imgix.net/") == true)
				{
					añadirServidor = false;
				}
				else if (enlace.Contains("https://i.imgur.com/") == true)
				{
					añadirServidor = false;
				}

				if (añadirServidor == true)
				{
					enlace = "https://wsrv.nl/?n=-1&output=webp&url=" + enlace;

					if (ancho > 0 && alto > 0)
					{
						enlace = enlace + "&w=" + ancho + "&h=" + alto + "&dpr=2";
					}
				}
			}

			return enlace;
		}
	}
}
