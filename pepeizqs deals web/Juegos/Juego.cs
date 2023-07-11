#nullable disable

//https://store.steampowered.com/api/appdetails?appids=1868140

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Juegos
{
	public class Juego
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; } //Id Steam
		public string Nombre { get; set; }
		public string Tipo { get; set; }
		public JuegoImagenes Imagenes { get; set; }
		public JuegoPrecio PrecioMinimoActual { get; set; }
		public JuegoPrecio PrecioMinimoHistorico { get; set; }
		public List<JuegoPrecio> PrecioActualesTiendas { get; set; }
		public JuegoAnalisis Analisis { get; set; }

	}

	public class JuegoImagenes
	{
		public string Header_460x215 { get; set; }
		public string Capsule_231x87 { get; set; }
		public string Library_600x900 { get; set; }
		public string Library_1920x620 { get; set; }
		public string Logo { get; set; }
	}

	public class JuegoPrecio
	{
		public string DRM { get; set; }
		public string Enlace { get; set; }
		public string Descuento { get; set; }
		public float Precio { get; set; }
		public string Tienda { get; set; }
		public DateTime FechaDetectado { get; set; }
	}

	public class JuegoAnalisis
	{
		public string Porcentaje { get; set; }
		public string Cantidad { get; set; }
	}




	//-------------------------------------------------------

	public class JuegoAdminBusqueda
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Nombre { get; set; }
	}
}
