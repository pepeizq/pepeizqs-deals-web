#nullable disable

using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;

namespace Herramientas
{
    public static class Imagenes
	{
		public static async Task<string> DescargarYGuardar(string fichero, string subCarpeta, string nombreCarpeta, string nombreFichero, string dominio)
		{
			if (string.IsNullOrEmpty(dominio) == false)
			{
				if (dominio.Contains("beta") == false)
				{
					if (File.Exists(Directory.GetCurrentDirectory() + "\\imagenes\\" + subCarpeta + "\\" + nombreCarpeta + "\\" + nombreFichero + ".webp") == false)
					{
						Stream ficheroStream = await CogerStreamFichero(fichero);

						if (ficheroStream != Stream.Null)
						{
							await GuardarStream(ficheroStream, subCarpeta, nombreCarpeta, nombreFichero);
						}
					}

					return "/imagenes/" + subCarpeta + "/" + nombreCarpeta + "/" + nombreFichero + ".webp";
				}
			}

			return fichero;
		}

		public static async Task<Stream> CogerStreamFichero(string ficheroEnlace)
		{
			HttpClient cliente = new HttpClient();

			try
			{
				Stream fileStream = await cliente.GetStreamAsync(ficheroEnlace);

				return fileStream;
			}
			catch
			{
				return Stream.Null;
			}
		}

		public static async Task GuardarStream(Stream ficheroStream, string subCarpeta, string nombreCarpeta, string nombreFichero)
		{
			string origenCarpeta = Directory.GetCurrentDirectory() + "\\imagenes\\";

			if (Directory.Exists(origenCarpeta) == false)
			{
				Directory.CreateDirectory(origenCarpeta);
			}

			string nuevaCarpeta = Directory.GetCurrentDirectory() + "\\imagenes\\" + subCarpeta;

			if (Directory.Exists(nuevaCarpeta) == false)
			{
				Directory.CreateDirectory(nuevaCarpeta);
			}

			string nuevaCarpeta2 = Directory.GetCurrentDirectory() + "\\imagenes\\" + subCarpeta + "\\" + nombreCarpeta;

			if (Directory.Exists(nuevaCarpeta2) == false)
			{
				Directory.CreateDirectory(nuevaCarpeta2);
			}

			string ruta = Path.Combine(nuevaCarpeta2, nombreFichero);

			using (FileStream ficheroSalidaStream = new FileStream(ruta + ".jpg", FileMode.Create))
			{
				try
				{
					await ficheroStream.CopyToAsync(ficheroSalidaStream);
				}
				catch { }

				using (var ficheroWebpStream = new FileStream(ruta + ".webp", FileMode.Create))
				{
					using (ImageFactory imagenFabrica = new ImageFactory(preserveExifData: false))
					{
						FormFile ficheroWebp = new FormFile(ficheroSalidaStream, 0, ficheroSalidaStream.Length, null, nombreFichero + ".webp");

						imagenFabrica.Load(ficheroWebp.OpenReadStream()).Format(new WebPFormat()).Quality(50).Save(ficheroWebpStream);
					}
				}
			}

			if (File.Exists(ruta + ".jpg") == true)
			{
				File.Delete(ruta + ".jpg");
			}
		}
	}
}
