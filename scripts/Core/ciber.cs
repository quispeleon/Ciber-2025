public class Cuenta
{
    public int Ncuenta { get; set; }
    public string Nombre { get; set; }
    public string Pass { get; set; }
    public int Dni { get; set; }
    public TimeSpan HoraRegistrada { get; set; }
}

public class Maquina
{
    public int Nmaquina { get; set; }
    public bool Estado { get; set; }
    public string Caracteristicas { get; set; }
}

public class Tipo
{
    public int IdTipo { get; set; }
    public string TipoDescripcion { get; set; }
}

public class Alquiler
{
    public int IdAlquiler { get; set; }
    public int Ncuenta { get; set; }
    public int Nmaquina { get; set; }
    public int Tipo { get; set; }
    public TimeSpan? CantidadTiempo { get; set; }
    public bool? Pagado { get; set; }
}

public class HistorialdeAlquiler
{
    public int IdHistorial { get; set; }
    public int Ncuenta { get; set; }
    public int Nmaquina { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal TotalPagar { get; set; }
}
