using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRestaurante.Modelos
{
    public class PedidoReporteDTO
    {
        public int Id { get; set; }
        public int Plato { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
    }
}
