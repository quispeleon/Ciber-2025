namespace Ciber.core;

public class Transaccion
{
    public int IdTransaccion { get; set; }
    public int Ncuenta { get; set; }
    public string Tipo { get; set; } = string.Empty; // Recarga, Consumo, Devolucion
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
    public int? IdAlquiler { get; set; } // Relación opcional con alquiler
    public string Descripcion { get; set; } = string.Empty;
    
    // Propiedades calculadas
    public bool EsRecarga => Tipo == "Recarga";
    public bool EsConsumo => Tipo == "Consumo";
    public bool EsDevolucion => Tipo == "Devolucion";
    public string SignoMonto => EsRecarga || EsDevolucion ? "+" : "-";
    public string MontoFormateado => $"{SignoMonto}${Monto:N2}";
}