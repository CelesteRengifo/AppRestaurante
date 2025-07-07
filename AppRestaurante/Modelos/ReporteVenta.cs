using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRestaurante.Modelos
{
    public class ReporteVenta
    {
        public string Plato { get; set; }
        public int CantidadVendida { get; set; }
        public DateTime? Fecha { get; set; } // nuevo
    }
}
