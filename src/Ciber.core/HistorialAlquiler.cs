namespace Ciber.core;

public class HistorialdeAlquiler
{
    public int IdHistorial { get; set; }
    public int IdAlquiler { get; set; } // Nuevo campo
    public int Ncuenta { get; set; }
    public int Nmaquina { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public TimeSpan? TiempoContratado { get; set; } // Nuevo campo
    public TimeSpan? TiempoUsado { get; set; } // Nuevo campo
    public decimal PrecioPorHora { get; set; } // Nuevo campo
    public decimal TotalPagar { get; set; }
    public decimal MontoPagado { get; set; }
    public string EstadoFinal { get; set; } = string.Empty; // Nuevo campo
    public string Observaciones { get; set; } = string.Empty; // Nuevo campo
    
    // Propiedades calculadas
    public TimeSpan DuracionTotal => FechaFin - FechaInicio;
    public decimal HorasUtilizadas => (decimal)DuracionTotal.TotalHours;
    public decimal MontoPendiente => TotalPagar - MontoPagado;
    public bool EstaCompletamentePagado => MontoPendiente <= 0;
    public bool FueTiempoExacto => TiempoContratado.HasValue && 
                                 Math.Abs((TiempoContratado.Value - DuracionTotal).TotalMinutes) <= 1;
}