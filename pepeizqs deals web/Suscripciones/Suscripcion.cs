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
		public bool UsuarioInteractuar;
		public bool ParaSiempre; //true si pagas juegos son para siempre
	}

    public class SuscripcionComponente
    {
        public Suscripcion Tipo;
        public List<JuegoSuscripcion> Juegos;
    }
}
