#nullable disable

using Juegos;

namespace Suscripciones2
{
	public class Suscripcion
	{
		public SuscripcionTipo Id;
		public string Nombre;
		public string ImagenLogo;
		public string ImagenIcono;
		public string Enlace;
		public DateTime FechaSugerencia;
		public JuegoDRM DRMDefecto;
		public string ImagenNoticia;
		public bool AdminInteractuar;
		public bool UsuarioEnlacesEspecificos;
		public bool ParaSiempre; //true si pagas los juegos son para siempre
		public SuscripcionTipo? IncluyeSuscripcion;
		public double Precio;
		public bool AdminPendientes;
		public string TablaPendientes;
    }

    public class SuscripcionComponente
    {
        public Suscripcion Tipo;
        public List<JuegoSuscripcion> Juegos;
    }
}
