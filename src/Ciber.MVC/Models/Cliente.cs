using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciber.MVC.Models
{
    public class Cliente
    {
        public int Ncuenta { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public int Dni { get; set; }
    }
}
