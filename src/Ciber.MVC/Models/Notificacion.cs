using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ciber.MVC.Models
{
    public class Notificacion
    {
        public string Mensaje { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty; // Ej: éxito, error
    }
}
