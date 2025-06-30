using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRestaurante.Modelos.Clases_pedidos
{
    public class Pedidos
    {
        public int Plato { get; set; }
        public int Empleado { get; set; }
        public int Mesa { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = "ordenado";
    }
}
