using System.Text.Json.Serialization;

namespace Ciber.core;

public class Cuenta
{
    public int Ncuenta { get; set; } 
    public string Nombre { get; set; } = string.Empty;
    
    [JsonIgnore]
    public string Pass { get; set; } = string.Empty;
    
    public string Dni { get; set; } = string.Empty; // Cambiado a string
    public TimeSpan HoraRegistrada { get; set; }
    public decimal Saldo { get; set; } = 0.00m; // Nuevo campo
    public bool Activa { get; set; } = true; // Nuevo campo
}