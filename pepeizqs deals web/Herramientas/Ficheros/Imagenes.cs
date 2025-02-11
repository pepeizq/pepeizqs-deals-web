#nullable disable

using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;

namespace Herramientas.Ficheros
{
	public static class Imagenes
	{
		public static async Task<string> DescargarYGuardar(string enlace, string nombreFichero)
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

							imagenFabrica.Load(ficheroWebp.OpenReadStream()).Format(new WebPFormat()).Quality(50).Save(ficheroWebpStream);

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
