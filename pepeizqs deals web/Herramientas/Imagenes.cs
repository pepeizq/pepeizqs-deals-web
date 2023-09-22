#nullable disable

using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.IO;
using System.Net;

namespace Herramientas
{
	public static class Imagenes
	{
		public static async Task DescargarYGuardar(string fichero, string destinationFolder, string destinationFileName)
		{
			Stream ficheroStream = await GetFileStream(fichero);

			if (ficheroStream != Stream.Null)
			{
				await SaveStream(ficheroStream, destinationFolder, destinationFileName);
			}
		}

		public static async Task<Stream> GetFileStream(string ficheroEnlace)
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

		public static async Task SaveStream(Stream fileStream, string destinationFolder, string destinationFileName)
		{
			if (!Directory.Exists(destinationFolder))
			{
				Directory.CreateDirectory(string.Format("{0}{1}{2}", Path.GetFullPath("wwwroot"), "\\", destinationFolder));
			}
				
			string path = Path.Combine(destinationFolder, destinationFileName);

			using (FileStream outputFileStream = new FileStream(path, FileMode.CreateNew))
			{
				await fileStream.CopyToAsync(outputFileStream);
			}
		}
	}
}
