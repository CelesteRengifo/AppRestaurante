using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRestaurante.Modelos
{
    public class PlatoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } // Este sí coincide con el JSON
        public decimal Precio { get; set; }
    }

}
