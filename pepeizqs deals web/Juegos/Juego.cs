﻿#nullable disable

using Bundles2;
using Gratis2;
using Herramientas;
using Microsoft.AspNetCore.Components;
using Suscripciones2;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Juegos
{
	public class Juego : ComponentBase, IComponent
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public int IdSteam { get; set; }
		public int IdGog { get; set; }
		public string Nombre { get; set; }
		public JuegoTipo Tipo { get; set; }
		public JuegoImagenes Imagenes { get; set; }
		public List<JuegoPrecio> PrecioMinimosHistoricos { get; set; }
		public List<JuegoPrecio> PrecioActualesTiendas { get; set; }
		public JuegoAnalisis Analisis { get; set; }
		public JuegoCaracteristicas Caracteristicas { get; set; }
		public JuegoMedia Media { get; set; }
		public DateTime FechaSteamAPIComprobacion { get; set; }
		public List<JuegoBundle> Bundles { get; set; }
		public List<JuegoGratis> Gratis { get; set; }
		public List<JuegoSuscripcion> Suscripciones { get; set; }
		public string NombreCodigo { get; set; }
		public int IdMaestra { get; set; }
		public List<JuegoUsuariosInteresados> UsuariosInteresados {  get; set; }
		public string SlugGOG { get; set; }
	}

	public static class JuegoCrear
	{
		public static Juego Generar()
		{
			JuegoImagenes imagenes = new JuegoImagenes();
			List<JuegoPrecio> precioMinimoHistorico = new List<JuegoPrecio>();
			List<JuegoPrecio> precioActualesTiendas = new List<JuegoPrecio>();

			Juego juego = new Juego
			{
				Imagenes = imagenes,
				PrecioMinimosHistoricos = precioMinimoHistorico,
				PrecioActualesTiendas = precioActualesTiendas
			};

			return juego;
		}
	}

	//-------------------------------------------------------

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
		public string Nombre { get; set; }
		public string Imagen { get; set; }
		public JuegoDRM DRM { get; set; }
		public string Enlace { get; set; }
		public int Descuento { get; set; }
		public decimal Precio { get; set; }
		public JuegoMoneda Moneda { get; set; }
		public string Tienda { get; set; }
		public DateTime FechaDetectado { get; set; }
		public DateTime FechaActualizacion { get; set; }
		public DateTime FechaTermina { get; set; }
		public int CodigoDescuento { get; set; }
		public string CodigoTexto { get; set; }
		public string SteamID { get; set; }
	}

	public class JuegoAnalisis
	{
		public string Porcentaje { get; set; }
		public string Cantidad { get; set; }
	}

	public class JuegoCaracteristicas
	{
		public bool Windows { get; set; }
		public bool Mac { get; set; }
		public bool Linux { get; set; }
		public List<string> Desarrolladores { get; set; }
		public List<string> Publishers { get; set; }
		public string Descripcion { get; set; }
	}

	public class JuegoMedia
	{ 
		public string Video { get; set; }
		public List<string> Capturas { get; set; }
		public List<string> Miniaturas { get; set; }
	}

	public class JuegoUsuariosInteresados
	{
		public string UsuarioId { get; set; }
		public JuegoDRM DRM { get; set; }
	}

	public class JuegoBundle
	{
		public int BundleId { get; set; }
		public BundleTipo Tipo { get; set; }
		public int JuegoId { get; set; }
		public string Nombre { get; set; }
		public string Imagen { get; set; }
		public string Enlace { get; set; }
		public DateTime FechaEmpieza { get; set; }
		public DateTime FechaTermina { get; set; }
		public JuegoDRM DRM { get; set; }
		public BundleTier Tier { get; set; }
	}

	public class JuegoGratis
	{
		public GratisTipo Tipo { get; set; }
		public int JuegoId { get; set; }
		public string Nombre { get; set; }
		public string Imagen { get; set; }
		public DateTime FechaEmpieza { get; set; }
		public DateTime FechaTermina { get; set; }
		public JuegoDRM DRM { get; set; }
		public string Enlace { get; set; }
		public string ImagenNoticia { get; set; }
	}

	public class JuegoSuscripcion
	{
		public SuscripcionTipo Tipo { get; set; }
		public int JuegoId { get; set; }
		public string Nombre { get; set; }
		public string Imagen { get; set; }
		public DateTime FechaEmpieza { get; set; }
		public DateTime FechaTermina { get; set; }
		public JuegoDRM DRM { get; set; }
		public string Enlace { get; set; }
		public string ImagenNoticia { get; set; }
	}

	//-------------------------------------------------------

	public enum JuegoTipo
	{
		Game,
		DLC,
		Bundle
	}
}
