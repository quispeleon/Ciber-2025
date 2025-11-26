namespace Ciber.core;

public class Alquiler
{
    public int IdAlquiler { get; set; }
    public int Ncuenta { get; set; }
    public int Nmaquina { get; set; }
    public int Tipo { get; set; }
    public DateTime FechaInicio { get; set; } = DateTime.Now;
    public DateTime? FechaFin { get; set; }
    public TimeSpan? TiempoContratado { get; set; } // Nuevo nombre (era CantidadTiempo)
    public TimeSpan? TiempoUsado { get; set; } // Nuevo campo
    public decimal PrecioPorHora { get; set; }
    public decimal TotalAPagar { get; set; } = 0.00m;
    public decimal MontoPagado { get; set; } = 0.00m;
    public string Estado { get; set; } = "Activo"; // Nuevo campo
    public bool SesionActiva { get; set; } = true; // Nuevo campo
    
    // Propiedades calculadas
    public bool EsActivo => SesionActiva && !FechaFin.HasValue;
    public TimeSpan TiempoTranscurrido => 
        FechaFin.HasValue ? FechaFin.Value - FechaInicio : DateTime.Now - FechaInicio;
    public decimal CostoActual => 
        (decimal)TiempoTranscurrido.TotalHours * PrecioPorHora;
    public bool TieneSaldoSuficiente(decimal saldoUsuario) => saldoUsuario >= CostoActual;
}