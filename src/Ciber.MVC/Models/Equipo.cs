using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciber.MVC.Models
{
    public class Equipo
    {
        public int Nmaquina { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string TipoEquipo { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }
}
