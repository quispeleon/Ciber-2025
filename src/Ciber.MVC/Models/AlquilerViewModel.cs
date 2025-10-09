using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciber.MVC.Models
{
    public class AlquilerViewModel
    {
        public int? IdAlquiler { get; set; }
        public int Ncuenta { get; set; }
        public int Nmaquina { get; set; }
        public int Tipo { get; set; }
        public TimeSpan? CantidadTiempo { get; set; }
        public bool? Pagado { get; set; }
    }
}
