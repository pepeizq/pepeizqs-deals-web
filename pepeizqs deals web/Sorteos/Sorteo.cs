﻿#nullable disable

namespace Sorteos2
{
    public class Sorteo
    {
        public int Id;
        public int JuegoId;
        public string GrupoId;
        public string Clave;
        public List<string> Participantes;
        public DateTime FechaTermina;
        public string GanadorId;
    }

    public class GrupoSorteo
    {
        public string Nombre;
        public string Id;
    }
}
