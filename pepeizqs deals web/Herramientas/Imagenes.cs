#nullable disable

using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Juegos;

namespace Herramientas
{
    public static class Imagenes
	{
		public static async void ComprobarJuego(Juego juego)
		{
			await DescargarYGuardar(juego.Imagenes.Logo, juego.Id.ToString(), "logo");
			await DescargarYGuardar(juego.Imagenes.Capsule_231x87, juego.Id.ToString(), "capsule_231x87");
		}

		public static async Task DescargarYGuardar(string fichero, string destinoCarpeta, string destinoFichero)
		{
			Stream ficheroStream = await CogerStreamFichero(fichero);

            if (ficheroStream != Stream.Null)
			{
				await GuardarStream(ficheroStream, destinoCarpeta, destinoFichero);
			}
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

		public static async Task GuardarStream(Stream ficheroStream, string nombreCarpeta, string nombreFichero)
		{
			string nuevaCarpeta = Path.GetFullPath("wwwroot") + "\\imagenes\\" + nombreCarpeta;

			if (!Directory.Exists(nuevaCarpeta))
			{
				Directory.CreateDirectory(nuevaCarpeta);
			}
				
			string ruta = Path.Combine(nuevaCarpeta, nombreFichero);

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
		}
	}
}
