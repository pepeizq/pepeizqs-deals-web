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
			if (File.Exists("./wwwroot/imagenes/webps/" + nombreFichero + ".webp") == false)
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
	}
}
