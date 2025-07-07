using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRestaurante.Modelos
{
    public class RespuestaPedidosReporteDTO
    {
        public int Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public List<PedidoReporteDTO> Results { get; set; } = new();
    }
}

