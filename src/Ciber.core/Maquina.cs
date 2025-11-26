namespace Ciber.core;

public class Maquina
{
    public int Nmaquina { get; set; }
    public string Estado { get; set; } = "Disponible"; // Cambiado a string/enum
    public string Caracteristicas { get; set; } = string.Empty;
    public decimal PrecioPorHora { get; set; } = 5.00m;
    public string TipoMaquina { get; set; } = "Estándar";
    public DateTime? UltimoMantenimiento { get; set; } // Nuevo campo
    public DateTime? ProximoMantenimiento { get; set; } // Nuevo campo
    
    // Propiedades calculadas
    public bool EstaDisponible => Estado == "Disponible";
    public bool EstaOcupada => Estado == "Ocupada";
    public bool EnMantenimiento => Estado == "Mantenimiento";
}