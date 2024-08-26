namespace Ciber.core;

public class Alquiler
{
    public int IdAlquiler { get; set; }
    public int Ncuenta { get; set; }
    public int Nmaquina { get; set; }
    public int Tipo { get; set; }
    public TimeSpan? CantidadTiempo { get; set; } // Si es nullable
    public bool? Pagado { get; set; } // Nullable
}

